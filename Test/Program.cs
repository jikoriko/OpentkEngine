using OpenTK;
using OpenTK.Graphics;
using OpenTkEngine.Core;
using OpenTkEngine.Core.Gui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            EngineWindow window = new EngineWindow();

            State state = new State(Graphics.RenderMode.Perspective);

            Graphics.SetClearColor(Color4.Blue);

            ScrollPanel panel = new ScrollPanel(0, 0, 0, 400, 640, state);
            panel.SetBackgroundColor(Color4.Red);
            ScrollPanel panel2 = new ScrollPanel(420, 10, 0, 350, 500, state);

            panel.SetContentDimensions(800, 800);
            panel2.SetContentDimensions(400, 400);

            Button button = new Button(10, 50, 0, 100, 100, "button.png", "button", state);
            TextField field = new TextField(10, 10, 0, 100, state);
            ListBox listBox = new ListBox(120, 10, 0, 200, 150, 10, state);

            DropDownBox dropDown = new DropDownBox(50, 420, 0, 100, state);

            TextBox textBox = new TextBox(10, 10, 0, 400, 400, state);
            RadioButton radio = new RadioButton(200, 420, 0, state);

            panel.AddControl(panel2);

            panel.AddControl(dropDown);
            panel.AddControl(textBox);
            panel.AddControl(radio);

            panel2.AddControl(button);
            panel2.AddControl(field);
            panel2.AddControl(listBox);

            state.AddControl(panel);


            FpsCamera camera = new FpsCamera(new Vector3(0, 0, 0));
            state.SetCamera(camera);

            ModelEntity ent = new ModelEntity(0, 0, -20, "Assets/Models/model.bin");
            state.AddEntity(ent);
            ent.SetScale(10f);

            ModelEntity ent2 = new ModelEntity(0, 0, 0, "Assets/Models/couch1.obj");
            state.AddEntity(ent2);
            ent.SetScale(0.2f);

            SpriteEntity sprite = new SpriteEntity(20, 0, -10, "tileset.png");
            state.AddEntity(sprite);

            StateHandler.Push(state);

            //WavefrontLoader.LoadModel("Assets/Models/couch1.obj");

            window.Run(30.0f);
        }
    }
}
