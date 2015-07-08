using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTkEngine.Core
{
    public interface KeyListener
    {
        void OnKeyDown(object sender, KeyboardKeyEventArgs args);

        void OnKeyUp(object sender, KeyboardKeyEventArgs args);

        void OnKeyPress(object sender, KeyPressEventArgs args);
    }
}
