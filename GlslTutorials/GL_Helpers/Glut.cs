using System;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Glut
	{
		public static void SolidCube(float size)
		{
			size /= 2;
			GL.Begin(PrimitiveType.Quads);

			GL.Vertex3(-size,+size,-size); //BL
			GL.Vertex3(+size,+size,-size); //BR
			GL.Vertex3(+size,-size,-size); //TR
			GL.Vertex3(-size,-size,-size); //TL

			GL.Vertex3(-size,-size, size); //BL
			GL.Vertex3(+size,-size, size); //BR
			GL.Vertex3(+size,+size, size); //TR
			GL.Vertex3(-size,+size, size); //TL

			GL.Vertex3(-size,-size,+size);  //BL
			GL.Vertex3(-size,+size,+size);  //BR
			GL.Vertex3(-size,+size,-size);  //TR
			GL.Vertex3(-size,-size,-size);  //TL

			GL.Vertex3( size,-size,-size); //BL
			GL.Vertex3( size,+size,-size); //BR
			GL.Vertex3( size,+size,+size); //TR
			GL.Vertex3( size,-size,+size); //TL

			GL.Vertex3(+size,-size,-size);
			GL.Vertex3(+size,-size,+size);
			GL.Vertex3(-size,-size,+size);
			GL.Vertex3(-size,-size,-size);

			GL.Vertex3(-size, size,-size);
			GL.Vertex3(-size, size,+size);
			GL.Vertex3(+size, size,+size);
			GL.Vertex3(+size, size,-size);

			GL.End();
		}
	}
}

