using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Textures
	{
		public Textures ()
		{
		}

		public static void EnableTextures()
		{
			GL.Enable(EnableCap.Texture2D);
			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			GL.Enable(EnableCap.DepthTest);
			GL.Enable(EnableCap.AlphaTest);
			GL.AlphaFunc(AlphaFunction.Gequal, 0.01f);
		}
		
		public static int Load(string fileName, int quality = 0, bool repeat = true, bool flip_y = false, bool oneTwenty = false)
		{
			string textureFilesDirectory = GlsTutorialsClass.ProjectDirectory + @"/Textures";
			Bitmap bitmap = new Bitmap(textureFilesDirectory + "//" + fileName);
		
			// Add 10% on each side for sphere mapping
			if (oneTwenty)
			{
				int width = bitmap.Width;
				int height = bitmap.Height;
				int extra = width/10;
				int newWidth = width + 2 * extra;
				Bitmap bitmap2 = new Bitmap(newWidth, height);
				Rectangle lastTen = new Rectangle(new Point(width - extra, 0), new Size(extra, height));
				Rectangle firstTen = new Rectangle(new Point(0, 0), new Size(extra, height));
    			using (Graphics g = Graphics.FromImage(bitmap2))
    			{
        			g.DrawImage(bitmap, 0, 0, lastTen, GraphicsUnit.Pixel);
        			g.DrawImage(bitmap, extra, 0);
					g.DrawImage(bitmap, extra + width, 0, firstTen, GraphicsUnit.Pixel);
    			}
				bitmap = bitmap2;
			}
			
		    //Flip the image
		    if (flip_y)
		        bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
		
		    //Generate a new texture target in gl
		    int texture = GL.GenTexture();
		
		    //Will bind the texture newly/empty created with GL.GenTexture
		    //All gl texture methods targeting Texture2D will relate to this texture
		    GL.BindTexture(TextureTarget.Texture2D, texture);
		
		    //The reason why your texture will show up glColor without setting these parameters is actually
		    //TextureMinFilters fault as its default is NearestMipmapLinear but we have not established mipmapping
		    //We are only using one texture at the moment since mipmapping is a collection of textures pre filtered
		    //I'm assuming it stops after not having a collection to check.
		    switch (quality)
		    {
		        case 0:
		        default://Low quality
		            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Linear);
		            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Linear);
		            break;
		        case 1://High quality
		            //This is in my opinion the best since it doesnt average the result and not blurred to shit
		            //but most consider this low quality...
		            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Nearest);
		            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Nearest);
		            break;
		    }
		
		    if (repeat)
		    {
		        //This will repeat the texture past its bounds set by TexImage2D
		        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)All.Repeat);
		        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)All.Repeat);
		    }
		    else
		    {
		        //This will clamp the texture to the edge, so manipulation will result in skewing
		        //It can also be useful for getting rid of repeating texture bits at the borders
		        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)All.ClampToEdge);
		        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)All.ClampToEdge);
		    }
		
		    //Creates a definition of a texture object in opengl
		    /* Parameters
		     * Target - Since we are using a 2D image we specify the target Texture2D
		     * MipMap Count / LOD - 0 as we are not using mipmapping at the moment
		     * InternalFormat - The format of the gl texture, Rgba is a base format it works all around
		     * Width;
		     * Height;
		     * Border - must be 0;
		     * 
		     * Format - this is the images format not gl's the format Bgra i believe is only language specific
		     *          C# uses little-endian so you have ARGB on the image A 24 R 16 G 8 B, B is the lowest
		     *          So it gets counted first, as with a language like Java it would be PixelFormat.Rgba
		     *          since Java is big-endian default meaning A is counted first.
		     *          but i could be wrong here it could be cpu specific :P
		     *          
		     * PixelType - The type we are using, eh in short UnsignedByte will just fill each 8 bit till the pixelformat is full
		     *             (don't quote me on that...)
		     *             you can be more specific and say for are RGBA to little-endian BGRA -> PixelType.UnsignedInt8888Reversed
		     *             this will mimic are 32bit uint in little-endian.
		     *             
		     * Data - No data at the moment it will be written with TexSubImage2D
		     */
		    GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmap.Width, bitmap.Height, 0, PixelFormat.Bgra, PixelType.UnsignedByte, IntPtr.Zero);
		
		    //Load the data from are loaded image into virtual memory so it can be read at runtime
		    System.Drawing.Imaging.BitmapData bitmap_data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
		        System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
		
		    //Writes data to are texture target
		    /* Target;
		     * MipMap;
		     * X Offset - Offset of the data on the x axis
		     * Y Offset - Offset of the data on the y axis
		     * Width;
		     * Height;
		     * Format;
		     * Type;
		     * Data - Now we have data from the loaded bitmap image we can load it into are texture data
		     */
		    GL.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, bitmap.Width, bitmap.Height, PixelFormat.Bgra, PixelType.UnsignedByte, bitmap_data.Scan0);
		
		    //Release from memory
		    bitmap.UnlockBits(bitmap_data);
		
		    //get rid of bitmap object its no longer needed in this method
		    bitmap.Dispose();
		
		    /*Binding to 0 is telling gl to use the default or null texture target
		    *This is useful to remember as you may forget that a texture is targeted
		    *And may overflow to functions that you dont necessarily want to
		    *Say you bind a texture
		    *
		    * Bind(Texture);
		    * DrawObject1();
		    *                <-- Insert Bind(NewTexture) or Bind(0)
		    * DrawObject2();
		    * 
		    * Object2 will use Texture if not set to 0 or another.
		    */
		    GL.BindTexture(TextureTarget.Texture2D, 0);
		
		    return texture;
		}	

		public static int CreateFromBitmap(Bitmap bitmap)
		{
			int texture = GL.GenTexture();
			GL.BindTexture(TextureTarget.Texture2D, texture);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Nearest);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Nearest);

			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)All.ClampToEdge);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)All.ClampToEdge);
			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmap.Width, bitmap.Height, 0, PixelFormat.Bgra, PixelType.UnsignedByte, IntPtr.Zero);

			System.Drawing.Imaging.BitmapData bitmap_data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
				System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			GL.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, bitmap.Width, bitmap.Height, PixelFormat.Bgra, PixelType.UnsignedByte, bitmap_data.Scan0);
			bitmap.UnlockBits(bitmap_data);
			GL.BindTexture(TextureTarget.Texture2D, 0);

			return texture;
		}	

		// Test Function
		public static void DrawTexture1D(int texture)
		{
			GL.PushMatrix();

			GL.Translate(256f, 256f, -5f);
			GL.Scale(new Vector3(100f, 100f, 100f));

			GL.Color4(Color.White);

			GL.BindTexture(TextureTarget.Texture1D, texture);

			GL.Begin(PrimitiveType.Quads);

			//Bind texture coordinates to vertices in ccw order

			//Top-Right
			GL.TexCoord2(1.0f, 0.0f);
			GL.Vertex2(1.0f, 1.0f);

			//Top-Left
			GL.TexCoord2(0f, 0f);
			GL.Vertex2(-1.0f, 1.0f);

			//Bottom-Left
			GL.TexCoord2(0f, 1f);
			GL.Vertex2(-1.0f, -1.0f);

			//Bottom-Right
			GL.TexCoord2(1f, 1f);
			GL.Vertex2(1.0f, -1.0f);

			GL.End();

			GL.BindTexture(TextureTarget.Texture1D, 0);

			GL.PopMatrix();
		}

		// Test Function
		public static void DrawTexture2D(int texture)
		{
			GL.PushMatrix();

			GL.Translate(256f, 256f, -5f);
			GL.Scale(new Vector3(100f, 100f, 100f));

			GL.BindTexture(TextureTarget.Texture2D, texture);

			GL.Begin(PrimitiveType.Quads);

			//Bind texture coordinates to vertices in ccw order

			//Top-Right
			GL.TexCoord2(1.0f, 1.0f);
			GL.Vertex2(1.0f, 1.0f);

			//Top-Left
			GL.TexCoord2(0f, 1.0f);
			GL.Vertex2(-1.0f, 1.0f);

			//Bottom-Left
			GL.TexCoord2(0f, 0f);
			GL.Vertex2(-1.0f, -1.0f);

			//Bottom-Right
			GL.TexCoord2(1f, 0f);
			GL.Vertex2(1.0f, -1.0f);

			GL.End();

			GL.BindTexture(TextureTarget.Texture2D, 0);

			GL.PopMatrix();
		}

		public static int CreateMipMapTexture(string fileName, int mipMapCount)
		{
			string textureFilesDirectory = GlsTutorialsClass.ProjectDirectory + @"/Textures";
			Bitmap bitmap = new Bitmap(textureFilesDirectory + "//" + fileName);

			int mipMapTexture;

			GL.GenTextures(1, out mipMapTexture);
			GL.BindTexture(TextureTarget.Texture2D, mipMapTexture);

			// Creating mipmaps manually
			int width = bitmap.Width;
			int height = bitmap.Height;

			for(int mipmapLevel = 0; mipmapLevel < mipMapCount; mipmapLevel++)
			{
				uint[] data = new uint[width*height];
				for (int col = 0; col < width; col++)
				{
					for (int row = 0; row < height; row++)
					{
						data[row * width + col] = (uint)bitmap.GetPixel(col, row).ToArgb();
					}
				}

				GL.TexImage2D(TextureTarget.Texture2D, mipmapLevel, PixelInternalFormat.Rgb8,
					width, height, 0, PixelFormat.Rgba, PixelType.UnsignedInt8888Reversed,
					data);
				bitmap = new Bitmap(bitmap, new Size(bitmap.Width/2, bitmap.Height/2));
				width = width/2; 
				height = height/2;
			}

			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBaseLevel, 0);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLevel, mipMapCount - 1);
			GL.BindTexture(TextureTarget.Texture2D, 0);
			return mipMapTexture;
		}
	}
}

