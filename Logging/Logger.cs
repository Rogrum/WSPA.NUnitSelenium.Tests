using System;
using System.IO;
using log4net;
using log4net.Config;
using NUnit.Framework;

namespace WPSF.NUnitSelenium.Tests.Logging
{
    public static class Logger
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(Logger));
        private static bool _configured = false;

        public static void EnsureConfigured()
        {
            if (_configured) return;
            var fi = new FileInfo(Path.Combine(TestContext.CurrentContext.WorkDirectory, "log4net.config"));
            if (fi.Exists) XmlConfigurator.Configure(fi);
            Directory.CreateDirectory(Path.Combine(TestContext.CurrentContext.WorkDirectory, "artifacts", "logs"));
            _configured = true;
        }

        public static void Info(string msg) { EnsureConfigured(); _log.Info(msg); }
        public static void Error(string msg, Exception? ex=null) { EnsureConfigured(); _log.Error(msg, ex); }
        public static void Debug(string msg) { EnsureConfigured(); _log.Debug(msg); }
        public static void Warn(string msg) { EnsureConfigured(); _log.Warn(msg); }
    }
}
