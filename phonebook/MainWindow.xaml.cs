using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace phonebook
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Sqlite.Init();
        }

        private void PhoneList_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemChanged)
            {
                
                Sqlite.Save(e.NewIndex);
            }
            if(e.ListChangedType == ListChangedType.ItemAdded)
            {
                Sqlite.Create(e.NewIndex,Sqlite.PhoneList[e.NewIndex].Number,
                    Sqlite.PhoneList[e.NewIndex].FullName,
                    Sqlite.PhoneList[e.NewIndex].Address);
            }
            Debug.WriteLine(e.NewIndex);
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            phoneGrid.ItemsSource = Sqlite.PhoneList;
            Sqlite.PhoneList.ListChanged += PhoneList_ListChanged;
            Sqlite.PhoneList.BeforeRemove += PhoneList_BeforeRemove;
        }

        private void PhoneList_BeforeRemove(Phone RemovedItem)
        {
            Sqlite.Delete(RemovedItem.ID);
        }

        private void phoneGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


        }

        private void phoneGrid_KeyDown(object sender, KeyEventArgs e)
        {

        }
        List<Phone> filter = new List<Phone>();
        private void TextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                filter.Clear();
                filter.AddRange(Sqlite.PhoneList.Where(text => text.Number.Contains(txt.Text)).ToList());
                filter.AddRange(Sqlite.PhoneList.Where(text => text.FullName.Contains(txt.Text)).ToList());
                filter.AddRange(Sqlite.PhoneList.Where(text => text.Address.Contains(txt.Text)).ToList());
                phoneGrid.ItemsSource = filter;
                if (txt.Text.Length == 0) phoneGrid.ItemsSource = Sqlite.PhoneList;
            }
            catch
            {

            }
        }
    }
    public class bindinglist<Phone> : BindingList<Phone>
    {
        protected override void RemoveItem(int index)
        {
            Phone deleteitem = this.Items[index];

            if(BeforeRemove != null)
            {
                BeforeRemove(deleteitem);
            }
            base.RemoveItem(index);
        }

        public delegate void RemoveItemInt(Phone RemovedItem);
        public event RemoveItemInt BeforeRemove;
    }

}
