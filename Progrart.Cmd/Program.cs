using Progrart.Core.ProjectSystem;
using Progrart.Core.Storage;

namespace Progrart.Cmd
{
	internal class Program
	{
		static void Main(string[] args)
		{
			string? project_file = null;
			string? configuration = null;
			for (int i = 0; i < args.Length; i++)
			{
				string? item = args[i];
				switch (item)
				{
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
						builder.OnProgressUpdate = (v, m) =>
						{
							Console.WriteLine($"{v}/{m}");
						};
						builder.OnCompleted = () =>
						{
							Environment.Exit(0);
						};
						Task.Run(async () =>
						{
							await builder.Build(configuration);
						});
						while (true)
						{
							Thread.Sleep(1000);
						}
					}
				}
		}
	}
}
