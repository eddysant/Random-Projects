using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Resources;
using System.Collections;
using System.IO;

namespace StringFinder
{
	class Program
	{
		static void Main(string[] args)
		{
			string outFileName = @"d:\unusedstrings.txt";
			string rootPath = @"project";
			string resourceFile = @"\Resources\trings.resx";

			var rsxr = new ResXResourceReader(resourceFile);

			using (System.IO.StreamWriter outFile = new System.IO.StreamWriter(outFileName))
			{
				foreach (DictionaryEntry entry in rsxr)
				{
					if (FindInDirectory(rootPath, ((string)entry.Key)))
						continue;
					else
						outFile.WriteLine(((string)entry.Key));
				}
			}
		}

		static bool FindInDirectory(string path, string value)
		{

			var dir = new DirectoryInfo(path);
			foreach (var di in dir.GetDirectories())
			{
				if (FindInDirectory(di.FullName, value))
					return true;
			}

			foreach (var fi in dir.GetFiles().Where(s => s.FullName.EndsWith(".cs") || s.FullName.EndsWith(".js") || s.FullName.EndsWith(".cshtml")))
			{
				if (FindInFile(fi.FullName, value))
					return true;
			}
			return false;
		}

		static bool FindInFile(string path, string value)
		{
			if (path.Contains("Strings") && path.Contains("Designer"))
				return false;

			string line;
			using (System.IO.StreamReader reader = new System.IO.StreamReader(path))
			{
				while ((line = reader.ReadLine()) != null)
				{
					if (line.Contains(value))
					{
						Console.WriteLine(line);
						return true;
					}
				}
				return false;
			}
		}

	}
}
