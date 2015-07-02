using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTkEngine.Core.Gui
{
    public class Button : Control
    {
        protected string _text;

        private static Rectangle[] sources = new Rectangle[] 
        {
            new Rectangle(0, 0, 6, 6), new Rectangle(6, 0, 1, 6), new Rectangle(7, 0, 6, 6),
            new Rectangle(0, 6, 6, 1), new Rectangle(6, 6, 1, 1), new Rectangle(7, 6, 6, 1),
            new Rectangle(0, 7, 6, 6), new Rectangle(6, 7, 1, 6), new Rectangle(7, 7, 6, 6)
        };

        public Button(int x, int y, int z, int width, int height, string texture, string text, State state)
            : base(x, y, z, width, height, state)
        {
            _text = text;
            _texture = Assets.GetTexture(texture);
        }

        public override void Render()
        {
            int offsetX = 0;
            if (_pressed)
                offsetX = 13;
            else if (Selectable())
                offsetX = 26;

            Rectangle dest, source;

            dest = new Rectangle(_body.X, _body.Y, 6, 6);
            source = sources[0];
            Graphics.DrawTexture(_texture, dest, -_zDepth, new Rectangle(source.X + offsetX, source.Y, source.Width, source.Height), Color.White);
            dest = new Rectangle(_body.X + 6, _body.Y, _body.Width - 12, 6);
            source = sources[1];
            Graphics.DrawTexture(_texture, dest, -_zDepth, new Rectangle(source.X + offsetX, source.Y, source.Width, source.Height), Color.White);
            dest = new Rectangle(_body.X + _body.Width - 6, _body.Y, 6, 6);
            source = sources[2];
            Graphics.DrawTexture(_texture, dest, -_zDepth, new Rectangle(source.X + offsetX, source.Y, source.Width, source.Height), Color.White);

            dest = new Rectangle(_body.X, _body.Y + 6, 6, _body.Height - 12);
            source = sources[3];
            Graphics.DrawTexture(_texture, dest, -_zDepth, new Rectangle(source.X + offsetX, source.Y, source.Width, source.Height), Color.White);
            dest = new Rectangle(_body.X + 6, _body.Y + 6, _body.Width - 12, _body.Height - 12);
            source = sources[4];
            Graphics.DrawTexture(_texture, dest, -_zDepth, new Rectangle(source.X + offsetX, source.Y, source.Width, source.Height), Color.White);
            dest = new Rectangle(_body.X + _body.Width - 6, _body.Y + 6, 6, _body.Height - 12);
            source = sources[5];
            Graphics.DrawTexture(_texture, dest, -_zDepth, new Rectangle(source.X + offsetX, source.Y, source.Width, source.Height), Color.White);

            dest = new Rectangle(_body.X, _body.Y + _body.Height - 6, 6, 6);
            source = sources[6];
            Graphics.DrawTexture(_texture, dest, -_zDepth, new Rectangle(source.X + offsetX, source.Y, source.Width, source.Height), Color.White);
            dest = new Rectangle(_body.X + 6, _body.Y + _body.Height - 6, _body.Width - 12, 6);
            source = sources[7];
            Graphics.DrawTexture(_texture, dest, -_zDepth, new Rectangle(source.X + offsetX, source.Y, source.Width, source.Height), Color.White);
            dest = new Rectangle(_body.X + _body.Width - 6, _body.Y + _body.Height - 6, 6, 6);
            source = sources[8];
            Graphics.DrawTexture(_texture, dest, -_zDepth, new Rectangle(source.X + offsetX, source.Y, source.Width, source.Height), Color.White);

            Rectangle clip = new Rectangle(GetRelativeX(), GetRelativeY(), _content.Width, _content.Height);
            Graphics.PushScreenClip(clip);

            Graphics.PushWorldMatrix();
            Graphics.TranslateWorld(new Vector3(_content.X, _content.Y, 0));

            this.RenderContent();

            Graphics.PopWorldMatrix();
            Graphics.PopScreenClip();
        }

        public override void RenderContent()
        {
            base.RenderContent();
            float textX = (_content.Width / 2) - (Graphics.GetFont().GetWidth(_text) / 2);
            float textY = (_content.Height / 2) - (Graphics.GetFont().GetHeight(_text) / 2);
            Graphics.DrawText(_text, textX, textY, -_zDepth, Color4.Black);
        }

    }
}
