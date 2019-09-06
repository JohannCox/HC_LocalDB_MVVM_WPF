using HC_LocalDB_MVVM_WPF.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC_LocalDB_MVVM_WPF.Services
{
    public class PersonRepository : IPersonRepository
    {
        PersonContext _context = new PersonContext();
        public List<Person> GetPeople()
        {
            return _context.People.ToList<Person>();
        }

        public async Task<Person> AddPersonAsync(Person person)
        {
            _context.People.Add(person);
            await _context.SaveChangesAsync();
            return person;
        }

        public async Task<List<Person>> RemovePersonAsync(int PersonId)
        {
            var person = _context.People.FirstOrDefault(p => p.id == PersonId);
            if (person != null)
            {
                _context.People.Remove(person);
            }
            await _context.SaveChangesAsync();

            return _context.People.ToList();
        }

        public async Task<Person> UpdatePersonAsync(Person dude)
        {
            if (!_context.People.Local.Any(p=>p.id == dude.id))
            {
                _context.People.Attach(dude);
            }

            _context.Entry(dude).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return dude;
        }



    }
}
