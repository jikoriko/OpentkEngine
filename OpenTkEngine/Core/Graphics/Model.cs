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
        private List<ModelMesh> _meshes;

        public Model(List<ModelMesh> meshes)
        {
            _meshes = meshes;
        }

        public List<ModelMesh> GetMeshes()
        {
            return _meshes;
        }

        public void DeleteBuffers()
        {
            foreach (ModelMesh mesh in _meshes)
            {
                mesh.DeleteBuffers();
            }
            _meshes.Clear();
        }
    }
}
