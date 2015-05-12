using System;
using OpenTK;

namespace GlslTutorials
{
	public class PaintBoxState
	{
		public int[] scoreInts = new int[6];
		Vector3 ballLocation = new Vector3();

		public PaintBoxState ()
		{
		}

		public int ScoreCount()
		{
			return scoreInts.Length;
		}

		public bool ScoreChanged(int scoreNum, int score)
		{
			if (scoreInts[scoreNum] != score)
			{
				scoreInts[scoreNum] = score;
				return true;
			}
			return false;
		}

		public void UpdatePositions(Vector3 bl)
		{
			ballLocation = bl;
		}
	}
}

