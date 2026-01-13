using Progrart.Core.ProjectSystem;
using Progrart.Core.Storage;
using System.Diagnostics;

namespace Progrart.Cmd
{
	internal class Program
	{
		static async Task Main(string[] args)
		{
			Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
			string? project_file = null;
			int jobCount = 1;
			string? configuration = null;
			for (int i = 0; i < args.Length; i++)
			{
				string? item = args[i];
				switch (item)
				{
					case "-j":
						i++;
						if (i >= args.Length)
						{
							jobCount = -1;
							break;
						}

						if (!int.TryParse(args[i], out jobCount))
						{
							jobCount = -1;
							i--;
						}

						break;
					case "-c":
					case "--config":
					case "--configuration":
						{
							i++;
							item = args[i];
							configuration = item;
						}
						break;
					default:
						{

							if (File.Exists(item))
							{
								project_file = item;
							}
						}
						break;
				}
			}
			if (project_file != null)
				if (configuration != null)
				{
					FileInfo fi = new FileInfo(project_file);
					if (fi.Directory is DirectoryInfo di)
					{
						using var fs = fi.OpenRead();
						using var sr = new StreamReader(fs);
						ClassicStorageProvider provider = new ClassicStorageProvider(di);
						Builder builder = new Builder(sr, provider);
						builder.OnProgressUpdate = (max, index) =>
						{
							Console.WriteLine($"Done {index}/{max}");
						};
						builder.OnCompleted = () =>
						{
							Environment.Exit(0);
						};
						await builder.Build(configuration, jobCount);
					}
				}
		}
	}
}
