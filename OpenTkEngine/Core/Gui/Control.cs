using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTkEngine.Core.Gui
{
    public class Control
    {
        protected Color4 _borderColor = Color4.RoyalBlue;
        protected Color4 _backgroundColor = Color4.LightBlue;
        protected Rectangle _body, _content;
        protected int _zDepth;
        protected int _borderThickness = 2;
        protected int _borderRadius = 0;
        protected bool _pressed, _triggered, _disabled;
        protected Texture _texture = null;
        protected Panel _parent = null;
        protected State _state;

        public Control(int x, int y, int z, int width, int height, State state)
        {
            _body = new Rectangle(x, y, width, height);
            int margin = GetMargin();
            _content = new Rectangle(x + margin, y + margin, width - (margin * 2), height - (margin * 2));
            _zDepth = z;
            _pressed = _triggered = _disabled = false;
            _state = state;
        }

        public virtual void Resize(int width, int height)
        {
            int differenceWidth = width - _body.Width;
            int differenceHeight = height - _body.Height;
            _body.Width += differenceWidth;
            _content.Width += differenceWidth;
            _body.Height += differenceHeight;
            _content.Height += differenceHeight;
        }

        public Rectangle GetBodyRect()
        {
            return _body;
        }

        public Rectangle GetContentRect()
        {
            return _content;
        }

        public Panel GetParent()
        {
            return _parent;
        }

        public virtual void SetParent(Panel parent)
        {
            _parent = parent;
        }

        public int GetWidth()
        {
            return _body.Width;
        }

        public int GetHeight()
        {
            return _body.Height;
        }

        private int Right()
        {
            return _body.Right;
        }

        private int Bottom()
        {
            return _body.Bottom;
        }

        public virtual int GetRelativeX()
        {
            if (_parent == null)
                return _content.X;
            else
                return _parent.GetRelativeX() + _content.X;
        }

        public virtual int GetRelativeY()
        {
            if (_parent == null)
                return _content.Y;
            else
                return _parent.GetRelativeY() + _content.Y;
        }

        public int GetMargin()
        {
            return _borderThickness;
        }

        public void SetBorderRadius(int radius)
        {
            _borderRadius = radius;
        }

        public virtual bool Selectable()
        {
            Vector2 mouse = Input.GetRelativeMousePosition();

            if (_disabled)
                return false;
            else if (_parent == null)
            {
                return (_body.Contains((int)mouse.X, (int)mouse.Y));
            }
            else
            {
                Rectangle location = new Rectangle(_parent.GetRelativeX() + _body.X, _parent.GetRelativeY() + _body.Y,
                    _body.Width, _body.Height);
                return (location.Contains((int)mouse.X, (int)mouse.Y) && _parent.Selectable());
            }
        }

        public virtual bool IsTriggered()
        {
            return _triggered;
        }

        public virtual bool IsDisabled()
        {
            return _disabled;
        }

        public virtual void Disable()
        {
            _disabled = true;
        }

        public virtual void Enable()
        {
            _disabled = false;
        }

        public virtual void OnTrigger()
        {
        }

        public virtual void OnRelease()
        {

        }

        public void SetBackgroundColor(Color4 color)
        {
            _backgroundColor = color;
        }

        public virtual void Update()
        {
            _triggered = false;
            if (_pressed)
            {
                if (!Input.MouseLeftDown())
                {
                    _pressed = false;
                    if (this.Selectable())
                    {
                        _triggered = true;
                        this.OnTrigger();
                    }
                }
            }
            else
            {
                if (this.Selectable() && Input.MouseLeftTriggered())
                {
                    _pressed = true;
                }
                else if (!this.Selectable() && Input.MouseLeftTriggered())
                {
                    this.OnRelease();
                }
            }
        }

        public virtual void Render()
        {
            Graphics.PushWorldMatrix();
            Graphics.TranslateWorld(new Vector3(_body.X, _body.Y, -_zDepth));

            Graphics.PushStencilDepth(StencilOp.Incr);
            //Graphics.FillRect(0, 0, 0, _body.Width, _body.Height, _backgroundColor);
            //if (!(this is HorizontalScrollBar || this is VerticalScrollBar))
            Graphics.FillRoundedRect(0, 0, 0, _body.Width, _body.Height, _borderRadius, _backgroundColor);

            //Graphics.PushStencilDepth(StencilOp.Keep);
            Graphics.SetStencilOp(StencilOp.Keep);

            if (_parent == null)
            {
                //Graphics.PushScreenClip(_content);
            }
            else
            {
                Rectangle clip = new Rectangle(_parent.GetRelativeX() + _content.X, _parent.GetRelativeY() + _content.Y, 
                                                _content.Width, _content.Height);
                //Graphics.PushScreenClip(clip);
            }
            Graphics.PushWorldMatrix();
            Graphics.TranslateWorld(new Vector3(GetMargin(), GetMargin(), 0));

            this.RenderContent();

            //Graphics.PopStencilDepth();

            Graphics.PopWorldMatrix();
            //Graphics.PopScreenClip();

            //Graphics.DrawRect(0, 0, 0, _body.Width, _body.Height, _margin, _borderColor);
            float padding = 0.25f * _borderThickness;//offsetting the rect drawing so its completely inside the clipped background
            Graphics.DrawRoundedRect(padding, padding, 0, _body.Width - (padding * 2) , _body.Height - (padding * 2), _borderRadius, _borderThickness, _borderColor);

            Graphics.PopStencilDepth();
            Graphics.PopWorldMatrix();
        }

        public virtual void RenderContent()
        {
        }
    }
}
