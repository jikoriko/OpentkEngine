using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTkEngine.Core
{
    public class StateHandler
    {
        private static List<State> _stateList = new List<State>();

        public static void Push(State state)
        {
            _stateList.Add(state);
            state.OnAdded();
        }

        public static void Pop()
        {
            if (_stateList.Count == 0)
                return;
            _stateList.Last().OnExit();
            _stateList.Remove(_stateList.Last());
        }

        public static State Last()
        {
            if (_stateList.Count == 0)
                return null;
            return _stateList.Last();
        }

        public static void OnMouseDown(object sender, MouseButtonEventArgs args)
        {
            Last().OnMouseDown(sender, args);
        }

        public static void OnMouseUp(object sender, MouseButtonEventArgs args)
        {
            Last().OnMouseUp(sender, args);
        }

        public static void OnMouseMove(object sender, MouseMoveEventArgs args)
        {
            Last().OnMouseMove(sender, args);
        }

        public static void OnMouseWheel(object sender, MouseWheelEventArgs args)
        {
            Last().OnMouseWheel(sender, args);
        }

        public static void OnKeyDown(object sender, KeyboardKeyEventArgs args)
        {
            Last().OnKeyDown(sender, args);
        }

        public static void OnKeyUp(object sender, KeyboardKeyEventArgs args)
        {
            Last().OnKeyUp(sender, args);
        }

        public static void OnKeyPress(object sender, KeyPressEventArgs args)
        {
            Last().OnKeyPress(sender, args);
        }

        public static void UpdateFrame(FrameEventArgs e)
        {
            if (_stateList.Count == 0)
                return;
            _stateList.Last().UpdateFrame(e);
        }

        public static void RenderFrame(FrameEventArgs e)
        {
            if (_stateList.Count == 0)
                return;
            List<State> renderStates = new List<State>();
            renderStates.Add(_stateList.Last());

            int stateIndex = _stateList.Count - 2;
            while(renderStates[renderStates.Count - 1].IsOverlay())
            {
                if (stateIndex < 0)
                    break;
                renderStates.Add(_stateList.ElementAt(stateIndex));
                stateIndex--;
            }

            for (int i = renderStates.Count - 1; i >= 0; i--)
            {
                renderStates[i].RenderFrame(e);
            }
        }

    }
}
