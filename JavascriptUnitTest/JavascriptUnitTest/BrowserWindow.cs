using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace JavascriptUnitTest
{
	public partial class BrowserWindow : Form
	{
		public bool passed = false;

		public BrowserWindow()
		{
			InitializeComponent();
		}

		private void BrowserWindow_Shown(object sender, EventArgs e)
		{
			try
			{
				webBrowser.Navigate(@"M:\a\src\csharp\server\DapAdministrator\qunit\index.html");

				while (webBrowser.ReadyState != WebBrowserReadyState.Complete)
					Application.DoEvents();

				var x = webBrowser.Document.GetElementById("qunit-finished");
				while (x.InnerText != "done")
					Application.DoEvents();

				var testResults = webBrowser.Document.GetElementById("qunit-testresult").InnerHtml;
				var matches = Regex.Matches(testResults, ">[0-9]+<");
				var success = int.Parse(matches[0].Value.Trim('<', '>'));
				var total = int.Parse(matches[1].Value.Trim('<', '>'));
				var failed = int.Parse(matches[2].Value.Trim('<', '>'));

				if (success != total)
					passed = false;
				else if (failed != 0)
					passed = false;
				else
					passed = true;
			}
			catch
			{
				passed = false;
			}
			this.Close();
		}
	}
}
