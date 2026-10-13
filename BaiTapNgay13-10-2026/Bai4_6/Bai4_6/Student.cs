
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Bai4_6
{
    public class Student : INotifyPropertyChanged
    {
        private string _studentId = "";
        private string _fullName = "";
        private int _age;
        private string _gender = "";
        private string _avatarPath = "";

        public string StudentId
        {
            get { return _studentId; }
            set
            {
                if (_studentId != value)
                {
                    _studentId = value;
                    OnPropertyChanged("StudentId");
                }
            }
        }

        public string FullName
        {
            get { return _fullName; }
            set
            {
                if (_fullName != value)
                {
                    _fullName = value;
                    OnPropertyChanged("FullName");
                }
            }
        }

        public int Age
        {
            get { return _age; }
            set
            {
                if (_age != value)
                {
                    _age = value;
                    OnPropertyChanged("Age");
                }
            }
        }

        public string Gender
        {
            get { return _gender; }
            set
            {
                if (_gender != value)
                {
                    _gender = value;
                    OnPropertyChanged("Gender");
                }
            }
        }

        public string AvatarPath
        {
            get { return _avatarPath; }
            set
            {
                if (_avatarPath != value)
                {
                    _avatarPath = value;
                    OnPropertyChanged("AvatarPath");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this,
                    new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
