using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using OpenTK;
using OpenTK.Graphics.OpenGL;

using MeshMap = System.Collections.Generic.Dictionary<string, GlslTutorials.SceneMesh>;
using MeshType = System.Collections.Generic.KeyValuePair<string, GlslTutorials.SceneMesh>;
using TextureMap = System.Collections.Generic.Dictionary<string, GlslTutorials.SceneTexture>;
using TextureType = System.Collections.Generic.KeyValuePair<string, GlslTutorials.SceneTexture>;
using ProgramMap = System.Collections.Generic.Dictionary<string, GlslTutorials.SceneProgram>;
using ProgramType = System.Collections.Generic.KeyValuePair<string, GlslTutorials.SceneProgram>;
using NodeMap = System.Collections.Generic.Dictionary<string, GlslTutorials.SceneNode>;
using NodeType = System.Collections.Generic.KeyValuePair<string, GlslTutorials.SceneNode>;

namespace GlslTutorials
{
	class SceneImpl
	{
		uint FORCE_SRGB_COLORSPACE_FMT = 1;
		private MeshMap m_meshes = new MeshMap();
		private TextureMap m_textures = new TextureMap();
		private  ProgramMap m_progs = new ProgramMap();
		private NodeMap m_nodes = new NodeMap();

		List<SceneNode> m_rootNodes = new List<SceneNode>();

		List<int> m_samplers = new List<int>();

		// To allow reflectoin
		VertexShaders vertexShaders = new VertexShaders();
		FragmentShaders fragmentShaders = new FragmentShaders();

		public SceneImpl(string filename)
		{
			string XmlFilesDirectory = GlsTutorialsClass.ProjectDirectory + @"/XmlFilesForMeshes/";
			Stream inputStream =  File.OpenRead(XmlFilesDirectory + filename);

			XmlDocument doc = new XmlDocument();
			doc.Load(inputStream);

			XmlElement pSceneNode = doc.DocumentElement;
		
			PARSE_THROW(pSceneNode, " Scene node not found in scene file.");

			try
			{
				ReadMeshes(pSceneNode);
				ReadTextures(pSceneNode);
				ReadPrograms(pSceneNode);
				ReadNodes(null, pSceneNode);
			}
			catch(Exception ex)
			{
				MessageBox.Show("Error creating scene " + ex.ToString());
				m_nodes.Clear();
				m_progs.Clear();
				m_textures.Clear();
				m_meshes.Clear();
			}

			MakeSamplerObjects(m_samplers);
		}

		~SceneImpl()
		{
			int[] samplers = m_samplers.ToArray();
			GL.DeleteSamplers(m_samplers.Count, samplers);
			m_samplers.Clear();
			m_nodes.Clear();
			m_progs.Clear();
			m_textures.Clear();
			m_meshes.Clear();
		}

		public void Render(Matrix4 cameraMatrix)
		{
			foreach (KeyValuePair<string, SceneNode> sn in m_nodes)
			{
				sn.Value.Render(m_samplers, cameraMatrix);
			}
		}

		public NodeRef FindNode(string nodeName)
		{
			foreach(NodeType nt in m_nodes)
			{
				if (nt.Key == nodeName) return new NodeRef(nt.Value);
			}
			MessageBox.Show("Could not find node for " + nodeName);
			return null;
		}

		public int FindProgram(string progName)
		{
			foreach (ProgramType pt in m_progs)
			{
				if (pt.Key == progName)
				{
					return pt.Value.GetProgram();
				}
			}
			MessageBox.Show("Could not find program " + progName);
			return -1;
		}

		public Mesh FindMesh(string meshName)
		{
			foreach (MeshType mt in  m_meshes)
			{
				if (mt.Key == meshName)
				{
					return mt.Value.GetMesh();
				}
			}
			MessageBox.Show("Could not find mesh " + meshName);
			return null;
		}

		public KeyValuePair<int, TextureTarget> FindTexture(string textureName)
		{
			foreach (TextureType tt in m_textures)
			{
				if (tt.Key == textureName)
				{
					return new KeyValuePair<int, TextureTarget>(tt.Value.GetTexture(), tt.Value.GetTextureType());
				}
			}
			MessageBox.Show("Could not find texture " + textureName);
			return  new KeyValuePair<int, TextureTarget>(-1, (TextureTarget)(-1));
		}

		private void ReadMeshes(XmlNode scene)
		{
			XmlNodeList pMeshNodes = ((XmlElement)scene).GetElementsByTagName("mesh");
			foreach(XmlNode pMeshNode in pMeshNodes)
			{
				ReadMesh((XmlElement)pMeshNode);
			}
		}

		private void ReadMesh(XmlElement meshNode)
		{
			string pNameNode = meshNode.GetAttribute("xml:id");
			string pFilenameNode = meshNode.GetAttribute("file");

			PARSE_THROW(pNameNode, "Mesh found with no `xml:id` name specified.");
			PARSE_THROW(pFilenameNode, "Mesh found with no `file` filename specified.");

			string name = pNameNode; //FIXME check make_string(*pNameNode)
			if (m_meshes.ContainsKey(name))
			{
				MessageBox.Show("The mesh named \"" + name + "\" already exists.");
			}

			string XmlFilesDirectory = GlsTutorialsClass.ProjectDirectory + @"/XmlFilesForMeshes";
			Stream fileStream = File.OpenRead(XmlFilesDirectory + @"/" + pFilenameNode);
			SceneMesh pMesh = new SceneMesh(fileStream);
			m_meshes.Add(name, pMesh);
		}

		private void ReadTextures(XmlElement scene)
		{
			XmlNodeList xnl = scene.GetElementsByTagName("texture");
			foreach (XmlNode pTexNode in xnl)
			{
				ReadTexture((XmlElement)pTexNode);
			}
		}

		private void ReadTexture(XmlElement TexNode)
		{
			string pNameNode = TexNode.GetAttribute("xml:id");
			string pFilenameNode = TexNode.GetAttribute("file");

			if (pNameNode == "") return;

			PARSE_THROW(pNameNode, "Texture found with no `xml:id` name specified.");
			PARSE_THROW(pFilenameNode, "Texture found with no `file` filename specified.");

			string pSrgbNode = TexNode.GetAttribute("srgb");

			if(m_textures.Keys.Contains(pNameNode))
			{
				MessageBox.Show("The texture named \"" + pNameNode + "\" already exists.");
			}

			uint creationFlags = 0;
			if(pSrgbNode != null)
				creationFlags |= FORCE_SRGB_COLORSPACE_FMT;

			SceneTexture pTexture = new SceneTexture(pFilenameNode, creationFlags);

			m_textures.Add(pNameNode, pTexture);
		}

		private void ReadPrograms(XmlElement scene)
		{
			XmlNodeList attributeNodes = scene.GetElementsByTagName("prog");
			foreach (XmlNode pProgNode in attributeNodes)
			{
				ReadProgram((XmlElement)pProgNode);
			}
		}

		private void ReadProgram(XmlElement progNode)
		{
			string pNameNode = progNode.GetAttribute("xml:id");
			string pVertexShaderNode = progNode.GetAttribute("vert");
			string pFragmentShaderNode = progNode.GetAttribute("frag");
			string pModelMatrixNode = progNode.GetAttribute("model-to-camera");

			PARSE_THROW(pNameNode, "Program found with no `xml:id` name specified.");
			PARSE_THROW(pVertexShaderNode, "Program found with no `vert` vertex shader specified.");
			PARSE_THROW(pFragmentShaderNode, "Program found with no `frag` fragment shader specified.");
			PARSE_THROW(pModelMatrixNode, "Program found with no model-to-camera matrix uniform name specified.");

			//Optional.
			string pNormalMatrixNode = progNode.GetAttribute("normal-model-to-camera");
			string pGeometryShaderNode = progNode.GetAttribute("geom");

			if(m_progs.Keys.Contains(pNameNode))
			{
				MessageBox.Show("The program named \"" + pNameNode + "\" already exists.");
			}
				
			List<int> shaders = new List<int>();
			int program = 0;
			string vertexShaderString = pVertexShaderNode.ToLower().Substring(0, pVertexShaderNode.Length - 5);
			var vertexShaderField = vertexShaders.GetType().GetField(vertexShaderString);
			string vertexShader = (string)vertexShaderField.GetValue(vertexShaders);

			string fragmentShaderString = pFragmentShaderNode.ToLower().Substring(0, pVertexShaderNode.Length - 5);
			var  fragmentShaderField = fragmentShaders.GetType().GetField(fragmentShaderString);
			string fragmentShader = (string)fragmentShaderField.GetValue(fragmentShaders);

			try
			{
				//  LoadShader(GL_VERTEX_SHADER, make_string(*pVertexShaderNode))
				// GL_FRAGMENT_SHADER, make_string(*pFragmentShaderNode))
				//LoadShader(GL_GEOMETRY_SHADER, make_string(*pGeometryShaderNode)
				shaders.Add(Shader.compileShader(ShaderType.VertexShader, vertexShader));  // FIXME parse shaders
				shaders.Add(Shader.compileShader(ShaderType.FragmentShader, fragmentShader)); // FIXME parse shaders
				if(pGeometryShaderNode != "")
					shaders.Add(Shader.compileShader(ShaderType.GeometryShader, "GEOMETRY_SHADER"));// FIXME parse shaders
				program = Shader.LinkProgram(shaders);
			}
			catch(Exception ex)
			{
				MessageBox.Show("Error creating program " + ex.ToString());
				foreach(int shader in shaders)
				{
					GL.DeleteShader(shader);
				}
			}

			foreach (int shader in shaders)
			{
				GL.DeleteShader(shader);
			}
				
			int matrixLoc = GL.GetUniformLocation(program, pModelMatrixNode);
			if(matrixLoc == -1)
			{
				GL.DeleteProgram(program);
				MessageBox.Show("Could not find the matrix uniform " + pModelMatrixNode +
					" in program " + pNameNode);
			}

			int normalMatLoc = -1;
			if(pNormalMatrixNode != "")
			{
				normalMatLoc = GL.GetUniformLocation(program, pNormalMatrixNode);
				if(normalMatLoc == -1)
				{
					GL.DeleteProgram(program);
					MessageBox.Show("Could not find the normal matrix uniform " + pNormalMatrixNode +
						" in program " + pNameNode);
				}
			}

			m_progs[pNameNode] = new SceneProgram(program, matrixLoc, normalMatLoc);

			ReadProgramContents(program, progNode);
		}

		private void ReadProgramContents(int program, XmlElement progNode)
		{
			List<string> blockBindings = new List<string>();
			List<string> samplerBindings = new List<string>();

			XmlNodeList pChildNodes = progNode.ChildNodes;

			foreach (XmlNode pChildNode in  pChildNodes)
			{
				//FIXME if(pChildNode.type() != rapidxml::node_element)
				// FIXME	continue;

				string childName = pChildNode.Name;
				if(childName == "block")
				{
					string pNameNode = ((XmlElement)pChildNode).GetAttribute("name");
					string pBindingNode = ((XmlElement)pChildNode).GetAttribute("binding");

					PARSE_THROW(pNameNode, "Program `block` element with no `name`.");
					PARSE_THROW(pBindingNode, "Program `block` element with no `binding`.");

					if(blockBindings.Contains(pNameNode))
					{
						MessageBox.Show("The uniform block named " + pNameNode + " is used twice in the same program.");
					}

					blockBindings.Add(pNameNode);

					// FIXME int blockIx = GL.GetUniformBlockIndex(program, name);

					// FIXME if(blockIx == GL_INVALID_INDEX)
					// FIXME {
					// FIXME 	std::cout << "Warning: the uniform block " << name << " could not be found." << std::endl;
					// FIXME 	continue;
					// FIXME }

					// FIXME int bindPoint = rapidxml::attrib_to_int(*pBindingNode, ThrowAttrib);
					// FIXME GL.UniformBlockBinding(program, blockIx, bindPoint);
				}
				else if(childName == "sampler")
				{
					string pNameNode = ((XmlElement)pChildNode).GetAttribute("name");
					string pTexunitNode = ((XmlElement)pChildNode).GetAttribute("unit");

					PARSE_THROW(pNameNode, "Program `sampler` element with no `name`.");
					PARSE_THROW(pTexunitNode, "Program `sampler` element with no `unit`.");

					if(samplerBindings.Contains(pNameNode))
					{
						MessageBox.Show("A sampler " + pNameNode + " is used twice in the same program.");
					}
						
					samplerBindings.Add(pNameNode);

					int samplerLoc = GL.GetUniformLocation(program, pNameNode);
					if(samplerLoc == -1)
					{
						MessageBox.Show("Warning: the sampler " + pNameNode + " could not be found.");
					}

					int textureUnit = int.Parse(pTexunitNode);
					GL.UseProgram(program);
					GL.Uniform1(samplerLoc, textureUnit);
					GL.UseProgram(0);
				}
				else
				{
					//Bad node. Die.
					MessageBox.Show("Unknown element found in program.");
				}
			}
		}

		private void ReadNodes(SceneNode pParent, XmlElement scene)
		{
			XmlNodeList xnl = scene.GetElementsByTagName("node");
			foreach(XmlNode pNodeNode in xnl)
			{
				ReadNode(pParent, (XmlElement)pNodeNode);
			}
		}

		private void ReadNode(SceneNode pParent, XmlElement nodeNode)
		{
			string pNameNode = nodeNode.GetAttribute("name");
			string pMeshNode = nodeNode.GetAttribute("mesh");
			string pProgNode = nodeNode.GetAttribute("prog");

			PARSE_THROW(pNameNode, "Node found with no `name` name specified.");
			PARSE_THROW(pMeshNode, "Node found with no `mesh` name specified.");
			PARSE_THROW(pProgNode, "Node found with no `prog` name specified.");

			string pPositionNode = nodeNode.GetAttribute("pos");
			string pOrientNode = nodeNode.GetAttribute("orient");
			string pScaleNode = nodeNode.GetAttribute("scale");

			PARSE_THROW(pPositionNode, "Node found with no `pos` specified.");

			if(m_nodes.Keys.Contains(pNameNode))
			{
				MessageBox.Show("The node named " + pNameNode + " already exist.");
			}
				
			if(!m_meshes.Keys.Contains(pMeshNode))
			{
				MessageBox.Show("The node named " + pNameNode + " references the mesh " + pMeshNode + " which does not exist.");
			}
				
			if(!m_progs.Keys.Contains(pProgNode))
			{
				MessageBox.Show("The node named " + pNameNode + " references the program " + pProgNode + " which does not exist.");
			}

			Vector3 nodePos = ParseVec3(pPositionNode);

			SceneNode pNode = new SceneNode(m_meshes[pMeshNode], m_progs[pProgNode], nodePos, ReadNodeTextures(nodeNode));
			m_nodes[pNameNode] = pNode;

			//parent/child nodes.
			if(pParent == null)
				m_rootNodes.Add(pNode);

			if(pOrientNode != null)
				pNode.SetNodeOrient(ParseQuaternion(pOrientNode));
				
			if(pScaleNode != null)
			{
				try
				{
					Vector3 result = ParseVec3(pScaleNode);
					pNode.SetNodeScale(result);
				}
				catch 
				{
					float unifScale = float.Parse(pScaleNode);
					pNode.SetNodeScale(new Vector3(unifScale));
				}
			}
			ReadNodeNotes(nodeNode);
		}

		private void ReadNodeNotes(XmlElement nodeNode)
		{
			XmlNodeList xnl = nodeNode.GetElementsByTagName("note");
			foreach(XmlNode noteNode in xnl)
			{
				XmlNodeList pNameNode = nodeNode.GetElementsByTagName("name");
				PARSE_THROW(pNameNode, "Notations on nodes must have a `name` attribute.");
			}
		}

		private List<TextureBinding> ReadNodeTextures(XmlElement nodeNode)
		{
			List<TextureBinding> texBindings = new List<TextureBinding>();
			List<int> texUnits = new List<int>();

			XmlNodeList xnl = nodeNode.GetElementsByTagName("texture");
			foreach(XmlNode texNode in xnl)
			{
				string pNameNode = ((XmlElement)texNode).GetAttribute("name");
				string pUnitName = ((XmlElement)texNode).GetAttribute("unit");
				string pSamplerName = ((XmlElement)texNode).GetAttribute("sampler");

				PARSE_THROW(pNameNode, "Textures on nodes must have a `name` attribute.");
				PARSE_THROW(pUnitName, "Textures on nodes must have a `unit` attribute.");
				PARSE_THROW(pSamplerName, "Textures on nodes must have a `sampler` attribute.");

				if(!m_textures.Keys.Contains(pNameNode))
				{
					MessageBox.Show("The node texture named " + pNameNode + "  is a texture which does not exist.");
				}

				TextureBinding binding = new TextureBinding();

				binding.pTex = m_textures[pNameNode];
				binding.texUnit = int.Parse(pUnitName);
				binding.sampler = GetTypeFromName(pSamplerName);

				if(texUnits.Contains(binding.texUnit))
				{
					MessageBox.Show("Multiply bound texture unit in node texture " + pNameNode);
				}
				texBindings.Add(binding);

				texUnits.Add(binding.texUnit);
			}

			return texBindings;
		}

		private void MakeSamplerObjects(List<int> samplers)
		{
			int[] samplerArray = new int[(int)SamplerTypes.MAX_SAMPLERS];
			GL.GenSamplers((int)SamplerTypes.MAX_SAMPLERS, samplerArray);
			foreach (int i in samplerArray)
			{
				samplers.Add(i);
			}

			//Always repeat.
			for(int samplerIx = 0; samplerIx < (int)SamplerTypes.MAX_SAMPLERS; samplerIx++)
			{
				GL.SamplerParameter(samplers[samplerIx], SamplerParameterName.TextureWrapS, (int)All.Repeat);
				GL.SamplerParameter(samplers[samplerIx], SamplerParameterName.TextureWrapT, (int)All.Repeat);
				GL.SamplerParameter(samplers[samplerIx], SamplerParameterName.TextureWrapR, (int)All.Repeat);
			}

			GL.SamplerParameter(samplers[0], SamplerParameterName.TextureMagFilter, (int)All.Nearest);
			GL.SamplerParameter(samplers[0], SamplerParameterName.TextureMinFilter, (int)All.Nearest);

			GL.SamplerParameter(samplers[1], SamplerParameterName.TextureMagFilter, (int)All.Linear);
			GL.SamplerParameter(samplers[1], SamplerParameterName.TextureMinFilter, (int)All.Linear);

			GL.SamplerParameter(samplers[2], SamplerParameterName.TextureMagFilter, (int)All.Nearest);
			GL.SamplerParameter(samplers[2], SamplerParameterName.TextureMinFilter, (int)All.NearestMipmapNearest);

			GL.SamplerParameter(samplers[3], SamplerParameterName.TextureMagFilter, (int)All.Linear);
			GL.SamplerParameter(samplers[3], SamplerParameterName.TextureMinFilter, (int)All.LinearMipmapLinear);

			float maxAniso = 4.0f;
			// FIXME GL.GetFloat(GL_MAX_TEXTURE_MAX_ANISOTROPY_EXT, &maxAniso);

			GL.SamplerParameter(samplers[4], SamplerParameterName.TextureMagFilter, (int)All.Linear);
			GL.SamplerParameter(samplers[4], SamplerParameterName.TextureMinFilter, (int)All.LinearMipmapLinear);
			GL.SamplerParameter(samplers[4], SamplerParameterName.TextureMaxAnisotropyExt, maxAniso / 2.0f);

			GL.SamplerParameter(samplers[5], SamplerParameterName.TextureMagFilter, (int)All.Linear);
			GL.SamplerParameter(samplers[5], SamplerParameterName.TextureMinFilter, (int)All.LinearMipmapLinear);
			GL.SamplerParameter(samplers[5], SamplerParameterName.TextureMaxAnisotropyExt, maxAniso);
		}

		SamplerTypes GetTypeFromName(string name)
		{
			string[] samplerNames = new string[]
			{
				"nearest",
				"linear",
				"mipmap nearest",
				"mipmap linear",
				"anisotropic",
				"half anisotropic",
			};
				
			for(int spl = 0; spl < (int)SamplerTypes.MAX_SAMPLERS; ++spl)
			{
				if(name == samplerNames[spl])
					return (SamplerTypes)(spl);
			}

			MessageBox.Show("Unknown sampler name: " + name);
			return (SamplerTypes)(-1);
		}


		private void PARSE_THROW(string message)
		{
			MessageBox.Show(message);
		}

		private void PARSE_THROW(object obj, string message)
		{
			if (obj == null) MessageBox.Show("Object is null " + message);
		}

		Vector3 ParseVec3(string strVec3)
		{
			List<float> newFloats = strVec3.Split(' ').Select(s => Convert.ToSingle(s)).ToList();
			Vector3 ret = new Vector3(newFloats[0], newFloats[1], newFloats[2]);
			return ret;
		}

		Quaternion ParseQuaternion(string strQuaternion)
		{
			List<float> newFloats = strQuaternion.Split(' ').Select(s => Convert.ToSingle(s)).ToList();
			Quaternion ret = new Quaternion(newFloats[0], newFloats[1], newFloats[2], newFloats[3]);
			return ret;
		}
	};
}

