using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using WifiScanner.Commands;
using WifiScanner.Models;
using WifiScanner.Repository;
using WifiScanner.Services;

namespace WifiScanner.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly WiFiService _wifiService;
        private readonly WiFiDbContext _dbContext;

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
        public ICommand SaveCommand { get; }

        public MainViewModel()
        {
            _wifiService = new WiFiService();
            _dbContext = new WiFiDbContext();

            Networks = new ObservableCollection<WifiNetwork>();
            ScanCommand = new RelayCommand(async () => await ScanNetworksAsync());
            SaveCommand = new RelayCommand(async () => await SaveNetworksToDatabase());

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

        private async Task SaveNetworksToDatabase()
        {
            if (Networks.Count == 0)
            {
                System.Windows.MessageBox.Show("No networks to save.");
                return;
            }

            await _dbContext.SaveNetworksAsync(Networks);
            System.Windows.MessageBox.Show("Networks saved to database.");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
