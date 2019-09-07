using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.Windows;
using System.ComponentModel;
using HC_LocalDB_MVVM_WPF.ViewModels;
using HC_LocalDB_MVVM_WPF.Views;
using HC_LocalDB_MVVM_WPF.Models;
using HC_LocalDB_MVVM_WPF.Services;

namespace HC_LocalDB_MVVM_WPF
{
    public partial class App : Application
    {
        private static readonly ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static MainWindow app;
        static byte[] imageBytes { get; set; }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Log.Info("Application Startup");

            // For catching Global uncaught exception
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(UnhandledExceptionOccured);

            Log.Info("Starting App");
            LogMachineDetails();
            app = new MainWindow();
            var context = new MainViewModel();
            app.DataContext = context;
            DoDB();

            app.Show();

            if (e.Args.Length == 1) //make sure an argument is passed
            {
                Log.Info("File type association: " + e.Args[0]);
                FileInfo file = new FileInfo(e.Args[0]);
                if (file.Exists) //make sure it's actually a file
                {
                    // Here, add you own code
                    // ((MainViewModel)app.DataContext).OpenFile(file.FullName);
                }
            }
        }

        static void UnhandledExceptionOccured(object sender, UnhandledExceptionEventArgs args)
        {
            // Here change path to the log.txt file
            var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
                                                    + "\\JCOX\\HC_LocalDB_MVVM_WPF\\log.txt";

            // Show a message before closing application
            var dialogService = new MvvmDialogs.DialogService();
            dialogService.ShowMessageBox((INotifyPropertyChanged)(app.DataContext),
                "A Unrecoverable Error has occured the application must close. Please review the " +
                "Log concerning the problem at: " + path + Environment.NewLine +
                "If the problem persist, please contact Support.",
                "Unhandled Error",
                MessageBoxButton.OK);

            Exception e = (Exception)args.ExceptionObject;
            Log.Fatal("Application has crashed", e);
        }

        private void LogMachineDetails()
        {
            var computer = new Microsoft.VisualBasic.Devices.ComputerInfo();

            string text = "OS: " + computer.OSPlatform + " v" + computer.OSVersion + Environment.NewLine +
                          computer.OSFullName + Environment.NewLine +
                          "RAM: " + computer.TotalPhysicalMemory.ToString() + Environment.NewLine +
                          "Language: " + computer.InstalledUICulture.EnglishName;
            Log.Info(text);
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
                List<Person> lsPerson = new List<Person>();
                lsPerson.AddRange(new[]
                {
                    new Person()
                    {
                        LastName = "Cox",
                        FirstName = "Johann",
                        Age = 30,
                        Address = "200 Heywood Ave, Spartanburg, SC 29304",
                        Interests = "Hiking, Biking, Cloud Watching",
                        ImagesBytes = imageBytes
                    },
                    new Person()
                    {
                        LastName = "Crane",
                        FirstName = "Miles",
                        Age = 30,
                        Address = "30 Marvin Ave, Spartanburg, SC 29304",
                        Interests = "Psy-Awards, NIMH",
                        ImagesBytes = null
                    },
                    new Person()
                    {
                      LastName = "Boggs",
                        FirstName = "Xavier",
                        Age = 21,
                        Address = "902 Westbury Rd, Boiling Springs, SC 29304",
                        Interests = "Weedeating, Mulching, Worm-husbandry",
                        ImagesBytes = imageBytes
                    },
                    new Person()
                    {
                        LastName = "Parnth",
                        FirstName = "Ivan",
                        Age = 45,
                        Address = "1402 Gypsy Wood Way, Greenville, SC 29316",
                        Interests = "Axe-throwing, Spas, and deep thoughts",
                        ImagesBytes = imageBytes
                    },
                    new Person()
                    {
                        LastName = "Zipster",
                        FirstName = "Ace",
                        Age = 99,
                        Address = "1 Barkley Circle, Spartanburg, SC 29304",
                        Interests = "Fast Cars, Fast Planes, Fiber",
                        ImagesBytes = imageBytes
                    },

                });

                if (db.People.Count<Person>() >= 6)
                {
                    var f = db.People.Count<Person>();
                    FormattableString sql = $"truncate table People;";
                    var b = db.Database.ExecuteSqlCommand(sql.ToString());
      
                    sql = $"select  top 10 * from People;";

                    List<Person> GetPeople =  db.People.SqlQuery(sql.ToString()).ToList<Person>();

                    if (!GetPeople.Any(p => p.FirstName.ToLowerInvariant() == "johann" ))
                    {
                        db.People.AddRange(lsPerson);
                        db.SaveChanges();
                    }
                }
                else
                {
                    var f = db.People.Count<Person>();
                    db.People.AddRange(lsPerson);
                    db.SaveChanges();
                    FormattableString sql = $"select  top 10 * from People;";
                    List<Person> GetPeople = db.People.SqlQuery(sql.ToString()).ToList<Person>();
                }
                int counter = 400;
                for (int i=0; i< counter; i++)
                {
                    db.People.AddRange(lsPerson);
                    db.SaveChanges();
                }
      
            }
        }

    }
}
