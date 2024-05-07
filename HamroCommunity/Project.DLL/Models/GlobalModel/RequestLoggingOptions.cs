using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DLL.Models.GlobalModel
{
    public class RequestLoggingOptions
    {
        public LogEventLevel LogLevel { get; set; } = LogEventLevel.Information;
        public bool IncludeHeader { get; set; } = false;
        public bool IncludeQueryString { get; set; } = false;
        public bool IncludeResponseBody { get; set; } = false;
        public bool IncludeRequestBody { get; set; } = false;

        //we can include more properties as needed
    }
}
