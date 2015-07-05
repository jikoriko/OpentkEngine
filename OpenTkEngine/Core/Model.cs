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
            int vNormalLocation = Graphics.GetShader().GetAttribLocation("vNormal");
            GL.EnableVertexAttribArray(vPositionLocation);
            GL.EnableVertexAttribArray(vNormalLocation);

            GL.VertexAttribPointer(vPositionLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.VertexAttribPointer(vNormalLocation, 3, VertexAttribPointerType.Float, true, 6 * sizeof(float), 3 * sizeof(float));

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

        public int GetVaoID()
        {
            return _vaoID;
        }

        public int GetIndicesLength()
        {
            return _indices.Length;
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
