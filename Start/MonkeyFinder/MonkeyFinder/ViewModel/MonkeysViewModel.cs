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

        public MonkeysViewModel()
        {
            Monkeys = new ObservableRangeCollection<Monkey>();
            Title = "Monkey Finder";
            GetMonkeysCommand = new Command(async () => await GetMonkeysAsync());
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



