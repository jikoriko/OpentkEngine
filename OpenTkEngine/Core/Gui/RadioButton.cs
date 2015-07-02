using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTkEngine.Core.Gui
{
    public class RadioButton : Control
    {
        private bool _checked;

        public RadioButton(int x, int y, int z, State state) :
            base(x, y, z, 20, 20, state)
        {
            _checked = false;
            SetBorderRadius(10);
        }

        public override void OnTrigger()
        {
            base.OnTrigger();
            _checked = _checked ? false : true;
        }

        public override void RenderContent()
        {
            base.RenderContent();
            if (_checked)
            {
                Graphics.FillRoundedRect(0, 0, 0, _content.Width, _content.Height, _borderRadius, Color4.DarkSlateBlue);
            }
        }
    }
}
