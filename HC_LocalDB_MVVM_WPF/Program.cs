using HC_LocalDB_MVVM_WPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC_LocalDB_MVVM_WPF
{
    public class Program
    {
        static byte[] imageBytes { get; set; }

        public static void Main(string[] args)
        {
            DoDB();
        }
        static void ImageStreamer()
        {
            byte[] imageStream;
            imageStream = System.IO.File.ReadAllBytes(@"./DB/me.jpg");
            imageBytes = imageStream;
        }

        public static void DoDB()
        {
            ImageStreamer();

          //  var dbConnection = Configuration.GetConnectionString("DBConn");


            using (var db = new PersonContext())
            {
                var customer = new Person
                {
                    LastName = "Cox",
                    FirstName = "Johann",
                    Age = 30,
                    Address = "200 Heywood Ave, Spartanburg, SC 29304",
                    Interests = "Hiking, Biking, Cloud Watching",
                    ImagesBytes = imageBytes
                };
                if (db.People.Count() >= 1)
                {
                    // string tablename = "Customers";
                    FormattableString sql = $"delete from Customers;";

                    db.Database.ExecuteSqlCommand(sql.ToString());
                    //db.Customers.RemoveRange(db.Customers);
                    db.SaveChanges();
                }
                db.People.Add(customer);
                db.SaveChangesAsync();
            }
        }

    }
}
