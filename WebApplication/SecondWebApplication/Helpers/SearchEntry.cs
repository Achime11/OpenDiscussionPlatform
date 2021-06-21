using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SecondWebApplication.Helpers {
    public class SearchEntry : IComparable<SearchEntry> {
        public string link { get; set; }
        public string path { get; set; }
        public string extra { get; set; }
        public int similarity { get; set; }

        public int CompareTo(SearchEntry other) {
            return this.similarity.CompareTo(other.similarity);
        }
    }
}