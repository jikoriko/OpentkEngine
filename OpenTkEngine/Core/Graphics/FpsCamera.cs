using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTkEngine.Core
{
    public class FpsCamera : Camera
    {

        public FpsCamera(Vector3 position)
            : base(position)
        {

        }

        public override void Update()
        {
            base.Update();

            if (Input.KeyDown(Key.W))
            {
                viewMatrix *= Matrix4.CreateTranslation(0, 0, 0.25f);
            }
            else if (Input.KeyDown(Key.S))
            {
                viewMatrix *= Matrix4.CreateTranslation(0, 0, -0.25f);
            }
            if (Input.KeyDown(Key.A))
            {
                viewMatrix *= Matrix4.CreateTranslation(0.25f, 0, 0);
            }
            else if (Input.KeyDown(Key.D))
            {
                viewMatrix *= Matrix4.CreateTranslation(-0.25f, 0, 0);
            }

            if (Input.KeyDown(Key.Space))
            {
                viewMatrix *= Matrix4.CreateTranslation(0, -0.25f, 0);
            }
            else if (Input.KeyDown(Key.LShift))
            {
                viewMatrix *= Matrix4.CreateTranslation(0, 0.25f, 0);
            }

            if (Input.KeyDown(Key.Q))
            {
                viewMatrix *= Matrix4.CreateRotationZ(-0.112f);
            }
            else if (Input.KeyDown(Key.E))
            {
                viewMatrix *= Matrix4.CreateRotationZ(0.112f);
            }

            Vector2 mouseMovement = Input.GetMouseScroll();
            if (mouseMovement.X != 0)
            {
                viewMatrix *= Matrix4.CreateRotationY(0.002f * mouseMovement.X);
            }
            if (mouseMovement.Y != 0)
            {
                viewMatrix *= Matrix4.CreateRotationX(0.002f * mouseMovement.Y);
            }
        }
    }
}
