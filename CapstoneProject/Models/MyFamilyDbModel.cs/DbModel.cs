using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CapstoneProject.Controllers;
using CapstoneProject.Models;
using System.Data.Entity;

namespace CapstoneProject.Models.MyFamilyDbModel
{
    public class DbModel : IDbModel
    {
        private FamilyProjectEntities5 db;

        #region Example Get All People
        public List<Person> GetAllPeople()
        {
            using (var db = new FamilyProjectEntities5())
            {
                var x = db.People.ToList();
                return x;
            }
        }
        #endregion

        public void AddPerson(Person per, tbl_RegisteredUsers person)
        {
            using (var db = new FamilyProjectEntities5())
            {
                if (per != null)
                {
                    db.People.Add(per);
                }
                if (person != null)
                {
                    db.tbl_RegisteredUsers.Add(person);
                }
                
                
                db.SaveChanges();
            }
        }


        #region Profile
        public RegisterViewModel RegisteredProfile(string UserName) //RegisteredUser Detail
        {
            using (var db = new FamilyProjectEntities5())
            {
                var reg =  db.tbl_RegisteredUsers.SingleOrDefault(x => x.UserName == UserName);
                var exchange = new RegisterViewModel(reg);
                return exchange;
            }
        }

      /*  public RegisterViewModel ImmediateRegisteredProfile(string username)
        {
            using (var db = new FamilyProjectEntities5())
            {
                var newreg = db.tbl_RegisteredUsers.SingleOrDefault(x => x.UserName == UserName);

            }
        }*/

        /* public Person PersonProfile(string Username)
         {
             using (var db = new FamilyProjectEntities5())
             {
                 long id = from PersonID in db.tbl_RegisteredUsers
                          where (PersonID.UserName == Username)
                          select PersonID;

                 return db.People.FirstOrDefault(x => x.PersonID ==id );
             }
         }*/
        #endregion



        public List<RegisterViewModel> GetImmediateFamily(string user)
        {
            using (var db = new FamilyProjectEntities5())
            {
                List<RegisterViewModel> reg = new List<RegisterViewModel>();

                var personID = db.tbl_RegisteredUsers
                    .First(x => x.UserName == user)
                    .PersonID;

                
                var list = (from p in db.People
                            join ru in db.tbl_RegisteredUsers
                            on p.PersonID equals ru.PersonID
                            join ff in db.Families
                            on ru.PersonID equals ff.SecondPersonID
                            join rel in db.RelationshipKeys
                            on ff.RID equals rel.RID
                            where ff.PersonID == personID 
                            && ff.RID != 5 && ff.RID != 6
                            
                            select new RegisterViewModel
                            {
                                Id = p.PersonID,
                                PersonName = p.PersonName,
                                CurrentLocation = ru.CurrentLocation,
                                PhoneNumber = ru.PhoneNumber,
                                BirthDate = ru.BirthDate,
                                UserName = ru.UserName,
                                Relationship = rel.Relationship,
                                Email = ru.Email,
                                ExpDate = p.ExpDate,
                                MarriedDate = ru.MarriedDate
                            })
                            .ToList();

                return list;
            }
        }

        public void EditPerson(RegisterViewModel person)
        {
            using (var db = new FamilyProjectEntities5())
            {
                db.Entry(person).State = EntityState.Modified;
                db.SaveChanges();

            }
        }

        public void CreatePerson(RegisterViewModel person)
        {
            using (var db = new FamilyProjectEntities5())
            {
                db.Entry(person).State = EntityState.Added;
                db.SaveChanges();
            }
        }


        public List<Person> FamilyMember(string relation, string searchString)
        {
            var person = db.People.AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                person = person.Where(s => s.PersonName.Contains(searchString));
            }
            
            return person.ToList();
        }
    }
}
