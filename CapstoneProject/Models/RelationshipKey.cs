//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CapstoneProject.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class RelationshipKey
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RelationshipKey()
        {
            this.Families = new HashSet<Family>();
        }
    
        public short RID { get; set; }
        public string Relationship { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Family> Families { get; set; }
        public virtual RelationshipKey RelationshipKey1 { get; set; }
        public virtual RelationshipKey RelationshipKey2 { get; set; }
    }
}
