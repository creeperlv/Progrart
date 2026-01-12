using Newtonsoft.Json;
using Progrart.Core.JSExecution;
using Progrart.Core.Storage;
using System.Diagnostics;

namespace Progrart.Core.ProjectSystem
{
	public class Builder
	{
		public Project project;
		public IStorageProvider provider;
		public Action<int, int>? OnProgressUpdate;
		public Action? OnCompleted;
		public Builder(StreamReader reader, IStorageProvider provider)
		{
			var project = JsonConvert.DeserializeObject<Project>(reader.ReadToEnd());
			if (project is null) throw new JsonSerializationException();
			this.project = project;
			this.provider = provider;
		}
		public Builder(Project project, IStorageProvider provider)
		{
			this.project = project;
			this.provider = provider;
		}
		public async Task Execute(BuildConfiguration config, BuildItem item)
		{
			var args = project.Arguments.Clone();
			args.MergeFrom(config.Arguments);
			args.MergeFrom(item.Arguments);
			ProgrartExecutor executor = new ProgrartExecutor(provider);
			using var stream = await provider.TryOpenRead(item.Source);
			if (stream is null)
				return;
			using var reader = new StreamReader(stream);
			var img = executor.RenderImage(item.Size, reader.ReadToEnd(), args);
			using var img_stream = await provider.TryOpenWrite(Path.Combine(project.OutputDir, item.Target ?? item.Source + ".png"));
			if (img_stream is null)
				return;
			img.DrawingCore.ToData().SaveTo(img_stream);
			img_stream.Flush();
		}
		public async Task Build(string targetConfig, bool isParallel = false)
		{
			foreach (var config in project.Configurations)
			{
				Console.WriteLine(config.Name);
				if (config.Name == targetConfig)
				{
					int index = 0;
					List<Task> tasks = new List<Task>();
					foreach (var item in config.Items)
					{
						if (!isParallel)
						{

							await Execute(config, item);
							index++;
							OnProgressUpdate?.Invoke(config.Items.Count, index);
						}
						else
						{

							var t = Task.Run(async () =>
							{
								index++;
								int i = index;
								await Execute(config, item);
								OnProgressUpdate?.Invoke(config.Items.Count, i);
							});
							tasks.Add(t);
						}

					}
					if (isParallel)
					{
						await Task.WhenAll(tasks);
					}
					OnCompleted?.Invoke();
					return;
				}
			}
			throw new Exception($"Configuration \"{targetConfig}\" not found!");
		}
	}
}
