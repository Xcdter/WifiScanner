using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WifiScanner.Models;
using Windows.Devices.WiFi;

namespace WifiScanner.Services
{
    public class WiFiService
    {
        public async Task<List<WifiNetwork>> ScanAvailableNetworksAsync()
        {
            var wifiNetworks = new List<WifiNetwork>();

            var access = await WiFiAdapter.RequestAccessAsync();
            if (access != WiFiAccessStatus.Allowed)
            {
                throw new UnauthorizedAccessException("WiFi access denied.");
            }

            var wifiAdapters = await WiFiAdapter.FindAllAdaptersAsync();
            if (wifiAdapters.Count == 0)
            {
                throw new InvalidOperationException("No WiFi adapters found on this device.");
            }

            var adapter = wifiAdapters[0];
            await adapter.ScanAsync();

            foreach (var network in adapter.NetworkReport.AvailableNetworks)
            {
                wifiNetworks.Add(new WifiNetwork
                {
                    SSID = network.Ssid,
                    BSSID = network.Bssid,
                    SignalStrength = (int)network.NetworkRssiInDecibelMilliwatts
                });
            }

            return wifiNetworks;
        }
    }
}
