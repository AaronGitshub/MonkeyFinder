using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;

using System.Linq;
using MonkeyFinder.Model;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace MonkeyFinder.ViewModel
{
    public class MonkeysViewModel : BaseViewModel
    {
        public ObservableCollection<Monkey> Monkeys { get; }

       public MonkeysViewModel()
{
        Monkeys = new ObservableCollection<Monkey>();
            Title = "Monky Finder";
}
        async Task GetMonkeyAsync()
            {
            if (IsBusy)
                return;
            try
                {
}               IsBusy = true;
            var monkeys = await DataService.GetMonkeysAsync();
            Monkeys.Clear();
            foreach (var monkey in monkeys) Monkeys.Add(monkey);
}
    }
}
