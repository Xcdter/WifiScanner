using Microsoft.Data.Sqlite;
using System.IO;
using System.Transactions;
using WifiScanner.Models;

namespace WifiScanner.Repository
{
    public class WiFiDbContext
    {
        private static readonly string ProjectPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;

        private static readonly string DbPath = Path.Combine(ProjectPath, "WiFiNetworks.db");

        private static readonly string ConnectionString = $"Data Source={DbPath}";

        public WiFiDbContext()
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
            CREATE TABLE IF NOT EXISTS WiFiNetworks (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                SSID TEXT NOT NULL,
                BSSID TEXT NOT NULL,
                SignalStrength INTEGER NOT NULL
            );";
            command.ExecuteNonQuery();
        }

        public async Task SaveNetworksAsync(IEnumerable<WifiNetwork> networks)
        {
            using var connection = new SqliteConnection(ConnectionString);
            await connection.OpenAsync();

            foreach (var network in networks)
            {
                var command = connection.CreateCommand();
                command.CommandText = @"
                INSERT INTO WiFiNetworks (SSID, BSSID, SignalStrength)
                VALUES ($ssid, $bssid, $signalStrength);";

                command.Parameters.AddWithValue("$ssid", network.SSID);
                command.Parameters.AddWithValue("$bssid", network.BSSID);
                command.Parameters.AddWithValue("$signalStrength", network.SignalStrength);

                await command.ExecuteNonQueryAsync();
            }
        }
    }
}
