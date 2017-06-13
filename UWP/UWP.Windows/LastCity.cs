using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace UWP
{
    class LastCitiesManager
    {
        public static LastCitiesManager instance = new LastCitiesManager();
        public static LastCitiesManager getInst() { return instance; }
        public ObservableCollection<LastCity> cities { get; set; }
        private string storageCollectionName = "lastCities";

        private LastCitiesManager()
        {
            cities = new ObservableCollection<LastCity>();
            LoadDataFromStorage();

        }

        ~LastCitiesManager()
        {
        }

        public void AddCity(LastCity city)
        {
            cities.Insert(0, city);
            SaveDataToStorage();
        }

        private async void SaveDataToStorage()
        {
            MemoryStream sessionData = new MemoryStream();
            DataContractSerializer serializer = new
                        DataContractSerializer(typeof(ObservableCollection<LastCity>));
            serializer.WriteObject(sessionData, cities);


            StorageFile file = await ApplicationData.Current.LocalFolder
                                     .CreateFileAsync(storageCollectionName, Windows.Storage.CreationCollisionOption.ReplaceExisting);
            using (Stream fileStream = await file.OpenStreamForWriteAsync())
            {
                sessionData.Seek(0, SeekOrigin.Begin);
                await sessionData.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
            }
        }

        private async void LoadDataFromStorage()
        {
            StorageFile file;
            try
            {
                file = await ApplicationData.Current.LocalFolder.
                               GetFileAsync(storageCollectionName);
            }
            catch (Exception e)
            {
                return;
            }
            using (IInputStream inStream = await file.OpenSequentialReadAsync())
            {
                DataContractSerializer serializer =
                        new DataContractSerializer(typeof(ObservableCollection<LastCity>));
                cities = (ObservableCollection<LastCity>)serializer
                                 .ReadObject(inStream.AsStreamForRead());
            }
        }
    }

    [DataContract]
    class LastCity
    {
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public DateTime date { get; set; }
        public LastCity(string name, DateTime date)
        {
            this.name = name;
            this.date = date;
        }
        public LastCity() //for serialization
        {

        }
    }
}
