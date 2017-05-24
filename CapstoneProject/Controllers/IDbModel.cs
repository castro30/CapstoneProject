using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapstoneProject.Models;

namespace CapstoneProject.Controllers
{
    interface IDbModel
    {
        List<Person> GetAllPeople();
    }
}
