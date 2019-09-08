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
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        PersonContext _context = new PersonContext();
        public List<Person> GetPeople()
        {
            return _context.People.ToList<Person>();
        }

        /// <summary>
        /// pageNum refers to the Page of rows requested, maximuRows refers to how many rows will be passed back into the Collection
        /// </summary>
        /// <param name="pageNum"></param>
        /// <param name="maximumRows"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public PagedRecInfo GetPeopleByPages(int startRowIndex, int maximumRows, string filter)
        {
            FormattableString sql = $"";
            FormattableString sqlTotalRows = $"";

            if (!String.IsNullOrEmpty(filter))
            {
                sql = $"select * FROM(SELECT ROW_NUMBER() OVER (ORDER BY Id) row_num, Id, LastName,FirstName, Age, [Address], Interests, ImagesBytes from People where LastName LIKE '%{filter}%' OR FirstName LIKE '%{filter}%') t Where row_num >= {startRowIndex} and row_num<({startRowIndex} + {maximumRows})";

                sqlTotalRows = $"select * FROM People where LastName LIKE '%{filter}%' OR FirstName LIKE '%{filter}%'";
            }
            else
            {
                sql = $"select * FROM(SELECT ROW_NUMBER() OVER (ORDER BY Id) row_num, Id, LastName,FirstName, Age, [Address], Interests, ImagesBytes from People) t Where row_num >= {startRowIndex} and row_num<({startRowIndex} + {maximumRows})";

                sqlTotalRows = $"select * FROM People";
            }

            int total = 0;
        
            total = (_context.People.SqlQuery(sqlTotalRows.ToString())).Count();

            PagedRecInfo pgInfo = new PagedRecInfo()
            {
                PagedFilteredRecords = (_context.People.SqlQuery(sql.ToString())).ToList<Person>(),
                TotalFilterRecords = total
            };

            return pgInfo;
        }

        public int GetTotalDbRows()
        {
            return _context.People.Count();
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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
