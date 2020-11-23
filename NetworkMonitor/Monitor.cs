using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.Versioning;
using System.Timers;

namespace NetworkMonitor
{
	[SupportedOSPlatform("windows")]
	public class Monitor
	{
		public ArrayList Adapters { get; set; }
		public ArrayList AdaptersMonitored { get; set; }

		public Timer Timer { get; set; }

		public Monitor()
		{
			this.Adapters = new ArrayList();
			this.AdaptersMonitored = new ArrayList();

			LoopAdapters();

			this.Timer = new Timer(1000);
			this.Timer.Elapsed += new ElapsedEventHandler(Timer_Elapsed);
		}

		private void LoopAdapters()
		{
			PerformanceCounterCategory networkInterface = new PerformanceCounterCategory("Network Interface");
			foreach (string name in networkInterface.GetInstanceNames())
			{
				if (name.ToLower() == "ms tcp loopback interface")
					continue;

				Adapter adapter = new Adapter(name);
				adapter.DownloadCounter = new PerformanceCounter("Network Interface", "Bytes Received/sec", name);
				adapter.UploadCounter = new PerformanceCounter("Network Interface", "Bytes Sent/sec", name);
				this.Adapters.Add(adapter);
			}
		}

		private void Timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			foreach (Adapter a in this.AdaptersMonitored)
				a.Update();
		}

		public void Start()
		{
			if (this.Adapters.Count > 0)
			{
				foreach (Adapter a in this.Adapters)
				{
					if (!this.AdaptersMonitored.Contains(a))
					{
						this.AdaptersMonitored.Add(a);
						a.Initialize();
					}
				}
				this.Timer.Enabled = true;
			}
		}

		public void Start(Adapter adapter)
		{
			if (!this.AdaptersMonitored.Contains(adapter))
			{
				this.AdaptersMonitored.Add(adapter);
				adapter.Initialize();
			}
			this.Timer.Enabled = true;
		}

		public void Stop()
		{
			this.AdaptersMonitored.Clear();
			this.Timer.Enabled = false;
		}

		public void Stop(Adapter adapter)
		{
			if (this.AdaptersMonitored.Contains(adapter))
				this.AdaptersMonitored.Remove(adapter);

			if (this.AdaptersMonitored.Count == 0)
				this.Timer.Enabled = false;
		}
	}
}
