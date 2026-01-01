using System;
using System.Collections.Generic;
using System.Text;

namespace Progrart.Core.ProjectSystem
{
	[Serializable]
	public class Project
	{
		public string OutputDir = "output";
		public List<BuildConfiguration> Configurations = new List<BuildConfiguration>();
		public ExecuteArguments Arguments = new ExecuteArguments();
	}
	[Serializable]
	public class BuildConfiguration
	{
		public string? Name;
		public string? OutputDir;
		public List<BuildItem> Items = new List<BuildItem>();
		public ExecuteArguments Arguments = new ExecuteArguments();
	}
	[Serializable]
	public class BuildItem
	{
		public string Source = "";
		public string? Target;
		public int Size;
		public ExecuteArguments Arguments = new ExecuteArguments();
	}
}
