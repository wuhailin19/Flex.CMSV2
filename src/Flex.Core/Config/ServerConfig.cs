using Flex.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Core.Config
{
    public class ServerConfig
    {
        public static string ImageServerUrl => "ImageServerUrl".Config(string.Empty);
        public static string FileServerUrl => "FileServerUrl".Config(string.Empty);
        public static string ServerUrl => "ServerUrl".Config(string.Empty);
        public static double SignalRExpiryTime => "SignalRConfig:ExpiryTime".Config<double>();
        public static double HeartbeatDelay => "SignalRConfig:HeartbeatDelay".Config<double>();
        public static string Content_Security_Policy=> "SecurityConfig:Content-Security-Policy".Config(string.Empty);
        public static string X_XSS_Protection=> "SecurityConfig:X-XSS-Protection".Config(string.Empty);
        public static string Strict_Transport_Security=> "SecurityConfig:Strict-Transport-Security".Config(string.Empty);
        public static string X_Content_Type_Options=> "SecurityConfig:X-Content-Type-Options".Config(string.Empty);
        public static string X_Permitted_Cross_Domain_Policies=> "SecurityConfig:X-Permitted-Cross-Domain-Policies".Config(string.Empty);
        public static string X_Frame_Options => "SecurityConfig:X-Frame-Options".Config(string.Empty);
        public static string X_Download_Options => "SecurityConfig:X-Download-Options".Config(string.Empty);
        public static string Referrer_Policy => "SecurityConfig:Referrer-Policy".Config(string.Empty);
    }
}
