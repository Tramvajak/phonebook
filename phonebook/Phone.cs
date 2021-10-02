using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace phonebook
{
    class Phone: INotifyPropertyChanged
    {

        private int _id;
        private string _Number;
        private string _FullName;
        private string _Address;

        public Phone()
        {

        }

        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Number
        {
            get { return _Number; }
            set 
            {
                if (_Number == value)
                    return;
                _Number = value;
                OnPropertyChanged();
            }
        }
        public string FullName
        {
            get { return _FullName; }
            set 
            {
                if (_FullName == value)
                    return;
                _FullName = value;
                OnPropertyChanged();
            }
        }
        public string Address
        {
            get { return _Address; }
            set 
            {
                if (_Address == value)
                    return;
                _Address = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Edit"));
        }
    }  
}
