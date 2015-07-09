using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace OpenTKEngine
{
	public class Ray
	{
		public Vector3 Start;
		public Vector3 End;
		public Vector3 Direction;

	    public Ray(Vector3 start, Vector3 direction)
	    {
	        Start = start;
            Direction = direction;
	        End = Start + Direction;
	    }

	    public double Length()
	    {
	        return Math.Sqrt((Direction.X*Direction.X) + (Direction.Y*Direction.Y));
	    }

		public Vector3 GetCollisionPosition(float t)
		{
			if (float.IsNaN (t)) {
				return Vector3.Zero;
			}

		    var pos = this.Start + this.Direction*t;

		    if (PointOnRay(pos.X, pos.Y))
		    {
		        return this.Start + this.Direction * t;
		    }
		    else
		    {
		        return Vector3.Zero;
		    }
		}

        public bool PointOnRay(float x, float y)
        {
            if ((!(x >= Start.X)) || (!(x <= End.X))) return false;
            return (y >= Start.Y) && (y <= End.Y);
        }
	}
	
    public class Ball : Entity
    {
        public Vector3 Velocity;
        public int Radius;
        public float Rotation = 0;
        public float RotationSpeed = 0;
        private Random _random;
 
        public bool IsColliding = false;

        public Ball(Vector3 pos, Sprite sprite, int radius, Type type) : base(pos, sprite, type)
        {
            // Effective random seed generation.
            _random = new Random(Guid.NewGuid().GetHashCode());

            Radius = radius;
            Velocity = new Vector3(0f, 0f, 0f);
        }

        public override void Update()
        {
            if (!IsColliding)
            {
                Velocity.Y += 3f;
            }

            Position.Value.X += Velocity.X;
            Position.Value.Y += Velocity.Y;

            Velocity.Y *= 0.95f;
            Velocity.X *= 0.99f;

            IsColliding = false;
        }

        public override void Render()
        {
            // Graphics.DrawCircle(Position.Value, Radius, Color4.Red);
        }

        public void Die()
        {
            Alive = false;
        }


        private List<float> CheckCollisionWithRay(float x1, float y1, float dx, float dy)
        {
            var dRay = new Vector2(dx, dy);

            var pcx = x1 - Position.Value.X;
            var pcy = y1 - Position.Value.Y;
            var dPc = new Vector2(pcx, pcy);

            var a = dRay.LengthSquared;
            var b = 2*Vector2.Dot(dRay, dPc);
            var c = dPc.LengthSquared - Radius*Radius;
            var discr = b*b - 4*a*c;

            if (discr < 0) return null;

            discr = (float)Math.Sqrt(discr);

            var ts = new List<float>();
            var t1 = discr - b;
            var t2 = -discr - b;

            if (t1 >= 0)
            {
                ts.Add(t1/(2*a));
            }
            if (t2 >= 0)
            {
                ts.Add(t2/(2*a));
            }

            return ts;
        }

        public float CheckCollision(Ray ray)
        {
            var list = CheckCollisionWithRay(ray.Start.X, ray.Start.Y, ray.Direction.X, ray.Direction.Y);

            if (list == null) return float.NaN;
            if (!list.Any()) return float.NaN;

            float biggest = list.First();
            if (biggest > list.Last()) biggest = list.Last();

            return biggest;
        }
    }
}
