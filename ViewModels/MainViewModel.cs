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
            foreach (var network in networks)
            {
                Networks.Add(network);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
