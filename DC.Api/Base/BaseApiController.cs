using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;

namespace DC.Api.Base
{
    //[Authorize]
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {

        private string roleId { get; set; } = null;

        /// <summary>
        /// 从jwt token获取用户RoleId
        /// </summary>
        public string RoleId
        {
            get
            {
                if (roleId == null)
                {
                    roleId = GetValByKeyToken("Role");
                }
                return roleId;
            }
        }
         
        private string adminId { get; set; } = null;

        /// <summary>
        /// 从jwt token获取用户ID
        /// </summary>
        public string AdminId
        {
            get
            {
                if (adminId == null)
                {
                    adminId = GetValByKeyToken(JwtRegisteredClaimNames.Jti);
                }
                return adminId;
            }
        }

        private string account { get; set; } = null;
        /// <summary>
        /// 从jwt token获取用户账户
        /// </summary>
        public string Account
        {
            get
            {
                if (account == null)
                {
                    account = GetValByKeyToken("Account");
                }
                return account;
            }
        }

        /// <summary>
        /// 从token根据Key获取用户信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>

        private string GetValByKeyToken(string key)
        {
            ClaimsPrincipal principal = HttpContext.User;
            if (principal == null || principal.Identity == null)
            {
                throw new AuthenticationException("无效的用户信息,请重新登录");
            }
            if (!principal.Identity.IsAuthenticated)
            {
                throw new AuthenticationException("尚未认证,请登录");
            }
            try
            {
                var identity = (ClaimsIdentity)principal.Identity;
                return identity.FindFirst(key)?.Value;
            }
            catch (Exception ex)
            {
                throw new AuthenticationException("尚未认证,请登录"+ ex.Message);
            }
        }

    }
}
