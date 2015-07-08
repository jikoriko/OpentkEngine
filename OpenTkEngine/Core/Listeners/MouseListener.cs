using OpenTK.Input;

namespace OpenTkEngine.Core
{
    public interface MouseListener
    {
        void OnMouseDown(object sender, MouseButtonEventArgs args);
        void OnMouseUp(object sender, MouseButtonEventArgs args);
        void OnMouseMove(object sender, MouseMoveEventArgs args);
        void OnMouseWheel(object sender, MouseWheelEventArgs args);
    }
}
