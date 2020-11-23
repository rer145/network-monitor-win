using System;
using System.Collections;
using System.Runtime.Versioning;
using System.Threading;

namespace NetworkMonitor
{
	[SupportedOSPlatform("windows")]
	class Program
	{
		static void Main(string[] args)
		{
			Monitor monitor = new Monitor();
			ArrayList adapters = monitor.Adapters;

			if (adapters.Count == 0)
			{
				Console.WriteLine("No network adapters found.");
				return;
			}

			monitor.Start();

			for (int i = 0; i < 10; i++)
			{
				foreach (Adapter adapter in adapters)
				{
					if (adapter.DownloadSpeedKbps > 0.0 || adapter.UploadSpeedKbps > 0.0)
					{
						Console.WriteLine(adapter.Name);
						Console.WriteLine(String.Format("Download: {0}kbps / Upload: {1}kbps", Math.Round(adapter.DownloadSpeedKbps, 3).ToString(), Math.Round(adapter.UploadSpeedKbps, 3).ToString()));
					}
				}
				Console.WriteLine();
				Thread.Sleep(1000);
			}

			monitor.Stop();
		}
	}
}
