using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTkEngine.Core.Gui
{
    public class Panel : Control
    {
        protected List<Control> _controls;

        public Panel(int x, int y, int z, int width, int height, State state)
            : base(x, y, z, width, height, state)
        {
            _controls = new List<Control>();
        }

        public void AddControl(Control control)
        {
            if (!_controls.Contains(control))
            {
                if (control.GetParent() != null)
                    control.GetParent().RemoveControl(control);
                control.SetParent(this);
                _controls.Add(control);
            }
        }

        public void RemoveControl(Control control)
        {
            _controls.Remove(control);
            control.SetParent(null);
        }

        public List<Control> GetControls()
        {
            return _controls;
        }

        public override void Update()
        {
            base.Update();
            for (int i = 0; i < _controls.Count; i++)
            {
                _controls[i].Update();
            }
        }

        public override void RenderContent()
        {
            base.RenderContent();
            for (int i = 0; i < _controls.Count; i++)
            {
                _controls[i].Render();
            }
        }

    }
}
