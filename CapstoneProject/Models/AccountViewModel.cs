using System;
using System.ComponentModel.DataAnnotations;

namespace CapstoneProject.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }
    }

    public class ManageUserViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        private readonly object regusers;
        public RegisterViewModel()
        {

        }

        public RegisterViewModel(tbl_RegisteredUsers RegisteredUser)
        {
            
            Name = RegisteredUser.Person.PersonName;
            Id = RegisteredUser.PersonID;
            UserName = RegisteredUser.UserName;
            BirthDate = RegisteredUser.BirthDate;
            MarriedDate = RegisteredUser.MarriedDate;
            ExpDate = RegisteredUser.Person.ExpDate;
            CurrentLocation = RegisteredUser.CurrentLocation;
            Email = RegisteredUser.Email;
            PhoneNumber = RegisteredUser.PhoneNumber;
            PersonName = Name;
            
        

        }

        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Id")]
        public long Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name ="Full Name")]
        public string PersonName { get; set; }

        [Required]
        [Display(Name ="Birth Date")]
        public System.DateTime? BirthDate { get; set; }

        [Display(Name = "Married Date")]
        public Nullable<System.DateTime> MarriedDate { get; set; }

        [Display(Name ="Exp Date")]
        public Nullable<System.DateTime> ExpDate { get; set; }
        
        [Required]
        [Display(Name ="Location")]
        public string CurrentLocation { get; set; }

        
        [Display(Name = "Relationship")]
        public string Relationship { get; set; }

        [Required]
        [Display(Name ="Email")]
        public string Email { get; set; }

        [Display(Name ="Phone Number")]
        public Nullable<long> PhoneNumber { get; set; }

        
    }

    public class ImmediateFamilyViewModel
    {
        public ImmediateFamilyViewModel(Family PersonID,RegisterViewModel person)
        {

        }
    
        [Required]
        [Display(Name = "Full Name")]
        public string PersonName { get; set; }

        [Display(Name = "Exp Date")]
        public Nullable<System.DateTime> ExpDate { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$")]
        [Required]
        [StringLength(30)]
        public string Relation { get; set; }


    }
}

