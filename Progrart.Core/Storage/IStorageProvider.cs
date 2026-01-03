using System;
using System.Collections.Generic;
using System.Text;

namespace Progrart.Core.Storage
{
	public interface IStorageProvider
	{
		Task<Stream?> TryOpenRead(string path);
		Task<Stream?> TryOpenWrite(string path);
	}
}
