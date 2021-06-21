using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SecondWebApplication.Models {
    public class Discussion {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "The title of the article is necessary!")]
        [StringLength(100, ErrorMessage = "The title cannot have more than 100 characters!")]
        public String Title { get; set; }

        [Required(ErrorMessage = "The body of the discussion is necessary!")]
        [DataType(DataType.MultilineText)]
        public String Text { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        public Category Category { get; set; }
        // Astfe de erori trebuie puse pe id si nu obiect deoarece cand primim raspunsul de la browser ni se da doar id-ul populat din forma si nu un obiect format
        [Required(ErrorMessage = "A category selection is needed!")]       
        public int CategoryId { get; set; }

        
        public ApplicationUser User { get; set; }
        [Required]
        public string UserId { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
    }
}