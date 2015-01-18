using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace JavascriptUnitTest
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			var window = new BrowserWindow();
			window.ShowDialog();			
		}
	}
}
