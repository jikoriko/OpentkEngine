using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;

namespace OpenTkEngine.Core
{
    public class Model
    {
        private int[] _vboIDs;
        private int _vaoID;

        private float[] _vertices;
        private int[] _indices;

        private bool _vbosBound = false;
        private bool _texCoords = false;

        public Model(string filename)
        {
            ModelUtility utility = ModelUtility.LoadModel(filename);
            _vertices = utility.Vertices;
            _indices = utility.Indices;
        }

        public Model(float[] verts, int[] indices, bool texCoords)
        {
            _vertices = verts;
            _indices = indices;
            _texCoords = texCoords;
        }

        private void BindVBO()
        {
            if (_vboIDs == null)
            {
                _vboIDs = new int[2];
                GL.GenBuffers(2, _vboIDs);
            }

            if (!_vbosBound)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, _vboIDs[0]);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(_vertices.Length * sizeof(float)), _vertices, BufferUsageHint.StaticDraw);
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, _vboIDs[1]);
                GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(_indices.Length * sizeof(float)), _indices, BufferUsageHint.StaticDraw);

                int size;
                GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out size);
                if (_vertices.Length * sizeof(float) != size)
                {
                    throw new ApplicationException("Vertex data not loaded onto graphics card correctly");
                }

                GL.GetBufferParameter(BufferTarget.ElementArrayBuffer, BufferParameterName.BufferSize, out size);
                if (_indices.Length * sizeof(float) != size)
                {
                    throw new ApplicationException("Index data not loaded onto graphics card correctly");
                }
            }
            else
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, _vboIDs[0]);
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, _vboIDs[1]);
            }
            

            int vPositionLocation = Graphics.GetShader().GetAttribLocation("vPosition");
            //int vNormalLocation = Graphics.GetShader().GetAttribLocation("vNormal");
            GL.EnableVertexAttribArray(vPositionLocation);
            //GL.EnableVertexAttribArray(vNormalLocation);

            GL.VertexAttribPointer(vPositionLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            //GL.VertexAttribPointer(vNormalLocation, 3, VertexAttribPointerType.Float, true, 6 * sizeof(float), 3 * sizeof(float));

            _vbosBound = true;
        }
        
        public void BindVAO()
        {
            if (_vaoID == 0)
            {
                _vaoID = GL.GenVertexArray();
            }
            GL.BindVertexArray(_vaoID);
            if (!_vbosBound) BindVBO();
        }

        public void PreBind()
        {
            BindVAO();
            BindNone();
        }

        public static void BindNone()
        {
            GL.BindVertexArray(0);
        }

        /*
        private void SetupLightMaterialProperties(Materials.Material material)
        {
            int uAmbientReflectivityLocation = Global.CurrentShader.GetUniformLocation("uMaterial.AmbientReflectivity");
            GL.Uniform3(uAmbientReflectivityLocation, material.ambient);
            int uDiffuseReflectivityLocation = Global.CurrentShader.GetUniformLocation("uMaterial.DiffuseReflectivity");
            GL.Uniform3(uDiffuseReflectivityLocation, material.diffuse);
            int uSpecularReflectivityLocation = Global.CurrentShader.GetUniformLocation("uMaterial.SpecularReflectivity");
            GL.Uniform3(uSpecularReflectivityLocation, material.specular);
            int uShininessLocation = Global.CurrentShader.GetUniformLocation("uMaterial.Shininess");
            GL.Uniform1(uShininessLocation, material.shininess);
        }
        */

        public void RenderVAO(Matrix4 matrix)
        {
            RenderVAO(matrix, 0);
        }

        public void RenderVAO(Matrix4 matrix, int offset)
        {
            RenderVAO(matrix, offset, _indices.Length - offset);
        }

        public void RenderVAO(Matrix4 matrix, int offset, int count)
        {
            Vector3 scale = matrix.ExtractScale();
            Quaternion rotation = matrix.ExtractRotation();
            Vector3 position = matrix.ExtractTranslation();

            Graphics.SetColor(Color4.Red);

            int uModel = Graphics.GetShader().GetUniformLocation("uModel");
            GL.UniformMatrix4(uModel, true, ref matrix);

            int uWorldLocation = Graphics.GetShader().GetUniformLocation("uWorld");
            Matrix4 worldMatrix = Matrix4.Identity;
            GL.UniformMatrix4(uWorldLocation, true, ref worldMatrix);

            //SetupLightMaterialProperties(Global.CurrentMaterial);
            BindVAO();
            GL.DrawElements(PrimitiveType.Triangles, count, DrawElementsType.UnsignedInt, offset * sizeof(uint));
            BindNone();
        }

        public void DeleteBuffers()
        {
            if (_vboIDs != null)
            {
                GL.DeleteBuffers(2, _vboIDs);
                _vboIDs = null;
                _vbosBound = false;
            }

            if (_vaoID != 0)
            {
                GL.DeleteVertexArray(_vaoID);
                _vaoID = 0;
            }
        }
    }
}
