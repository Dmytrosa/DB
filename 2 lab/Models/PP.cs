using System;
using System.Collections.Generic;

namespace DB_lab2
{
    public partial class PP
    {
        public int Id { get; set; }
        public int PoisonId { get; set; }
        public int PoisonerId { get; set; }

        public virtual Poison Poison { get; set; } = null!;
        public virtual Poisoner Poisoner { get; set; } = null!;
    }
}
