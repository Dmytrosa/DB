using System;
using System.Collections.Generic;

namespace DB_lab2
{
    public partial class Herb_P
    {
        public int Id { get; set; }
        public int PoisonerId { get; set; }
        public int HerbId { get; set; }

        public virtual Herb Herb { get; set; } = null!;
        public virtual Poisoner Poisoner { get; set; } = null!;
    }
}
