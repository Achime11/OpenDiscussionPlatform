using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Comentariul este obligatoriu!")]
        [StringLength(500, ErrorMessage = "Comentariul nu poate avea mai mult de 500 de caractere!")]
        public String Content { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public Discussion Discussion { get; set; }
        [Required(ErrorMessage = "Selectia unei discutii este necesara!")]
        public int DiscussionId { get; set; }

        public User User { get; set; }
        public int UserId { get; set; }

        // Used for dropdown helper
        public IEnumerable<SelectListItem> Discussions { get; set; }

    }
}