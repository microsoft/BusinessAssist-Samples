using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;

namespace BusinessAssistAPIClient.Models
{

    public class BAValidateTenantModel
    {
        public string? Token { get; set; }
        public string Status { get; set; }
        public string Response { get; set; }
        public object? JsonResponse { get; set; }
    }
}
