using System;
using System.Collections.Generic;

namespace DB_lab2
{
    public partial class Poison
    {
        public Poison()
        {
            PPs = new HashSet<PP>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<PP> PPs { get; set; }
    }
}
