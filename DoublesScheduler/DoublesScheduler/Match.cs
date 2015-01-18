using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoublesScheduler
{
	internal class Match
	{
		internal Team Team1 { get; set; }
		internal Team Team2 { get; set; }

		internal Match(Team t1, Team t2)
		{
			Team1 = t1;
			Team2 = t2;
		}

		public override string ToString()
		{
			return string.Format("{0} VS {1}", Team1.ToString(), Team2.ToString());
		}
	}
}
