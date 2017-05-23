using System;
using CapstoneProject.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CapstoneProject.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string PersonName { get; set; }
        public DateTime BirthDate { get; set;}
        public Nullable<System.DateTime> MarriedDate { get; set; }
        public string CurrentLocation { get; set; }
        public string Email { get; set; }
        public Nullable<long> PhoneNumber { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("FamilyProject")
        {
        }
    }
}