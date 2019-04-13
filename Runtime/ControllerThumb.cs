using System;
using UnityEngine;

namespace Popcron.Input
{
    [Serializable]
    public class ControllerThumb
    {
        public int buttonNumber;
        public int horizontalAxis;
        public int verticalAxis;
        public bool invertHorizontal;
        public bool invertVertical;

        public ControllerThumb(int buttonNumber, int horizontalAxis, int verticalAxis)
        {
            this.buttonNumber = buttonNumber;
            this.horizontalAxis = horizontalAxis;
            this.verticalAxis = verticalAxis;
            this.invertHorizontal = false;
            this.invertVertical = false;
        }

        public ControllerThumb(int buttonNumber, int horizontalAxis, int verticalAxis, bool invertHorizontal, bool invertVertical)
        {
            this.buttonNumber = buttonNumber;
            this.horizontalAxis = horizontalAxis;
            this.verticalAxis = verticalAxis;
            this.invertHorizontal = invertHorizontal;
            this.invertVertical = invertVertical;
        }
    }
}