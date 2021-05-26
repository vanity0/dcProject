using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;
using IP2Region;
using System.IO;
using DC.Utils.Configuration;
using DC.Utils.IPUtils.Models;
using DC.Utils.Extensions; 

namespace DC.Utils.IPUtils
{
    /// <summary>
    /// 1. 我想获取客户端IP地址
    /// 2. 我想拦截掉所有的某地区的访问流量；
    /// 3. 我想拦截请求中包含敏感词的流量；
    /// 4. 我想拦截掉黑名单中的IP；
    /// 5. 我想拦截特定的IP地址段；
    /// 6. 我想限制同一IP每分钟的最大请求数为300次/分钟，若一分钟内请求数超过300则认为是恶意请求，冻结该IP 1分钟。
    /// </summary>
    public static class IPAddressExtensions
    {
        /// <summary>
        /// 全局禁止IP
        /// </summary>
        public static string DenyIP { get; set; }
        /// <summary>
        /// IP黑名单地址段
        /// </summary>
        public static Dictionary<string, string> DenyIPRange { get; set; }
        /// <summary>
        /// ip白名单
        /// </summary>
        public static List<string> IPWhiteList = File.ReadAllText(Path.Combine(AppContext.BaseDirectory + "App_Data", "ip_bai_ming_dan.txt")).Split(',', '，').ToList();

        //获取IP地址是否是禁区的方法封装
        private static readonly DbSearcher Searcher = new DbSearcher(Path.Combine(AppContext.BaseDirectory + "App_Data", "ip2region.db"));

        /// <summary>
        /// 是否是禁区
        /// </summary>
        /// <param name="ips"></param>
        /// <returns></returns>
        public static bool IsInDenyArea(this string ips)
        {
            if (AppSetting.Get("enableDenyArea") == "true")
            {
                var denyAreas = AppSetting.Get("denyArea", "").Split(',', '，');
                foreach (var item in ips.Split(','))
                {
                    var pos = GetIPLocation(item);
                    return pos.Contains(denyAreas) || denyAreas.Intersect(pos.Split("|")).Any();
                }
            }
            return false;
        }
         
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static string GetIPLocation(this IPAddress ip) => GetIPLocation(ip.MapToIPv4().ToString());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ips"></param>
        /// <returns></returns>
        public static string GetIPLocation(this string ips)
        {
            var res = ips.Split(',', StringSplitOptions.RemoveEmptyEntries)
                      .Select(s => Searcher.MemorySearch(s.Trim())?.Region);
            return string.Join(" , ", res);
        }

        /// <summary>
        /// 判断IP地址是否被黑名单
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsDenyIpAddress(this string ip)
        {
            if (IPWhiteList.Contains(ip))
            {
                return false;
            }

            return DenyIP.Contains(ip) || DenyIPRange.AsParallel().Any(kv => kv.Key.StartsWith(ip.Split('.')[0]) && ip.IpAddressInRange(kv.Key, kv.Value));
        }

        #region 获取客户端IP地址信息

        /// <summary>
        /// 根据IP地址获取详细地理信息
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static async Task<Tuple<string, List<string>>> GetIPAddressInfo(this string ip)
        {
            ip.MatchInetAddress(out var isIpAddress);
            if (isIpAddress)
            {
                var address = await GetPhysicsAddressInfo(ip);
                if (address.Status == 0)
                {
                    string detail = $"{address.AddressResult.FormattedAddress} {address.AddressResult.AddressComponent.Direction}{address.AddressResult.AddressComponent.Distance ?? "0"}米";
                    List<string> pois = address.AddressResult.Pois.Select(p => $"{p.AddressDetail}{p.Name} {p.Direction}{p.Distance ?? "0"}米").ToList();
                    return new Tuple<string, List<string>>(detail, pois);
                }
                return new Tuple<string, List<string>>("IP地址不正确", new List<string>());
            }
            return new Tuple<string, List<string>>($"{ip}不是一个合法的IP地址", new List<string>());
        }

        /// <summary>
        /// 根据IP地址获取详细地理信息对象
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static async Task<PhysicsAddress> GetPhysicsAddressInfo(this string ip)
        {
            if (!ip.MatchInetAddress())
            {
                return null;
            }

            string ak = AppSetting.Configuration["BaiduAK"];
            if (string.IsNullOrEmpty(ak))
            {
                throw new Exception("未配置BaiduAK，请先在您的应用程序appsettings.json中下添加BaiduAK配置节(注意大小写)");
            }

            using var client = new HttpClient() { BaseAddress = new Uri("http://api.map.baidu.com") };
            client.DefaultRequestHeaders.Referrer = new Uri("http://lbsyun.baidu.com/jsdemo.htm");
            var task = client.GetAsync($"/location/ip?ak={ak}&ip={ip}&coor=bd09ll").ContinueWith(async t =>
            {
                if (t.IsFaulted || t.IsCanceled)
                {
                    return null;
                }
                var res = await t;
                if (!res.IsSuccessStatusCode)
                {
                    return null;
                }

                var ipAddress = JsonConvert.DeserializeObject<BaiduIP>(await res.Content.ReadAsStringAsync());
                if (ipAddress.Status == 0)
                {
                    var point = ipAddress.AddressInfo.LatiLongitude;
                    var result = client.GetStringAsync($"/geocoder/v2/?location={point.Y},{point.X}&output=json&pois=1&radius=1000&latest_admin=1&coordtype=bd09ll&ak={ak}").Result;
                    var address = JsonConvert.DeserializeObject<PhysicsAddress>(result);
                    if (address.Status == 0)
                    {
                        return address;
                    }
                }
                else
                {
                    using var client2 = new HttpClient { BaseAddress = new Uri("http://ip.taobao.com") };
                    return await await client2.GetAsync($"/service/getIpInfo.php?ip={ip}").ContinueWith(async tt =>
                    {
                        if (tt.IsFaulted || tt.IsCanceled)
                        {
                            return null;
                        }
                        var result = await tt;
                        if (!result.IsSuccessStatusCode)
                        {
                            return null;
                        }

                        var taobaoIp = JsonConvert.DeserializeObject<TaobaoIP>(await result.Content.ReadAsStringAsync());
                        if (taobaoIp.Code == 0)
                        {
                            return new PhysicsAddress()
                            {
                                Status = 0,
                                AddressResult = new AddressResult()
                                {
                                    FormattedAddress = taobaoIp.IpData.Country + taobaoIp.IpData.Region + taobaoIp.IpData.City,
                                    AddressComponent = new AddressComponent()
                                    {
                                        Province = taobaoIp.IpData.Region
                                    },
                                    Pois = new List<Pois>()
                                }
                            };
                        }
                        return null;
                    });
                }
                return null;
            });
            return await await task;
        }

        /// <summary>
        /// 根据IP地址获取ISP
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static string GetISP(this string ip)
        {
            if (!ip.MatchInetAddress())
            {
                return $"{ip}不是一个合法的IP";
            }

            using var client = new HttpClient { BaseAddress = new Uri("http://ip.taobao.com") };
            var task = client.GetAsync($"/service/getIpInfo.php?ip={ip}").ContinueWith(async t =>
            {
                if (t.IsFaulted)
                {
                    return $"未能找到{ip}的ISP信息";
                }
                var result = await t;
                if (result.IsSuccessStatusCode)
                {
                    var taobaoIp = JsonConvert.DeserializeObject<TaobaoIP>(await result.Content.ReadAsStringAsync());
                    if (taobaoIp.Code == 0)
                    {
                        return taobaoIp.IpData.Isp;
                    }
                }
                return $"未能找到{ip}的ISP信息";
            });
            return task.Result.Result;
        }

        #endregion

        /// <summary>
        /// 获取IP4地址
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static string ToIPv4String(this IPAddress address)
        {
            var ipv4Address = (address ?? IPAddress.IPv6Loopback).ToString();
            return ipv4Address.StartsWith("::ffff:") ? address.MapToIPv4().ToString() : ipv4Address;
        }

        /// <summary>
        /// 判断IP地址在不在某个IP地址段
        /// </summary>
        /// <param name="input">需要判断的IP地址</param>
        /// <param name="begin">起始地址</param>
        /// <param name="ends">结束地址</param>
        /// <returns></returns>
        public static bool IpAddressInRange(this string input, string begin, string ends)
        {
            uint current = IPToID(input);
            return current >= IPToID(begin) && current <= IPToID(ends);
        }

        /// <summary>
        /// IP地址转换成数字
        /// </summary>
        /// <param name="addr">IP地址</param>
        /// <returns>数字,输入无效IP地址返回0</returns>
        private static uint IPToID(string addr)
        {
            if (!IPAddress.TryParse(addr, out var ip))
            {
                return 0;
            }

            byte[] bInt = ip.GetAddressBytes();
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bInt);
            }

            return BitConverter.ToUInt32(bInt, 0);
        }

        /// <summary>
        /// 判断IP是否是私有地址
        /// </summary>
        /// <param name="myIPAddress"></param>
        /// <returns></returns>
        public static bool IsPrivateIP(this IPAddress myIPAddress)
        {
            if (IPAddress.IsLoopback(myIPAddress)) return true;
            if (myIPAddress.AddressFamily == AddressFamily.InterNetwork)
            {
                byte[] ipBytes = myIPAddress.GetAddressBytes();
                // 10.0.0.0/24 
                if (ipBytes[0] == 10)
                {
                    return true;
                }
                // 169.254.0.0/16
                if (ipBytes[0] == 169 && ipBytes[1] == 254)
                {
                    return true;
                }
                // 172.16.0.0/16
                if (ipBytes[0] == 172 && ipBytes[1] == 16)
                {
                    return true;
                }
                // 192.168.0.0/16
                if (ipBytes[0] == 192 && ipBytes[1] == 168)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 判断IP是否是私有地址
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsPrivateIP(this string ip)
        {
            if (MatchExtensions.MatchInetAddress(ip))
            {
                return IsPrivateIP(IPAddress.Parse(ip));
            }
            throw new ArgumentException(ip + "不是一个合法的ip地址");
        }

        /// <summary>
        /// 判断url是否是外部地址
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool IsExternalAddress(this string url)
        {
            var uri = new Uri(url);
            switch (uri.HostNameType)
            {
                case UriHostNameType.Dns:
                    var ipHostEntry = Dns.GetHostEntry(uri.DnsSafeHost);
                    if (ipHostEntry.AddressList.Where(ipAddress => ipAddress.AddressFamily == AddressFamily.InterNetwork).Any(ipAddress => !ipAddress.IsPrivateIP()))
                    {
                        return true;
                    }
                    break;
                case UriHostNameType.IPv4:
                    return !IPAddress.Parse(uri.DnsSafeHost).IsPrivateIP();
            }
            return false;
        }


    }
}
