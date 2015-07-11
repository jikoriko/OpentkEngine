using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTkEngine.Core
{
    public class ModelEntity : Entity
    {
        protected Model _model;
        protected Material _material = Material.Crimson;
        protected float _scale = 1.0f;

        public ModelEntity(float x, float y, float z, string modelName)
            : base(x, y, z)
        {
            _model = Assets.GetModel(modelName);
        }

        public void SetScale(float scale)
        {
            _scale = scale;
        }

        public override void Render()
        {
            base.Render();
            _material.Bind();
            Matrix4 matrix = Matrix4.CreateScale(_scale) * Matrix4.CreateTranslation(_position);

            foreach(ModelMesh mesh in _model.GetMeshes())
                Graphics.RenderModelMesh(mesh, matrix, Color4.White);
        }

    }
}
