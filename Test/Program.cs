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

            Graphics.SetClearColor(Color4.Blue);

            State state = new GameState(Graphics.RenderMode.Perspective, false);

            //push the state to the state stack - top state is the state that runs
            StateHandler.Push(state);

            Sound sound = Assets.GetSound("Main loop.wav");
            //sound.Play();

            window.Run(30.0f);
        }
    }
}
