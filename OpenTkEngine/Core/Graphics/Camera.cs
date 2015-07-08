using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTkEngine.Core
{
    public class Camera
    {
        protected Matrix4 viewMatrix;

        public Camera(Vector3 _position)
        {
            viewMatrix = Matrix4.CreateTranslation(_position);
        }

        public virtual void Update()
        {
        }

        public virtual void LookThrough()
        {
            Graphics.SetWorldMatrix(viewMatrix);
        }

    }
}
