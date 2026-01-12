namespace Progrart.Core.Storage
{
	public class ClassicStorageProvider : IStorageProvider
	{
		public DirectoryInfo BaseDirectory;

		public ClassicStorageProvider(DirectoryInfo baseDirectory)
		{
			BaseDirectory = baseDirectory;
		}

		public async Task<Stream?> TryOpenRead(string path)
		{
			try
			{
				return File.OpenRead(Path.Combine(BaseDirectory.FullName, path));
			}
			catch (Exception)
			{
				return null;
			}
		}

		public async Task<Stream?> TryOpenWrite(string path)
		{
			try
			{
				FileInfo fi = new FileInfo(Path.Combine(BaseDirectory.FullName, path));
				var di = fi.Directory;
				if (di is not null)
				{
					if (!di.Exists)
						di.Create();
				}
				return File.OpenWrite(Path.Combine(BaseDirectory.FullName, path));
			}
			catch (Exception)
			{
				return null;
			}
		}
	}
}
