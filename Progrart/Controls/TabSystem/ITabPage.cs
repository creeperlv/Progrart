using System;
using System.Collections.Generic;
using System.Text;

namespace Progrart.Controls.TabSystem
{
	public interface ITabPage
	{
		void SetHost(TabHost host);
		void BindButton(TabButton button);
		bool IsModified();
		void Save();
		void Execute();
	}
}
