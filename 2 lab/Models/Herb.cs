using System;
using System.Collections.Generic;

namespace DB_lab2
{
    public partial class Herb
    {
        public Herb()
        {
            Herbs_Ps = new HashSet<Herb_P>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Herb_P> Herbs_Ps { get; set; }
    }
}
