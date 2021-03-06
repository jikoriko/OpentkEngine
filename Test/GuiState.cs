﻿using OpenTK.Graphics;
using OpenTkEngine.Core;
using OpenTkEngine.Core.Gui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class GuiState : State
    {

        public GuiState(Graphics.RenderMode renderMode, bool overlay)
            : base(renderMode, overlay)
        {
        }

        protected override void Initialize()
        {
            base.Initialize();

            //create a bunch of UI elements and link them to the state
            ScrollPanel panel = new ScrollPanel(0, 0, 0, 400, 640, this);
            panel.SetBackgroundColor(Color4.Red);
            ScrollPanel panel2 = new ScrollPanel(420, 10, 0, 350, 500, this);

            panel.SetContentDimensions(800, 800);
            panel2.SetContentDimensions(400, 400);

            Button button = new Button(10, 50, 0, 100, 100, "button.png", "button", this);
            TextField field = new TextField(10, 10, 0, 100, this);
            ListBox listBox = new ListBox(120, 10, 0, 200, 150, 10, this);

            DropDownBox dropDown = new DropDownBox(50, 420, 0, 100, this);

            TextBox textBox = new TextBox(10, 10, 0, 400, 400, this);
            RadioButton radio = new RadioButton(200, 420, 0, this);

            panel.AddControl(panel2);

            panel.AddControl(dropDown);
            panel.AddControl(textBox);
            panel.AddControl(radio);

            panel2.AddControl(button);
            panel2.AddControl(field);
            panel2.AddControl(listBox);

            this.AddControl(panel);

        }

        public override void OnKeyDown(object sender, OpenTK.Input.KeyboardKeyEventArgs args)
        {
            base.OnKeyDown(sender, args);
            if (args.Key == OpenTK.Input.Key.Escape)
            {
                StateHandler.Pop();
            }
        }

    }
}
