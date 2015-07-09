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

            //create a bunch of UI elements and link them to the state
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

            //add a camera control for the state
            FpsCamera camera = new FpsCamera(new Vector3(0, 0, 0));
            state.SetCamera(camera);

            //add some entities to the state
            ModelEntity ent = new ModelEntity(0, 0, -20, "model.bin");
            state.AddEntity(ent);
            ent.SetScale(10f);

            ModelEntity ent2 = new ModelEntity(0, 0, 0, "couch1.obj");
            state.AddEntity(ent2);
            ent2.SetScale(0.2f);

            ModelEntity ent3 = new ModelEntity(-200, 0, 0, "model.bin");
            state.AddEntity(ent3);
            ent3.SetScale(100f);

            SpriteEntity sprite = new SpriteEntity(20, 0, -10, "tileset.png");
            state.AddEntity(sprite);

            //push the state to the state stack - top state is the state that runs
            StateHandler.Push(state);

            window.Run(30.0f);
        }
    }
}
