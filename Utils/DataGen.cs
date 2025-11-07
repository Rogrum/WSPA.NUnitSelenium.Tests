using System;
namespace WPSF.NUnitSelenium.Tests.Utils
{
    public static class DataGen
    {
        public static string UniqueEmail(string prefix="autotest", string domain="example.com")
        {
            var stamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
            return $"{prefix}+{stamp}@{domain}";
        }
    }
}
