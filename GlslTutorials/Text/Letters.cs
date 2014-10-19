using System;
using OpenTK;

namespace GlslTutorials
{
	public class Letters
	{
		public static float[] A;
		public static float[] B;
		public static float[] C;
		public static float[] D;
		public static float[] E;
		public static float[] F;
		public static float[] G;
		public static float[] H;
		public static float[] I;
		public static float[] J;
		public static float[] K;
		public static float[] L;
		public static float[] M;
		public static float[] N;
		public static float[] O;
		public static float[] P;
		public static float[] Q;
		public static float[] R;
		public static float[] S;
		public static float[] T;
		public static float[] U;
		public static float[] V;
		public static float[] W;
		public static float[] X;
		public static float[] Y;
		public static float[] Z;
		
		static Letters ()
		{
			AddA();
			AddB();
			AddC();
			AddD();
			AddE();
			AddF();
			AddG();
			AddH();
			AddI();
			AddJ();
			AddK();
			AddL();
			AddM();
			AddN();
			AddO();
			AddP();
			AddQ();
			AddR();
			AddS();
			AddT();
			AddU();
			AddV();
			AddW();
			AddX();
			AddY();
			AddZ();
		}
		
		private static float[] Rectangle(float width, float height)
	    {
			return Symbols.Rectangle(width, height);
	    }
		
		private static void AddVertex(float[] number, int vertexNumber, Vector3 vertex)
		{
			number[vertexNumber * 3 + 0] = vertex.X;
			number[vertexNumber * 3 + 1] = vertex.Y;
			number[vertexNumber * 3 + 2] = vertex.Z;
		}
		
		/*	Y0					V0		V2
		 * 
		 * 
		 * 	Y1			V6						V8
		 * 	
		 * 	Y2			V7						V9
		 * 
		 * 
		 * 	Y3	V1		V3						V4		V5
		 * 		X0		X1		X2		X3		X4		X5
		 */
		
		private static void AddA()
	    {
			float X0 = -3f;
			float X1 = -2.0f;
			float X2 = -0.5f;
			float X3 = -X2;
			float X4 = -X1;
			float X5 = -X0;			
			
			float Y0 = 6f;
			float Y1 = 0.0f;
			float Y2 = -1.0f;
			float Y3 = -6f;
			
			Vector3 V0 = new Vector3(X2, Y0, 0f);
			Vector3 V1 = new Vector3(X0, Y3, 0f);
			Vector3 V2 = new Vector3(X3, Y0, 0f);
			Vector3 V3 = new Vector3(X1, Y3, 0f);
			
			Vector3 V4 = new Vector3(X4, Y3, 0f);
			Vector3 V5 = new Vector3(X5, Y3, 0f);
			
			Vector3 V6 = new Vector3(X1, Y1, 0f);
			Vector3 V7 = new Vector3(X1, Y2, 0f);
			Vector3 V8 = new Vector3(X4, Y1, 0f);
			Vector3 V9 = new Vector3(X4, Y2, 0f);
			
	        A = new float [54];
			
			AddVertex(A, 0, V0);
			AddVertex(A, 1, V1);
			AddVertex(A, 2, V2);
			AddVertex(A, 3, V2);
			AddVertex(A, 4, V1);
			AddVertex(A, 5, V3);

			AddVertex(A, 6, V0);
			AddVertex(A, 7, V4);
			AddVertex(A, 8, V5);
			AddVertex(A, 9, V0);
			AddVertex(A, 10, V5);
			AddVertex(A, 11, V2);

			AddVertex(A, 12, V6);
			AddVertex(A, 13, V7);
			AddVertex(A, 14, V8);
			AddVertex(A, 15, V8);
			AddVertex(A, 16, V7);
			AddVertex(A, 17, V9);
	    }
		
		private static void AddB()
		{
			B = Symbols.Dash;
		}
			
		private static void AddC()
		{
			C = Symbols.Dash;
		}
		
		private static void AddD()
		{
			D = Symbols.Dash;
		}
		
		private static void AddE()
		{
			E = Symbols.Dash;
		}
		
		private static void AddF()
		{
			F = Symbols.Dash;
		}
		
		private static void AddG()
		{
			G = Symbols.Dash;
		}
		
		private static void AddH()
		{
			H = Symbols.Dash;
		}
		
		private static void AddI()
		{
			I = Symbols.Rectangle(2f, 6f);
		}
		
		private static void AddJ()
		{
			J = Symbols.Dash;
		}
		
		private static void AddK()
		{
			K = Symbols.Dash;
		}
		
		private static void AddL()
		{
			L = Symbols.Dash;
		}
		
		private static void AddM()
		{
			M = Symbols.Dash;
		}
		
		private static void AddN()
		{
			N = Symbols.Dash;
		}
		
		private static void AddO()
		{
			O = Symbols.Dash;
		}
		
		private static void AddP()
		{
			P = Symbols.Dash;
		}
		
		private static void AddQ()
		{
			Q = Symbols.Dash;
		}
		
		private static void AddR()
		{
			R = Symbols.Dash;
		}
		
		private static void AddS()
		{
			S = Symbols.Dash;
		}
		
		private static void AddT()
		{
			T = Symbols.Dash;
		}
		
		private static void AddU()
		{
			U = Symbols.Dash;
		}
		
		private static void AddV()
		{
			V = Symbols.Dash;
		}
		
		private static void AddW()
		{
			W = Symbols.Dash;
		}
		
		private static void AddX()
		{
			X = Symbols.Dash;
		}
		
		private static void AddY()
		{
			Y = Symbols.Dash;
		}
		
		private static void AddZ()
		{
			Z = Symbols.Dash;
		}
		
				
	}
}

