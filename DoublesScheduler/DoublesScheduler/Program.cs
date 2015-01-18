using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoublesScheduler
{
	class Program
	{
		static void Main(string[] args)
		{
			int[,] playerMatrix = Matrix.PrepareMatrix(Constants.NumPlayers);

			Console.Write("\t");
			for (int i = 0; i < Constants.NumCourts; i++)
				Console.Write("Court {0}\t\t\t", i);
			Console.Write("\n");

			for (int i = 0; i < Constants.NumMatches; i++)
			{
				Round round = new Round(i + 1);

				var playersThisRound = Matrix.RefreshRoundItem(Constants.NumPlayers);
				for (int j = 0; j < (Constants.NumPlayers / 2); j++)
				{
					int p1 = Matrix.FindFirstAvailableItem(ref playersThisRound, Constants.NumPlayers);
					int p2 = Matrix.FirstAvailablePartner(ref playerMatrix, ref playersThisRound, p1, i);				
					round.AddTeam(new Team(p1, p2));
				}
				Constants.rounds.Add(round);
			}
			
			for (int i = 0; i < Constants.NumMatches; i++)
			{
				Round round = Constants.rounds[i];
				var teamsThisRound = Matrix.RefreshRoundItem(Constants.NumTeams);
				int[,] teamMatrix = Matrix.PrepareMatrix(Constants.NumTeams);
				for (int j = 0; j < (Constants.NumTeams / 2); j++)
				{
					int t1 = Matrix.FindFirstAvailableItem(ref teamsThisRound, Constants.NumTeams);
					int t2 = Matrix.FirstAvailableOpponent(ref teamMatrix, ref teamsThisRound, t1, 0, round);
					round.AddMatch(new Match(round.GetTeam(t1), round.GetTeam(t2)));
				}
				round.SetCourts();
				Console.Write(round.ToString());				
			}

			Console.WriteLine("\nSolution Penalty:");
			Console.WriteLine("# of times opponents face each other more than once: {0}", Constants.PlayerPenalties());
			Console.WriteLine("Max # of times one opponent faces another: {0}", Constants.MaxPlayerPenalty());
			Console.WriteLine("# of times player plays on same court more than once: {0}", Constants.CourtPenalties());
			Console.WriteLine("Max # of times a player plays on the same court: {0}", Constants.MaxCourtPenalty());

			Console.ReadLine();
		}
	}
}
