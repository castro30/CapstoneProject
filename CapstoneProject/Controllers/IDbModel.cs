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
        List<RegisterViewModel> GetImmediateFamily(string user);
    }
}
