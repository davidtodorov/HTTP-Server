using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.HTTP.Common
{
    public static class GlobalConstants
    {
        public const string HTTpOneProtocolFragment = "HTTP/1.1";
        public const string HostHeaderKey = "Host";
        public const string HttpNewLine = "\r\n";
        public const string UnsupportedHttpRequestMethodException = "The Http Method - {0} is not supported.";
    }
}
