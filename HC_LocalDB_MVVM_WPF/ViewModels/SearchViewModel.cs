using HC_LocalDB_MVVM_WPF.Models;
using HC_LocalDB_MVVM_WPF.Services;
using MvvmDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace HC_LocalDB_MVVM_WPF.ViewModels
{
    class SearchViewModel : ViewModelBase, IModalDialogViewModel
    {
        //public PropertyChangedEventHandler PropertyChanged;
        public bool? DialogResult { get { return false; } }

        public int MaximumRows { get; set; }

        private int pageNum = 1;
        public int PageNum
        {
            get
            {
                return this.pageNum;
            }
            set
            {
                if (this.PageNum > 0 || !this.PageNum.Equals(value))
                {
                    this.pageNum = value;

                    LoadFilteredData(SearchBoxed);
                }
            }
        }
        public int StartRowIndex
        {
            get
            {
                return ((PageNum * MaximumRows) - MaximumRows) + 1;
            }
        }

        private string searchBoxed;

        public string SearchBoxed
        {
            get
            {
                return this.searchBoxed;
            }
            set
            {
                if (this.SearchBoxed == null || !this.SearchBoxed.Equals(value))
                {
                    this.searchBoxed = value;

                    LoadFilteredData(value);
                }
            }
        }

        private string totalCount;

        public string TotalCount
        {
            get
            {
                return String.Format(" of {0}" , this.totalCount?.ToString() ?? "0");
            }
            set
            {
                if (String.IsNullOrEmpty(this.TotalCount) || !this.TotalCount.Equals(value.ToString()))
                {
                    this.totalCount = value.ToString();
                }
            }
        }

        private string filterCount;

        public string FilteredCount
        {
            get
            {
                return this.filterCount?.ToString() ?? "0";
            }
            set
            {
                if (String.IsNullOrEmpty(this.FilteredCount)|| !this.FilteredCount.Equals(value.ToString()))
                {
                    this.filterCount = value.ToString();
                }
            }
        } 

        public string Content
        {
            get
            {
                var thanks = String.IsNullOrEmpty(Environment.UserName)   ? "" :String.Format("Thank you {0} for your review.", Environment.UserName);

                return "HC_LocalDB_MVVM_WPF" + Environment.NewLine +
                        "Created by Johann Cox  -  2019" + Environment.NewLine +
                        "(864)804-8046" + Environment.NewLine +
                        Environment.NewLine +
                        "Templates, Libraries and Frameworks:" + Environment.NewLine +
                        "WPFApp with MVVM, Entity Framework v6.2, Log4Net v2.0.8, Extended.WPF.Toolkit v3.0.0, MvvmDialogs v4.0.0" +
                         Environment.NewLine + thanks; 
                        
            }
        }

        public string VersionText
        {
            get
            {
                var version1 = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

                // For external assemblies
                // var ver2 = typeof(Assembly1.ClassOfAssembly1).Assembly.GetName().Version;
                // var ver3 = typeof(Assembly2.ClassOfAssembly2).Assembly.GetName().Version;

                return "HC_LocalDB_MVVM_WPF v" + version1.ToString();
            }
        }

        public Image PersonImage
        {
            get
            {
                return null;
            }
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged += new PropertyChangedEventHandler(_PropertyChanged);
        }

        private void _PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private ObservableCollection<Person> people;

        public ObservableCollection<Person> Peoples
        {
            get
            {
                return people;
            }
            set
            {
                people = value;
                RaisePropertyChanged("People");
            }


        }

        private void RaisePropertyChanged(string propertyName)
        {
        /*
         * Can implement Events -> Event Handlers here
         */
        }
        public SearchViewModel()
        {
            PageNum = 1;
            MaximumRows = 10;
            LoadData();
        }

        private void LoadData()
        {
            var repo = new PersonRepository();
            
            LoadFilteredData("");
            totalCount = repo.GetTotalDbRows().ToString();
            //var getPeople = new PersonRepository();
            //Peoples = new ObservableCollection<Person>(getPeople.GetPeopleByPages(PageNum,MaximumRows, SearchBoxed));
            //this.TotalCount =  String.Format(" of {0}", Peoples.Count());
        }

        private void LoadFilteredData(string filter)
        {
            
            var db = new PersonContext();
            var getPeople = new PersonRepository();

            if (!String.IsNullOrEmpty(filter))
            {
                Peoples = new ObservableCollection<Person>(getPeople.GetPeopleByPages(StartRowIndex, MaximumRows, SearchBoxed));
                this.TotalCount = String.Format(" of {0}", Peoples.Count());

                //Peoples = new ObservableCollection<Person>(db.People.Where(p => (p.LastName.ToLower().Contains(SearchBoxed.ToLower())) || (p.FirstName.ToLower().Contains(SearchBoxed.ToLower()))).ToList<Person>());
                //FilteredCount = db.People.Count(p => (p.LastName.ToLower().Contains(SearchBoxed.ToLower())) || (p.FirstName.ToLower().Contains(SearchBoxed.ToLower()))).ToString();
            } 
            else
            {
                Peoples = new ObservableCollection<Person>(getPeople.GetPeopleByPages(StartRowIndex, MaximumRows, SearchBoxed));
                this.TotalCount = String.Format(" of {0}", Peoples.Count());

                FilteredCount = Peoples.Count().ToString();

                // FilteredCount = db.People.Count().ToString();
            }

            //Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate { this.UpdateLayout(); }));

        }
    }
       
    
}
