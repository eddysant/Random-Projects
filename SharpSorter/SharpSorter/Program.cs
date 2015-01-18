using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.IO.MemoryMappedFiles;
using System.IO;
using System.Threading;

namespace SharpSorter
{
	class Program
	{

		static long max_read_write = 100000000;

		static string tempDirectory = Directory.GetCurrentDirectory();
		static List<string> sortedFiles = new List<string>();

		static void Main(string[] args)
		{
			FileStream readfile = null;
			FileStream writefile = null;
			List<FileStream> streams = null;

			try
			{
				readfile = File.OpenRead(Path.Combine(tempDirectory, "data.hmtp"));
				ReadAndSort(readfile);
				
				writefile = File.Create(Path.Combine(tempDirectory, "sorted.hmtp"));
				BinaryWriter writer = new BinaryWriter(writefile);				

				streams = new List<FileStream>();
				for (int i = 0; i < sortedFiles.Count; i++)
					streams.Add(File.OpenRead(sortedFiles[i]));

				long[] toWrite = new long[sortedFiles.Count];
				int[] indexes = new int[sortedFiles.Count];

				for (int i = 0; i < streams.Count; i++)
				{
					BinaryReader reader = new BinaryReader(streams[i]);
					toWrite[i] = reader.ReadInt64();
					indexes[i] = i;
				}

				DoubleQuickSort(toWrite, ref indexes, 0, toWrite.Length - 1);
				bool exit_condition = false;

				while (true)
				{
					int index = 0;

					while (indexes[index] == -1)
					{
						index++;
						if (index > (indexes.Length - 1))
						{
							exit_condition = true;
							break;
						}
					}

					if (exit_condition)
						break;

					if (index == (toWrite.Length - 1))
					{
						try
						{
							BinaryReader reader = new BinaryReader(streams[indexes[index]]);
							writer.Write(reader.ReadInt64());
						}
						catch { break; }
					}

					writer.Write(toWrite[index]);

					try
					{
						BinaryReader reader = new BinaryReader(streams[indexes[index]]);
						toWrite[index] = reader.ReadInt64();

						while(index < (toWrite.Length - 1) && toWrite[index] < toWrite[index + 1])
						{ 
							writer.Write(toWrite[index]);
							toWrite[index] = reader.ReadInt64();
						}

						index++;
						writer.Write(toWrite[index]);
						reader = new BinaryReader(streams[indexes[index]]);
						toWrite[index] = reader.ReadInt64();
					}
					catch
					{
						indexes[index] = -1;
					}

					toWrite = InsertionSort(toWrite, ref indexes);
				}

			}
			finally
			{
				if (readfile != null)
					readfile.Close();
				if (writefile != null)
					writefile.Close();

				if (streams != null)
				{
					for (int i = 0; i < streams.Count; i++)
						streams[i].Close();
				}

				foreach (var tfile in sortedFiles)
					File.Delete(tfile);				
			}
		}

		static void ReadAndSort(FileStream file)
		{
			long position = 0;

			for (position = 0; position < ( file.Length / 8); position += max_read_write)
			{
				long[] data = new long[max_read_write];

				BinaryReader reader = new BinaryReader(file);
				for (int i = 0; i < max_read_write; i++)
					data[i] = reader.ReadInt64();

				QuickSort(data, 0, data.Length - 1);
				WriteTempFile(data);
				GC.Collect();
			}
		}

		static void WriteTempFile(long[] array)
		{			
			string tempFile = Path.Combine(tempDirectory, Guid.NewGuid().ToString());

			using (FileStream stream = File.Create(tempFile))
			{
				BinaryWriter writer = new BinaryWriter(stream);
				for (int i = 0; i < max_read_write; i++)
					writer.Write(array[i]);
			}
			sortedFiles.Add(tempFile);
		}


		private static long[] InsertionSort(long[] array, ref int[] array2)
		{
			for (int i = 1; i <= array.Length - 1; i++)
			{
				long val = array[i];
				int val2 = array2[i];
				int x = i;

				while ((x > 0) && (array[x - 1] > val))
				{
					array[x] = array[x - 1];
					array2[x] = array2[x - 1];						
					x--;
				}
				array[x] = val;
				array2[x] = val2;
			}
			return array;
		}

		private static long[] DoubleQuickSort(long[] array, ref int[] array2, int left, int right)
		{
			if (right > left)
			{
				int pivot = DoublePartition(array, ref array2, left, right);
				array = DoubleQuickSort(array, ref array2, left, pivot);
				array = DoubleQuickSort(array, ref array2, pivot + 1, right);
			}
			return array;
		}

		static int DoublePartition(long[] array, ref int[] array2, int left, int right)
		{
			long val = array[left];
			int val2 = array2[left];			

			int i = left - 1;
			int j = right + 1;

			long tempVal = 0;
			int tempVal2 = 0;

			while (true)
			{
				do
				{
					j--;
				} while (array[j] > val);

				do
				{
					i++;
				} while (array[i] < val);

				if (i < j)
				{
					tempVal = array[i];
					array[i] = array[j];
					array[j] = tempVal;

					tempVal2 = array2[i];
					array2[i] = array2[j];
					array2[j] = tempVal2;
				}

				else return j;
			}

		}

		private static long[] QuickSort(long[] array, int left, int right)
		{
			if (right > left)
			{
				int pivot = Partition(array, left, right);
				array = QuickSort(array, left, pivot);
				array = QuickSort(array, pivot + 1, right);
			}

			return array;
		}

		static int Partition(long[] array, int left, int right)
		{
			long val = array[left];
			int i = left - 1;
			int j = right + 1;

			long tempVal = 0;

			while (true)
			{
				do
				{
					j--;
				} while (array[j] > val);

				do
				{
					i++;
				} while (array[i] < val);

				if (i < j)
				{
					tempVal = array[i];
					array[i] = array[j];
					array[j] = tempVal;
				}

				else return j;
			}

		}
	}
}
