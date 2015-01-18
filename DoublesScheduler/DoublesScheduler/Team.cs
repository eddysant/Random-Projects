using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoublesScheduler
{
	internal class Team
	{
		internal int P1 { get; set; }
		internal int P2 { get; set; }

		internal Team(int p1, int p2)
		{
			P1 = p1;
			P2 = p2;
		}

		public override string ToString()
		{
			string p1 = P1.ToString();
			if (P1 < 10)
				p1 = string.Format(" {0}", p1);
			string p2 = P2.ToString();
			if (P2 < 10)
				p2 = string.Format(" {0}", p2);

			return string.Format("[{0} {1}]", p1, p2);
		}
	}
}
