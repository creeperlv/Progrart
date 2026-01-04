using Newtonsoft.Json;
using Progrart.Core.JSExecution;
using Progrart.Core.Storage;

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
			//FileInfo src = new FileInfo(Path.Combine(basePath, item.Source));
			//FileInfo tgt = new FileInfo(Path.Combine(basePath, project.OutputDir, item.Target ?? item.Source + ".png"));
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
		public async Task Build(string targetConfig)
		{
			foreach (var config in project.Configurations)
			{
				if (config.Name == targetConfig)
				{
					int index = 0;
					foreach (var item in config.Items)
					{
						await Execute(config, item);
						index++;
						OnProgressUpdate?.Invoke(config.Items.Count, index);
					}
					OnCompleted?.Invoke();
					return;
				}
			}
		}
	}
}
