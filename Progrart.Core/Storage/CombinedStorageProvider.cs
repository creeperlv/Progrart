namespace Progrart.Core.Storage
{
    public class CombinedStorageProvider : IStorageProvider
	{
		public List<IStorageProvider> providers = new List<IStorageProvider>();
		public async Task<Stream?> TryOpenRead(string path)
		{
			foreach (var item in providers)
			{
				var v = await item.TryOpenRead(path);
				if (v != null) return v;
			}
			return null;
		}

		public async Task<Stream?> TryOpenWrite(string path)
		{
			foreach (var item in providers)
			{
				var v = await item.TryOpenWrite(path);
				if (v != null) return v;
			}
			return null;
		}
	}
}
