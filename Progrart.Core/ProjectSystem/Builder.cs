using Newtonsoft.Json;
using Progrart.Core.JSExecution;

namespace Progrart.Core.ProjectSystem
{
	public class Builder
	{
		public Project project;
		public string basePath;
		public Action<int, int>? OnProgressUpdate;
		public Action? OnCompleted;
		public Builder(StreamReader reader, string basePath)
		{
			var project = JsonConvert.DeserializeObject<Project>(reader.ReadToEnd());
			if (project is null) throw new JsonSerializationException();
			this.project = project;
			this.basePath = basePath;
		}
		public Builder(Project project, string basePath)
		{
			this.project = project;
			this.basePath = basePath;
		}
		public void Execute(BuildConfiguration config, BuildItem item)
		{
			FileInfo src = new FileInfo(Path.Combine(basePath, item.Source));
			FileInfo tgt = new FileInfo(Path.Combine(basePath, project.OutputDir, item.Target ?? item.Source + ".png"));
			var args = project.Arguments.Clone();
			args.MergeFrom(config.Arguments);
			args.MergeFrom(item.Arguments);
			ProgrartExecutor executor = new ProgrartExecutor();
			using var stream = src.OpenRead();
			using var reader = new StreamReader(stream);
			var img = executor.RenderImage(item.Size, reader.ReadToEnd(), args);
			using var img_stream = tgt.OpenWrite();
			img.DrawingCore.ToData().SaveTo(img_stream);
			img_stream.Flush();
		}
		public void Build(string targetConfig)
		{
			foreach (var config in project.Configurations)
			{
				if (config.Name == targetConfig)
				{
					int index = 0;
					foreach (var item in config.Items)
					{
						Execute(config, item);
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
