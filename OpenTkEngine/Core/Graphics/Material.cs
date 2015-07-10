using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTkEngine.Core
{
    public class Material
    {
        public Vector3 Ambient;
        public Vector3 Diffuse;
        public Vector3 Specular;
        public float Shininess;

        public Texture DiffuseTexture = null;

        public Material()
        {
            Ambient = new Vector3();
            Diffuse = new Vector3();
            Specular = new Vector3();
            Shininess = 128.0f;
        }

        public Material(float a1, float a2, float a3, float d1, float d2, float d3, float s1, float s2, float s3, float sh)
        {
            Ambient = new Vector3(a1, a2, a3);
            Diffuse = new Vector3(d1, d2, d3);
            Specular = new Vector3(s1, s2, s3);
            Shininess = sh * 128.0f;
        }

        public void Bind()
        {
            int uAmbientReflectivityLocation = Graphics.GetShader().GetUniformLocation("uMaterial.AmbientReflectivity");
            GL.Uniform3(uAmbientReflectivityLocation, Ambient);
            int uDiffuseReflectivityLocation = Graphics.GetShader().GetUniformLocation("uMaterial.DiffuseReflectivity");
            GL.Uniform3(uDiffuseReflectivityLocation, Diffuse);
            int uSpecularReflectivityLocation = Graphics.GetShader().GetUniformLocation("uMaterial.SpecularReflectivity");
            GL.Uniform3(uSpecularReflectivityLocation, Specular);
            int uShininessLocation = Graphics.GetShader().GetUniformLocation("uMaterial.Shininess");
            GL.Uniform1(uShininessLocation, Shininess);

            if (DiffuseTexture != null)
                DiffuseTexture.Bind();
        }

        //gems
        public static Material Emerald = new Material(0.0215f, 0.1745f, 0.0215f, 0.07568f, 0.61424f, 0.07568f, 0.633f, 0.727811f, 0.633f, 0.6f);
        public static Material Jade = new Material(0.135f, 0.2225f, 0.1575f, 0.54f, 0.89f, 0.63f, 0.316228f, 0.316228f, 0.316228f, 0.1f);
        public static Material Obsidian = new Material(0.05375f, 0.05f, 0.06625f, 0.18275f, 0.17f, 0.22525f, 0.332741f, 0.328634f, 0.346435f, 0.3f);
        public static Material Pearl = new Material(0.25f, 0.20725f, 0.20725f, 1f, 0.829f, 0.829f, 0.296648f, 0.296648f, 0.296648f, 0.088f);
        public static Material Ruby = new Material(0.1745f, 0.01175f, 0.01175f, 0.61424f, 0.04136f, 0.04136f, 0.727811f, 0.626959f, 0.626959f, 0.6f);
        public static Material Turquoise = new Material(0.1f, 0.18725f, 0.1745f, 0.396f, 0.74151f, 0.69102f, 0.297254f, 0.30829f, 0.306678f, 0.1f);
        //metals
        public static Material Brass = new Material(0.329412f, 0.223529f, 0.027451f, 0.780392f, 0.568627f, 0.113725f, 0.992157f, 0.941176f, 0.807843f, 0.21794872f);
        public static Material Bronze = new Material(0.2125f, 0.1275f, 0.054f, 0.714f, 0.4284f, 0.18144f, 0.393548f, 0.271906f, 0.166721f, 0.2f);
        public static Material Chrome = new Material(0.25f, 0.25f, 0.25f, 0.4f, 0.4f, 0.4f, 0.774597f, 0.774597f, 0.774597f, 0.6f);
        public static Material Copper = new Material(0.19125f, 0.0735f, 0.0225f, 0.7038f, 0.27048f, 0.0828f, 0.256777f, 0.137622f, 0.086014f, 0.1f);
        public static Material Gold = new Material(0.24725f, 0.1995f, 0.0745f, 0.75164f, 0.60648f, 0.22648f, 0.628281f, 0.555802f, 0.366065f, 0.4f);
        public static Material Silver = new Material(0.19225f, 0.19225f, 0.19225f, 0.50754f, 0.50754f, 0.50754f, 0.508273f, 0.508273f, 0.508273f, 0.4f);
        //plastics
        public static Material BlackPlastic = new Material(0.0f, 0.0f, 0.0f, 0.01f, 0.01f, 0.01f, 0.5f, 0.5f, 0.5f, .25f);
        public static Material CyanPlastic = new Material(0.1f, 0.1f, 0.06f, 0.0f, 0.50980392f, 0.50980392f, 0.50196078f, 0.50196078f, 0.50196078f, .25f);
        public static Material GreenPlastic = new Material(0.0f, 0.0f, 0.0f, 0.1f, 0.35f, 0.1f, 0.45f, 0.55f, 0.45f, .25f);
        public static Material RedPlastic = new Material(0.0f, 0.0f, 0.0f, 0.5f, 0.0f, 0.0f, 0.7f, 0.6f, 0.6f, .25f);
        public static Material WhitePlastic = new Material(0.0f, 0.0f, 0.0f, 0.55f, 0.55f, 0.55f, 0.70f, 0.70f, 0.70f, .25f);
        public static Material YellowPlastic = new Material(0.0f, 0.0f, 0.0f, 0.5f, 0.5f, 0.0f, 0.60f, 0.60f, 0.50f, .25f);
        //rubbers
        public static Material BlackRubber = new Material(0.02f, 0.02f, 0.02f, 0.01f, 0.01f, 0.01f, 0.4f, 0.4f, 0.4f, .078125f);
        public static Material CyanRubber = new Material(0.0f, 0.05f, 0.05f, 0.4f, 0.5f, 0.5f, 0.04f, 0.7f, 0.7f, .078125f);
        public static Material GreenRubber = new Material(0.0f, 0.05f, 0.0f, 0.4f, 0.5f, 0.4f, 0.04f, 0.7f, 0.04f, .078125f);
        public static Material RedRubber = new Material(0.05f, 0.0f, 0.0f, 0.5f, 0.4f, 0.4f, 0.7f, 0.04f, 0.04f, .078125f);
        public static Material WhiteRubber = new Material(0.05f, 0.05f, 0.05f, 0.5f, 0.5f, 0.5f, 0.7f, 0.7f, 0.7f, .078125f);
        public static Material YellowRubber = new Material(0.05f, 0.05f, 0.0f, 0.5f, 0.5f, 0.4f, 0.7f, 0.7f, 0.04f, .078125f);

        public static Material Crimson = new Material(0.05f, 0f, 0f, 1f, 0f, 0.247f, 1f, 0f, 0.25f, 0.25f);
        public static Material DodgerBlue = new Material(0f, 0f, 0.05f, 0.117f, 0.565f, 1f, 0.1f, 0.12f, 1f, 0.25f);
    }
}
