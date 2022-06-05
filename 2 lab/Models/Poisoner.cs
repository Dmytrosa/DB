using System;
using System.Collections.Generic;

namespace DB_lab2
{
    public partial class Poisoner
    {
        public Poisoner()
        {
            Herbs_Ps = new HashSet<Herb_P>();
            HPs = new HashSet<HP>();
            PPs = new HashSet<PP>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int AddressId { get; set; }
        public int BirthDate { get; set; }
        public string Info { get; set; } = null!;

        public virtual Address Address { get; set; } = null!;
        public virtual ICollection<HP> HPs { get; set; }
        public virtual ICollection<PP> PPs { get; set; }
        public virtual ICollection<Herb_P> Herbs_Ps { get; set; }
    }
}
