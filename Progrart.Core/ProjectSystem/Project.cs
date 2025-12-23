using System;
using System.Collections.Generic;
using System.Text;

namespace Progrart.Core.ProjectSystem
{
	[Serializable]
	public class Project
	{
		public List<BuildConfiguration> Configurations = new List<BuildConfiguration>();
	}
	[Serializable]
	public class BuildConfiguration
	{
		public string? Name;
		public List<BuildItem> Items = new List<BuildItem>();
	}
	[Serializable]
	public class BuildItem
	{
		public string? Source;
		public string? Target;
	}
}
