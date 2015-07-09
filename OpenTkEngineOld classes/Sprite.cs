using System.Drawing;
using OpenTK;
using OpenTK.Graphics;

namespace OpenTKEngine
{
    public class Sprite
    {
        public Texture SpriteSheet;
        private readonly int _xFrames = 1;
        private readonly int _yFrames = 1;
        private int _xFrame = 0, _yFrame = 0;
        private readonly int _spriteWidth;
        private readonly int _spriteHeight;

        public Sprite(Texture spriteSheet)
        {
            SpriteSheet = spriteSheet;
            this._spriteWidth = spriteSheet.GetWidth() / _xFrames;
            this._spriteHeight = spriteSheet.GetHeight() / _yFrames;
        }

        public Sprite(Texture spriteSheet, int xFrames, int yFrames)
        {
            xFrames = xFrames < 1 ? 1 : xFrames;
            yFrames = yFrames < 1 ? 1 : yFrames;
            this.SpriteSheet = spriteSheet;
            this._xFrames = xFrames; 
            this._yFrames = yFrames;
            this._spriteWidth = spriteSheet.GetWidth() / xFrames;
            this._spriteHeight = spriteSheet.GetHeight() / yFrames;
        }

        public int GetWidth()
        {
            return _spriteWidth;
        }

        public int GetHeight()
        {
            return _spriteHeight;
        }

        public void IncrementXFrame()
        {
            this._xFrame = this._xFrame < this._xFrames - 1 ? this._xFrame + 1 : 0;
        }

        public void IncrementYFrame()
        {
            this._yFrame = this._yFrame < this._yFrames - 1 ? this._yFrame + 1 : 0;
        }

        public void Render(float x, float y, float z)
        {
            Render(new Vector3(x, y, z));
        }

        public void Render(Vector3 pos)
        {
            Render(pos, 0, Vector3.Zero, Color4.White);
        }

        public void Render(Vector3 pos, Color4 color)
        {
            Render(pos, 0, Vector3.Zero, color);
        }

        public void Render(Vector3 pos, float rotation, Vector3 offset, Color4 color)
        {
            Vector3 rotate = new Vector3(0, 0, rotation);
            Vector2 dimension = new Vector2(_spriteWidth, _spriteHeight);
            Vector2 source = new Vector2(_xFrame * _spriteWidth, _yFrame * _spriteHeight);
            Graphics.DrawTexture(SpriteSheet, pos - offset, dimension, source, dimension, offset, rotate, color);
        }
    }
}
