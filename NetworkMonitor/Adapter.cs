using System.Diagnostics;
using System.Runtime.Versioning;

namespace NetworkMonitor
{
	[SupportedOSPlatform("windows")]
	public class Adapter
	{
		public string Name { get; set; }

		public PerformanceCounter DownloadCounter { get; set; }
		public PerformanceCounter UploadCounter { get; set; }


		public long DownloadSpeed { get; set; }
		public long UploadSpeed { get; set; }
		public double DownloadSpeedKbps 
		{
			get { return this.DownloadSpeed / 1024.0; }
		}
		public double UploadSpeedKbps
		{
			get { return this.UploadSpeed / 1024.0; }
		}

		public Adapter(string name)
		{
			this.Name = name;
		}

		public override string ToString()
		{
			return this.Name;
		}


		private long _oldDownloadValue;
		private long _oldUploadValue;

		public void Initialize()
		{
			_oldDownloadValue = this.DownloadCounter.NextSample().RawValue;
			_oldUploadValue = this.UploadCounter.NextSample().RawValue;
		}

		public void Update()
		{
			long tempDownload = this.DownloadCounter.NextSample().RawValue;
			long tempUpload = this.UploadCounter.NextSample().RawValue;

			this.DownloadSpeed = tempDownload - _oldDownloadValue;
			this.UploadSpeed = tempUpload - _oldUploadValue;

			_oldDownloadValue = tempDownload;
			_oldUploadValue = tempUpload;
		}
	}
}
