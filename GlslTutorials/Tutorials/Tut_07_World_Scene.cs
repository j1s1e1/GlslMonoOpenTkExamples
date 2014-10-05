using System;
using System.IO;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Tut_07_World_Scene : TutorialBase 
	{
	    class ProgramData
	    {
	        public int theProgram;
	        public int positionAttribute;
	        public int colorAttribute;
	        public int modelToWorldMatrixUnif;
	        public int worldToCameraMatrixUnif;
	        public int cameraToClipMatrixUnif;
	        public int baseColorUnif;
	    };
	
	    static float g_fzNear = 1.0f;
	    static float g_fzFar = 1000.0f;
	
	    static ProgramData UniformColor;
	    static ProgramData ObjectColor;
	    static ProgramData UniformColorTint;
	
	    ProgramData LoadProgram(String strVertexShader, String strFragmentShader)
	    {
	        ProgramData data = new ProgramData();
	        int vertex_shader = Shader.loadShader(ShaderType.VertexShader, strVertexShader);
	        int fragment_shader = Shader.loadShader(ShaderType.FragmentShader, strFragmentShader);
	        data.theProgram  = Shader.createAndLinkProgram(vertex_shader, fragment_shader);
	
	
	        data.positionAttribute = GL.GetAttribLocation(data.theProgram, "position");
			if (data.positionAttribute != -1) 
			{
				if (data.positionAttribute != 0)
				{
					MessageBox.Show("These meshes only work with position at location 0 " + strVertexShader);
				}
			}
	        data.colorAttribute = GL.GetAttribLocation(data.theProgram, "color");
	
	        data.modelToWorldMatrixUnif = GL.GetUniformLocation(data.theProgram, "modelToWorldMatrix");
	        data.worldToCameraMatrixUnif = GL.GetUniformLocation(data.theProgram, "worldToCameraMatrix");
	        data.cameraToClipMatrixUnif = GL.GetUniformLocation(data.theProgram, "cameraToClipMatrix");
	        data.baseColorUnif = GL.GetUniformLocation(data.theProgram, "baseColor");
	
	        return data;
	    }
	
	    void InitializeProgram()
	    {
	        UniformColor = LoadProgram(VertexShaders.PosOnlyWorldTransform_vert, FragmentShaders.ColorUniform_frag);
	        ObjectColor = LoadProgram(VertexShaders.PosColorWorldTransform_vert, FragmentShaders.ColorPassthrough_frag);
	        UniformColorTint = LoadProgram(VertexShaders.PosColorWorldTransform_vert, FragmentShaders.ColorMultUniform_frag);
	    }
	
	    static Mesh g_pConeMesh;
	    static Mesh g_pCylinderMesh;
	    static Mesh g_pCubeTintMesh;
	    static Mesh g_pCubeColorMesh;
	    static Mesh g_pPlaneMesh;
	
	    //Called after the window and OpenGL are initialized. Called exactly once, before the main loop.
	    protected override void init()
	    {
	        InitializeProgram();
	
	        try
	        {
				string XmlFilesDirectory = GlsTutorialsClass.ProjectDirectory + @"/XmlFilesForMeshes";
	            Stream UnitConeTint = File.OpenRead(XmlFilesDirectory + @"/unitconetint.xml");
	            g_pConeMesh = new Mesh(UnitConeTint);
	            Stream UnitCylinderTint =  File.OpenRead(XmlFilesDirectory + @"/unitcylindertint.xml");
	            g_pCylinderMesh = new Mesh(UnitCylinderTint);
	            Stream UnitCubeTint = File.OpenRead(XmlFilesDirectory + @"/unitcubetint.xml");
	            g_pCubeTintMesh = new Mesh(UnitCubeTint);
	            Stream UnitCubeColor = File.OpenRead(XmlFilesDirectory + @"/unitcubecolor.xml");
	            g_pCubeColorMesh = new Mesh(UnitCubeColor);
	            Stream UnitPlane = File.OpenRead(XmlFilesDirectory + @"/unitplane.xml");
	            g_pPlaneMesh = new Mesh(UnitPlane);
	        }
	        catch (Exception ex)
	        {
	            throw new Exception("Error " + ex.ToString());
	        }
			
		    GL.Enable(EnableCap.CullFace);
	        GL.CullFace(CullFaceMode.Back);
	        GL.FrontFace(FrontFaceDirection.Cw);
	
	        GL.Enable(EnableCap.DepthTest);
	        GL.DepthMask(true);
	        GL.DepthFunc(DepthFunction.Lequal);
	        GL.DepthRange(0.0f, 1.0f);
	
	        //GL.Enable(EnableCap.DepthClamp);
			//GLES20.GL_CLAMP_TO_EDGE);
	    }
	
	    //Trees are 3x3 in X/Z, and fTrunkHeight+fConeHeight in the Y.
	    static void DrawTree(MatrixStack modelMatrix)
	    {
	        DrawTree(modelMatrix, 2.0f, 3.0f);
	    }
	
	    static void DrawTree(MatrixStack modelMatrix, float fTrunkHeight, float fConeHeight)
	    {
	        using (PushStack pushstack = new PushStack(modelMatrix))
	        {
	
	            modelMatrix.Scale(1.0f, fTrunkHeight, 1.0f);
	            modelMatrix.Translate(0.0f, 0.5f, 0.0f);
	
	            GL.UseProgram(UniformColorTint.theProgram);
	            Matrix4 mm = modelMatrix.Top();
	            GL.UniformMatrix4(UniformColorTint.modelToWorldMatrixUnif, false, ref mm);
	            GL.Uniform4(UniformColorTint.baseColorUnif, 0.694f, 0.4f, 0.106f, 1.0f);
	            g_pCylinderMesh.Render();
	            GL.UseProgram(0);
	        }
	
	        //Draw the treetop
	        using (PushStack pushstack = new PushStack(modelMatrix))
	        {
	            modelMatrix.Translate(0.0f, fTrunkHeight, 0.0f);
	            modelMatrix.Scale(3.0f, fConeHeight, 3.0f);
	
	            GL.UseProgram(UniformColorTint.theProgram);
	            Matrix4 mm = modelMatrix.Top();
	
	            GL.UniformMatrix4(UniformColorTint.modelToWorldMatrixUnif, false, ref mm);
	            GL.Uniform4(UniformColorTint.baseColorUnif, 0.0f, 1.0f, 0.0f, 1.0f);
	
	            g_pConeMesh.Render();
	            GL.UseProgram(0);
	        }
	    }
	
	    float g_fColumnBaseHeight = 0.25f;
	
	    //Columns are 1x1 in the X/Z, and fHieght units in the Y.
	    void DrawColumn(MatrixStack modelMatrix)
	    {
	        DrawColumn(modelMatrix, 5.0f);
	    }
	    
	    void DrawColumn(MatrixStack modelMatrix, float fHeight)
	    {
	        //Draw the bottom of the column.
	        using (PushStack pushstack = new PushStack(modelMatrix))
	        {
	            modelMatrix.Scale(1.0f, g_fColumnBaseHeight, 1.0f);
	            modelMatrix.Translate(0.0f, 0.5f, 0.0f);
	
	            GL.UseProgram(UniformColorTint.theProgram);
	            Matrix4 mm = modelMatrix.Top();
	            GL.UniformMatrix4(UniformColorTint.modelToWorldMatrixUnif, false, ref mm);
	            GL.Uniform4(UniformColorTint.baseColorUnif, 1.0f, 1.0f, 1.0f, 1.0f);
	            g_pCubeTintMesh.Render();
	            GL.UseProgram(0);
	        }
	
	        //Draw the top of the column.
	        using (PushStack pushstack = new PushStack(modelMatrix))
	        {
	            modelMatrix.Translate(new Vector3(0.0f, fHeight - g_fColumnBaseHeight, 0.0f));
	            modelMatrix.Scale(new Vector3(1.0f, g_fColumnBaseHeight, 1.0f));
	            modelMatrix.Translate(new Vector3(0.0f, 0.5f, 0.0f));
	
	            GL.UseProgram(UniformColorTint.theProgram);
	            Matrix4 mm = modelMatrix.Top();
	            GL.UniformMatrix4(UniformColorTint.modelToWorldMatrixUnif, false, ref mm);
	            GL.Uniform4(UniformColorTint.baseColorUnif, 0.9f, 0.9f, 0.9f, 0.9f);
	            g_pCubeTintMesh.Render();
	            GL.UseProgram(0);
	        }
	
	        //Draw the main column.
	        using (PushStack pushstack = new PushStack(modelMatrix))
	        {
	            modelMatrix.Translate(new Vector3(0.0f, g_fColumnBaseHeight, 0.0f));
	            modelMatrix.Scale(new Vector3(0.8f, fHeight - (g_fColumnBaseHeight * 2.0f), 0.8f));
	            modelMatrix.Translate(new Vector3(0.0f, 0.5f, 0.0f));
	
	            GL.UseProgram(UniformColorTint.theProgram);
	            Matrix4 mm = modelMatrix.Top();
	            GL.UniformMatrix4(UniformColorTint.modelToWorldMatrixUnif, false, ref mm);
	            GL.Uniform4(UniformColorTint.baseColorUnif, 0.9f, 0.9f, 0.9f, 0.9f);
	            g_pCylinderMesh.Render();
	            GL.UseProgram(0);
	        }
	    }
	
	    float g_fParthenonWidth = 14.0f;
	    float g_fParthenonLength = 20.0f;
	    float g_fParthenonColumnHeight = 5.0f;
	    float g_fParthenonBaseHeight = 1.0f;
	    float g_fParthenonTopHeight = 2.0f;
	
	    void DrawParthenon(MatrixStack modelMatrix)
	    {
	        //Draw base.
	        using (PushStack pushstack = new PushStack(modelMatrix))
	        {
	            modelMatrix.Scale(g_fParthenonWidth, g_fParthenonBaseHeight, g_fParthenonLength);
	            modelMatrix.Translate(0.0f, 0.5f, 0.0f);
	
	            GL.UseProgram(UniformColorTint.theProgram);
	            SetGlobalMatrices(UniformColorTint);
	            Matrix4 mm = modelMatrix.Top();
	            GL.UniformMatrix4(UniformColorTint.modelToWorldMatrixUnif, false, ref mm);
	            GL.Uniform4(UniformColorTint.baseColorUnif, 0.9f, 0.9f, 0.9f, 0.9f);
	            g_pCubeTintMesh.Render();
	            GL.UseProgram(0);
	        }
	
	        //Draw top.
	        using (PushStack pushstack = new PushStack(modelMatrix))
	        {
	            modelMatrix.Translate(0.0f, g_fParthenonColumnHeight + g_fParthenonBaseHeight, 0.0f);
	            modelMatrix.Scale(g_fParthenonWidth, g_fParthenonTopHeight, g_fParthenonLength);
	            modelMatrix.Translate(0.0f, 0.5f, 0.0f);
	
	            GL.UseProgram(UniformColorTint.theProgram);
	            Matrix4 mm = modelMatrix.Top();
	            GL.UniformMatrix4(UniformColorTint.modelToWorldMatrixUnif, false, ref mm);
	            GL.Uniform4(UniformColorTint.baseColorUnif, 0.9f, 0.9f, 0.9f, 0.9f);
	            g_pCubeTintMesh.Render();
	            GL.UseProgram(0);
	        }
	
	        //Draw columns.
	        float fFrontZVal = (g_fParthenonLength / 2.0f) - 1.0f;
	        float fRightXVal = (g_fParthenonWidth / 2.0f) - 1.0f;
	
	        for (int iColumnNum = 0; iColumnNum < (int)(g_fParthenonWidth / 2.0f); iColumnNum++)
	        {
	            using (PushStack pushstack = new PushStack(modelMatrix))
	            {
	                modelMatrix.Translate((2.0f * iColumnNum) - (g_fParthenonWidth / 2.0f) + 1.0f,
	                        g_fParthenonBaseHeight, fFrontZVal);
	
	                DrawColumn(modelMatrix, g_fParthenonColumnHeight);
	            }
	            using (PushStack pushstack = new PushStack(modelMatrix))
	            {
	                modelMatrix.Translate((2.0f * iColumnNum) - (g_fParthenonWidth / 2.0f) + 1.0f,
	                        g_fParthenonBaseHeight, -fFrontZVal);
	
	                DrawColumn(modelMatrix, g_fParthenonColumnHeight);
	            }
	        }
	
	        //Don't draw the first or last columns, since they've been drawn already.
	        for (int iColumnNum = 1; iColumnNum < (int)((g_fParthenonLength - 2.0f) / 2.0f); iColumnNum++)
	        {
	            using (PushStack pushstack = new PushStack(modelMatrix))
	            {
	                modelMatrix.Translate(fRightXVal,
	                        g_fParthenonBaseHeight, (2.0f * iColumnNum) - (g_fParthenonLength / 2.0f) + 1.0f);
	
	                DrawColumn(modelMatrix, g_fParthenonColumnHeight);
	            }
	            using (PushStack pushstack = new PushStack(modelMatrix))
	            {
	                modelMatrix.Translate(-fRightXVal,
	                        g_fParthenonBaseHeight, (2.0f * iColumnNum) - (g_fParthenonLength / 2.0f) + 1.0f);
	
	                DrawColumn(modelMatrix, g_fParthenonColumnHeight);
	            }
	        }
	
	        //Draw interior.
	        using (PushStack pushstack = new PushStack(modelMatrix))
	        {
	            modelMatrix.Translate(0.0f, 1.0f, 0.0f);
	            modelMatrix.Scale(g_fParthenonWidth - 6.0f, g_fParthenonColumnHeight,
	                    g_fParthenonLength - 6.0f);
	            modelMatrix.Translate(0.0f, 0.5f, 0.0f);
	
	            GL.UseProgram(ObjectColor.theProgram);
	            Matrix4 mm = modelMatrix.Top();
	            GL.UniformMatrix4(ObjectColor.modelToWorldMatrixUnif, false, ref mm);
	            g_pCubeColorMesh.Render();
	            GL.UseProgram(0);
	        }
	
	        //Draw headpiece.
	        using (PushStack pushstack = new PushStack(modelMatrix))
	        {
	            modelMatrix.Translate(
	                    0.0f,
	                    g_fParthenonColumnHeight + g_fParthenonBaseHeight + (g_fParthenonTopHeight / 2.0f),
	                    g_fParthenonLength / 2.0f);
	            modelMatrix.RotateX(-135.0f);
	            modelMatrix.RotateY(45.0f);
	
	            GL.UseProgram(ObjectColor.theProgram);
	            Matrix4 mm = modelMatrix.Top();
	            GL.UniformMatrix4(ObjectColor.modelToWorldMatrixUnif, false, ref mm);
	            g_pCubeColorMesh.Render();
	            GL.UseProgram(0);
	        }
	    }
	
	    static void DrawForest(MatrixStack modelMatrix)
	    {
	        for (int iTree = 0; iTree < TreeData.g_forest.Length; iTree++)
	        {
	            TreeData currTree = TreeData.g_forest[iTree];
	            using (PushStack pushstack = new PushStack(modelMatrix))
	            {
	                modelMatrix.Translate(currTree.fXPos, 0.0f, currTree.fZPos);
	                DrawTree(modelMatrix, currTree.fTrunkHeight, currTree.fConeHeight);
	            }
	        }
	    }
	
	    static bool g_bDrawLookatPoint = false;
	
	    //Called to update the display.
	    //You should call glutSwapBuffers after all of your rendering to display what you rendered.
	    //If you need continuous updates of the screen, call glutPostRedisplay() at the end of the function.
	    public override void display()
	    {
	        GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);
	        GL.ClearDepth(1.0f);
	        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
	
	        if ((g_pConeMesh != null) && (g_pCylinderMesh != null) && (g_pCubeTintMesh != null) &&
	                (g_pCubeColorMesh != null) && (g_pPlaneMesh != null)) {
	
	
	            MatrixStack modelMatrix = new MatrixStack();
	
	            //Render the ground plane.
	            using (PushStack pushstack = new PushStack(modelMatrix))
	            {
	                modelMatrix.Scale(100.0f, 1.0f, 100.0f);
	
	                GL.UseProgram(UniformColor.theProgram);
	
	                Matrix4 mm = modelMatrix.Top();
	                GL.UniformMatrix4(UniformColor.modelToWorldMatrixUnif, false, ref mm);
	                GL.Uniform4(UniformColor.baseColorUnif, 0.302f, 0.416f, 0.0589f, 1.0f);
	                g_pPlaneMesh.Render();
	                GL.UseProgram(0);
	            }
	
	
	            //Draw the trees
	            DrawForest(modelMatrix);
	
	            //Draw the building.
	            using (PushStack pushstack = new PushStack(modelMatrix))
	            {
	                modelMatrix.Translate(20.0f, 0.0f, -10.0f);
	                DrawParthenon(modelMatrix);
	            }
	
	            if (g_bDrawLookatPoint)
	            using (PushStack pushstack = new PushStack(modelMatrix))
	            {
	                GL.Disable(EnableCap.DepthTest);
	
	                modelMatrix.Translate(Camera.g_camTarget);
	                modelMatrix.Scale(1.0f, 1.0f, 1.0f);
	
	                GL.UseProgram(ObjectColor.theProgram);
	                Matrix4 mm = modelMatrix.Top();
	                GL.UniformMatrix4(ObjectColor.modelToWorldMatrixUnif, false, ref mm);
	                g_pCubeColorMesh.Render();
	                GL.UseProgram(0);
	                GL.Enable(EnableCap.DepthTest);
	            }
	        }
	    }
	
	    static Matrix4 pm;
	    static Matrix4 cm;
	
	    static private void SetGlobalMatrices(ProgramData program)
	    {
	        GL.UseProgram(program.theProgram);
	        GL.UniformMatrix4(program.cameraToClipMatrixUnif, false, ref pm);  // this one is first
	        GL.UniformMatrix4(program.worldToCameraMatrixUnif, false, ref cm); // this is the second one
	        GL.UseProgram(0);
	    }
	
	    //Called whenever the window is resized. The new window size is given, in pixels.
	    //This is an opportunity to call glViewport or glScissor to keep up with the change in size.
	    public override void reshape()
	    {
	        MatrixStack camMatrix = new MatrixStack();
	        camMatrix.SetMatrix(Camera.GetLookAtMatrix());
	
	        cm = camMatrix.Top();
	
	        MatrixStack persMatrix = new MatrixStack();
	        persMatrix.Perspective(45.0f, (width / (float)height), g_fzNear, g_fzFar);
	        pm = persMatrix.Top();
	
	        SetGlobalMatrices(UniformColor);
	        SetGlobalMatrices(ObjectColor);
	        SetGlobalMatrices(UniformColorTint);
	
	        GL.Viewport(0, 0, width, height);
	    }
	
	    //Called whenever a key on the keyboard was pressed.
	    //The key is given by the ''key'' parameter, which is in ASCII.
	    //It's often a good idea to have the escape key (ASCII value 27) call glutLeaveMainLoop() to 
	    //exit the program.
	    public override String keyboard(Keys keyCode, int x, int y)
	    {
	        String result = "";
	        switch (keyCode) {
	            case Keys.Escape:
	                g_pConeMesh = null;
	                g_pCylinderMesh = null;
	                g_pCubeTintMesh = null;
	                g_pCubeColorMesh = null;
	                g_pPlaneMesh = null;
	                //timer.Enabled = false;
	                return "Escape";
	            case Keys.A:
	                result = "A Decrease g_camTarget.X";
	                Camera.MoveTarget(-4.0f, 0, 0);
	                break;
	            case Keys.D:
	                result = "D Increase g_camTarget.X";
	                Camera.MoveTarget(4.0f, 0, 0);
	                break;
	            case Keys.E:
	                result = "E Decrease g_camTarget.Y";
	                Camera.MoveTarget(0, -4.0f, 0);
	                break;
	            case Keys.Q:
	                result = "Q Increase g_camTarget.Y";
	                Camera.MoveTarget(0, 4.0f, 0);
	                break;
	            case Keys.W:
	                result = "W Decrease g_camTarget.Z";
	                Camera.MoveTarget(0, 0, -4.0f);
	                break;
	            case Keys.S:
	                result = "S Increase g_camTarget.Z";
	                Camera.MoveTarget(0, 0, 4.0f);
	                break;
	            case Keys.J:
	                result = "J Decrease g_sphereCamRelPos.Y";
	                Camera.Move(-4.0f, 0, 0);
	                break;
	            case Keys.L:
	                result = "L Increase g_sphereCamRelPos.Y";
	                Camera.Move(4.0f, 0, 0);
	                break;
	            case Keys.I:
	                result = "I Decrease g_sphereCamRelPos.Y";
	                Camera.Move(0, -4.0f, 0);
	                break;
	            case Keys.K:
	                result = "K Increase g_sphereCamRelPos.Y";
	                Camera.Move(0, 4.0f, 0);
	                break;
	            case Keys.O:
	                result = "O Decrease g_sphereCamRelPos.Y";
	                Camera.Move(0, 0, -4.0f);
	                break;
	            case Keys.U:
	                result = "U Increase g_sphereCamRelPos.Y";
	                Camera.Move(0, 0, 4.0f);
	                break;
	
	            case Keys.Space:
	                result = "Space";
	                g_bDrawLookatPoint = !g_bDrawLookatPoint;
	                break;
	
	        }
	        result = result + Camera.GetPositionString();
	        result = result + Camera.GetTargetString();
	
	        reshape();
	        return result;
	    }
	
	    public override void TouchEvent(int x_position, int y_position)
	    {
	        if (x_position > width * 3/4)
	        {
	            Camera.MoveTarget(-4.0f, 0, 0);
	        }
	        if (x_position < width * 1/4)
	        {
	            Camera.MoveTarget(0, 0, 4.0f);
	        }
	        reshape();
	        display();
	    }
	}
}

