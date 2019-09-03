using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC_LocalDB_MVVM_WPF.Models
{
    public class Person
    {
        public int id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
        public string Interests { get; set; }
        public byte[] ImagesBytes { get; set; }

    }
}
