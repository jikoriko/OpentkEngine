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

            State state = new State();

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

            StateHandler.Push(state);

            window.Run(30.0f);
        }
    }
}
