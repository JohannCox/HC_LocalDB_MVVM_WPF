using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC_LocalDB_MVVM_WPF.Models
{
    public class PersonContext: DbContext
    {
        public PersonContext() : base("name=PersonContext")
        {
        }

        public virtual DbSet<Person> People { get; set; }

    }
}
