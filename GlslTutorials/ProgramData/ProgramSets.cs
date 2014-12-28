using System;
using System.Collections.Generic;

namespace GlslTutorials
{
	public class ProgramSets
	{
		public static List<ProgramSet> programs;
		static ProgramSets ()
		{
			programs = new List<ProgramSet>();
			programs.Add(new ProgramSet("PosOnlyWorldTransform_vert ColorUniform_frag", 
				VertexShaders.PosOnlyWorldTransform_vert, FragmentShaders.ColorUniform_frag));

			programs.Add(new ProgramSet("PosColorWorldTransform_vert ColorMultUniform_frag", 
				VertexShaders.PosColorWorldTransform_vert, FragmentShaders.ColorMultUniform_frag));

			programs.Add(new ProgramSet("DirAmbVertexLighting_PN_vert ColorPassthrough_frag", 
				VertexShaders.DirAmbVertexLighting_PN_vert, FragmentShaders.ColorPassthrough_frag));

			programs.Add(new ProgramSet("DirVertexLighting_PCN ColorPassthrough_frag", 
				VertexShaders.DirVertexLighting_PCN, FragmentShaders.ColorPassthrough_frag));

			programs.Add(new ProgramSet("unlit", VertexShaders.unlit, FragmentShaders.unlit));

			programs.Add(new ProgramSet("BasicTexture_PN ShaderGaussian", VertexShaders.BasicTexture_PN, 
				FragmentShaders.ShaderGaussian));
				
			programs.Add(new ProgramSet("PosColorWorldTransform_vert ColorPassthrough_frag", 
				VertexShaders.PosColorWorldTransform_vert, FragmentShaders.ColorPassthrough_frag));

			programs.Add(new ProgramSet("PosColorLocalTransform_vert ColorPassthrough_frag", 
				VertexShaders.PosColorLocalTransform_vert, FragmentShaders.ColorPassthrough_frag));

			programs.Add(new ProgramSet("HDR_PCN DiffuseSpecularHDR", 
				VertexShaders.HDR_PCN, FragmentShaders.DiffuseSpecularHDR));

			programs.Add(new ProgramSet("HDR_PCN DiffuseOnlyHDR", 
				VertexShaders.HDR_PCN, FragmentShaders.DiffuseOnlyHDR));

			programs.Add(new ProgramSet("HDR_PCN DiffuseSpecularMtlHDR", 
				VertexShaders.HDR_PCN, FragmentShaders.DiffuseSpecularMtlHDR));

			programs.Add(new ProgramSet("HDR_PCN DiffuseOnlyMtlHDR", 
				VertexShaders.HDR_PCN, FragmentShaders.DiffuseOnlyMtlHDR));

		}

		public static ProgramSet Find(string name)
		{
			foreach (ProgramSet ps in programs)
			{
				if (ps.name.Equals(name))
					{
						return ps;
					}
			}

			return null;
		}
	}
}

