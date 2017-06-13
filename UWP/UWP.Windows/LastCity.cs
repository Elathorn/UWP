using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWP
{
    class LastCitiesManager
    {
        public static LastCitiesManager instance = new LastCitiesManager();
        public static LastCitiesManager getInst() { return instance; }
        public ObservableCollection<LastCity> cities { get; }

        private LastCitiesManager()
        {
            cities = new ObservableCollection<LastCity>();
        }

        public void AddCity(LastCity city)
        {
            cities.Insert(0, city);
        }
    }

    class LastCity
    {
        public string name { get; set; }
        public DateTime date { get; set; }
        public LastCity(string name, DateTime date)
        {
            this.name = name;
            this.date = date;
        }
    }
}
