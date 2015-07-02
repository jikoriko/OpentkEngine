using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTkEngine.Core.Gui
{
    public class ScrollPanel : Panel
    {
        protected HorizontalScrollBar _horizontalScroll;
        protected VerticalScrollBar _verticalScroll;

        protected bool _horizontalEnabled = false;
        protected bool _verticalEnabled = false;

        public ScrollPanel(int x, int y, int z, int width, int height, State state)
            : base(x, y, z, width, height, state)
        {
            _horizontalScroll = new HorizontalScrollBar(this, _content.Width, state);
            _verticalScroll = new VerticalScrollBar(this, _content.Height, state);
            _horizontalScroll.SetBorderRadius(10);
            _verticalScroll.SetBorderRadius(10);
            EnableHorizontalScroll();
            EnableVerticalScroll();
        }

        public void EnableHorizontalScroll()
        {
            if (!_horizontalEnabled)
            {
                _horizontalEnabled = true;
                Resize(_body.Width, _body.Height - _horizontalScroll.GetHeight());
            }
        }

        public void DisableHorizontalScroll()
        {
            if (_horizontalEnabled)
            {
                _horizontalEnabled = false;
                Resize(_body.Width, _body.Height + _horizontalScroll.GetHeight());
            }
        }

        public void EnableVerticalScroll()
        {
            if (!_verticalEnabled)
            {
                _verticalEnabled = true;
                Resize(_body.Width - _verticalScroll.GetWidth(), _body.Height);
            }
        }

        public void DisableVerticalScroll()
        {
            if (_verticalEnabled)
            {
                _verticalEnabled = false;
                Resize(_body.Width + _verticalScroll.GetWidth(), _body.Height);
            }
        }

        public override void SetParent(Panel parent)
        {
            base.SetParent(parent);
            _horizontalScroll.SetParent(parent);
            _verticalScroll.SetParent(parent);
        }

        public override int GetRelativeX()
        {
            return base.GetRelativeX() + _horizontalScroll.GetRelativeX();
        }

        public override int GetRelativeY()
        {
            return base.GetRelativeY() + _verticalScroll.GetRelativeY();
        }

        public void SetContentDimensions(int width, int height)
        {
            if (width < _content.Width)
                width = _content.Width;
            if (height < _content.Height)
                height = _content.Height;
            _horizontalScroll.SetScrollAmount(width);
            _verticalScroll.SetScrollAmount(height);
        }


        public override void Resize(int width, int height)
        {
            base.Resize(width, height);
            _horizontalScroll.Resize(width, _horizontalScroll.GetHeight());
            _verticalScroll.Resize(_verticalScroll.GetWidth(), height);
        }

        public override void Update()
        {
            base.Update();
            if (_horizontalEnabled)
                _horizontalScroll.Update();
            if (_verticalEnabled)
                _verticalScroll.Update();

        }

        public override void Render()
        {
            base.Render();
            if (_horizontalEnabled)
                _horizontalScroll.Render();
            if (_verticalEnabled)
                _verticalScroll.Render();
        }

        public override void RenderContent()
        {
            Graphics.TranslateWorld(new Vector3(_horizontalScroll.GetRelativeX(), _verticalScroll.GetRelativeY(), 0));
            base.RenderContent();
        }

    }

}
