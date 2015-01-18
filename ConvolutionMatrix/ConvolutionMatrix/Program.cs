using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace ConvolutionMatrix
{
	class Program
	{
		const int FilterSizeInX = 5;
		const int FilterSizeInY = 5;
		const int FilterSizeInZ = 5;

		const int PointsInX = 1024;
		const int PointsInY = 512;
		const int PointsInZ = 256;

		const int StartX = (FilterSizeInX / 2);
		const int StartY = (FilterSizeInY / 2);
		const int StartZ = (FilterSizeInZ / 2);

		const int EndX = PointsInX - StartX;
		const int EndY = PointsInY - StartY;
		const int EndZ = PointsInZ - StartZ;

		const int resultX = PointsInX - (FilterSizeInX - 1);
		const int resultY = PointsInY - (FilterSizeInY - 1);
		const int resultZ = PointsInZ - (FilterSizeInZ - 1);

		const string dataFileName = @"D:\Sept9.data";
		const string filterFileName = @"D:\Sept9.filter";
		const string resultFileName = @"D:\result.data";

		static double[] result = new double[resultX * resultY * resultZ];
		static double[] data;
		static double[] filter;

		static int threads = 10;
		static Semaphore semaphore = new Semaphore(threads, threads);

		static void Main(string[] args)
		{
			var stopWatchT = new Stopwatch();
			var stopWatchP = new Stopwatch();

			stopWatchT.Start();

			Console.Write("Reading voxel...");
			stopWatchP.Start();
			ReadVoxel();
			Console.WriteLine("Done {0}ms", stopWatchP.ElapsedMilliseconds);

			Console.Write("Reading filter...");
			stopWatchP.Restart();
			ReadFilter();
			Console.WriteLine("Done {0}ms", stopWatchP.ElapsedMilliseconds);

			Console.Write("Applying filter...");
			stopWatchP.Restart();
			for (var x = 2; x < EndX; x++)
			{
				int xStart = x - 2;
				semaphore.WaitOne();
				System.Threading.ThreadPool.QueueUserWorkItem(new WaitCallback(ComputeSlice), xStart);
			}

			for (var i = 0; i < threads; i++)
				semaphore.WaitOne();
			Console.WriteLine("Done {0}ms", stopWatchP.ElapsedMilliseconds);

			Console.Write("Writing results...");
			stopWatchP.Restart();
			WriteVoxel();
			Console.WriteLine("Done {0}ms", stopWatchP.ElapsedMilliseconds);

			stopWatchT.Stop();
			Console.WriteLine("Completed in {0}ms", stopWatchT.ElapsedMilliseconds);
			Console.ReadLine();
		}

		static void ComputeSlice(object obj)
		{
			int xStart = (int)obj;

			for (var y = 2; y < EndY; y++)
			{
				int yStart = y - 2;
				for (var z = 2; z < EndZ; z++)
					ApplyFilter(xStart, yStart, z);
			}

			semaphore.Release();
		}

		static void ApplyFilter(int xStart, int yStart, int z)
		{
			double total = 0;
			double[] dataLocal = data;
			double[] filterLocal = filter;

			int zStart = z - 2;
			int xEnd = xStart + 5;
			int yEnd = yStart + 5;

			int filterPosition = 0;

			for (var i = xStart; i < xEnd; i++)
				for (var j = yStart; j < yEnd; j++)
				{
					int position = (i * PointsInX) + (j * PointsInY) + (zStart * PointsInZ);

					total += (dataLocal[position] * filterLocal[filterPosition]);
					position++;
					filterPosition++;

					total += (dataLocal[position] * filterLocal[filterPosition]);
					position++;
					filterPosition++;

					total += (dataLocal[position] * filterLocal[filterPosition]);
					position++;
					filterPosition++;

					total += (dataLocal[position] * filterLocal[filterPosition]);
					position++;
					filterPosition++;

					total += (dataLocal[position] * filterLocal[filterPosition]);
					filterPosition++;
				}

			result[xStart * yStart * zStart] = total;
		}

		static void ReadVoxel()
		{
			int total = PointsInX * PointsInY * PointsInZ;
			var localData = new double[total];
			using (var readStream = new FileStream(dataFileName, FileMode.Open, FileAccess.Read, FileShare.None, (sizeof(double) * total), false))
			using (var binaryReader = new BinaryReader(readStream))
			{
				for (var i = 0; i < total; i++)
					localData[i] = binaryReader.ReadDouble();
			}
			data = localData;
		}

		static void ReadFilter()
		{
			int total = FilterSizeInX * FilterSizeInY * FilterSizeInZ;
			var localFilter = new double[total];
			using (var readStream = new FileStream(filterFileName, FileMode.Open, FileAccess.Read, FileShare.None, (sizeof(double) * total)))				
			using (var binaryReader = new BinaryReader(readStream))
			{
				for (var i = 0; i < total; i++)
					localFilter[i] = binaryReader.ReadDouble();
			}
			filter = localFilter;
		}

		static void WriteVoxel()
		{
			int total = resultX * resultY * resultZ;

			if (File.Exists(resultFileName))
				File.Delete(resultFileName);		 

			using (var writeStream = new FileStream(resultFileName, FileMode.Create, FileAccess.Write, FileShare.None))			
			using (var binaryWriter = new BinaryWriter(writeStream))
			{
				for (var i = 0; i < total; i++)
					binaryWriter.Write(result[i]);
			}
		}
	}
}
