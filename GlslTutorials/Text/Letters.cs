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
		
		private static void AddTrianglePair(float[] number, int firstVertex, 
		                       Vector3 V0, Vector3 V1, Vector3 V2, Vector3 V3)
		{
			AddVertex(number, firstVertex++, V0);
			AddVertex(number, firstVertex++, V1);
			AddVertex(number, firstVertex++, V2);
			AddVertex(number, firstVertex++, V2);
			AddVertex(number, firstVertex++, V1);
			AddVertex(number, firstVertex++, V3);
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
			float Y1 = -0.5f;
			float Y2 = -1.5f;
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
			
			AddTrianglePair(A, 0, V0, V1, V2, V3);
			AddTrianglePair(A, 6, V0, V4, V2, V5);
			AddTrianglePair(A, 12, V6, V7, V8, V9);
	    }
		
		/*	Y0	V0		V2		V4		V6
		 *  
		 * 	Y1			V8		V9		
		 * 	
		 *  Y2			V10		V12
		 *  
		 * 	Y3			V11		V13
		 * 	
		 *  Y4			V14		V15
		 * 
		 * 	Y5	V1		V3		V5		V7
		 * 		X0		X1		X2		X3
		 */
		private static void AddB()
		{
			float X0 = -3f;
			float X1 = -2.0f;
			float X2 = -X1;
			float X3 = -X0;			
			
			float Y0 = 6f;
			float Y1 = 5.0f;
			float Y2 = 1f;
			float Y3 = -Y2;
			float Y4 = -Y1;
			float Y5 = -Y0;			
			
			
			Vector3 V0 = new Vector3(X0, Y0, 0f);
			Vector3 V1 = new Vector3(X0, Y5, 0f);
			Vector3 V2 = new Vector3(X1, Y0, 0f);
			Vector3 V3 = new Vector3(X1, Y5, 0f);
			
			Vector3 V4 = new Vector3(X2, Y0, 0f);
			Vector3 V5 = new Vector3(X2, Y5, 0f);
			Vector3 V6 = new Vector3(X3, Y0, 0f);
			Vector3 V7 = new Vector3(X3, Y5, 0f);
			
			Vector3 V8 = new Vector3(X1, Y1, 0f);
			Vector3 V9 = new Vector3(X2, Y1, 0f);
			
			Vector3 V10 = new Vector3(X1, Y2, 0f);
			Vector3 V11 = new Vector3(X1, Y3, 0f);
			Vector3 V12 = new Vector3(X2, Y2, 0f);
			Vector3 V13 = new Vector3(X2, Y3, 0f);

			Vector3 V14 = new Vector3(X1, Y4, 0f);
			Vector3 V15 = new Vector3(X2, Y4, 0f);
			
	        B = new float[90];
			AddTrianglePair(B, 0, V0, V1, V2, V3);
			AddTrianglePair(B, 6, V4, V5, V6, V7);
			AddTrianglePair(B, 12, V2, V8, V4, V9);
			AddTrianglePair(B, 18, V10, V11, V12, V13);
			AddTrianglePair(B, 24, V14, V3, V15, V5);
		}
			
				
		/*	Y0			V0		V2
		 * 
		 * 	Y1	V1		V8				V3
		 * 	
		 *  Y2	
		 * 	
		 *  Y3
		 * 	
		 *  Y4	V4		V9				V6
		 * 
		 * 	Y5			V5		V7
		 * 		X0		X1		X2		X3
		 */
		private static void AddC()
		{
			float X0 = -3f;
			float X1 = -2.0f;
			float X2 = -X1;
			float X3 = -X0;			
			
			float Y0 = 6f;
			float Y1 = 5.0f;
			float Y4 = -Y1;
			float Y5 = -Y0;			
			
			
			Vector3 V0 = new Vector3(X1, Y0, 0f);
			Vector3 V1 = new Vector3(X0, Y1, 0f);
			Vector3 V2 = new Vector3(X2, Y0, 0f);
			Vector3 V3 = new Vector3(X3, Y1, 0f);
			
			Vector3 V4 = new Vector3(X0, Y4, 0f);
			Vector3 V5 = new Vector3(X1, Y5, 0f);
			Vector3 V6 = new Vector3(X3, Y4, 0f);
			Vector3 V7 = new Vector3(X2, Y5, 0f);
			
			Vector3 V8 = new Vector3(X1, Y1, 0f);
			Vector3 V9 = new Vector3(X1, Y4, 0f);
			
	        C = new float[54];
			AddTrianglePair(C, 0, V0, V1, V2, V3);
			AddTrianglePair(C, 6, V4, V5, V6, V7);
			AddTrianglePair(C, 12, V1, V4, V8, V9);
		}
		
		/*	Y0	V0		V2		V4		V6
		 * 
		 * 	Y1			V8		V9
		 * 	
		 *  Y2			V10		V11
		 * 
		 * 	Y3	V1		V3		V5		V7
		 * 		X0		X1		X2		X3
		 */
		private static void AddD()
		{
			float X0 = -3f;
			float X1 = -2.0f;
			float X2 = -X1;
			float X3 = -X0;			
			
			float Y0 = 6f;
			float Y1 = 5.0f;
			float Y2 = -Y1;
			float Y3 = -Y0;
			
			Vector3 V0 = new Vector3(X0, Y0, 0f);
			Vector3 V1 = new Vector3(X0, Y3, 0f);
			Vector3 V2 = new Vector3(X1, Y0, 0f);
			Vector3 V3 = new Vector3(X1, Y3, 0f);
			
			Vector3 V4 = new Vector3(X2, Y0, 0f);
			Vector3 V5 = new Vector3(X2, Y3, 0f);
			Vector3 V6 = new Vector3(X3, Y0, 0f);
			Vector3 V7 = new Vector3(X3, Y3, 0f);
			
			Vector3 V8 = new Vector3(X1, Y1, 0f);
			Vector3 V9 = new Vector3(X2, Y1, 0f);
			Vector3 V10 = new Vector3(X1, Y2, 0f);
			Vector3 V11 = new Vector3(X2, Y2, 0f);
			
	        D = new float[72];
			AddTrianglePair(D, 0, V0, V1, V2, V3);
			AddTrianglePair(D, 6, V4, V5, V6, V7);
			AddTrianglePair(D, 12, V2, V8, V4, V9);
			AddTrianglePair(D, 18, V10, V3, V11, V5);
		}
		
		/*	Y0	V0		V2				V5
		 *  
		 * 	Y1			V4				V6
		 * 	
		 *  Y2			V7		V9
		 *  
		 * 	Y3			V8		V10
		 * 	
		 *  Y4			V11				V12
		 * 
		 * 	Y5	V1		V3				V13
		 * 		X0		X1		X2		X3
		 */
		private static void AddE()
		{
			float X0 = -3f;
			float X1 = -2.0f;
			float X2 = -X1;
			float X3 = -X0;			
			
			float Y0 = 6f;
			float Y1 = 5.0f;
			float Y2 = 1f;
			float Y3 = -Y2;
			float Y4 = -Y1;
			float Y5 = -Y0;			
			
			
			Vector3 V0 = new Vector3(X0, Y0, 0f);
			Vector3 V1 = new Vector3(X0, Y5, 0f);
			Vector3 V2 = new Vector3(X1, Y0, 0f);
			Vector3 V3 = new Vector3(X1, Y5, 0f);
			
			Vector3 V4 = new Vector3(X1, Y1, 0f);
			Vector3 V5 = new Vector3(X3, Y0, 0f);
			Vector3 V6 = new Vector3(X3, Y1, 0f);
			
			Vector3 V7 = new Vector3(X1, Y2, 0f);
			Vector3 V8 = new Vector3(X1, Y3, 0f);
			Vector3 V9 = new Vector3(X2, Y2, 0f);
			Vector3 V10 = new Vector3(X2, Y3, 0f);
			
			Vector3 V11 = new Vector3(X1, Y4, 0f);
			Vector3 V12 = new Vector3(X3, Y4, 0f);
			Vector3 V13 = new Vector3(X3, Y5, 0f);
			
	        E = new float[72];
			AddTrianglePair(E, 0, V0, V1, V2, V3);
			AddTrianglePair(E, 6, V2, V4, V5, V6);
			AddTrianglePair(E, 12, V7, V8, V9, V10);
			AddTrianglePair(E, 18, V11, V3, V12, V13);
		}
		
		/*	Y0	V0		V2				V5
		 *  
		 * 	Y1			V4				V6
		 * 	
		 *  Y2			V7		V9
		 *  
		 * 	Y3			V8		V10
		 * 	
		 *  Y4							
		 * 
		 * 	Y5	V1		V3				
		 * 		X0		X1		X2		X3
		 */
		private static void AddF()
		{
			float X0 = -3f;
			float X1 = -2.0f;
			float X2 = -X1;
			float X3 = -X0;			
			
			float Y0 = 6f;
			float Y1 = 5.0f;
			float Y2 = 1f;
			float Y3 = -Y2;
			float Y5 = -Y0;			
			
			Vector3 V0 = new Vector3(X0, Y0, 0f);
			Vector3 V1 = new Vector3(X0, Y5, 0f);
			Vector3 V2 = new Vector3(X1, Y0, 0f);
			Vector3 V3 = new Vector3(X1, Y5, 0f);
			
			Vector3 V4 = new Vector3(X1, Y1, 0f);
			Vector3 V5 = new Vector3(X3, Y0, 0f);
			Vector3 V6 = new Vector3(X3, Y1, 0f);
			
			Vector3 V7 = new Vector3(X1, Y2, 0f);
			Vector3 V8 = new Vector3(X1, Y3, 0f);
			Vector3 V9 = new Vector3(X2, Y2, 0f);
			Vector3 V10 = new Vector3(X2, Y3, 0f);
			
	        F = new float[54];
			AddTrianglePair(F, 0, V0, V1, V2, V3);
			AddTrianglePair(F, 6, V2, V4, V5, V6);
			AddTrianglePair(F, 12, V7, V8, V9, V10);
		}
		
		/*	Y0			V0				V2
		 * 
		 * 	Y1	V1		V8						V3
		 * 	
		 *  Y2					X10				X12
		 * 	
		 *  Y3					X11		X14		X13
		 * 					
		 *  Y4	V4		V9				X15		V6
		 * 
		 * 	Y5			V5				V7
		 * 		X0		X1		X2		X3		X4
		 */
		private static void AddG()
		{
			float X0 = -3f;
			float X1 = -2.0f;
			float X2 = 0f;
			float X3 = -X1;
			float X4 = -X0;			
			
			float Y0 = 6f;
			float Y1 = 5.0f;
			float Y2 = 0.5f;
			float Y3 = -Y2;		
			float Y4 = -Y1;
			float Y5 = -Y0;			
			
			
			Vector3 V0 = new Vector3(X1, Y0, 0f);
			Vector3 V1 = new Vector3(X0, Y1, 0f);
			Vector3 V2 = new Vector3(X3, Y0, 0f);
			Vector3 V3 = new Vector3(X4, Y1, 0f);
			
			Vector3 V4 = new Vector3(X0, Y4, 0f);
			Vector3 V5 = new Vector3(X1, Y5, 0f);
			Vector3 V6 = new Vector3(X4, Y4, 0f);
			Vector3 V7 = new Vector3(X3, Y5, 0f);
			
			Vector3 V8 = new Vector3(X1, Y1, 0f);
			Vector3 V9 = new Vector3(X1, Y4, 0f);
			
			Vector3 V10 = new Vector3(X2, Y2, 0f);
			Vector3 V11 = new Vector3(X2, Y3, 0f);
			Vector3 V12 = new Vector3(X4, Y2, 0f);
			Vector3 V13 = new Vector3(X4, Y3, 0f);
			
			Vector3 V14 = new Vector3(X3, Y3, 0f);
			Vector3 V15 = new Vector3(X3, Y4, 0f);
			
			
	        G = new float[90];
			AddTrianglePair(G, 0, V0, V1, V2, V3);
			AddTrianglePair(G, 6, V4, V5, V6, V7);
			AddTrianglePair(G, 12, V1, V4, V8, V9);
			AddTrianglePair(G, 18, V10, V11, V12, V13);
			AddTrianglePair(G, 24, V14, V15, V13, V6);
		}
		
		/*	Y0	V0		V2		V4		V6
		 * 
		 * 	Y1			V8		V10					
		 * 	
		 *  Y2			V9		V11		
		 * 	
		 *  Y3	V1		V3		V5		V7
		 * 					
		 * 		X0		X1		X2		X3
		 */
		private static void AddH()
		{
			float X0 = -3f;
			float X1 = -2.0f;
			float X2 = -X1;
			float X3 = -X0;			
			
			float Y0 = 6f;
			float Y1 = 0.5f;	
			float Y2 = -Y1;
			float Y3 = -Y0;			
			
			
			Vector3 V0 = new Vector3(X0, Y0, 0f);
			Vector3 V1 = new Vector3(X0, Y3, 0f);
			Vector3 V2 = new Vector3(X1, Y0, 0f);
			Vector3 V3 = new Vector3(X1, Y3, 0f);
			
			Vector3 V4 = new Vector3(X2, Y0, 0f);
			Vector3 V5 = new Vector3(X2, Y3, 0f);
			Vector3 V6 = new Vector3(X3, Y0, 0f);
			Vector3 V7 = new Vector3(X3, Y3, 0f);
			
			Vector3 V8 = new Vector3(X1, Y1, 0f);
			Vector3 V9 = new Vector3(X1, Y2, 0f);
			Vector3 V10 = new Vector3(X2, Y1, 0f);
			Vector3 V11 = new Vector3(X2, Y2, 0f);	
			
	        H = new float[54];
			AddTrianglePair(H, 0, V0, V1, V2, V3);
			AddTrianglePair(H, 6, V4, V5, V6, V7);
			AddTrianglePair(H, 12, V8, V9, V10, V11);
		}
		
		private static void AddI()
		{
			I = Symbols.Rectangle(1.5f, 12f);
		}
		
		/*	Y0					V0		V2
		 * 
		 * 
		 * 
		 * 	Y1	V7		V8							
		 * 	
		 *  Y2	V4		V9		V1		V3	
		 * 	
		 *  Y3			V5		V6		
		 * 						
		 * 		X0		X1		X2		X3
		 */
		private static void AddJ()
		{
			float X0 = -3f;
			float X1 = -2.0f;
			float X2 = -X1;
			float X3 = -X0;			
			
			float Y0 = 6f;
			float Y1 = -2f;	
			float Y2 = -5f;
			float Y3 = -Y0;			
			
			
			Vector3 V0 = new Vector3(X2, Y0, 0f);
			Vector3 V1 = new Vector3(X2, Y2, 0f);
			Vector3 V2 = new Vector3(X3, Y0, 0f);
			Vector3 V3 = new Vector3(X3, Y2, 0f);
			
			Vector3 V4 = new Vector3(X0, Y2, 0f);
			Vector3 V5 = new Vector3(X1, Y3, 0f);
			Vector3 V6 = new Vector3(X2, Y3, 0f);

			Vector3 V7 = new Vector3(X0, Y1, 0f);
			Vector3 V8 = new Vector3(X1, Y1, 0f);
			Vector3 V9 = new Vector3(X1, Y2, 0f);	
			
	        J = new float[54];
			AddTrianglePair(J, 0, V0, V1, V2, V3);
			AddTrianglePair(J, 6, V4, V5, V3, V6);
			AddTrianglePair(J, 12, V7, V4, V8, V9);
		}
		
		/*	Y0	V0		V2		V4		V6
		 * 
		 * 	Y1			V5							
		 * 	
		 *  Y2			V7
		 * 	
		 *  Y3	V1		V3		V9		V8
		 * 						
		 * 		X0		X1		X2		X3
		 */
		private static void AddK()
		{
			float X0 = -3f;
			float X1 = -2.0f;
			float X2 = 1.75f;
			float X3 = -X0;			
			
			float Y0 = 6f;
			float Y1 = -0.75f;	
			float Y2 = -Y1;
			float Y3 = -Y0;			
			
			Vector3 V0 = new Vector3(X0, Y0, 0f);
			Vector3 V1 = new Vector3(X0, Y3, 0f);
			Vector3 V2 = new Vector3(X1, Y0, 0f);
			Vector3 V3 = new Vector3(X1, Y3, 0f);
			
			Vector3 V4 = new Vector3(X2, Y0, 0f);
			Vector3 V5 = new Vector3(X1, Y1, 0f);
			Vector3 V6 = new Vector3(X3, Y0, 0f);
			Vector3 V7 = new Vector3(X1, Y2, 0f);
			
			Vector3 V8 = new Vector3(X3, Y3, 0f);
			Vector3 V9 = new Vector3(X2, Y3, 0f);	
			
	        K = new float[54];
			AddTrianglePair(K, 0, V0, V1, V2, V3);
			AddTrianglePair(K, 6, V4, V5, V6, V7);
			AddTrianglePair(K, 12, V5, V7, V8, V9);
		}
		
		/*	Y0	V0		V2		
		 * 
		 * 	Y1			V4		V5
		 * 
		 * 	Y2	V1		V3		V6
		 * 		X0		X1		X2
		 */
		private static void AddL()
		{
			float X0 = -3f;
			float X1 = -2.0f;
			float X2 = -X0;		
			
			float Y0 = 6f;
			float Y1 = -5f;
			float Y2 = -6f;
			
			Vector3 V0 = new Vector3(X0, Y0, 0f);
			Vector3 V1 = new Vector3(X0, Y2, 0f);
			Vector3 V2 = new Vector3(X1, Y0, 0f);
			Vector3 V3 = new Vector3(X1, Y2, 0f);
			
			Vector3 V4 = new Vector3(X1, Y1, 0f);
			Vector3 V5 = new Vector3(X2, Y1, 0f);		
			Vector3 V6 = new Vector3(X2, Y2, 0f);
			
	        L = new float[36];
			AddTrianglePair(L, 0, V0, V1, V2, V3);
			AddTrianglePair(L, 6, V4, V3, V5, V6);
		}
		
		/*	Y0	V0		V2						V6		V8
		 * 
		 * 
		 * 	Y1					V4		V5
		 * 	
		 * 
		 * 	Y2	V1		V3						V7		V9
		 * 		X0		X1		X2		X3		X4		X5
		 */
		private static void AddM()
		{
			float X0 = -3f;
			float X1 = -2.0f;
			float X2 = -0.5f;
			float X3 = -X2;
			float X4 = -X1;
			float X5 = -X0;			
			
			float Y0 = 6f;
			float Y1 = 0.0f;
			float Y2 = -6f;
			
			Vector3 V0 = new Vector3(X0, Y0, 0f);
			Vector3 V1 = new Vector3(X0, Y2, 0f);
			Vector3 V2 = new Vector3(X1, Y0, 0f);
			Vector3 V3 = new Vector3(X1, Y2, 0f);
			
			Vector3 V4 = new Vector3(X2, Y1, 0f);
			Vector3 V5 = new Vector3(X3, Y1, 0f);
			
			Vector3 V6 = new Vector3(X4, Y0, 0f);
			Vector3 V7 = new Vector3(X4, Y2, 0f);
			Vector3 V8 = new Vector3(X5, Y0, 0f);
			Vector3 V9 = new Vector3(X5, Y2, 0f);
			
	        M = new float[72];
			AddTrianglePair(M, 0, V0, V1, V2, V3);
			AddTrianglePair(M, 6, V0, V4, V2, V5);
			AddTrianglePair(M, 12, V6, V4, V8, V5);
			AddTrianglePair(M, 18, V6, V7, V8, V9);
		}
		
		/*	Y0	V0		V2		V4		V6
		 * 
		 * 
		 * 	
		 * 
		 * 	Y1	V1		V3		V5		V7
		 * 		X0		X1		X2		X3
		 */
		private static void AddN()
		{
			float X0 = -3f;
			float X1 = -2.0f;
			float X2 = -X1;
			float X3 = -X0;			
			
			float Y0 = 6f;
			float Y1 = -6f;
			
			Vector3 V0 = new Vector3(X0, Y0, 0f);
			Vector3 V1 = new Vector3(X0, Y1, 0f);
			Vector3 V2 = new Vector3(X1, Y0, 0f);
			Vector3 V3 = new Vector3(X1, Y1, 0f);
			
			Vector3 V4 = new Vector3(X2, Y0, 0f);
			Vector3 V5 = new Vector3(X2, Y1, 0f);
			Vector3 V6 = new Vector3(X3, Y0, 0f);
			Vector3 V7 = new Vector3(X3, Y1, 0f);
		
	        N = new float[54];
			AddTrianglePair(N, 0, V0, V1, V2, V3);
			AddTrianglePair(N, 6, V4, V5, V6, V7);
			AddTrianglePair(N, 12, V0, V5, V2, V7);
		}
		
		/*	Y0	V0		V2		V4		V6
		 * 
		 * 	Y1			V8		V9
		 * 	
		 *  Y2			V10		V11
		 * 
		 * 	Y3	V1		V3		V5		V7
		 * 		X0		X1		X2		X3
		 */
		private static void AddO()
		{
			float X0 = -3f;
			float X1 = -2.0f;
			float X2 = -X1;
			float X3 = -X0;			
			
			float Y0 = 6f;
			float Y1 = 5.0f;
			float Y2 = -Y1;
			float Y3 = -Y0;
			
			Vector3 V0 = new Vector3(X0, Y0, 0f);
			Vector3 V1 = new Vector3(X0, Y3, 0f);
			Vector3 V2 = new Vector3(X1, Y0, 0f);
			Vector3 V3 = new Vector3(X1, Y3, 0f);
			
			Vector3 V4 = new Vector3(X2, Y0, 0f);
			Vector3 V5 = new Vector3(X2, Y3, 0f);
			Vector3 V6 = new Vector3(X3, Y0, 0f);
			Vector3 V7 = new Vector3(X3, Y3, 0f);
			
			Vector3 V8 = new Vector3(X1, Y1, 0f);
			Vector3 V9 = new Vector3(X2, Y1, 0f);
			Vector3 V10 = new Vector3(X1, Y2, 0f);
			Vector3 V11 = new Vector3(X2, Y2, 0f);
			
	        O = new float[72];
			AddTrianglePair(O, 0, V0, V1, V2, V3);
			AddTrianglePair(O, 6, V4, V5, V6, V7);
			AddTrianglePair(O, 12, V2, V8, V4, V9);
			AddTrianglePair(O, 18, V10, V3, V11, V5);
		}
		
		/*	Y0	V0		V2		V4		
		 *  
		 * 	Y1			V8		V9		V6		
		 * 	
		 *  Y2			V10		V12		V7
		 *  
		 * 	Y3			V11		V5
		 * 	
		 *  Y4					
		 * 
		 * 	Y5	V1		V3				
		 * 		X0		X1		X2		X3
		 */
		private static void AddP()
		{
			float X0 = -3f;
			float X1 = -2.0f;
			float X2 = -X1;
			float X3 = -X0;			
			
			float Y0 = 6f;
			float Y1 = 5.0f;
			float Y2 = 1f;
			float Y3 = -Y2;
			float Y5 = -Y0;			
			
			
			Vector3 V0 = new Vector3(X0, Y0, 0f);
			Vector3 V1 = new Vector3(X0, Y5, 0f);
			Vector3 V2 = new Vector3(X1, Y0, 0f);
			Vector3 V3 = new Vector3(X1, Y5, 0f);
			
			Vector3 V4 = new Vector3(X2, Y0, 0f);
			Vector3 V5 = new Vector3(X2, Y3, 0f);
			Vector3 V6 = new Vector3(X3, Y1, 0f);
			Vector3 V7 = new Vector3(X3, Y2, 0f);
			
			Vector3 V8 = new Vector3(X1, Y1, 0f);
			Vector3 V9 = new Vector3(X2, Y1, 0f);
			
			Vector3 V10 = new Vector3(X1, Y2, 0f);
			Vector3 V11 = new Vector3(X1, Y3, 0f);
			Vector3 V12 = new Vector3(X2, Y2, 0f);
			
	        P = new float[72];
			AddTrianglePair(P, 0, V0, V1, V2, V3);
			AddTrianglePair(P, 6, V4, V5, V6, V7);
			AddTrianglePair(P, 12, V2, V8, V4, V9);
			AddTrianglePair(P, 18, V10, V11, V12, V5);
		}
		
		/*	Y0			V2		V4		
		 *
		 * 	Y1	V0		V8		V9		V6
		 * 		 	
		 *  Y2				V12
		 * 	
		 *  Y3	V1		V10		V11	V14	V7
		 * 	
		 *  Y4				V13			
		 * 
		 * 	Y5			V3		V5	V15		
		 * 		X0		X1	X2	X3	X4	X5
		 */
		private static void AddQ()
		{
			float X0 = -3f;
			float X1 = -1.5f;
			float X2 = 0f;
			float X3 = -X1;
			float X4 = 2.5f;
			float X5 = -X0;			
			
			float Y0 = 6f;
			float Y1 = 5.0f;
			float Y2 = 0f;
			float Y3 = -Y1;			
			float Y4 = -5.5f;
			float Y5 = -Y0;
			
			Vector3 V0 = new Vector3(X0, Y1, 0f);
			Vector3 V1 = new Vector3(X0, Y3, 0f);
			Vector3 V2 = new Vector3(X1, Y0, 0f);
			Vector3 V3 = new Vector3(X1, Y5, 0f);
			
			Vector3 V4 = new Vector3(X3, Y0, 0f);
			Vector3 V5 = new Vector3(X3, Y5, 0f);
			Vector3 V6 = new Vector3(X5, Y1, 0f);
			Vector3 V7 = new Vector3(X5, Y3, 0f);
			
			Vector3 V8 = new Vector3(X1, Y1, 0f);
			Vector3 V9 = new Vector3(X3, Y1, 0f);
			
			Vector3 V10 = new Vector3(X1, Y3, 0f);
			Vector3 V11 = new Vector3(X3, Y3, 0f);
			
			Vector3 V12 = new Vector3(X2, Y2, 0f);
			Vector3 V13 = new Vector3(X2, Y4, 0f);
			Vector3 V14 = new Vector3(X4, Y3, 0f);
			Vector3 V15 = new Vector3(X4, Y5, 0f);
			
	        Q = new float[90];
			AddTrianglePair(Q, 0, V0, V1, V2, V3);
			AddTrianglePair(Q, 6, V4, V5, V6, V7);
			AddTrianglePair(Q, 12, V2, V8, V4, V9);
			AddTrianglePair(Q, 18, V10, V3, V11, V5);
			AddTrianglePair(Q, 24, V12, V13, V14, V15);
		}
		
		/*	Y0	V0		V2		V4		
		 *  
		 * 	Y1			V8		V9		V6		
		 * 	
		 *  Y2			V10		V12
		 *  
		 * 	Y3			V11		V13
		 * 	
		 *  Y4					
		 * 
		 * 	Y5	V1		V3		V5		V7
		 * 		X0		X1		X2		X3
		 */
		private static void AddR()
		{
			float X0 = -3f;
			float X1 = -2.0f;
			float X2 = -X1;
			float X3 = -X0;			
			
			float Y0 = 6f;
			float Y1 = 5.0f;
			float Y2 = 1f;
			float Y3 = -Y2;
			float Y5 = -Y0;			
			
			
			Vector3 V0 = new Vector3(X0, Y0, 0f);
			Vector3 V1 = new Vector3(X0, Y5, 0f);
			Vector3 V2 = new Vector3(X1, Y0, 0f);
			Vector3 V3 = new Vector3(X1, Y5, 0f);
			
			Vector3 V4 = new Vector3(X2, Y0, 0f);
			Vector3 V5 = new Vector3(X2, Y5, 0f);
			Vector3 V6 = new Vector3(X3, Y1, 0f);
			Vector3 V7 = new Vector3(X3, Y5, 0f);
			
			Vector3 V8 = new Vector3(X1, Y1, 0f);
			Vector3 V9 = new Vector3(X2, Y1, 0f);
			
			Vector3 V10 = new Vector3(X1, Y2, 0f);
			Vector3 V11 = new Vector3(X1, Y3, 0f);
			Vector3 V12 = new Vector3(X2, Y2, 0f);
			Vector3 V13 = new Vector3(X2, Y3, 0f);
			
	        R = new float[72];
			AddTrianglePair(R, 0, V0, V1, V2, V3);
			AddTrianglePair(R, 6, V4, V5, V6, V7);
			AddTrianglePair(R, 12, V2, V8, V4, V9);
			AddTrianglePair(R, 18, V10, V11, V12, V13);
		}
		
		/*	Y0			V0		V2
		 *  
		 * 	Y1	V1		V12				V3		
		 * 	
		 *  Y2	V8		V13		V10	
		 *  
		 * 	Y3			V9		V14		V11	
		 * 	
		 *  Y4	V4				V15		V6
		 * 
		 * 	Y5			V5		V7
		 * 		X0		X1		X2		X3
		 */
		private static void AddS()
		{
			float X0 = -3f;
			float X1 = -2.0f;
			float X2 = -X1;
			float X3 = -X0;			
			
			float Y0 = 6f;
			float Y1 = 5.0f;
			float Y2 = 1f;
			float Y3 = -Y2;
			float Y4 = -Y1;
			float Y5 = -Y0;			
			
			
			Vector3 V0 = new Vector3(X1, Y0, 0f);
			Vector3 V1 = new Vector3(X0, Y1, 0f);
			Vector3 V2 = new Vector3(X2, Y0, 0f);
			Vector3 V3 = new Vector3(X3, Y1, 0f);
			
			Vector3 V4 = new Vector3(X0, Y4, 0f);
			Vector3 V5 = new Vector3(X1, Y5, 0f);
			Vector3 V6 = new Vector3(X3, Y4, 0f);
			Vector3 V7 = new Vector3(X2, Y5, 0f);
			
			Vector3 V8 = new Vector3(X0, Y2, 0f);
			Vector3 V9 = new Vector3(X1, Y3, 0f);
			Vector3 V10 = new Vector3(X2, Y2, 0f);
			Vector3 V11 = new Vector3(X3, Y3, 0f);
			
			Vector3 V12 = new Vector3(X1, Y1, 0f);
			Vector3 V13 = new Vector3(X1, Y2, 0f);
			
			Vector3 V14 = new Vector3(X2, Y3, 0f);
			Vector3 V15 = new Vector3(X2, Y4, 0f);			
			
	        S = new float[90];
			AddTrianglePair(S, 0, V0, V1, V2, V3);
			AddTrianglePair(S, 6, V4, V5, V6, V7);
			AddTrianglePair(S, 12, V8, V9, V10, V11);
			AddTrianglePair(S, 18, V1, V8, V12, V13);
			AddTrianglePair(S, 24, V14, V15, V11, V6);
		}
		
		/*	Y0	V0						V2				
		 *  
		 * 	Y1	V1		V4		V5		V3		
		 * 
		 * 	Y2	 		V6		V7		
		 *
		 * 		X0		X1		X2		X3
		 */
		private static void AddT()
		{
			float X0 = -3f;
			float X1 = -0.5f;
			float X2 = -X1;
			float X3 = -X0;			
			
			float Y0 = 6f;
			float Y1 = 5.0f;
			float Y2 = -Y0;			
			
			
			Vector3 V0 = new Vector3(X0, Y0, 0f);
			Vector3 V1 = new Vector3(X0, Y1, 0f);
			Vector3 V2 = new Vector3(X3, Y0, 0f);
			Vector3 V3 = new Vector3(X3, Y1, 0f);
			
			Vector3 V4 = new Vector3(X1, Y1, 0f);
			Vector3 V5 = new Vector3(X1, Y2, 0f);
			Vector3 V6 = new Vector3(X2, Y1, 0f);
			Vector3 V7 = new Vector3(X2, Y2, 0f);
					
	        T = new float[36];
			
			AddTrianglePair(T, 0, V0, V1, V2, V3);
			AddTrianglePair(T, 6, V4, V5, V6, V7);
		}
		/*	Y0	V0		V2		V4		V6				
		 *  
		 * 	Y1	V1		V8		V9		V7					
		 *  
		 * 	Y2			V3		V5	
		 *  
		 * 		X0		X1		X2		X3
		 */
		private static void AddU()
		{
			float X0 = -3f;
			float X1 = -2.0f;
			float X2 = -X1;
			float X3 = -X0;			
			
			float Y0 = 6f;
			float Y1 = -5f;
			float Y2 = -Y0;
			
			Vector3 V0 = new Vector3(X0, Y0, 0f);
			Vector3 V1 = new Vector3(X0, Y1, 0f);
			Vector3 V2 = new Vector3(X1, Y0, 0f);
			Vector3 V3 = new Vector3(X1, Y2, 0f);
			
			Vector3 V4 = new Vector3(X2, Y0, 0f);
			Vector3 V5 = new Vector3(X2, Y2, 0f);
			Vector3 V6 = new Vector3(X3, Y0, 0f);
			Vector3 V7 = new Vector3(X3, Y1, 0f);
			
			Vector3 V8 = new Vector3(X1, Y1, 0f);
			Vector3 V9 = new Vector3(X2, Y1, 0f);
			
	        U = new float[54];
			AddTrianglePair(U, 0, V0, V1, V2, V3);
			AddTrianglePair(U, 6, V4, V5, V6, V7);
			AddTrianglePair(U, 12, V8, V3, V9, V5);
		}
		
		/*	Y0	V0		V2						V4		V5				
		 *  
		 * 
		 * 
		 * 	Y1					V1		V3		
		 *  
		 * 		X0		X1		X2		X3		X4		X5
		 */
		private static void AddV()
		{
			float X0 = -3f;
			float X1 = -2.0f;
			float X2 = -0.5f;
			float X3 = -X2;
			float X4 = -X1;
			float X5 = -X0;			
			
			float Y0 = 6f;
			float Y1 = -Y0;
			
			Vector3 V0 = new Vector3(X0, Y0, 0f);
			Vector3 V1 = new Vector3(X2, Y1, 0f);
			Vector3 V2 = new Vector3(X1, Y0, 0f);
			Vector3 V3 = new Vector3(X3, Y1, 0f);
			
			Vector3 V4 = new Vector3(X4, Y0, 0f);
			Vector3 V5 = new Vector3(X5, Y0, 0f);
			
	        V = new float[36];
			AddTrianglePair(V, 0, V0, V1, V2, V3);
			AddTrianglePair(V, 6, V4, V1, V5, V3);
		}
		
		/*	Y0	V0		V2						V6		V8
		 * 
		 * 
		 * 	Y1					V4		V5
		 * 	
		 * 
		 * 	Y2		V1		V3				V7		V9
		 * 		X0	X1	X2	X3	X4		X5	X6	X7	X8	X9
		 */
		private static void AddW()
		{
			float X0 = -3f;
			float X1 = -2.5f;
			float X2 = -2f;
			float X3 = -1.5f;
			float X4 = -0.5f;
			float X5 = -X4;		
			float X6 = -X3;
			float X7 = -X2;
			float X8 = -X1;
			float X9 = -X0;
			
			float Y0 = 6f;
			float Y1 = 0.0f;
			float Y2 = -6f;
			
			Vector3 V0 = new Vector3(X0, Y0, 0f);
			Vector3 V1 = new Vector3(X1, Y2, 0f);
			Vector3 V2 = new Vector3(X2, Y0, 0f);
			Vector3 V3 = new Vector3(X3, Y2, 0f);
			
			Vector3 V4 = new Vector3(X4, Y1, 0f);
			Vector3 V5 = new Vector3(X5, Y1, 0f);
			
			Vector3 V6 = new Vector3(X7, Y0, 0f);
			Vector3 V7 = new Vector3(X6, Y2, 0f);
			Vector3 V8 = new Vector3(X9, Y0, 0f);
			Vector3 V9 = new Vector3(X8, Y2, 0f);
			
	        W = new float[72];
			AddTrianglePair(W, 0, V0, V1, V2, V3);
			AddTrianglePair(W, 6, V4, V1, V5, V3);
			AddTrianglePair(W, 12, V4, V7, V5, V9);
			AddTrianglePair(W, 18, V6, V7, V8, V9);
		}
		
		/*	Y0	V0		V2		V4		V6				
		 *  
		 * 	Y1	V5		V7		V1		V3		
		 *  
		 * 		X0		X1		X2		X3
		 */
		private static void AddX()
		{
			float X0 = -3f;
			float X1 = -2.0f;
			float X2 = -X1;
			float X3 = -X0;			
			
			float Y0 = 6f;
			float Y1 = -Y0;
			
			Vector3 V0 = new Vector3(X0, Y0, 0f);
			Vector3 V1 = new Vector3(X2, Y1, 0f);
			Vector3 V2 = new Vector3(X1, Y0, 0f);
			Vector3 V3 = new Vector3(X3, Y1, 0f);
			
			Vector3 V4 = new Vector3(X2, Y0, 0f);
			Vector3 V5 = new Vector3(X0, Y1, 0f);
			Vector3 V6 = new Vector3(X3, Y0, 0f);
			Vector3 V7 = new Vector3(X1, Y1, 0f);
			
	        X = new float[36];
			AddTrianglePair(X, 0, V0, V1, V2, V3);
			AddTrianglePair(X, 6, V4, V5, V6, V7);
		}
		
		/*	Y0	V0		V2						V4		V5				
		 *  
		 * 	Y1					V1		V3		
		 * 	
		 *  Y2					V6		V7
		 *  
		 * 		X0		X1		X2		X3		X4		X5
		 */
		private static void AddY()
		{
			float X0 = -3f;
			float X1 = -2.0f;
			float X2 = -0.5f;
			float X3 = -X2;
			float X4 = -X1;
			float X5 = -X0;			
			
			float Y0 = 6f;
			float Y1 = 1f;
			float Y2 = -6f;
			
			Vector3 V0 = new Vector3(X0, Y0, 0f);
			Vector3 V1 = new Vector3(X2, Y1, 0f);
			Vector3 V2 = new Vector3(X1, Y0, 0f);
			Vector3 V3 = new Vector3(X3, Y1, 0f);
			
			Vector3 V4 = new Vector3(X4, Y0, 0f);
			Vector3 V5 = new Vector3(X5, Y0, 0f);
			
			Vector3 V6 = new Vector3(X2, Y2, 0f);
			Vector3 V7 = new Vector3(X3, Y2, 0f);
			
	        Y = new float[54];
			AddTrianglePair(Y, 0, V0, V1, V2, V3);
			AddTrianglePair(Y, 6, V4, V1, V5, V3);
			AddTrianglePair(Y, 12, V1, V6, V3, V7);
		}
		
		/*	Y0	V0						V2			
		 *  
		 * 	Y1	V1						V3		
		 * 	
		 *  Y2	V4						V6
		 *  
		 *  Y3	V5						V7
		 *  
		 * 		X0						X1
		 */
		private static void AddZ()
		{
			float X0 = -3f;
			float X1 = -X0;			
			
			float Y0 = 6f;
			float Y1 = 5f;
			float Y2 = -Y1;
			float Y3 = -Y0;
			
			Vector3 V0 = new Vector3(X0, Y0, 0f);
			Vector3 V1 = new Vector3(X0, Y1, 0f);
			Vector3 V2 = new Vector3(X1, Y0, 0f);
			Vector3 V3 = new Vector3(X1, Y1, 0f);
			
			Vector3 V4 = new Vector3(X0, Y2, 0f);
			Vector3 V5 = new Vector3(X0, Y3, 0f);
			Vector3 V6 = new Vector3(X1, Y2, 0f);
			Vector3 V7 = new Vector3(X1, Y3, 0f);
			
	        Z = new float[54];
			AddTrianglePair(Z, 0, V0, V1, V2, V3);
			AddTrianglePair(Z, 6, V4, V5, V6, V7);
			AddTrianglePair(Z, 12, V4, V5, V2, V3);
		}		
	}
}

