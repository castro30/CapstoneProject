using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapstoneProject.Models;

namespace CapstoneProject.Controllers
{
    public interface IDbModel
    {
            void AddPerson(Person per, tbl_RegisteredUsers person);
            void EditPerson(RegisterViewModel person);
            List<Person> GetAllPeople();
           // Person PersonProfile(string UserName);
            RegisterViewModel RegisteredProfile(string UserName);
        IQueryable<RegisterViewModel> GetImmediateFamily(long personID);
        Person PersonProfile(int id);
        tbl_RegisteredUsers GetRegisterUser(string user);
        int GetNextFamilyId();
        void AddPersonToFamily(Person per, Family family);
        List<RelationshipKey> GetRelations();
        long GetCurrentUserId(string user);
    }
}
