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
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace HC_LocalDB_MVVM_WPF.ViewModels
{
    class SearchViewModel : ViewModelBase, IModalDialogViewModel
    {
        //public PropertyChangedEventHandler PropertyChanged;
        public bool? DialogResult { get { return false; } }

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

                   // this.NotifyPropertyChanged(() => this.SearchBox);
                }
            }
        }

        public string Content
        {
            get
            {
                return "HC_LocalDB_MVVM_WPF" + Environment.NewLine +
                        "Created by JCOX" + Environment.NewLine +
                        "Address" + Environment.NewLine +
                        "2019";
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

        //public ImageSource Photo
        //{
        //    get
        //    {
        //        BitmapImage biImg = new BitmapImage();
        //        MemoryStream ms = new MemoryStream();
        //        biImg.BeginInit();
        //        biImg.StreamSource = ms;
        //        biImg.EndInit();

        //        ImageSource imgSrc = biImg as ImageSource;


        //        return imgSrc;
        //    }
        //}

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged += new PropertyChangedEventHandler(_PropertyChanged); // .Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void _PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private ObservableCollection<Person> people;
        //private PersonRepository repo;

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
            //if (PropertyChanged != null)
            //{
            //    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            //}
        }
        public SearchViewModel()
        {
            LoadData();
        }

        private void LoadData()
        {
            var getPeople = new PersonRepository();
            Peoples = new ObservableCollection<Person>(getPeople.GetPeople());
        }

        private void LoadFilteredData(string filter)
        {
            var getPeople = new PersonRepository();
            Peoples = new ObservableCollection<Person>((getPeople.GetPeople()).Where(
                n => n.LastName.ToLowerInvariant().Contains(filter.ToLowerInvariant())
                || n.FirstName.ToLowerInvariant().Contains(filter.ToLowerInvariant())
                ).ToList());
           
            
            //Peoples = new ObservableCollection<Person>(Peoples.Where(
            //    n=> n.LastName.ToLowerInvariant().Contains(filter.ToLowerInvariant())
            //    || n.FirstName.ToLowerInvariant().Contains(filter.ToLowerInvariant())
                
            //    ).ToList());
        }
    }
       
    
}
