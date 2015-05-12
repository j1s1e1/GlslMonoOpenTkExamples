using System;
using OpenTK;

namespace GlslTutorials
{
	public class PaintBox
	{
		static PaintWall backWall;
		static PaintWall bottomWall;
		static PaintWall rightWall;
		static PaintWall topWall;
		static PaintWall leftWall;
		static PaintWall frontWall;

		float textureRotation = -90f;

		float moveZ = -1f;

		Vector3 lowLimits;
		Vector3 highLimits;

		Vector3 center;

		Vector3 backWallOffset;
		Vector3 frontWallOffset;
		Vector3 bottomWallOffset;
		Vector3 topWallOffset;
		Vector3 rightWallOffset;
		Vector3 leftWallOffset;

		int paintWidth = 15;

		int[] scores;

		public PaintBox()
		{
			center = new Vector3(0f, 0f, moveZ);
			backWallOffset = new Vector3(0f, 0f, -2f);
			frontWallOffset = new Vector3(0f, 0f, 0f);
			bottomWallOffset = new Vector3(0f, -1f, moveZ);
			topWallOffset = new Vector3(0f, 1f, moveZ);
			rightWallOffset = new Vector3(1f, 0f, moveZ);
			leftWallOffset = new Vector3(-1f, 0f, moveZ);		
			// back
			backWall = new PaintWall();
			backWall.Scale(1.0f);
			backWall.RotateShape(new Vector3(1f, 0f, 0f), 0f);
			backWall.Move(backWallOffset);
			backWall.SetLightPosition(new Vector3(0f, 0f, 1.6f));

			// bottom
			bottomWall = new PaintWall();
			bottomWall.Scale(1.0f);
			bottomWall.RotateShape(new Vector3(1f, 0f, 0f), textureRotation);
			bottomWall.Move(bottomWallOffset);

			// right
			rightWall = new PaintWall();
			rightWall.Scale(1.0f);
			rightWall.RotateShape(new Vector3(0f, 1f, 0f), textureRotation);
			rightWall.Move(rightWallOffset);

			// top
			topWall = new PaintWall();
			topWall.Scale(1.0f);
			topWall.RotateShape(new Vector3(1f, 0f, 0f), -textureRotation);
			topWall.Move(topWallOffset);

			// left
			leftWall = new PaintWall();
			leftWall.Scale(1.0f);
			leftWall.RotateShape(new Vector3(0f, 1f, 0f), -textureRotation);
			leftWall.Move(leftWallOffset);

			// front
			frontWall = new PaintWall();
			frontWall.Scale(1.0f);
			frontWall.RotateShape(new Vector3(1f, 0f, 0f), 0f);
			frontWall.Move(frontWallOffset);
			frontWall.SetLightPosition(new Vector3(0f, 0f, 0.2f));
			scores = new int[6];
			for (int i = 0; i < scores.Length; i++)
			{
				scores[i] = 1000;
			}
		}

		public void SetLimits(Vector3 low, Vector3 high, Vector3 epsilon)
		{
			lowLimits = low + epsilon;
			highLimits = high - epsilon;
		}

		public string GetLimits()
		{
			return lowLimits.ToString() + " " + highLimits.ToString();
		}

		public void Paint(Vector3 position)
		{
			Vector3 wallSpot = position - center;
			if (position.X < lowLimits.X)
			{
				leftWall.Paint(-wallSpot.Z, wallSpot.Y, paintWidth);
				if (scores[0] > 0) scores[0]--;
			}
			if (position.X > highLimits.X)
			{
				rightWall.Paint(wallSpot.Z, wallSpot.Y, paintWidth);
				if (scores[1] > 0) scores[1]--;
			}
			if (position.Y < lowLimits.Y)
			{
				bottomWall.Paint(wallSpot.X, -wallSpot.Z, paintWidth);
				if (scores[2] > 0) scores[2]--;
			}
			if (position.Y > highLimits.Y)
			{
				topWall.Paint(wallSpot.X, wallSpot.Z, paintWidth);
				if (scores[3] > 0) scores[3]--;
			}
			if (position.Z < lowLimits.Z)
			{
				backWall.Paint(wallSpot.X, wallSpot.Y, paintWidth);
				if (scores[4] > 0) scores[4]--;
			}
			if (position.Z > highLimits.Z)
			{
				frontWall.Paint(wallSpot.X, wallSpot.Y, paintWidth);
				if (scores[5] > 0) scores[5]--;
			}
		}

		public int[] GetScores()
		{
			return scores;
		}

		public void AddToScore(int i, int addition)
		{
			scores[i] += addition;
		}

		public void Move(Vector3 v)
		{
			center = center + v;
			backWall.Move(v);
			bottomWall.Move(v);
			rightWall.Move(v);
			topWall.Move(v);
			leftWall.Move(v);
			frontWall.Move(v);
			lowLimits += v;
			highLimits += v;
		}

		public void RotateShape(Vector3 r, float a)
		{
			backWall.RotateShape(r, a);
			bottomWall.RotateShape(r, a);
			rightWall.RotateShape(r, a);
			topWall.RotateShape(r, a);
			leftWall.RotateShape(r, a);
			frontWall.RotateShape(r, a);
		}

		public void RotateShapeOffset(Vector3 r, float a)
		{
			backWall.RotateShape(center, r, a);
			bottomWall.RotateShape(center, r, a);
			rightWall.RotateShape(center,r, a);
			topWall.RotateShape(center,r, a);
			leftWall.RotateShape(center,r, a);
			frontWall.RotateShape(center,r, a);
		}

		public void MoveFront(Vector3 v)
		{
			frontWall.Move(v);
		}

		public void Clear()
		{
			backWall.Clear();
			bottomWall.Clear();
			rightWall.Clear();
			topWall.Clear();
			leftWall.Clear();
			frontWall.Clear();
		}

		public void Draw()
		{
			backWall.Draw();
			bottomWall.Draw();
			rightWall.Draw();
			topWall.Draw();
			leftWall.Draw();
			frontWall.Draw();
		}
	}
}

