using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HC_LocalDB_MVVM_WPF.Models;

namespace HC_LocalDB_MVVM_WPF.Services
{
    public interface IPersonRepository : IDisposable
    {
        // Task<Person> AddPersonAsync(Person person);
        List<Person> GetPeople();
        PagedRecInfo GetPeopleByPages(int pageNum, int maximumRows, string filter);
        int GetTotalDbRows();
        // Task<List<Person>> RemovePersonAsync(int PersonId);
        // Task<Person> UpdatePersonAsync(Person dude);
    }
}