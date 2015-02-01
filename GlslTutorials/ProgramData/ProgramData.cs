using System;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class ProgramData
	{
        private int theProgram;
        private int positionAttribute;
        private int colorAttribute;
        private int modelToCameraMatrixUnif;
        private int modelToWorldMatrixUnif;
        private int worldToCameraMatrixUnif;
        private int cameraToClipMatrixUnif;
        private int baseColorUnif;

        private int normalModelToCameraMatrixUnif;
        private int dirToLightUnif;
		private int lightPosUnif;
		private int modelSpaceLightPosUnif;
        private int lightIntensityUnif;
        private int ambientIntensityUnif;
        private int normalAttribute;
		private int texCoordAttribute;
		private int colorTextureUnif;
		private int scaleUniform;
		
		private int sampler = 0;
		private int texUnit = 0;
		private int current_texture;
			
		string vertexShader;
		string fragmentShader;
		
		int BYTES_PER_FLOAT = 4;
		int COORDS_PER_VERTEX = 3;
		int POSITION_DATA_SIZE_IN_ELEMENTS = 3;
		int COLOR_DATA_SIZE_IN_ELEMENTS = 4;	
		int NORMAL_DATA_SIZE_IN_ELEMENTS = 3;
		int NORMAL_START = 3 * 4; // POSITION_DATA_SIZE_IN_ELEMENTS * BYTES_PER_FLOAT;
		int TEXTURE_DATA_SIZE_IN_ELEMENTS = 2;
		int TEXTURE_START = 3 * 4 + 3 * 4;
		protected int vertexStride = 3 * 4; // bytes per vertex default to only 3 position floats

		LightBlock lightBlock;
		
		public ProgramData(string vertexShaderIn, string fragmentShaderIn)
	    {
			vertexShader = vertexShaderIn;
			fragmentShader = fragmentShaderIn;
	        int vertex_shader = Shader.compileShader(ShaderType.VertexShader, vertexShader);
	        int fragment_shader = Shader.compileShader(ShaderType.FragmentShader, fragmentShader);
	        theProgram  = Shader.createAndLinkProgram(vertex_shader, fragment_shader);
	
	        positionAttribute = GL.GetAttribLocation(theProgram, "position");
	        colorAttribute = GL.GetAttribLocation(theProgram, "color");
			if (positionAttribute != -1) 
			{
				if (positionAttribute != 0)
				{
					MessageBox.Show("These meshes only work with position at location 0 " + vertexShader);
				}
			}
			if (colorAttribute != -1) 
			{
				if (colorAttribute != 1)
				{
					MessageBox.Show("These meshes only work with color at location 1" + vertexShader);
				}
			}
	
	        modelToWorldMatrixUnif = GL.GetUniformLocation(theProgram, "modelToWorldMatrix");
	        worldToCameraMatrixUnif = GL.GetUniformLocation(theProgram, "worldToCameraMatrix");
	        cameraToClipMatrixUnif = GL.GetUniformLocation(theProgram, "cameraToClipMatrix");
			if (cameraToClipMatrixUnif == -1)
			{
				cameraToClipMatrixUnif = GL.GetUniformLocation(theProgram, "Projection.cameraToClipMatrix");
			}
	        baseColorUnif = GL.GetUniformLocation(theProgram, "baseColor");
	
	        modelToCameraMatrixUnif = GL.GetUniformLocation(theProgram, "modelToCameraMatrix");
	
	        normalModelToCameraMatrixUnif = GL.GetUniformLocation(theProgram, "normalModelToCameraMatrix");
	        dirToLightUnif =  GL.GetUniformLocation(theProgram, "dirToLight");
			lightPosUnif = GL.GetUniformLocation(theProgram, "lightPos");
			modelSpaceLightPosUnif = GL.GetUniformLocation(theProgram, "modelSpaceLightPos");
	        lightIntensityUnif = GL.GetUniformLocation(theProgram, "lightIntensity");
	        ambientIntensityUnif = GL.GetUniformLocation(theProgram, "ambientIntensity");
	        normalAttribute = GL.GetAttribLocation(theProgram, "normal");
			if (normalAttribute != -1)
			{
				vertexStride = 3 * 4 * 2;
			}
			texCoordAttribute = GL.GetAttribLocation(theProgram, "texCoord");
			if (texCoordAttribute != -1)
			{
				CreateSampler();
				vertexStride = 3 * 4 * 2 + 2 * 4;
			} 
			colorTextureUnif = GL.GetUniformLocation(theProgram, "diffuseColorTex");
			scaleUniform = GL.GetUniformLocation(theProgram, "scaleFactor");
	    }
		
		void CreateSampler()
		{
			GL.GenSamplers(1, out sampler);
			GL.SamplerParameter(sampler, SamplerParameterName.TextureMagFilter,  (int)TextureMagFilter.Nearest);
			GL.SamplerParameter(sampler, SamplerParameterName.TextureMinFilter,  (int)TextureMinFilter.Nearest);
			GL.SamplerParameter(sampler, SamplerParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
		}
		
		public bool CompareShaders(string vertexShaderIn, string fragmentShaderIn)
		{
			return ((vertexShaderIn == vertexShader) & (fragmentShader == fragmentShaderIn));
		}
		
		public void Draw(int[] vertexBufferObject, int[] indexBufferObject, Matrix4 mm, int indexDataLength, 
			float[] color)
		{
			GL.UseProgram(theProgram);	
			GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject[0]);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBufferObject[0]);
			
	        GL.UniformMatrix4(cameraToClipMatrixUnif, false, ref Shape.cameraToClip);
	        GL.UniformMatrix4(worldToCameraMatrixUnif, false, ref Shape.worldToCamera);
	        
			if (modelToWorldMatrixUnif != -1) GL.UniformMatrix4(modelToWorldMatrixUnif, false, ref mm);
	        if (modelToCameraMatrixUnif != -1) GL.UniformMatrix4(modelToCameraMatrixUnif, false, ref mm);
			if (baseColorUnif != -1) GL.Uniform4(baseColorUnif, 1, color);
			

			GL.EnableVertexAttribArray(positionAttribute);
	        // Prepare the triangle coordinate data
	        GL.VertexAttribPointer(positionAttribute, POSITION_DATA_SIZE_IN_ELEMENTS, 
				VertexAttribPointerType.Float, false, vertexStride, (IntPtr)0);
			
			if (normalAttribute != -1)
			{
				GL.EnableVertexAttribArray(normalAttribute);
				GL.VertexAttribPointer(normalAttribute, NORMAL_DATA_SIZE_IN_ELEMENTS, 
					VertexAttribPointerType.Float, false, vertexStride, (IntPtr)NORMAL_START);
			}
			
			if (texCoordAttribute != -1)
			{
				GL.Enable(EnableCap.Texture2D);
				GL.EnableVertexAttribArray(texCoordAttribute);
				GL.VertexAttribPointer(texCoordAttribute, TEXTURE_DATA_SIZE_IN_ELEMENTS, 
					VertexAttribPointerType.Float, false, vertexStride, (IntPtr)TEXTURE_START);
				GL.BindTexture(TextureTarget.Texture2D, current_texture);
				GL.BindSampler(texUnit, sampler);
			}			
			
	        // Draw the triangle
	 		GL.DrawElements(PrimitiveType.Triangles, indexDataLength, DrawElementsType.UnsignedShort, 0);
			
	        // Disable vertex array
	        GL.DisableVertexAttribArray(positionAttribute);
			if (normalAttribute != -1) GL.DisableVertexAttribArray(normalAttribute);
			if (texCoordAttribute != -1)
			{
				GL.DisableVertexAttribArray(texCoordAttribute);
			}
			
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
	        GL.UseProgram(0);	
		}

		public void DrawWireFrame(int[] vertexBufferObject, int[] indexBufferObject, Matrix4 mm, int indexDataLength, 
			float[] color)
		{
			GL.UseProgram(theProgram);	
			GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject[0]);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBufferObject[0]);

			GL.UniformMatrix4(cameraToClipMatrixUnif, false, ref Shape.cameraToClip);
			GL.UniformMatrix4(worldToCameraMatrixUnif, false, ref Shape.worldToCamera);

			if (modelToWorldMatrixUnif != -1) GL.UniformMatrix4(modelToWorldMatrixUnif, false, ref mm);
			if (modelToCameraMatrixUnif != -1) GL.UniformMatrix4(modelToCameraMatrixUnif, false, ref mm);
			if (baseColorUnif != -1) GL.Uniform4(baseColorUnif, 1, color);


			GL.EnableVertexAttribArray(positionAttribute);
			// Prepare the triangle coordinate data
			GL.VertexAttribPointer(positionAttribute, POSITION_DATA_SIZE_IN_ELEMENTS, 
				VertexAttribPointerType.Float, false, vertexStride, (IntPtr)0);

			if (normalAttribute != -1)
			{
				GL.EnableVertexAttribArray(normalAttribute);
				GL.VertexAttribPointer(normalAttribute, NORMAL_DATA_SIZE_IN_ELEMENTS, 
					VertexAttribPointerType.Float, false, vertexStride, (IntPtr)NORMAL_START);
			}

			if (texCoordAttribute != -1)
			{
				GL.Enable(EnableCap.Texture2D);
				GL.EnableVertexAttribArray(texCoordAttribute);
				GL.VertexAttribPointer(texCoordAttribute, TEXTURE_DATA_SIZE_IN_ELEMENTS, 
					VertexAttribPointerType.Float, false, vertexStride, (IntPtr)TEXTURE_START);
				GL.BindTexture(TextureTarget.Texture2D, current_texture);
				GL.BindSampler(texUnit, sampler);
			}			

			// Draw the wireframes
			for (int i = 0; i < indexDataLength; i += 3)
			{
				GL.DrawElements(PrimitiveType.LineLoop, 3, DrawElementsType.UnsignedShort, i * sizeof(ushort));
			}

			// Disable vertex array
			GL.DisableVertexAttribArray(positionAttribute);
			if (normalAttribute != -1) GL.DisableVertexAttribArray(normalAttribute);
			if (texCoordAttribute != -1)
			{
				GL.DisableVertexAttribArray(texCoordAttribute);
			}

			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
			GL.UseProgram(0);	
		}
			
		public override string ToString()
		{
			StringBuilder result = new StringBuilder();
			result.AppendLine("theProgram = " + theProgram.ToString());
			result.AppendLine("positionAttribute = " + positionAttribute.ToString());
			result.AppendLine("colorAttribute = " + colorAttribute.ToString());
			result.AppendLine("modelToCameraMatrixUnif = " + modelToCameraMatrixUnif.ToString());
			result.AppendLine("modelToWorldMatrixUnif = " + modelToWorldMatrixUnif.ToString());
			result.AppendLine("worldToCameraMatrixUnif = " + worldToCameraMatrixUnif.ToString());
			result.AppendLine("cameraToClipMatrixUnif = " + cameraToClipMatrixUnif.ToString());
			result.AppendLine("baseColorUnif = " + baseColorUnif.ToString());
			result.AppendLine("normalModelToCameraMatrixUnif = " + normalModelToCameraMatrixUnif.ToString());
			result.AppendLine("dirToLightUnif = " + dirToLightUnif.ToString());
			result.AppendLine("lightIntensityUnif = " + lightIntensityUnif.ToString());
			result.AppendLine("ambientIntensityUnif = " + ambientIntensityUnif.ToString());
			result.AppendLine("normalAttribute = " + normalAttribute.ToString());
			return result.ToString();
		}
		
		public void SetUniformColor(Vector4 color)
		{
			GL.UseProgram(theProgram);
			GL.Uniform4(baseColorUnif, color);
			GL.UseProgram(0);
		}
		
		public void SetUniformTexture(int colorTexUnit)
		{
			GL.UseProgram(theProgram);
			GL.Uniform1(colorTextureUnif, colorTexUnit);
			GL.UseProgram(0);
		}

		public void SetUniformScale(float scale)
		{
			GL.UseProgram(theProgram);
			GL.Uniform1(scaleUniform, scale);
			GL.UseProgram(0);
		}
			
		public void SetTexture(string texture, bool oneTwenty)
		{
			current_texture = Textures.Load(texture, 1, false, false, oneTwenty);
		}

		public void SetTexture(int texture)
		{
			current_texture = texture;
		}
		
		public void SetLightPosition(Vector3 lightPosition)
		{
			GL.UseProgram(theProgram);
			GL.Uniform3(lightPosUnif, lightPosition);
			GL.UseProgram(0);
		}
		
		public void SetModelSpaceLightPosition(Vector3 modelSpaceLightPos)
		{
			GL.UseProgram(theProgram);
			GL.Uniform3(modelSpaceLightPosUnif, modelSpaceLightPos);
			GL.UseProgram(0);
		}
		
		public void SetDirectionToLight(Vector3 dirToLight)
		{
			GL.UseProgram(theProgram);
			GL.Uniform3(dirToLightUnif, dirToLight);
			GL.UseProgram(0);
		}
		
		public void SetLightIntensity(Vector4 lightIntensity)
		{
			GL.UseProgram(theProgram);
			GL.Uniform4(lightIntensityUnif, lightIntensity);
			GL.UseProgram(0);
		}
		
		public void SetAmbientIntensity(Vector4 ambientIntensity)
		{
			GL.UseProgram(theProgram);
			GL.Uniform4(ambientIntensityUnif, ambientIntensity);
			GL.UseProgram(0);
		}

		public void SetNormalModelToCameraMatrix(Matrix3 normalModelToCameraMatrix)
		{
			GL.UseProgram(theProgram);
			GL.UniformMatrix3(normalModelToCameraMatrixUnif, false, ref normalModelToCameraMatrix);
			GL.UseProgram(0); 
		}
		
		public void SetModelToCameraMatrix(Matrix4 modelToCameraMatrix)
		{
			GL.UseProgram(theProgram);
			GL.UniformMatrix4(modelToCameraMatrixUnif, false, ref modelToCameraMatrix);
			GL.UseProgram(0); 
		}

		public void SetUpLightBlock(int numberOfLights)
		{
			lightBlock = new LightBlock(numberOfLights);
			lightBlock.SetUniforms(theProgram);	
		}

		public void UpdateLightBlock(LightBlock lb)
		{
			lightBlock.Update(lb);
		}
		
		public void SetVertexStride(int vertexStrideIn)
		{
			vertexStride = vertexStrideIn;
		}
	}
}

