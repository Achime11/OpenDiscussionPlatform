using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication.Models {
    public class Discussion {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Titlu articolului este obligatoriu!")]
        [StringLength(100, ErrorMessage = "Titlu nu poate avea mai mult de 100 de caractere!")]
        public String Title { get; set; }

        [Required(ErrorMessage = "Continutul discutiei este obligatoriu!")]
        [DataType(DataType.MultilineText)]
        public String Text { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        public Category Category { get; set; }
        [Required(ErrorMessage = "Categoria este obligatorie!")]
        public int CategoryId { get; set; }

        public User User { get; set; }
        public int UserId { get; set; } // Faptul ca aici e definit un int il face Required

        public virtual ICollection<Comment> Comments { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
    }
}