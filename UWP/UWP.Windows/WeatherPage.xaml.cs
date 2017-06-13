using UWP.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json;
using Windows.Data.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using Windows.UI.Popups;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace UWP
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class WeatherPage : Page
    {

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }


        public WeatherPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
            lvLastUsedCities.ItemsSource = LastCitiesManager.getInst().cities;
        }

        public async void setTextOnTextBock(string city)
        {
            RESTClient rest = new RESTClient();
            string response;
            try
            {
                response = rest.getWeatherForLocation(city);
            }
            catch (HttpRequestException e)
            {
                String dialogStr = String.Format("City \"{0}\" doesn't exists", city).ToString();
                MessageDialog dialog = new MessageDialog(dialogStr);
                await dialog.ShowAsync();
                return;
            }
            WeatherInfo weatherInfo = JsonConvert.DeserializeObject<WeatherInfo>(response);

            tbCityValue.Text = weatherInfo.city;
            tbWeatherValue.Text = weatherInfo.getWeatherDescription();
            tbTemperatureValue.Text = weatherInfo.getTemperatureAsString();
            tbHumidityValue.Text = weatherInfo.getHumidityAsString();
            tbWindValue.Text = weatherInfo.getWindAsString();

            LastCitiesManager.getInst().AddCity(new LastCity(city, DateTime.Now));
        }

        /// <summary>
        /// Populates the page with content passed during navigation. Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="Common.NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session. The state will be null the first time a page is visited.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="Common.SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="Common.NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="Common.NavigationHelper.LoadState"/>
        /// and <see cref="Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(tbCityName.Text))
                setTextOnTextBock(tbCityName.Text);
        }

        private void lvLastUsedCities_ItemClick(object sender, ItemClickEventArgs e)
        {
            LastCity clickedCity = e.ClickedItem as LastCity;
            setTextOnTextBock(clickedCity.name);
        }
    }
}
