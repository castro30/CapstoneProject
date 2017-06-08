using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CapstoneProject.Controllers;
using CapstoneProject.Models;
using System.Data.Entity;

namespace CapstoneProject.Models.MyFamilyDbModel
{
    public class DbModel : IDbModel, IDisposable
    {
        private FamilyProjectEntities5 db;

        #region Example Get All People
        public List<Person> GetAllPeople()
        {
                var x = db.People.ToList();
                return x;
        }

        public List<RelationshipKey> GetRelations()
        {
            var x = db.RelationshipKeys.ToList();
            return x;
        }
        #endregion

        public DbModel()
        {
            db = new FamilyProjectEntities5 ();
        }

        public void AddPerson(Person per, tbl_RegisteredUsers person)
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

        public void AddPersonToFamily(Person per, Family family)
        {
            if (per != null)
            {
                db.People.Add(per);
            }
            if (family != null)
            {
                db.Families.Add(family);
            }


            db.SaveChanges();
        }

            public void Dispose()
        { Dispose(true); }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                db?.Dispose();
                db = null;

            }
        }

        #region Profile
        public RegisterViewModel RegisteredProfile(string UserName) //RegisteredUser Detail
        {
            
                var reg =  db.tbl_RegisteredUsers.SingleOrDefault(x => x.UserName == UserName);
                var exchange = new RegisterViewModel(reg);
                return exchange;
            
        }
        

        public long GetCurrentUserId(string user) //RegisteredUser Detail
        {

            var reg = db.tbl_RegisteredUsers.SingleOrDefault(x => x.UserName == user);
            return reg.PersonID;
        }

        /*  public RegisterViewModel ImmediateRegisteredProfile(string username)
          {
                  var newreg = db.tbl_RegisteredUsers.SingleOrDefault(x => x.UserName == UserName);


          }*/

        public Person PersonProfile(int id)
         {
                 return db.People.FirstOrDefault(x => x.PersonID == id );
         }
        #endregion
        
        /*public Person NewPerson(int id)
        {
            var get = db.People.FirstOrDefault(x => x.PersonID == id);

            return Person;
        }
        */
        public tbl_RegisteredUsers GetRegisterUser(string user)
        {
                return db.tbl_RegisteredUsers
                       .FirstOrDefault(x => x.UserName == user);
        }

        public IQueryable<RegisterViewModel> GetImmediateFamily(long personID)
        {

                /*var personID = db.tbl_RegisteredUsers
                    .First(x => x.UserName == user)
                    .PersonID;*/

                
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
                                RID = rel.RID,
                                Email = ru.Email,
                                ExpDate = p.ExpDate,
                                MarriedDate = ru.MarriedDate
                            })
                            ;

                return list;
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetNextFamilyId()
        {
            return (int)db.Families.Max(x=>x.ID) + 1;
        }


        public void EditPerson(RegisterViewModel person)
        {
                db.Entry(person).State = EntityState.Modified;
                db.SaveChanges();
        }

        public void CreatePerson(RegisterViewModel person)
        {
                db.Entry(person).State = EntityState.Added;
                db.SaveChanges();
        }


      /*  public List<Person> FamilyMember(string relation, string searchString)
        {
            var person = dbm.People.AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                person = person.Where(s => s.PersonName.Contains(searchString));
            }
            
            return person.ToList();
        }*/
    }
}
