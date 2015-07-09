using OpenTK;
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
        protected Materials.Material _material = Materials.Crimson;
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
            Graphics.SetModelMaterial(_material);
            Matrix4 matrix = Matrix4.CreateScale(_scale) * Matrix4.CreateTranslation(_position);
            Graphics.RenderModel(_model, matrix);
        }

    }
}
