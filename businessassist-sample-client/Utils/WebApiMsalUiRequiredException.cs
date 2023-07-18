using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessAssistAPIClient.Utils
{
    public class WebApiMsalUiRequiredException:Exception
    {
        public WebApiMsalUiRequiredException(string message) : base(message) { }
    }
}
