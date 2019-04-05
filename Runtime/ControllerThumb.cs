using System;
using UnityEngine;

namespace Popcron.Input
{
    [Serializable]
    public class ControllerThumb
    {
        public ControllerBind button = new ControllerBind("Button", KeyCode.JoystickButton8);

        public int horizontalAxis;
        public int verticalAxis;

        public bool invertHorizontal;
        public bool invertVertical;

        public ControllerThumb(KeyCode key, int horizontalAxis, int verticalAxis)
        {
            this.button = new ControllerBind("Button", key);
            this.horizontalAxis = horizontalAxis;
            this.verticalAxis = verticalAxis;
            this.invertHorizontal = false;
            this.invertVertical = false;
        }

        public ControllerThumb(KeyCode key, int horizontalAxis, int verticalAxis, bool invertHorizontal, bool invertVertical)
        {
            this.button = new ControllerBind("Button", key);
            this.horizontalAxis = horizontalAxis;
            this.verticalAxis = verticalAxis;
            this.invertHorizontal = invertHorizontal;
            this.invertVertical = invertVertical;
        }
    }
}