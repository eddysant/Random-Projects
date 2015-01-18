using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoublesScheduler
{
	internal class Player
	{
		int Id { get; set; }

		Dictionary<int, int> opponents;
		Dictionary<int, int> courts;

		public Player(int id)
		{
			Id = id;
			opponents = new Dictionary<int, int>();
			courts = new Dictionary<int, int>();
		}

		public void AddOpponents(Team team)
		{
			if (opponents.ContainsKey(team.P1))
				opponents[team.P1] = opponents[team.P1] + 1;
			else 
				opponents.Add(team.P1, 1);

			if (opponents.ContainsKey(team.P2))
				opponents[team.P2] = opponents[team.P2] + 1;
			else 
				opponents.Add(team.P2, 1);
		}

		public int PlayCount(Team team)
		{
			int p1PlayCount = 0;
			if (opponents.ContainsKey(team.P1))
				p1PlayCount = opponents[team.P1];
			int p2PlayCount = 0;
			if (opponents.ContainsKey(team.P2))
				p2PlayCount = opponents[team.P2];
			
			return Math.Max(p1PlayCount, p2PlayCount);
		}

		public int TotalPlayerPenalties()
		{
			int count = 0;			
			foreach (var opponent in opponents)
			{
				if (opponent.Value > 1)
					count++;
			}
			return count;
		}

		public int MaxPlayerPenalties()
		{
			int max = 0;
			foreach (var opponent in opponents)
				max = Math.Max(max, opponent.Value);
			return max;
		}

		public void AddCourt(int court)
		{
			if (courts.ContainsKey(court))
				courts[court] = courts[court] + 1;
			else
				courts.Add(court, 1);
		}

		public int CourtPlayCount(int court)
		{			
			if (courts.ContainsKey(court))
				return courts[court];
			return 0;
		}

		public int TotalCourtPenalties()
		{
			int count = 0;
			foreach (var court in courts)
			{
				if (court.Value > 1)
					count++;
			}
			return count;
		}

		public int MaxCourtPenalties()
		{
			int max = 0;
			foreach (var court in courts)
				max = Math.Max(max, court.Value);
			return max;
		}

	}


}
