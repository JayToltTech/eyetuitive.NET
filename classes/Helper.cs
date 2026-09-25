using System;
using System.Management;
using System.Runtime.InteropServices;
using Google.Protobuf;
using Microsoft.Extensions.Logging;

namespace eyetuitive.NET.classes
{
    /// <summary>
    /// General helper class for various utility functions
    /// </summary>
    internal static class Helper
    {
        /// <summary>
        /// Convert a ByteString to a Guid
        /// </summary>
        /// <param name="byteString"></param>
        /// <returns></returns>
        internal static Guid FromByteString(ByteString byteString)
        {
            if (byteString == null || byteString.IsEmpty) return Guid.Empty;
            try
            {
                return new Guid(byteString.ToByteArray());
            }
            catch (Exception) { }
            return Guid.Empty;
        }

        /// <summary>
        /// Check if the current platform is Windows
        /// </summary>
        /// <returns></returns>
        internal static bool IsWindowsPlatform()
        {
#if NET6_0_OR_GREATER
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
#else
        // For .NET Framework, assume Windows
        return Environment.OSVersion.Platform == PlatformID.Win32NT;
#endif
        }

        private static readonly TimeSpan HostQueryTimeout = TimeSpan.FromSeconds(5);
        // Set only by a successful query; a failure is retried on the next call.
        private static volatile Tuple<string, string> _hostInfo;

        /// <summary>
        /// Get the computer manufacturer and model information
        /// </summary>
        /// <returns></returns>
        public static (string Manufacturer, string Model) GetComputerVendorAndModel()
        {
            if(!IsWindowsPlatform()) return ("Unknown", "Unknown"); //Check if running on Windows platform, if not, return unknown values

            var cached = _hostInfo;
            if (cached != null) return (cached.Item1, cached.Item2);

            try
            {
                var options = new EnumerationOptions { Timeout = HostQueryTimeout };
                using (var searcher = new ManagementObjectSearcher(null, "SELECT Manufacturer, Model FROM Win32_ComputerSystem", options))
                {
                    foreach (var obj in searcher.Get())
                    {
                        string manufacturer = obj["Manufacturer"]?.ToString() ?? "Unknown";
                        string model = obj["Model"]?.ToString() ?? "Unknown";
                        _hostInfo = Tuple.Create(manufacturer, model);
                        return (manufacturer, model);
                    }
                }
            }
            catch (Exception ex)
            {
                GazeFirst.eyetuitive._logger?.LogWarning(ex, "Failed to query computer vendor and model");
            }

            return ("Unknown", "Unknown");
        }
    }
}
