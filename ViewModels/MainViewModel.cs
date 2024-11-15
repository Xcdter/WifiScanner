using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using WifiScanner.Commands;
using WifiScanner.Models;
using WifiScanner.Services;

namespace WifiScanner.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly WiFiService _wifiService;

        private ObservableCollection<WifiNetwork> _networks;

        public ObservableCollection<WifiNetwork> Networks
        {
            get => _networks;
            set
            {
                _networks = value;
                OnPropertyChanged();
            }
        }

        private string _bestNetwork;

        public string BestNetwork
        {
            get => _bestNetwork;

            set
            {
                _bestNetwork = value;
                OnPropertyChanged();
            }
        }

        public ICommand ScanCommand { get; }

        public MainViewModel()
        {
            _wifiService = new WiFiService();
            Networks = new ObservableCollection<WifiNetwork>();
            ScanCommand = new RelayCommand(async () => await ScanNetworksAsync());
        }

        private async Task ScanNetworksAsync()
        {
            var networks = await _wifiService.ScanAvailableNetworksAsync();
            Networks.Clear();

            if (networks.Count > 0)
            {
                foreach (var network in networks.OrderByDescending(n => n.SignalStrength))
                {
                    Networks.Add(network);
                }

                BestNetwork = Networks[0].SSID;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
