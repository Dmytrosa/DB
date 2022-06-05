using System;
using System.Collections.Generic;

namespace DB_lab2
{
    public partial class Herbalist
    {
        public Herbalist()
        {
            HPs = new HashSet<HP>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<HP> HPs { get; set; }
    }
}
