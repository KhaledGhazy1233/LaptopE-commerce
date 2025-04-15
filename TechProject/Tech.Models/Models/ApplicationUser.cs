using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechProject.Models;
namespace TechProject.Models
{
    public class ApplicationUser :IdentityUser
    {
        [Required]

        public string ?City { get; set; }

        public string ? State { get; set; }
     
        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }
        public string? FirstName { get; set; }
      
        public string? LastName { get; set; }
        public string? Gender { get; set; }
 
       
        public string? Country { get; set; }
        public string? Street { get; set; }
    }
}
