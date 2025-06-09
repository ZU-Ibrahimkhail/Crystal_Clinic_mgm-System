using Microsoft.AspNetCore.Mvc;
using System.Net.NetworkInformation;

namespace Crystal_Clinic_Mgm.UI.Controllers.General
{
    [Route("api/[controller]")]
    [ApiController]
    public class IPandMACAddressController : ControllerBase
    {
        public class IpAddressModel
        {
            public string ServerIP { get; set; } = string.Empty;
            public string ServerMACAddress { get; set; } = string.Empty;
            public string ClientIP { get; set; } = string.Empty;
            public string ClientMACAddress { get; set; } = string.Empty;
            public bool NetworkAvailable { get; set; } = false;
        }

        [HttpGet("GetIPandMACAddress")]
        public IpAddressModel IPandMAC()
        {
            var ipMode = new IpAddressModel
            {
                ServerIP = IpAddress(),
                ServerMACAddress = GetServerMACAddress(),
                ClientIP = GetClientIp(),
                ClientMACAddress = "",
                NetworkAvailable = checkInternet()
            };
            return ipMode;
        }

        private string GetClientIp()
        {
            return HttpContext.Connection.RemoteIpAddress!.MapToIPv4().ToString();
        }
        private string IpAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"]!;
            else
            {
                System.Net.IPAddress ipaddres =
                    System.Net.Dns.GetHostEntry("").AddressList.Where(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).First();
                return ipaddres.ToString();
            }
        }

        private bool checkInternet()
        {
            return NetworkInterface.GetIsNetworkAvailable();
        }
        private string GetServerMACAddress()
        {
            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            //---for Ethernet
            var physicalInterface = networkInterfaces.FirstOrDefault(i => i.NetworkInterfaceType == NetworkInterfaceType.Ethernet);
            if (physicalInterface == null)
            {//---for Wireless
                networkInterfaces.FirstOrDefault(i => i.NetworkInterfaceType == NetworkInterfaceType.Wireless80211);
            }
            return physicalInterface != null ? GetFormattedMACAddress(physicalInterface.GetPhysicalAddress()) : string.Empty;
        }
        private string GetFormattedMACAddress(PhysicalAddress physicalAddress)
        {
            byte[] bytes = physicalAddress.GetAddressBytes();
            string macAddress = string.Join(":", bytes.Select(b => b.ToString("X2")));
            return macAddress;
        }


    }

}
