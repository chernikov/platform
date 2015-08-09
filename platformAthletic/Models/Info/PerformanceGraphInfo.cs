using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace platformAthletic.Models.Info
{
    public class PerformanceGraphInfo
    {
        public string label {get; set;}
        public string fillColor {get; set;}
        public string strokeColor {get; set;}
        public string pointColor {get; set;}
        public string pointStrokeColor {get; set;}
        public string pointHighlightFill {get; set;}
        public string pointHighlightStroke {get; set;}

        public bool datasetFill { get; set; }

        public List<int?> data {get; set;}
    }
}