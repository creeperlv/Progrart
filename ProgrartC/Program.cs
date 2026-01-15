using Progrart.Core;
using Progrart.Core.JSExecution;
using Progrart.Core.ProjectSystem;
using Progrart.Core.Storage;
using SkiaSharp;
using System.Diagnostics;

namespace ProgrartC
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
			List<string> includePaths = new List<string>();
			string? sourceFile = null;
			string? outputFile = null;
			ExecuteArguments executeArguments = new ExecuteArguments();
			for (int i = 0; i < args.Length; i++)
			{
				string? item = args[i];
				switch (item)
				{
					case "-I":
						i++;
						includePaths.Add(args[i]);
						break;
					case "-o":
						i++;
						outputFile = args[i];
						break;
					default:
						if (File.Exists(item))
						{
							sourceFile = item;
						}
						else
						{
							if (item.StartsWith("-D"))
							{
								string define = item.Substring(2);
								string[] parts = define.Split('=', 2);
								if (parts.Length == 2)
								{
									executeArguments.data[parts[0]] = parts[1];
								}
								else
								{
									executeArguments.data[parts[0]] = "true";
								}
							}
						}
						break;
				}
			}
			if (sourceFile == null)
			{
				Trace.WriteLine("no input files");
				return;
			}
			if (outputFile == null)
			{
				outputFile = Path.ChangeExtension(sourceFile, "png");
			}
			CombinedStorageProvider storageProvider = new();
			{
				FileInfo sourceFileInfo = new FileInfo(sourceFile);
				if (sourceFileInfo.Directory is not null)
					includePaths.Add(sourceFileInfo.Directory.FullName);
			}
			if (!includePaths.Contains("."))
				includePaths.Add(".");
			foreach (var item in includePaths)
			{
				storageProvider.providers.Add(new ClassicStorageProvider(new DirectoryInfo(item)));
			}
			int Scale = 1024;

			if (executeArguments.data.TryGetValue("Scale", out var scale))
			{
				if (!int.TryParse(scale, out Scale)) Scale = 1024;
			}
			ProgrartExecutor progrartExecutor = new(storageProvider);
			var result = progrartExecutor.RenderImage(Scale, File.ReadAllText(sourceFile), executeArguments);

			SKData data;
			FileInfo fi = new FileInfo(outputFile);
			var ext = fi.Extension.ToLower();
			data = result.DrawingCore.ToData(ext);
			if(data is null)
			{
				Trace.WriteLine("failed to encode image");
				return;
			}
			using var img_stream = File.OpenWrite(outputFile);
			if (img_stream is null)
				return;
			data.SaveTo(img_stream);
			img_stream.Flush();
		}
	}
}
