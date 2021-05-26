using Coldairarrow.Util;
using DC.Api.Base;
using DC.Api.Filter;
using DC.Api.JwtAuth;
using DC.Api.Models;
using DC.Domain.Emuns;
using DC.Domain.Global;
using DC.Domain.Input;
using DC.Domain.Models;
using DC.Domain.Output;
using DC.IService;
using DC.Utils;
using DC.Utils.Configuration;
using DC.Utils.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DC.Api.Controllers
{
    /// <summary>
    /// 登录模块
    /// </summary>
    [Route("api/[controller]")]
    public class LoginController : BaseApiController
    {

        private readonly ICacheService _cache;
        private readonly IDCUserService _dcUserService;
        private readonly IPermissionService _permissionService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITokenService _tokenService;
        private readonly IActionLogService _actionLogService;

        /// <summary>
        /// 构造注入
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="sysAdminService"></param>
        /// <param name="sysLoginLogService"></param>
        /// <param name="httpContextAccessor"></param>
        /// <param name="sysPermissionService"></param>
        /// <param name="tokenService"></param>
        public LoginController(ICacheService cache, IDCUserService dcUserService, IHttpContextAccessor httpContextAccessor,
            IPermissionService sysPermissionService, ITokenService tokenService, IActionLogService actionLogService)
        {
            _cache = cache;
            _tokenService = tokenService;
            _dcUserService = dcUserService;
            _actionLogService = actionLogService;
            _permissionService = sysPermissionService;
            _httpContextAccessor = httpContextAccessor;
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="inputLogin"></param>
        /// <returns></returns>
        [HttpPost("login"), AnyFilter(Controller = "SysLogin", Action = "Login")]
        [AllowAnonymous]
        public async Task<ApiResult<string>> Login([FromBody] LoginInput inputLogin)
        {
            var res = new ApiResult<string>()
            {
                Success = false,
                StatusCode = StatusCodeEnum.Waring
            };

            #region 1. 判断用户登录次数限制以及过期时间

            //获得用户登录限制次数

            //获得登录次数和过期时间
            var loginConfig = _cache.Get<CacheAdminLogin>(CacheKey.LoginCount) ?? new CacheAdminLogin();
            if (loginConfig.Count != 0 && loginConfig.DelayMinute != null)
            {
                //说明存在过期时间，需要判断
                if (DateTime.Now <= loginConfig.DelayMinute)
                {
                    res.Msg = "您已连续登录已超过设定次数，请稍后再次登录~";
                    return res;
                }
                else
                {
                    //已经过了登录的预设时间，重置登录配置参数
                    loginConfig.Count = 0;
                    loginConfig.DelayMinute = null;
                }
            }
            #endregion

            #region 2.验证用户信息 

            var model = await _dcUserService.GetModelAsync(m => m.Account == inputLogin.Account && m.Pwd == inputLogin.Password.ToMD5Encrypt32());
            if (model == null)
            {
                res.Msg = "账户或密码错误";
                return res;
            }
            if (!model.Status)
            {
                res.Msg = "账号已被禁用";
                return res;
            }
            #endregion

            #region 3. 设置Identity User信息
            model.LastLoginIp = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            model.LastLoginTime = DateTime.Now;
            model.LoginTotalCount += 1;
            await _dcUserService.UpdateAsync(model);
            //var identity = new ClaimsPrincipal(
            // new ClaimsIdentity(new[]
            //     {
            //                  new Claim(ClaimTypes.Sid,model.Id),
            //                  new Claim(ClaimTypes.Role,model.Id),
            //                  new Claim(ClaimTypes.Thumbprint,model.Email),
            //                  new Claim(ClaimTypes.Name,model.NickName),
            //                  new Claim(ClaimTypes.WindowsAccountName,model.Account),
            //                  new Claim(ClaimTypes.UserData,model.LastLoginTime.ToString())
            //     }, CookieAuthenticationDefaults.AuthenticationScheme)
            //);

            ////如果保存用户类型是Session，则默认设置cookie退出浏览器 清空，并且保存用户信息
            //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            //    identity, new AuthenticationProperties
            //    {
            //        AllowRefresh = false
            //    });
            #endregion

            #region 4. 生成token信息，并且返回给前端

            var _appConfig = AppSetting.Get<AppConfig>();

            var token = _tokenService.IssueToken(new TokenModel()
            {
                Id = model.Id,
                NickName = model.Name,
                Account = model.Account,
                Role = model.RoleId,
                ProjectName = "DC.Admin",
                Issuer = _appConfig.JwtConfig.Issuer,
                Audience = _appConfig.JwtConfig.Audience,
                Expires = _appConfig.JwtConfig.Expires,
                SecretKey = _appConfig.JwtConfig.SecurityKey
            });

            //清楚缓存
            _cache.Del(CacheKey.LoginCount);

            #endregion

            #region 5. 保存日志

            var agent = HttpContext.Request.Headers["User-Agent"];


            var log = new ActionLog()
            {
                Id = IdHelper.GetId(),
                CreateTime = DateTime.Now,
                CreateUser = model.Account,
                MethodName = "Login",
                ClassName = "LoginController",
                Method = "HttpPost",
                RequestParams = JsonConvert.SerializeObject(inputLogin),
                RequestTime = DateTime.Now,
                RemoteIp = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString(),
                UserAgent = agent.ToString()
            };
            await _actionLogService.AddAsync(log);

            #endregion

            res.Data = token;
            res.Success = true;
            res.StatusCode = StatusCodeEnum.OK;
            return res;
        }

        /// <summary>
        /// 请求刷新Token（以旧换新）
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet("refreshToken"), ApiFilter(Controller = "SysLogin", Action = "RefreshToken")]
        public async Task<ApiResult<string>> RefreshToken(string token = "")
        {
            string jwtStr = string.Empty;

            if (string.IsNullOrEmpty(token))
            {
                return new ApiResult<string>()
                {
                    Success = false,
                    Msg = "token无效，请重新登录！",
                };
            }
            var tokenModel = _tokenService.SerializeToken(token);
            if (tokenModel != null && !string.IsNullOrEmpty(tokenModel.Id))
            {
                var user = await _dcUserService.GetModelAsync(m => m.Id == tokenModel.Id);
                if (user != null)
                {
                    var refreshToken = _tokenService.IssueToken(tokenModel);
                    return new ApiResult<string>()
                    {
                        Success = true,
                        Msg = "获取成功",
                        Data = refreshToken
                    };
                }
            }

            return new ApiResult<string>()
            {
                Success = false,
                Msg = "认证失败！",
            };
        }


        /// <summary>
        /// 获取用户信息
        /// </summary> 
        /// <returns></returns>
        [HttpGet("getUserInfo"), ApiFilter(Controller = "SysLogin", Action = "GetUserInfo")]
        public async Task<ApiResult<DCUserOutput>> GetUserInfo()
        {
            return await _dcUserService.GetModelAsync(AdminId);
        }

        /// <summary>
        /// 获取用户菜单权限
        /// </summary>
        /// <returns></returns>
        [HttpGet("getNavRouters"), ApiFilter(Controller = "SysLogin", Action = "GetNav")]
        public async Task<ApiResult<List<NavigationOutput>>> GetNavRouters()
        {
            return await _permissionService.GetNavigationList(this.RoleId);
        }

        /// <summary>
        /// 登出操作
        /// </summary>
        /// <returns></returns>

        [HttpPost("logout"), ApiFilter(Controller = "SysLogin", Action = "Logout")]
        //[Authorize]
        public async Task<ApiResult<string>> Logout()
        {
            //await HttpContext.SignOutAsync(JwtBearerDefaults.AuthenticationScheme);
            return new ApiResult<string>();
        }
    }
}