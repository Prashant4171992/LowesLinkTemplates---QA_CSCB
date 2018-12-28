using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LowesLinkTemplates.Models
{
    /// <summary>
    /// properties for capturing wiki page's content & other metadata
    /// </summary>
    public class LLMain
    {
        public string PageName { get; set; }
        public string Content { get; set; }
        public string urlVal { get; set; }
        public string urlType { get; set; }
        public Dictionary<int,string> urlDictProp { get; set; }
    }
    /// <summary>
    /// properties for capturing Errors
    /// </summary>
    public class LLMainErr
    {
        public string Error { get; set; }
    }
}