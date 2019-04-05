using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Popcron.Input
{
    [InitializeOnLoad]
    public class Initializer
    {
        static Initializer()
        {
            const int ArraySize = Controls.MaxControllers * Controls.MaxJoystickAxes;
            if (ControlsHelper.AxesCount != ArraySize)
            {
                //check the input manager settings
                ControlsHelper.ResetUnityInputs();
            }
        }
    }
}