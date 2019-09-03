using MvvmDialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC_LocalDB_MVVM_WPF.ViewModels
{
    class AboutViewModel : ViewModelBase, IModalDialogViewModel
    {
        public bool? DialogResult { get { return false; } }

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
    }
}
