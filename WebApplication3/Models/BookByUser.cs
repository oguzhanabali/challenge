using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WebApplication3.Models
{
    public class BookByUser
    {
        [Key]
        public int BookByUserId { get; set; }
        public int BookId { get; set; }
        [StringLength(128)]
        public string Users_Id { get; set; }
        public DateTime? ShipDate { get; set; }
        public DateTime? IssueDate { get; set; }
        public virtual Book Library { get; set; }
    }
}