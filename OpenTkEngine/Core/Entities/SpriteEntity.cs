using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTkEngine.Core
{
    public class SpriteEntity : Entity
    {
        protected Texture _texture;

        public SpriteEntity(float x, float y, float z, string spriteName)
            : base(x, y, z)
        {
            _texture = Assets.GetTexture(spriteName);
        }

        public override void Render()
        {
            base.Render();
            Graphics.DrawTexture(_texture, _position.X, _position.Y, _position.Z, Color4.White);
        }
    }
}
