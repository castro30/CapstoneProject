using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CapstoneProject.Models.NewMemberCheck
{
    public class IdentityCheck
    {
        public bool UserNameCheck(AspNetUser UserName)
        {
            tbl_RegisteredUsers db = new tbl_RegisteredUsers();
            string username = db.UserName;
            if (username == db.UserName)
            { return true; }
            return false;
        }

        //Does person exist
        public bool PersonExist(tbl_RegisteredUsers RegisteredUser)
        {
            RegisterViewModel db = new RegisterViewModel();
            string Name = RegisteredUser.Person.PersonName;
            string pername = db.PersonName;
            if (Name == pername)
            {
                //Create a person with this Name==pername 
                //Compare RID to verify right person 
                return true;
            }
            else {
                //Set PersonID to this personID
                return false;
            }
            
        }

    }
}