using OpenTK;
using OpenTK.Input;
using OpenTkEngine.Core.Gui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTkEngine.Core
{
    public class State : KeyListener, MouseListener
    {
        protected bool _active = true;

        protected List<KeyListener> _keyListeners;
        protected List<MouseListener> _mouseListeners;

        protected List<Control> _controls;

        public State()
        {
            _keyListeners = new List<KeyListener>();
            _mouseListeners = new List<MouseListener>();
            _controls = new List<Control>();
        }

        public void AddKeyListener(KeyListener listener)
        {
            if (!_keyListeners.Contains(listener))
                _keyListeners.Add(listener);
        }

        public void RemoveKeyListener(KeyListener listener)
        {
            _keyListeners.Remove(listener);
        }

        public void AddMouseListener(MouseListener listener)
        {
            if (!_mouseListeners.Contains(listener))
                _mouseListeners.Add(listener);
        }

        public void removeMouseListener(MouseListener listener)
        {
            _mouseListeners.Remove(listener);
        }

        public void AddControl(Control control)
        {
            if (!_controls.Contains(control))
                _controls.Add(control);
        }

        public void RemoveControl(Control control)
        {
            _controls.Remove(control);
        }

        public virtual void OnAdded()
        {
        }

        public virtual void OnExit()
        {
        }

        public virtual void OnMouseDown(object sender, MouseButtonEventArgs args)
        {
            if (!_active) return;
            foreach (MouseListener listner in _mouseListeners)
                listner.OnMouseDown(sender, args);
        }

        public virtual void OnMouseUp(object sender, MouseButtonEventArgs args)
        {
            if (!_active) return;
            foreach (MouseListener listner in _mouseListeners)
                listner.OnMouseUp(sender, args);
        }

        public virtual void OnMouseMove(object sender, MouseMoveEventArgs args)
        {
            if (!_active) return;
            foreach (MouseListener listner in _mouseListeners)
                listner.OnMouseMove(sender, args);
        }

        public virtual void OnMouseWheel(object sender, MouseWheelEventArgs args)
        {
            if (!_active) return;
            foreach (MouseListener listner in _mouseListeners)
                listner.OnMouseWheel(sender, args);
        }

        public virtual void OnKeyDown(object sender, KeyboardKeyEventArgs args)
        {
            if (!_active) return;
            foreach (KeyListener listner in _keyListeners)
                listner.OnKeyDown(sender, args);
        }

        public virtual void OnKeyUp(object sender, KeyboardKeyEventArgs args)
        {
            if (!_active) return;
            foreach (KeyListener listner in _keyListeners)
                listner.OnKeyUp(sender, args);
        }

        public virtual void OnKeyPress(object sender, KeyPressEventArgs args)
        {
            if (!_active) return;
            foreach (KeyListener listner in _keyListeners)
                listner.OnKeyPress(sender, args);
        }

        public virtual void UpdateFrame(FrameEventArgs e)
        {
            if (!_active) return;

            foreach (Control control in _controls)
            {
                control.Update();
            }
        }

        public virtual void RenderFrame(FrameEventArgs e)
        {
            foreach (Control control in _controls)
            {
                control.Render();
            }
        }

    }
}
