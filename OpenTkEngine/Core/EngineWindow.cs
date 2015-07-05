using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace OpenTkEngine.Core
{
    public class EngineWindow : GameWindow
    {
        Model _model;

        public Vector3 cameraPosition = Vector3.Zero;

        public EngineWindow()
            : base(1000, 640, new GraphicsMode(32, 24, 8, 4), "OpenTK", GameWindowFlags.Default, DisplayDevice.Default, 3, 1, GraphicsContextFlags.ForwardCompatible)
        {
            Global.window = this;
            string versionOpenGL = GL.GetString(StringName.Version);
            string shaderVersion = GL.GetString(StringName.ShadingLanguageVersion);
            Console.WriteLine("OpenGL: " + versionOpenGL);
            Console.WriteLine("GLSL: " + shaderVersion);
        }

        protected override void OnLoad(EventArgs e)
        {
            Graphics.Initialize();

            MouseDown += (sender, args) => StateHandler.OnMouseDown(sender, args);
            MouseUp += (sender, args) => StateHandler.OnMouseUp(sender, args);
            MouseMove += (sender, args) => StateHandler.OnMouseMove(sender, args);
            MouseWheel += (sender, args) => StateHandler.OnMouseWheel(sender, args);
            KeyDown += (sender, args) => StateHandler.OnKeyDown(sender, args);
            KeyUp += (sender, args) => StateHandler.OnKeyUp(sender, args);
            KeyPress += (sender, args) => StateHandler.OnKeyPress(sender, args);

            _model = new Model("Assets/Models/model.bin");

            base.OnLoad(e);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Graphics.SetProjection(this.ClientRectangle);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            Input.Update();
            StateHandler.UpdateFrame(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            Graphics.Clear();

            Graphics.PushWorldMatrix();
            Graphics.TranslateWorld(cameraPosition);
            Graphics.SetRenderMode(Graphics.RenderMode.Perspective);
            Graphics.SetColor(Color4.Pink);
            Graphics.RenderModel(_model, Matrix4.CreateScale(10f) * Matrix4.CreateTranslation(20, -20, -40));
            Graphics.SetRenderMode(Graphics.RenderMode.Ortho);
            Graphics.PopWorldMatrix();

            StateHandler.RenderFrame(e);

            this.SwapBuffers();
        }

        protected override void OnUnload(EventArgs e)
        {
            Assets.Delete();
            Graphics.Destroy();
            base.OnUnload(e);
        }
    }
}
