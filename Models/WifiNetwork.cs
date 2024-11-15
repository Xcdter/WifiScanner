using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WifiScanner.Models
{
    public class WifiNetwork
    {
        public string SSID { get; set; }
        public string BSSID { get; set; }
        public int SignalStrength { get; set; }
    }
}
