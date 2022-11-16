using System.Text;
using System.Net.NetworkInformation;

namespace PersistAssist.Utils
{
    class Network
    {
        public static string IpConfig() {
            StringBuilder sb = new StringBuilder();

            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();

            if (interfaces == null) { return "[*] No network interfaces detected"; }

            foreach(NetworkInterface cInterface in interfaces) {
                IPInterfaceProperties cProps = cInterface.GetIPProperties();

                sb.AppendLine($"{cInterface.Name}");
                sb.AppendLine($" {nameof(cInterface.NetworkInterfaceType)} : {cInterface.NetworkInterfaceType}");
                sb.AppendLine($" {nameof(cInterface.OperationalStatus)} : {cInterface.OperationalStatus}");
                sb.AppendLine($" {"IP Address"} : {cProps.DnsAddresses[0]}");
                if (cProps.GatewayAddresses.Count != 0) { sb.AppendLine($" {"Gateway Address"} : {cProps.GatewayAddresses[0].Address}"); }
                if (!(cInterface.GetPhysicalAddress().GetAddressBytes().Length is 0)) { sb.AppendLine($" {"MAC Address"} : {cInterface.GetPhysicalAddress()}"); }
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
