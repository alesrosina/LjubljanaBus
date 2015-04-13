using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace LjubljanaBus
{
    public class LinesViewModel : INotifyPropertyChanged
    {
         private string _id;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public string ID
        {
            get
            {
                return _id;
            }
            set
            {
                if (value != _id)
                {
                    _id = value;
                    NotifyPropertyChanged("ID");
                }
            }
        }

        private string _idNumber;

        public string IDNumberOnly
        {
            get
            {
                return _idNumber;
            }
            set
            {
                if (value != _idNumber)
                {
                    _idNumber = value;
                    NotifyPropertyChanged("IDNumberOnly");
                }
            }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                if (value != _name)
                {
                    _name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }

        private string _times;

        public string Times
        {
            get { return _times; }
            set
            {
                if (value != _times)
                {
                    _times = value;
                    NotifyPropertyChanged("Times");
                }
            }
        }

        //private Color _color;

        public string LineColor
        {
            get { return ConvertLineToColor(this.IDNumberOnly).ToString(); }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        private Color ConvertLineToColor(string no)
        {
            int nobus = 0 ;
            int.TryParse(no, out nobus);
            Color tmp;

            switch (nobus)
            {
                case 0:
                    tmp = Color.FromArgb(255, 0, 0, 0);
                    break;
                case 1:
                    tmp = Color.FromArgb(255, 171, 51, 46);
                    break;
                case 2:
                    tmp = Color.FromArgb(255, 115, 120, 56);
                    break;
                case 3:
                    tmp = Color.FromArgb(255, 229, 89, 61);
                    break;
                case 5:
                    tmp = Color.FromArgb(255, 140, 74, 148);
                    break;
                case 6:
                    tmp = Color.FromArgb(255, 186, 191, 195);
                    break;
                case 7:
                    tmp = Color.FromArgb(255, 0, 171, 214);
                    break;
                case 8:
                    tmp = Color.FromArgb(255, 46, 79, 148);
                    break;
                case 9:
                    tmp = Color.FromArgb(255, 128, 153, 191);
                    break;
                case 11:
                    tmp = Color.FromArgb(255, 219, 186, 59);
                    break;
                case 12:
                    tmp = Color.FromArgb(255, 46, 48, 145);
                    break;
                case 13:
                    tmp = Color.FromArgb(255, 181, 204, 82);
                    break;
                case 14:
                    tmp = Color.FromArgb(255, 232, 23, 135);
                    break;
                case 18:
                    tmp = Color.FromArgb(255, 107, 74, 38);
                    break;
                case 19:
                    tmp = Color.FromArgb(255, 229, 148, 178);
                    break;
                case 20:
                    tmp = Color.FromArgb(255, 0, 105, 64);
                    break;
                case 21:
                    tmp = Color.FromArgb(255, 0, 156, 84);
                    break;
                case 22:
                    tmp = Color.FromArgb(255, 237, 161, 56);
                    break;
                case 23:
                    tmp = Color.FromArgb(255, 204, 140, 92);
                    break;
                case 24:
                    tmp = Color.FromArgb(255, 204, 36, 46);
                    break;
                case 25:
                    tmp = Color.FromArgb(255, 0, 125, 181);
                    break;
                case 27:
                    tmp = Color.FromArgb(255, 229, 51, 125);
                    break;
                case 28:
                    tmp = Color.FromArgb(255, 0, 173, 240);
                    break;
                case 29:
                    tmp = Color.FromArgb(255, 237, 28, 36);
                    break;

                default:
                    tmp = Color.FromArgb(255, 0, 0, 0);
                    break;
            }

            return tmp;

        }
    }
}