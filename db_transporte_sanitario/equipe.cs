//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace db_transporte_sanitario
{
    using System;
    using System.Collections.Generic;
    
    public partial class equipe
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public equipe()
        {
            this.equipe_solam = new HashSet<equipe_solam>();
        }
    
        public int idEquipe { get; set; }
        public string Condutor { get; set; }
        public string Enfermeiros { get; set; }
        public System.DateTime DtEscala { get; set; }
        public Nullable<int> idAM { get; set; }
    
        public virtual ambulancia ambulancia { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<equipe_solam> equipe_solam { get; set; }
    }
}
