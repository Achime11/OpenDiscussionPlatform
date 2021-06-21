using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SecondWebApplication.Helpers {
    public class BreadCrumbItem {
        public string name { get; set; }
        public string path { get; set; }
        public Boolean last_item { get; set; } = false;
    }
}