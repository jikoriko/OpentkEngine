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
    public class VerticalScrollBar : Control
    {
        protected ScrollPanel _scrollParent;
        protected Rectangle _slider;
        protected int _scrollableAmount;
        protected bool _grabbed;

        public VerticalScrollBar(ScrollPanel parent, int scrollAmount, State state)
            : base(parent.GetBodyRect().Right - 20, parent.GetBodyRect().Y, 0, 20, parent.GetBodyRect().Height, state)
        {
            _backgroundColor = Color4.LightGray;
            _scrollParent = parent;
            _slider = new Rectangle(_content.X, _content.Y, _content.Width, _content.Height);
            this.SetScrollAmount(scrollAmount);
            _grabbed = false;

        }

        public override void Resize(int width, int height)
        {
            base.Resize(width, height);
            SetScrollAmount(_scrollableAmount);
        }

        public void SetScrollAmount(int amount)
        {
            if (amount < _content.Height)
                amount = _content.Height;
            _scrollableAmount = amount;
            _slider.Height = this.GetSliderHeight();
            if (_slider.Y > _content.Y)
                _slider.Y = _content.Y;
        }

        public int GetScrollAmount()
        {
            return _scrollableAmount;
        }

        public int GetSliderHeight()
        {
            float percent = (float)_content.Height / (float)_scrollableAmount;
            float size = percent * (float)_content.Height;
            if (size < 15)
                return 15;
            return (int)size;
        }

        private bool IsScrollable()
        {
            if (_scrollableAmount <= _content.Height)
                return false;
            else if (_grabbed)
                return true;
            else if (!this.Selectable())
                return false;
            if (_parent == null)
                return (_slider.Contains(Input.GetRelativeMouseX(), Input.GetRelativeMouseY()));
            else
            {
                Rectangle slider = new Rectangle(_slider.X + _parent.GetRelativeX(), _slider.Y + _parent.GetRelativeY(),
                    _slider.Width, _slider.Height);
                return slider.Contains(Input.GetRelativeMouseX(), Input.GetRelativeMouseY());
            }
        }

        public int GetRelativeY()
        {
            float trackHeight = _content.Height - _slider.Height;
            if (trackHeight == 0)
                return 0;
            float scale = _scrollableAmount - _content.Height;
            scale /= trackHeight;
            float scrolledY = _content.Y - _slider.Y;
            float rY = scrolledY * scale;
            return (int)rY;
        }

        public override void Update()
        {
            base.Update();

            if (_grabbed)
            {
                if (Input.MouseLeftDown())
                {
                    if (Input.GetMouseScrolledY() != 0)
                    {
                        _slider.Y += Input.GetMouseScrolledY();
                        if (_slider.Y < _content.Y)
                            _slider.Y = _content.Y;
                        else if (_slider.Bottom > _content.Bottom)
                            _slider.Y = _content.Bottom - _slider.Height;
                    }
                }
                else
                    _grabbed = false;
            }
            else
            {
                if (this.IsScrollable() && Input.MouseLeftTriggered())
                    _grabbed = true;
            }
        }

        public override void RenderContent()
        {
            base.RenderContent();
            Graphics.FillRoundedRect(_slider.X - _content.X, _slider.Y - _content.Y, 0, _slider.Width, _slider.Height, _borderRadius, Color.Teal);
        }
    }
}
