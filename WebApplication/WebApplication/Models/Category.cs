using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication.Models {
    public class Category {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Numele categoriei este obligatoriu!")]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public virtual ICollection<Discussion> Discussions { get; set; }
    }
}