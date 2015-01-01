using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class TextClass : Shape
	{
		string VertexShader = VertexShaders.PosOnlyWorldTransform_vert;
		string FragmentShader = FragmentShaders.ColorUniform_frag;
		
		int progarmNumber;
	
	    float scaleFactor;
	    float letter_offset;
	    int current_letter;
		
		bool staticText = false;
		bool reverseRotation = true;
		
		bool updateLock = false;
    	bool waitingForUpdate = false;
		
	    private float[] SwapX(float[] input)
	    {
	        for (int i = 0; i < input.Length; i = i + 3)
	        {
	            input[i] = -input[i];
	        }
	        return input;
	    }
	
	    private float[] SwapY(float[] input)
	    {
	        for (int i = 1; i < input.Length; i = i + 3)
	        {
	            input[i] = -input[i];
	        }
	        return input;
	    }
	
	    private float[] ReverseRotation(float[] input)
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
	
	    private float[] MoveX(float[] input, float distance)
	    {
	        for (int i = 0; i < input.Length; i = i + 3)
	        {
	            input[i] = input[i] + distance;
	        }
	        return input;
	    }
	
	    private float[] MoveY(float[] input, float distance)
	    {
	        for (int i = 1; i < input.Length; i = i + 3)
	        {
	            input[i] = input[i] + distance;
	        }
	        return input;
	    }
	
	    private float[] MoveXY(float[] input, float distanceX, float distanceY)
	    {
	        input = MoveX(input, distanceX);
	        input = MoveY(input, distanceY);
	        return input;
	    }
	
	    private float[] Rectangle(float width, float height)
	    {
			return Symbols.Rectangle(width, height);
	    }
	
	    private float[] GetChar(char next_char)
	    {
	        float[] result;
	        switch (next_char)
	        {
	            case (char)'A': result = Letters.A; break;
				case (char)'B': result = Letters.B; break;
				case (char)'C': result = Letters.C; break;
				case (char)'D': result = Letters.D; break;
				case (char)'E': result = Letters.E; break;
				case (char)'F': result = Letters.F; break;
				case (char)'G': result = Letters.G; break;
				case (char)'H': result = Letters.H; break;
				case (char)'I': result = Letters.I; break;
				case (char)'J': result = Letters.J; break;
				case (char)'K': result = Letters.K; break;
	            case (char)'L': result = Letters.L; break;
				case (char)'M': result = Letters.M; break;
				case (char)'N': result = Letters.N; break;
				case (char)'O': result = Letters.O; break;
				case (char)'P': result = Letters.P; break;
				case (char)'Q': result = Letters.Q; break;
				case (char)'R': result = Letters.R; break;
				case (char)'S': result = Letters.S; break;
				case (char)'T': result = Letters.T; break;
				case (char)'U': result = Letters.U; break;
				case (char)'V': result = Letters.V; break;
				case (char)'W': result = Letters.W; break;
				case (char)'X': result = Letters.X; break;
				case (char)'Y': result = Letters.Y; break;
				case (char)'Z': result = Letters.Z; break;
				
	            case (char)'0': result = Numbers.Zero; break;
	            case (char)'1': result = Numbers.One; break;
	            case (char)'2': result = Numbers.Two; break;
	            case (char)'3': result = Numbers.Three; break;
	            case (char)'4': result = Numbers.Four; break;
	            case (char)'5': result = Numbers.Five; break;
	            case (char)'6': result = Numbers.Six; break;
	            case (char)'7': result = Numbers.Seven; break;
	            case (char)'8': result = Numbers.Eight; break;
	            case (char)'9': result = Numbers.Nine; break;
				
				case (char)' ': result = Symbols.Space; break;
				case (char)'.': result = Symbols.Period; break;
				
	            default:  result = Symbols.Dash; break;
	        }
	        result = ScaleChar(result);
	        result = ShiftChar(result);
	        return result;
	    }
	
	    private float[] ScaleChar(float[] unscaled_char)
	    {
	        float[] scaled_char = new float[unscaled_char.Length];
	        for (int i = 0; i < unscaled_char.Length; i++)
	        {
	            scaled_char[i] = unscaled_char[i] / 100f * scaleFactor;
	        }
	        return scaled_char;
	    }
	
	    private float[] ShiftChar(float[] unshifted_char)
	    {
	        float[] shifted_char = new float[unshifted_char.Length];
	        for (int i = 0; i < unshifted_char.Length; i++)
	        {
	            if (i % 3 == 0)
	            {
	                shifted_char[i] = unshifted_char[i] + letter_offset * current_letter;
	            }
	            else
	            {
	                shifted_char[i] = unshifted_char[i];
	            }
	        }
	        return shifted_char;
	    }
	
	    private float[] GetCoordsFromText(String text)
	    {
			int i;
	        List<float> new_text = new List<float>();
	        for (i = 0; i < text.Length; i++)
	        {
	            new_text.AddRange(GetChar(text[i]));
	            current_letter++;
	        }
	        return new_text.ToArray();
	    }
	
	    public TextClass(String text, float scaleFactorIn, float letterOffsetIn, 
		                 bool staticTextIn = false, bool reverseRotationIn = true) 
		{
			staticText = staticTextIn;
			reverseRotation = reverseRotationIn;
			text = text.ToUpper();
	        current_letter = 0;
	        scaleFactor = scaleFactorIn;
	        letter_offset = letterOffsetIn;
			vertexStride = 3 * 4; // bytes per vertex, no color
			
	        vertexData = GetCoordsFromText(text);
			if (reverseRotation)
			{
				vertexData = ReverseRotation(vertexData);
			}
	        vertexCount = vertexData.Length / COORDS_PER_VERTEX;
			SetupSimpleIndexBuffer();
			InitializeVertexBuffer();
	
			programNumber = Programs.AddProgram(VertexShader, FragmentShader);
	    }
		
		public void UpdateText(String text)
	    {
	        updateLock = true;
	        text = text.ToUpper();
	        current_letter = 0;
	        vertexStride = 3 * 4; // bytes per vertex, no color
	        vertexData = GetCoordsFromText(text);
	        if (reverseRotation)
	        {
	            vertexData = ReverseRotation(vertexData);
	        }
	        vertexCount = vertexData.Length / COORDS_PER_VERTEX;
	        SetupSimpleIndexBuffer();
	        InitializeVertexBuffer();
	        updateLock = false;
	    }
	
	    public override void Draw() 
		{
		 	Matrix4 mm = Rotate(modelToWorld, axis, angle);
			mm.M41 = offset.X;
			mm.M42 = offset.Y;
			mm.M43 = offset.Z;	
			
			Matrix4 wtc = worldToCamera;
			if (staticText)
			{
				wtc = Matrix4.Identity;
			}
			
			if (updateLock == false)
	        {
	            Programs.Draw(programNumber, vertexBufferObject, indexBufferObject, mm, indexData.Length, color);
				waitingForUpdate = false;
	        }
	        else
	        {
	            waitingForUpdate = true;
	        }
	    }
	}
}

