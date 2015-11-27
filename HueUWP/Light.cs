using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BindingToCommandsUWP
{
    public class Light : INotifyPropertyChanged
    {
        public int ID { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }

        private string checkedInDateTime;

        public string CheckedInDateTime
        {
            get { return checkedInDateTime; }
            set
            {
                checkedInDateTime = value;
                NotifyPropertyChanged(nameof(CheckedInDateTime));
            }
        }


 
        public Light()
        {
 
        }

        public void CheckInCar()
        {
            this.CheckedInDateTime = String.Format("{0:t}", DateTime.Now);
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }

    public class LightDataSource
    {
        private static ObservableCollection<Light> _cars
            = new ObservableCollection<Light>();

        public static ObservableCollection<Light> GetCars()
        {
            if (_cars.Count == 0)
            {
                _cars.Add(new Light() { ID = 1, Make = "Tesla", Model = "Model S" });
                _cars.Add(new Light() { ID = 2, Make = "Tesla", Model = "Model 3" });
                _cars.Add(new Light() { ID = 3, Make = "Tesla", Model = "Model X" });
            }
            return _cars;
        }

        //public static void CheckInCar(Car car)
        //{
        //    _cars.FirstOrDefault(p => p.ID == car.ID).CheckedInDateTime = String.Format("{0:t}", DateTime.Now);
        //}
    }


   

}
