using OpenTK;
using OpenTkEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class GameState : State
    {

        public GameState(Graphics.RenderMode renderMode, bool overlay)
            : base(renderMode, overlay)
        {
        }

        protected override void Initialize()
        {
            base.Initialize();

            //add a camera control for the state
            FpsCamera camera = new FpsCamera(new Vector3(0, 0, 0));
            this.SetCamera(camera);

            //add some entities to the state
            ModelEntity ent = new ModelEntity(0, 0, -20, "model.bin");
            this.AddEntity(ent);
            ent.SetScale(10f);

            ModelEntity ent2 = new ModelEntity(0, 0, 0, "couch1.obj");
            this.AddEntity(ent2);
            ent2.SetScale(0.2f);

            ModelEntity ent3 = new ModelEntity(-200, 0, 0, "model.bin");
            this.AddEntity(ent3);
            ent3.SetScale(100f);

            SpriteEntity sprite = new SpriteEntity(20, 0, -10, "tileset.png");
            this.AddEntity(sprite);

        }

        public override void OnKeyDown(object sender, OpenTK.Input.KeyboardKeyEventArgs args)
        {
            base.OnKeyDown(sender, args);
            if (args.Key == OpenTK.Input.Key.Escape)
            {
                StateHandler.Push(new GuiState(Graphics.RenderMode.Ortho, true));
            }
        }
    }
}
