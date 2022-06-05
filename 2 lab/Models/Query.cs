using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DB_lab2.Models
{
    public class Query
    {
        public string QueryId { get; set; }

        public string Error { get; set; }
        public int ErrorFlag { get; set; }
        public string HerbalistName { get; set; }

        public string PoisonName { get; set; }

        [Required(ErrorMessage = "Поле не повинне бути порожнім")]
        [Display(Name = "Рік")]
       
        public int BirthDate { get; set; }

        [Required(ErrorMessage = "Поле не повинне бути порожнім")]
        [Range(0, 2021, ErrorMessage = "Недопустиме значення")]
        public int CountOfPoisons { get; set; }
        public string PoisonerName { get; set; }

        public List<string> PoisonersNames { get; set; }
        public List<string> AddressesNames { get; set; }
       public List<string> HerbalistsNames { get; set; }
       public List<string> PoisonsNames { get; set; }
        public List<int> PoisonersBirthDates { get; set; }
        public List<string> FilmsDescription { get; set; }
/*        public List<string> HerbalistsNames { get; internal set; }
*/    }
}