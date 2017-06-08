using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapstoneProject.Models
{
    public class FamilyPersonModel
    {
        public List<SelectListItem> People { get; set; }
        public List<SelectListItem> Relation { get; set; }

        [DisplayName("Person")]
        public long PersonId { get; set; }

        [DisplayName("Relationship")]
        public short RID { get; set; }

    }
}