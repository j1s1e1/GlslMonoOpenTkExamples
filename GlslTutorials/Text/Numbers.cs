using System;
using OpenTK;

namespace GlslTutorials
{
	public class Numbers
	{
		public static float[] Zero;
	    public static float[] One;
	    public static float[] Two;
	    public static float[] Three;
	    public static float[] Four;
	    public static float[] Five;
	    public static float[] Six;
	    public static float[] Seven;
	    public static float[] Eight;
	    public static float[] Nine;
		
 		static Numbers()
		{
			FillZero();
			FillOne();
			FillTwo();
			FillThree();
			FillFour();
			FillFive();
			FillSix();
			FillSeven();
			FillEight();
			FillNine();
		}

	    private static float[] MoveX(float[] input, float distance)
	    {
	        for (int i = 0; i < input.Length; i = i + 3)
	        {
	            input[i] = input[i] + distance;
	        }
	        return input;
	    }
	
	    private static float[] MoveY(float[] input, float distance)
	    {
	        for (int i = 1; i < input.Length; i = i + 3)
	        {
	            input[i] = input[i] + distance;
	        }
	        return input;
	    }
	
	    private static float[] MoveXY(float[] input, float distanceX, float distanceY)
	    {
	        input = MoveX(input, distanceX);
	        input = MoveY(input, distanceY);
	        return input;
	    }
	
	    private static float[] SwapY(float[] input)
	    {
	        for (int i = 1; i < input.Length; i = i + 3)
	        {
	            input[i] = -input[i];
	        }
	        return input;
	    }
	
	    private static float[] SwapX(float[] input)
	    {
	        for (int i = 0; i < input.Length; i = i + 3)
	        {
	            input[i] = -input[i];
	        }
	        return input;
	    }
	
	    private static float[] ReverseRotation(float[] input)
	    {
	        float[] output = new float[input.Length];
	        for (int i = 0; i < output.Length; i++)
	        {
	            switch (i % 9)
	            {
	                case 0:
	                case 1:
	                case 2:
	                    output[i] = input[i];
	                    break;
	                case 3:
	                case 4:
	                case 5:
	                    output[i] = input[i+3];
	                    break;
	                case 6:
	                case 7:
	                case 8:
	                    output[i] = input[i-3];
	                    break;
	            }
	        }
	        return output;
	    }
		
		/*	Y0		V0		V2			
		 * 	Y1		V1		V3		
		 * 			X0		X1
		 */
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
		
		/*	Y0			V0		V2			
		 * 	Y1		V1				V3		
		 * 			X0	X1		X2	X3
		 */
	    private static float[] ZeroTop()
	    {
			float X0 = -2.5f;
			float X1 = -1.5f;
			float X2 = -X1;
			float X3 = -X0;
			float Y0 = 6f;
			float Y1 = 5f;

			Vector3 V0 = new Vector3(X1, Y0, 0f);
			Vector3 V1 = new Vector3(X0, Y1, 0f);
			Vector3 V2 = new Vector3(X2, Y0, 0f);
			Vector3 V3 = new Vector3(X3, Y1, 0f);
			
	        float[] ZeroTop = new float [18];
			AddVertex(ZeroTop, 0, V0);
			AddVertex(ZeroTop, 1, V1);
			AddVertex(ZeroTop, 2, V2);
			AddVertex(ZeroTop, 3, V2);
			AddVertex(ZeroTop, 4, V1);
			AddVertex(ZeroTop, 5, V3);
	        return ZeroTop;
	    }
	
		/*	Y0		V0		V2			
		 * 	Y1		V1		V3		
		 * 			X0		X1
		 */
	    private static float[] ZeroLeft()
	    {
			float X0 = -2.5f;
			float X1 = -1.5f;
			float Y0 = 5f;
			float Y1 = -5f;
			
			Vector3 V0 = new Vector3(X0, Y0, 0f);
			Vector3 V1 = new Vector3(X0, Y1, 0f);
			Vector3 V2 = new Vector3(X1, Y0, 0f);
			Vector3 V3 = new Vector3(X1, Y1, 0f);

	        float[] ZeroLeft = new float [18];
			AddVertex(ZeroLeft, 0, V0);
			AddVertex(ZeroLeft, 1, V1);
			AddVertex(ZeroLeft, 2, V2);
			AddVertex(ZeroLeft, 3, V2);
			AddVertex(ZeroLeft, 4, V1);
			AddVertex(ZeroLeft, 5, V3);
	        return ZeroLeft;
	    }
	
	    private static float[]  ZeroBottom()
	    {
	        float[] ZeroBottom = ZeroTop();
	        ZeroBottom = SwapY(ZeroBottom);
	        ZeroBottom = ReverseRotation(ZeroBottom);
	        return ZeroBottom;
	    }
	
	    private static float[]  ZeroRight()
	    {
	        float[] ZeroRight = ZeroLeft();
	        ZeroRight = SwapX(ZeroRight);
	        ZeroRight = ReverseRotation(ZeroRight);
	        return ZeroRight;
	    }
	
	    private static void FillZero()
	    {
	        Zero = new float [72];
	        Array.Copy(ZeroTop(),0,Zero,0,18);
	        Array.Copy(ZeroBottom(),0,Zero,18,18);
	        Array.Copy(ZeroLeft(),0,Zero,36,18);
	        Array.Copy(ZeroRight(),0,Zero,54,18);
	    }
	
	    private static void FillOne()
	    {
	        One = Rectangle(1f, 12f);
	    }
	
	    private static float[] TwoMiddle()
	    {
	        float[] TwoMiddle = FiveMiddle();
	        TwoMiddle = SwapY(TwoMiddle);
	        TwoMiddle = ReverseRotation(TwoMiddle);
	        return TwoMiddle;
	    }
	
	    private static void FillTwo()
	    {
	        Two = new float [90];
	        Array.Copy(ZeroTop(),0,Two,0,18);
	        Array.Copy(ZeroBottom(),0,Two,18,18);
	        Array.Copy(TwoMiddle(),0,Two,36,18);
	        Array.Copy(EightUpperRight(),0,Two,54,18);
	        Array.Copy(EightLowerLeft(),0,Two,72,18);
	    }
	
	    private static float[] ThreeMiddleTop()
	    {
	        float[] ThreeMiddleTop = new float[18];
	        ThreeMiddleTop[0] = -1.5f;
	        ThreeMiddleTop[1] = 1f;
	        ThreeMiddleTop[2] = 0f;
	        ThreeMiddleTop[3] = -2.5f;
	        ThreeMiddleTop[4] = 0f;
	        ThreeMiddleTop[5] = 0f;
	        ThreeMiddleTop[6] = -ThreeMiddleTop[0];
	        ThreeMiddleTop[7] = 0f;
	        ThreeMiddleTop[8] = 0f;
	
	        ThreeMiddleTop[9] = ThreeMiddleTop[0];
	        ThreeMiddleTop[10] = ThreeMiddleTop[1];
	        ThreeMiddleTop[11] = 0f;
	        ThreeMiddleTop[12] = -ThreeMiddleTop[0];
	        ThreeMiddleTop[13] = 0f;
	        ThreeMiddleTop[14] = 0f;
	        ThreeMiddleTop[15] = -ThreeMiddleTop[3];
	        ThreeMiddleTop[16] = ThreeMiddleTop[1];
	        ThreeMiddleTop[17] = 0f;
	        return ThreeMiddleTop;
	    }
	
	    private static float[] ThreeMiddleBottom()
	    {
	        float[] ThreeMiddleBottom = ThreeMiddleTop();
	        ThreeMiddleBottom = SwapY(ThreeMiddleBottom);
	        ThreeMiddleBottom = ReverseRotation(ThreeMiddleBottom);
	        return ThreeMiddleBottom;
	    }
	
	    private static float[] ThreeMiddle()
	    {
	        float[] ThreeMiddle = new float[36];
	        Array.Copy(ThreeMiddleTop(),0,ThreeMiddle,0,18);
	        Array.Copy(ThreeMiddleBottom(),0,ThreeMiddle,18,18);
	        return ThreeMiddle;
	    }
	
	    private static void FillThree()
	    {
	        Three = new float [108];
	        Array.Copy(ZeroTop(),0,Three,0,18);
	        Array.Copy(ZeroBottom(),0,Three,18,18);
	        Array.Copy(ThreeMiddle(),0,Three,36,36);
	        Array.Copy(EightUpperRight(),0,Three,72,18);
	        Array.Copy(EightLowerRight(),0,Three,90,18);
	    }
	
	    private static void FillFour()
	    {
	        Four = new float [72];
	        Array.Copy(EightUpperBottom(),0,Four,0,18);
	        Array.Copy(EightUpperLeft(),0,Four,18,18);
	        Array.Copy(EightUpperRight(),0,Four,36,18);
	        Array.Copy(NineLowerRight(),0,Four,54,18);
	    }
	
	    private static float[] FiveMiddle()
	    {
	        float[] FiveMiddle = new float[18];
	        FiveMiddle[0] = -2.5f;
	        FiveMiddle[1] = 1f;
	        FiveMiddle[2] = 0f;
	        FiveMiddle[3] = -1.5f;
	        FiveMiddle[4] = -FiveMiddle[1];
	        FiveMiddle[5] = 0f;
	        FiveMiddle[6] = -FiveMiddle[0];
	        FiveMiddle[7] =  -FiveMiddle[1];
	        FiveMiddle[8] = 0f;
	
	        FiveMiddle[9] = FiveMiddle[0];
	        FiveMiddle[10] = FiveMiddle[1];
	        FiveMiddle[11] = 0f;
	        FiveMiddle[12] = -FiveMiddle[0];
	        FiveMiddle[13] =  -FiveMiddle[1];
	        FiveMiddle[14] = 0f;
	        FiveMiddle[15] = -FiveMiddle[3];
	        FiveMiddle[16] =  FiveMiddle[1];
	        FiveMiddle[17] = 0f;
	
	        return FiveMiddle;
	    }
	
	    private static void FillFive()
	    {
	        Five = new float [90];
	        Array.Copy(ZeroTop(),0,Five,0,18);
	        Array.Copy(ZeroBottom(),0,Five,18,18);
	        Array.Copy(FiveMiddle(),0,Five,36,18);
	        Array.Copy(EightUpperLeft(),0,Five,54,18);
	        Array.Copy(EightLowerRight(),0,Five,72,18);
	    }
	
	    private static void FillSix()
	    {
	        Six = new float [108];
	        Array.Copy(ZeroTop(),0,Six,0,18);
	        Array.Copy(EightLowerTop(),0,Six,18,18);
	        Array.Copy(EightLowerLeft(),0,Six,36,18);
	        Array.Copy(EightLowerRight(),0,Six,54,18);
	        Array.Copy(SixUpperLeft(),0,Six,72,18);
	        Array.Copy(ZeroBottom(),0,Six,90,18);
	    }
	
	    private static void FillSeven()
	    {
	        Seven = new float [54];
	        Array.Copy(ZeroTop(),0,Seven,0,18);
	        Array.Copy(EightUpperRight(),0,Seven,18,18);
	        Array.Copy(NineLowerRight(),0,Seven,36,18);
	    }
	
	    private static float[] EightUpperBottom()
	    {
	        float[] EightUpperBottom = ZeroBottom();
	        EightUpperBottom = MoveY(EightUpperBottom, 6f);
	        return EightUpperBottom;
	    }
	    private static float[] EightLowerTop()
	    {
	        float[] EightLowerTop = ZeroTop();
	        EightLowerTop = MoveY(EightLowerTop, -6f);
	        return EightLowerTop;
	    }
	
	    private static float[] EightUpperLeft()
	    {
	        float[] EightUpperLeft = Rectangle(1f, 4f);
	        EightUpperLeft = MoveXY(EightUpperLeft, -2f, 3f);
	        return EightUpperLeft;
	    }
	
	    private static float[] SixUpperLeft()
	    {
	        float[] EightUpperLeft = Rectangle(1f, 6f);
	        EightUpperLeft = MoveXY(EightUpperLeft, -2f, 2f);
	        return EightUpperLeft;
	    }
	
	    private static float[] EightUpperRight()
	    {
	        float[] EightUpperRight = Rectangle(1f, 4f);
	        EightUpperRight = MoveXY(EightUpperRight, 2f, 3f);
	        return EightUpperRight;
	    }
	
	    private static float[] EightLowerLeft()
	    {
	        float[] EightUpperLeft = Rectangle(1f, 4f);
	        EightUpperLeft = MoveXY(EightUpperLeft, -2f, -3f);
	        return EightUpperLeft;
	    }
	
	    private static float[] EightLowerRight()
	    {
	        float[] EightUpperRight = Rectangle(1f, 4f);
	        EightUpperRight = MoveXY(EightUpperRight, 2f, -3f);
	        return EightUpperRight;
	    }
	
	    private static void FillEight()
	    {
	        Eight = new float [144];
	        Array.Copy(ZeroTop(),0,Eight,0,18);
	        Array.Copy(ZeroBottom(),0,Eight,18,18);
	        Array.Copy(EightUpperBottom(),0,Eight,36,18);
	        Array.Copy(EightLowerTop(),0,Eight,54,18);
	        Array.Copy(EightUpperLeft(),0,Eight,72,18);
	        Array.Copy(EightUpperRight(),0,Eight,90,18);
	        Array.Copy(EightLowerLeft(),0,Eight,108,18);
	        Array.Copy(EightLowerRight(),0,Eight,126,18);
	    }
	
	    private static float[] NineLowerRight()
	    {
	        float[] EightUpperRight = Rectangle(1f, 6f);
	        EightUpperRight = MoveXY(EightUpperRight, 2f, -2f);
	        return EightUpperRight;
	    }
	
	    private static void FillNine()
	    {
	        Nine = new float [108];
	        Array.Copy(ZeroTop(),0,Nine,0,18);
	        Array.Copy(EightUpperBottom(),0,Nine,18,18);
	        Array.Copy(EightUpperLeft(),0,Nine,36,18);
	        Array.Copy(EightUpperRight(),0,Nine,54,18);
	        Array.Copy(NineLowerRight(),0,Nine,72,18);
	        Array.Copy(ZeroBottom(),0,Nine,90,18);
	    }
	}
}

