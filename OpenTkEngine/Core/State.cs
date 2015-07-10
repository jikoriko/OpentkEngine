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
        protected bool _overlay;

        protected List<KeyListener> _keyListeners;
        protected List<MouseListener> _mouseListeners;

        protected List<Entity> _entities;
        protected List<Control> _controls;

        protected Camera _camera = null;
        protected Graphics.RenderMode _renderMode;

        public State(Graphics.RenderMode renderMode, bool overlay)
        {
            _keyListeners = new List<KeyListener>();
            _mouseListeners = new List<MouseListener>();
            _entities = new List<Entity>();
            _controls = new List<Control>();
            _renderMode = renderMode;
            _overlay = overlay;
            Initialize();
        }

        protected virtual void Initialize()
        {
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

        public void SetCamera(Camera camera)
        {
            _camera = camera;
        }

        public void AddEntity(Entity entity)
        {
            if (!_entities.Contains(entity))
            {
                _entities.Add(entity);
            }
        }

        public void RemoveEntity(Entity entity)
        {
            _entities.Remove(entity);
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

        public bool IsOverlay()
        {
            return _overlay;
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

            if (_camera != null)
            {
                _camera.Update();
            }

            foreach (Entity entity in _entities)
            {
                entity.Update();
            }

            foreach (Control control in _controls)
            {
                control.Update();
            }
        }

        public virtual void RenderFrame(FrameEventArgs e)
        {
            Graphics.SetRenderMode(_renderMode);

            Graphics.PushWorldMatrix();

            if (_camera != null)
            {
                _camera.LookThrough();
            }

            Graphics.PushWorldMatrix();
            foreach (Entity entity in _entities)
            {
                entity.Render();
            }
            Graphics.PopWorldMatrix();

            Graphics.PopWorldMatrix();

            //back to ortho for ui overlay, absract to ui manager?
            //TODO disable depth test on GL side so ui render is forced on top
            Graphics.SetRenderMode(Graphics.RenderMode.Ortho);
            foreach (Control control in _controls)
            {
                control.Render();
            }
        }

    }
}
