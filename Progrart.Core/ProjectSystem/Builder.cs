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
		public async Task Build(string targetConfig, int maxJobCount = 1)
		{
			int finalJobCount= Math.Max(maxJobCount < 0 ? Environment.ProcessorCount : maxJobCount, 1);
			foreach (var config in project.Configurations)
			{
				if (config.Name == targetConfig)
				{
					int index = 0;
					//List<Task> tasks = new List<Task>();
					if (finalJobCount == 1)
					{
						foreach (var item in config.Items)
						{
							await Execute(config, item);
							index++;
							OnProgressUpdate?.Invoke(config.Items.Count, index);
						}
					}
					else
					{
						var options = new ParallelOptions
						{
							MaxDegreeOfParallelism = finalJobCount
						};

						await Parallel.ForEachAsync(config.Items, options, async (item, token) =>
						{
							await Execute(config, item);

							int currentCount = Interlocked.Increment(ref index);
							OnProgressUpdate?.Invoke(config.Items.Count, currentCount);
						});
					}
					OnCompleted?.Invoke();
					return;
				}
			}
			throw new Exception($"Configuration \"{targetConfig}\" not found!");
		}
	}
}
