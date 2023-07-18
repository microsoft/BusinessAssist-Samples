using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;

namespace BusinessAssistAPIClient.Models
{
    public class SelfHelpSolutions
    {
        public string? Title { get; set; }
        public string? Details { get; set; }
        public double? SolutionConfidence { get; set; }
    }

    public class SelfHelpWebSearchResults
    {
        public string? Title { get; set; }
        public string? Details { get; set; }
        public string? Url { get; set; }
    }

    public class SelfHelpResults
    {
        public List<SelfHelpSolutions> Solutions { get; set; }
        public List<SelfHelpWebSearchResults> WebSearchResults { get; set; }

    }

    public class BASelfHelpModel
    {
        public string QueryText { get; set; }

        public string Status { get; set; }

        public string Response { get; set; }

        public SelfHelpResults Results { get; set; }

    }
}
