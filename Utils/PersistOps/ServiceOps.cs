using System.Text;
using System.ServiceProcess;


namespace PersistAssist.Utils
{
    public class ServiceOps
    {
        public static string CreateService()
        {
            return "";
        }

        public static string RemoveService()
        {
            return "";
        }

        public static string ModifyService()
        {
            return "";
        }

        public static string ListServices()
        {

            StringBuilder outSvc = new StringBuilder();

            ServiceController[] services = ServiceController.GetServices();

            int svcSNLen = getMaxSNLen(services);
            int svcDNLen = getMaxDNLen(services);

            outSvc.AppendLine($"{"ServiceName".Align(svcSNLen)}  {"DisplayName".Align(svcDNLen)} {"Status"}");
            outSvc.AppendLine($"{"----------".Align(svcSNLen)}  {"------------".Align(svcDNLen)} {"------"}");

            foreach (ServiceController service in services)
            {
                outSvc.AppendLine($"{service.ServiceName.Align(svcSNLen)}  {service.DisplayName.Align(svcDNLen)} {service.Status}");
            }

            return outSvc.ToString();
        }

        public static string ListServices(string svcName)
        {

            StringBuilder outSvc = new StringBuilder();

            ServiceController[] services = ServiceController.GetServices();

            int svcSNLen = getMaxSNLen(services);
            int svcDNLen = getMaxDNLen(services);

            outSvc.AppendLine($"{"ServiceName".Align(svcSNLen)}  {"DisplayName".Align(svcDNLen)} {"Status"}");
            outSvc.AppendLine($"{"----------".Align(svcSNLen)}  {"------------".Align(svcDNLen)} {"------"}");

            foreach (ServiceController service in services)
            {
                if (service.ServiceName == svcName)
                {
                    outSvc.AppendLine($"{service.ServiceName.Align(svcSNLen)}  {service.DisplayName.Align(svcDNLen)} {service.Status}");
                }
            }

            return outSvc.ToString();
        }

        public static string ListServices(string svcName, string svcStatus)
        {

            StringBuilder outSvc = new StringBuilder();

            ServiceController[] services = ServiceController.GetServices();

            int svcSNLen = getMaxSNLen(services);
            int svcDNLen = getMaxDNLen(services);

            outSvc.AppendLine($"{"ServiceName".Align(svcSNLen)}  {"DisplayName".Align(svcDNLen)} {"Status"}");
            outSvc.AppendLine($"{"----------".Align(svcSNLen)}  {"------------".Align(svcDNLen)} {"------"}");

            foreach (ServiceController service in services)
            {
                if (service.ServiceName == svcName && service.Status.ToString().ToLower() == svcStatus.ToLower())
                {
                    outSvc.AppendLine($"{service.ServiceName.Align(svcSNLen)}  {service.DisplayName.Align(svcDNLen)} {service.Status}");
                }
            }

            return outSvc.ToString();
        }

        public static string ServiceInfo(string svcName)
        {
            StringBuilder svcInfo = new StringBuilder();

            ServiceController[] svcs = ServiceController.GetServices();

            foreach (ServiceController svc in svcs)
            {
                if (svc.ServiceName == svcName)
                {
                    svcInfo.AppendLine(
                        $"ServiceName:  {svc.ServiceName}\n" +
                        $"DisplayName:  {svc.DisplayName}\n" +
                        $"Status:       {svc.Status}\n" +
                        $"ServiceType:  {svc.ServiceType}\n"
                        );
                }
            }

            return svcInfo.ToString();
        }

        private static int getMaxSNLen(ServiceController[] services)
        {
            var maxLen = 0;
            foreach (ServiceController service in services)
            {
                if (service.ServiceName.ToString().Length > maxLen) { maxLen = service.ServiceName.ToString().Length; }
            }

            return maxLen;
        }

        private static int getMaxDNLen(ServiceController[] services)
        {
            int maxLen = 0;
            foreach (ServiceController service in services)
            {
                if (service.DisplayName.Length > maxLen) { maxLen = service.DisplayName.Length; }
            }

            return maxLen;
        }
    }
}
