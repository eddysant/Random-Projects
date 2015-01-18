using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoublesScheduler
{
	internal class Round
	{
		private List<Team> teams;
		private List<Match> matches;
		private SortedDictionary<int, Match> courtSetup;
		private int round;

		internal Round(int round)
		{
			this.round = round;
			teams = new List<Team>();
			matches = new List<Match>();
			courtSetup = new SortedDictionary<int, Match>();
		}

		internal void AddTeam(Team team)
		{
			teams.Add(team);
		}

		internal Team GetTeam(int index)
		{
			return teams[index];
		}

		internal void AddMatch(Match match)
		{
			matches.Add(match);
		}

		internal void SetCourts()
		{
			var availableCourts = Matrix.RefreshRoundItem(Constants.NumCourts);
			for (int i = 0; i < matches.Count; i++)
				courtSetup.Add(FirstAvailableCourt(ref availableCourts, 0, i), matches[i]);
		}

		private int FirstAvailableCourt(ref int[] usedMatrix, int relaxValue, int matchValue)
		{
			for (int i = 0; i < Constants.NumCourts; i++)
			{				
				if (usedMatrix[i] != -1)
				{
					if (HighestCourtPlayedCount(matchValue, i) > relaxValue)
						continue;
					AddToCourt(matchValue, i);
					usedMatrix[i] = -1;
					return i;
				}
			}

			return FirstAvailableCourt(ref usedMatrix, (relaxValue + 1), matchValue);
		}

		private void AddToCourt(int matchIndex, int court)
		{
			Team team1 = matches[matchIndex].Team1;
			Team team2 = matches[matchIndex].Team2;
			Constants.players[team1.P1].AddCourt(court);
			Constants.players[team1.P2].AddCourt(court);
			Constants.players[team2.P1].AddCourt(court);
			Constants.players[team2.P2].AddCourt(court);
		}

		private int HighestCourtPlayedCount(int matchIndex, int court)
		{
			Team team1 = matches[matchIndex].Team1;
			Team team2 = matches[matchIndex].Team2;
			int playCount1 = Constants.players[team1.P1].CourtPlayCount(court);
			int playCount2 = Constants.players[team1.P2].CourtPlayCount(court);
			int playCount3 = Constants.players[team2.P1].CourtPlayCount(court);
			int playCount4 = Constants.players[team2.P2].CourtPlayCount(court);

			return Math.Max(Math.Max(playCount1, playCount2), Math.Max(playCount3, playCount4)) ;
		}

		public override string ToString()
		{
			StringBuilder roundToString = new StringBuilder();
			roundToString.AppendFormat("R{0}:\t", round);
			foreach (var match in courtSetup)
				roundToString.AppendFormat("{0}\t", match.Value.ToString());
			roundToString.AppendLine();
			return roundToString.ToString();
		}
	}
}
