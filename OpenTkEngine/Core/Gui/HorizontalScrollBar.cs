using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTkEngine.Core.Gui
{
    public class HorizontalScrollBar : Control
    {
        protected ScrollPanel _scrollParent;
        protected Rectangle _slider;
        protected int _scrollableAmount;
        protected bool _grabbed;

        public HorizontalScrollBar(ScrollPanel parent, int scrollAmount, State state)
            : base(parent.GetBodyRect().X, parent.GetBodyRect().Bottom - 20, 0, parent.GetBodyRect().Width, 20, state)
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
            this.SetScrollAmount(_scrollableAmount);
        }

        public void SetScrollAmount(int amount)
        {
            if (amount < _content.Width)
                amount = _content.Width;
            _scrollableAmount = amount;
            _slider.Width = this.GetSliderWidth();
            if (_slider.Right > _content.Right)
            {
                int difference = _slider.Right - _content.Right;
                _slider.X -= difference;
            }
            
        }

        public int GetScrollAmount()
        {
            return _scrollableAmount;
        }

        public int GetSliderWidth()
        {
            float percent = (float)_content.Width / (float)_scrollableAmount;
            float size = percent * (float)_content.Width;
            if (size < 15)
                return 15;
            return (int)size;
        }

        private bool IsScrollable()
        {
            if (_scrollableAmount <= _content.Width)
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

        public int GetRelativeX()
        {
            float trackWidth = _content.Width - _slider.Width;
            if (trackWidth == 0)
                return 0;
            float scale = _scrollableAmount - _content.Width;
            scale /= trackWidth;
            float scrolledX = _content.X - _slider.X;
            float rX = scrolledX * scale;
            return (int)rX;
        }

        public override void Update()
        {
            base.Update();

            if (_grabbed)
            {
                if (Input.MouseLeftDown())
                {
                    if (Input.GetMouseScrolledX() != 0)
                    {
                        _slider.X += Input.GetMouseScrolledX();
                        if (_slider.X < _content.X)
                            _slider.X = _content.X;
                        else if (_slider.Right > _content.Right)
                            _slider.X = _content.Right - _slider.Width;
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
            Graphics.FillRoundedRect(_slider.X - _content.X, _slider.Y - _content.Y, -1, _slider.Width, _slider.Height, _borderRadius, Color.Teal);
        } 
    }
}
