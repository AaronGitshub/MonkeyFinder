using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;

using System.Linq;
using MonkeyFinder.Model;
using System.Diagnostics;
using System.Collections.ObjectModel;
using MvvmHelpers;
using System.Windows.Input;

namespace MonkeyFinder.ViewModel
{
    public class MonkeysViewModel : BaseViewModel
    {
       public ObservableRangeCollection<Monkey> Monkeys { get; }

        public Command GetMonkeysCommand { get; }
        public Command GetClosestCommand { get; }

        public MonkeysViewModel()
        {
            Monkeys = new ObservableRangeCollection<Monkey>();
            Title = "Monkey Finder";
            GetMonkeysCommand = new Command(async () => await GetMonkeysAsync());
        }

        async Task GetClosestAsync()
        {
            //Check if I have monkeys
            if (IsBusy || Monkeys.Count == 0)
                return;

            try
            {
                // Get cached location, else get real location.
                var location = await Geolocation.GetLastKnownLocationAsync();
                if (location == null)
                {
                    location = await Geolocation.GetLocationAsync(new GeolocationRequest
                    {
                        DesiredAccuracy = GeolocationAccuracy.Medium,
                        Timeout = TimeSpan.FromSeconds(30)
                    });
                }

                // Find closest monkey to us
                var first = Monkeys.OrderBy(m => location.CalculateDistance(
                    new Location(m.Latitude, m.Longitude), DistanceUnits.Miles))
                    .FirstOrDefault();

                await Application.Current.MainPage.DisplayAlert("", first.Name + " " +
                    first.Location, "OK");

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to query location: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error!", ex.Message, "OK");
            }
        }

        async Task GetMonkeysAsync()
        {
            if (IsBusy)
                return;
            try
            {
                IsBusy = true;
                var monkeys = await DataService.GetMonkeysAsync();
                Monkeys.ReplaceRange(monkeys);

                Title = $"Monkey Finder ({Monkeys.Count})";
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to get monkeys: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error!", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}



