using System;
using System.Collections.Generic;

namespace DB_lab2
{
    public partial class HP
    {
        public int Id { get; set; }
        public int PoisonerId { get; set; }
        public int HerbalistId { get; set; }

        public virtual Herbalist Herbalist { get; set; } = null!;
        public virtual Poisoner Poisoner { get; set; } = null!;
    }
}
