using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoublesScheduler
{
	internal static class Constants
	{
		internal static int NumCourts = 3;
		internal static int NumPlayers = (NumCourts * 4);
		internal static int NumTeams = (NumPlayers / 2);
		internal static int NumMatches = 4;

		internal static List<Round> rounds = new List<Round>();
		internal static List<Player> players = GeneratePlayers();

		private static List<Player> GeneratePlayers()
		{
			List<Player> players = new List<Player>();
			for (int i = 0; i < NumPlayers; i++)
				players.Add(new Player(i));
			return players;
		}

		internal static int PlayerPenalties()
		{
			int count = 0;
			foreach (var player in Constants.players)
				count += player.TotalPlayerPenalties();
			return count;
		}

		internal static int MaxPlayerPenalty()
		{
			int max = 0;
			foreach (var player in Constants.players)
				max = Math.Max(max, player.MaxPlayerPenalties());
			return max;
		}

		internal static int CourtPenalties()
		{
			int count = 0;
			foreach (var player in Constants.players)
				count += player.TotalCourtPenalties();
			return count;
		}

		internal static int MaxCourtPenalty()
		{
			int max = 0;
			foreach (var player in Constants.players)
				max = Math.Max(max, player.MaxCourtPenalties());
			return max;
		}
	}
		
}
