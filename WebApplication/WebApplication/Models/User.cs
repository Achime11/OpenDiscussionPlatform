using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication.Models {
    public enum Privilege {
        NORMAL_USER,ADMIN
    }

    public class User {
        [Key]
        public int Id { get; set; }
        [Required]
        public String FirstName { get; set; }
        [Required]
        public String LastName { get; set; }
        [Required]
        public Privilege Status { get; set; }
        [Required]
        public String Email { get; set; }   // Email is required for login
        [Required]
        public String Password { get; set; }

        public virtual ICollection<Discussion> Discussions { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}