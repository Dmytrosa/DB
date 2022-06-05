using System;
using System.Collections.Generic;

namespace DB_lab2
{
    public partial class Address
    {
        public Address()
        {
            Poisoners = new HashSet<Poisoner>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Poisoner> Poisoners { get; set; }
    }
}
