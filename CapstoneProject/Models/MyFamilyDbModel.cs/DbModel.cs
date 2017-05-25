using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CapstoneProject.Controllers;
using CapstoneProject.Models;

namespace CapstoneProject.Models.MyFamilyDbModel
{
    public class DbModel : IDbModel
    {
#region Example Get All People
        public List<Person> GetAllPeople()
        {
            using (var db = new FamilyProjectEntities4())
            {
                var x = db.People.ToList();


                return x;
            }
        }
#endregion
        public void AddPerson(Person person)
        {
            using (var db = new FamilyProjectEntities4())
            {
                db.People.Add(person);
                db.SaveChanges();
            }
            
        }
    }
}