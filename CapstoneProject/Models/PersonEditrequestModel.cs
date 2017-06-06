using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CapstoneProject.Models
{
    public class PersonEditrequestModel
    {
        public PersonEditrequestModel()
        { }
        public PersonEditrequestModel(RegisterViewModel person)
        {
            
            this.PhoneNumber = person.PhoneNumber;
           this.Email = person.Email;
           this.MarriedDate = person.MarriedDate;
           /*this.BirthDate = person.BirthDate;
           this.UserName = person.UserName;
           */this.CurrentLocation = person.CurrentLocation;
        }

        [Required]
        public string PersonName { get; set; }

        [Required]
        public System.DateTime BirthDate { get; set; }

        public Nullable<System.DateTime> MarriedDate { get; set; }

        public Nullable<System.DateTime> ExpDate { get; set; }

        [Required]
        public string CurrentLocation { get; set; }

        [Required]
        public string Email { get; set; }

        public Nullable<long> PhoneNumber { get; set; }

        public Person AsPerson(long id, string Username)
        {
            Person missing = new Person();
           // missing.PhoneNumber = this.PhoneNumber;
            missing.PersonName = this.PersonName;
            //missing.Email = this.Email;
           // missing.MarriedDate = this.MarriedDate;
            missing.ExpDate = this.ExpDate;
            //missing.BirthDate = this.BirthDate;
           // missing.CurrentLocation = this.CurrentLocation;
            missing.PersonID = id;
            //missing.Username = Username;
            return missing;

        }
    }
}