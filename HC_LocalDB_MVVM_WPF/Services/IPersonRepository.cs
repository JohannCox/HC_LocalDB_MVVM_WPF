using System.Collections.Generic;
using System.Threading.Tasks;
using HC_LocalDB_MVVM_WPF.Models;

namespace HC_LocalDB_MVVM_WPF.Services
{
    public interface IPersonRepository
    {
        // Task<Person> AddPersonAsync(Person person);
        List<Person> GetPeople();
        // Task<List<Person>> RemovePersonAsync(int PersonId);
       // Task<Person> UpdatePersonAsync(Person dude);
    }
}