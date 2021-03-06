﻿using HC_LocalDB_MVVM_WPF.Models;
using HC_LocalDB_MVVM_WPF.Services;
using HC_LocalDB_MVVM_WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HC_LocalDB_MVVM_WPF.Views
{
    /// <summary>
    /// Interaction logic for Search.xaml
    /// </summary>
    public partial class Search : Window //, IPersonRepository
    {
        private SearchViewModel searchViewModel { get; set; }

        public Search()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            searchViewModel = DataContext as SearchViewModel;
        }

        public string MultiColumnFilter { get; set; } = "";

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var binding = ((TextBox)sender).GetBindingExpression(TextBox.TextProperty);
            MultiColumnFilter = ((TextBox)sender).Text;

            binding.UpdateSource();
          peopleDataGrid.Items.Refresh();

            //CollectionViewSource.GetDefaultView(counterTextBlock.Text = ((HC_LocalDB_MVVM_WPF.ViewModels.SearchViewModel)binding.DataItem).FilteredCount);

            CollectionViewSource.GetDefaultView(peopleDataGrid.ItemsSource).Refresh();
            UpdateDatagridSource();
        }

         private void CollectionViewSource_Filter(object sender, FilterEventArgs e)
        {
            
            Person t = e.Item as Person;
            if (t != null && MultiColumnFilter != "")
            {
                if (t.LastName.ToLowerInvariant().Contains(MultiColumnFilter.ToLowerInvariant()) || t.FirstName.ToLowerInvariant().Contains(MultiColumnFilter.ToLowerInvariant()))
                    e.Accepted = true;
                else
                    e.Accepted = false;
            }

        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void PeopleDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        public class ByteToImageConverter : IValueConverter
        {
            public BitmapImage ConvertByteArrayToBitMapImage(byte[] imageByteArray)
            {
                BitmapImage img = new BitmapImage();
                using (MemoryStream memStream = new MemoryStream(imageByteArray))
                {
                    img.StreamSource=memStream;
                }
                return img;
            }

            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                BitmapImage img = new BitmapImage();
                if (value != null)
                {
                    img = this.ConvertByteArrayToBitMapImage(value as byte[]);
                }
                return img;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return null;
            }

            object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }

            object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        private void ForwardButton_Click(object sender, RoutedEventArgs e)
        {
            SearchViewModel searchViewModel = this.DataContext as SearchViewModel;
            if (searchViewModel != null)
            {
                searchViewModel.PageNum++;

                UpdateDatagridSource();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (searchViewModel != null && searchViewModel.PageNum >= 2)
            {
                searchViewModel.PageNum--;
                UpdateDatagridSource();
            }
        }

        private void UpdateDatagridSource()
        {
            peopleDataGrid.ItemsSource = searchViewModel.Peoples;

            // PageNumber
            PageNumTextBlock1.Text = searchViewModel.PageNum.ToString();
            PageNumTextBlock2.Text = searchViewModel.PageNum.ToString();
            // Total Pages
            TotalCountTextBlock1.Text = searchViewModel.TotalCount.ToString();
            TotalCountTextBlock2.Text = searchViewModel.TotalCount.ToString();

            peopleDataGrid.Items.Refresh();
            CollectionViewSource.GetDefaultView(peopleDataGrid.ItemsSource).Refresh();
        }

        private void PeopleDataGrid_SourceUpdated(object sender, DataTransferEventArgs e)
        {

        }
    }
}
