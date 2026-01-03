using Avalonia.Platform.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Progrart.Core.Storage
{
	public class AvaloniaStorageProvider : IStorageProvider
	{
		IStorageFolder baseFolder;

		public AvaloniaStorageProvider(IStorageFolder baseFolder)
		{
			this.baseFolder = baseFolder;
		}

		public async Task<Stream?> TryOpenRead(string path)
		{
			var file = await baseFolder.GetFileAsync(path);
			if (file != null)
			{
				return await file.OpenReadAsync();
			}
			return null;
		}

		public async Task<Stream?> TryOpenWrite(string path)
		{
			var file = await baseFolder.GetFileAsync(path);
			if (file != null)
			{
				return await file.OpenWriteAsync();
			}
			file = await baseFolder.CreateFileAsync(path);
			if (file is not null)
				return await file.OpenWriteAsync();
			return null;
		}
	}
}
