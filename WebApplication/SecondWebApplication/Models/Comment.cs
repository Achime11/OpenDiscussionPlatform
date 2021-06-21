using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SecondWebApplication.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "The comment body is necessary!")]
        [StringLength(500, ErrorMessage = "The comment body cannot have more than 500 characters!")]
        [DataType(DataType.MultilineText)]
        public String Content { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public Discussion Discussion { get; set; }
        [Required(ErrorMessage = "The selection of a discussion field is necessary!")]
        public int DiscussionId { get; set; }

        
        public ApplicationUser User { get; set; }
        //[Required] Can't use required because cascadeDelete will be disabled
        public string UserId { get; set; }

        // Used for dropdown helper
        public IEnumerable<SelectListItem> Discussions { get; set; }

    }
}