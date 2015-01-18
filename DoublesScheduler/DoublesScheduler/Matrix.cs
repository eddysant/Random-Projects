using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoublesScheduler
{
	internal static class Matrix
	{
		internal static int FirstAvailablePartner(ref int[,] matrix, ref int[] usedMatrix, int index, int rnd)
		{
			if (rnd % 2 == 0)
			{
				for (int i = Constants.NumPlayers - 1; i >= 0; i--)
				{
					var value = CheckForPartner(ref matrix, ref usedMatrix, i, index);
					if (value != -1)
						return value;
				}
			}
			else
			{
				for (int i = 0; i < Constants.NumPlayers; i++)
				{
					var value = CheckForPartner(ref matrix, ref usedMatrix, i, index);
					if (value != -1)
						return value;
				}
			}
			return -1;
		}

		private static int CheckForPartner(ref int[,] matrix, ref int[] usedMatrix, int i, int index)
		{
			int y = (i + index) % Constants.NumPlayers;
			if (matrix[index, y] != -1 && usedMatrix[y] != -1)
			{
				usedMatrix[y] = -1;
				int temp = matrix[index, y];
				matrix[y, index] = -1;
				matrix[index, y] = -1;
				return temp;
			}
			return -1;
		}

		internal static int FirstAvailableOpponent(ref int[,] matrix, ref int[] usedMatrix, int index, int relaxValue, Round round)
		{
			Random random = new Random();
			int rnd = random.Next(Constants.NumTeams);

			for (int i = 0; i < Constants.NumTeams; i++)
			{
				int y = (i + index) % Constants.NumTeams;
				if (matrix[index, y] != -1 && usedMatrix[y] != -1)
				{
					if (HighestPlayedCount(round, index, y) > relaxValue)
						continue;

					AddOpponents(round, index, y);

					usedMatrix[y] = -1;
					int temp = matrix[index, y];
					matrix[y, index] = -1;
					matrix[index, y] = -1;
					return temp;
				}
			}

			return FirstAvailableOpponent(ref matrix, ref usedMatrix, index, (relaxValue + 1), round);
		}

		private static void AddOpponents(Round round, int t1, int t2)
		{
			Team team1 = round.GetTeam(t1);
			Team team2 = round.GetTeam(t2);
			int p1 = team1.P1;
			int p2 = team1.P2;
			Constants.players[p1].AddOpponents(team2);
			Constants.players[p2].AddOpponents(team2);
		}

		private static int HighestPlayedCount(Round round, int t1, int t2)
		{
			Team team1 = round.GetTeam(t1);
			Team team2 = round.GetTeam(t2);

			int p1 = team1.P1;
			int p2 = team1.P2;

			int playCount1 = Constants.players[p1].PlayCount(team2);
			int playCount2 = Constants.players[p2].PlayCount(team2);

			return Math.Max(playCount1, playCount2);
		}

		internal static int FindFirstAvailableItem(ref int[] matrix, int max)
		{
			for (int i = 0; i < max; i++)
			{
				int x = i % max;
				if (matrix[x] != -1)
				{
					matrix[x] = -1;
					return x;
				}
			}
			return -1;
		}

		internal static int[] RefreshRoundItem(int max)
		{
			int[] players = new int[max];
			for (int i = 0; i < max; i++)
				players[i] = i;
			return players;
		}

		internal static int[,] PrepareMatrix(int max)
		{
			int[,] matrix = new int[max, max];
			for (int i = 0; i < max; i++)
			{
				for (int j = 0; j < max; j++)
				{
					if (i == j)
						matrix[i, j] = -1;
					else
						matrix[i, j] = j;
				}
			}
			return matrix;
		}

	}
}
