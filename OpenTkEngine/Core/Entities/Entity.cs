using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTkEngine.Core
{
    public class Entity
    {
        protected Vector3 _position;

        public Entity(float x, float y, float z)
        {
            _position = new Vector3(x, y, z);
        }

        public Vector3 GetPosition()
        {
            return _position;
        }

        public void SetPosition(Vector3 position)
        {
            _position = position;
        }

        public void SetPosition(float x, float y, float z)
        {
            _position.X = x;
            _position.Y = y;
            _position.Z = z;
        }

        public virtual void Update()
        {

        }

        public virtual void Render()
        {

        }
    }
}
