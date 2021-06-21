using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SecondWebApplication.Models {
    public class Category {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="The name of the category is imperative!")]
        [StringLength(100, ErrorMessage = "The title cannot be longer than 100 characters!")]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public virtual ICollection<Discussion> Discussions { get; set; }
    }
}