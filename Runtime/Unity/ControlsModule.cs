using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Popcron.Input
{
    using Input = UnityEngine.Input;

    [AddComponentMenu("Popcron/Input/Controls module")]
    public class ControlsModule : StandaloneInputModule
    {
        [Header("Buttons")]
        [SerializeField]
        private bool m_canProcessControls = true;

        [SerializeField]
        private string m_Left = "Left";

        [SerializeField]
        private string m_Right = "Right";

        [SerializeField]
        private string m_Up = "Up";

        [SerializeField]
        private string m_Down = "Down";

        private Vector2 mm_LastMousePosition;
        private Vector2 mm_MousePosition;
        private float mm_NextAction;

        public override void UpdateModule()
        {
            base.UpdateModule();

            mm_LastMousePosition = mm_MousePosition;
            mm_MousePosition = Input.mousePosition;
        }

        public override bool ShouldActivateModule()
        {
            bool shouldActivate = Controls.GetButtonDown(submitButton);
            shouldActivate |= Controls.GetButtonDown(cancelButton);
            //shouldActivate |= !Mathf.Approximately(Input.GetAxisRaw(horizontalAxis), 0.0f);
            //shouldActivate |= !Mathf.Approximately(Input.GetAxisRaw(verticalAxis), 0.0f);
            shouldActivate |= (mm_MousePosition - mm_LastMousePosition).sqrMagnitude > 0.0f;
            shouldActivate |= Input.GetMouseButtonDown(0);
            return shouldActivate;
        }

        public override void Process()
        {
            bool usedEvent = SendUpdateEventToSelectedObject();

            if (eventSystem.sendNavigationEvents)
            {
                if (!usedEvent)
                {
                    usedEvent |= SendMoveEventToSelectedObject();
                }
                if (!usedEvent)
                {
                    SendSubmitEventToSelectedObject();
                }
            }

            ProcessMouseEvent();
        }

        /// <summary>
        /// Process submit keys.
        /// </summary>
        private new bool SendSubmitEventToSelectedObject()
        {
            if (eventSystem.currentSelectedGameObject == null) return false;

            BaseEventData data = GetBaseEventData();
            if (Controls.GetButtonDown(submitButton))
            {
                ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, data, ExecuteEvents.submitHandler);
            }

            if (Controls.GetButtonDown(cancelButton))
            {
                ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, data, ExecuteEvents.cancelHandler);
            }

            return data.used;
        }

        private bool AllowMoveEventProcessing(float time)
        {
            bool allow = false;
            allow |= GetRawMoveVector().sqrMagnitude != 0;
            allow |= (time > mm_NextAction);
            return allow;
        }

        private Vector2 GetRawMoveVector()
        {
            Vector2 move = Vector2.zero;
            if (!m_canProcessControls) return move;

            if (Controls.GetButtonDown(m_Right)) move.x++;
            if (Controls.GetButtonDown(m_Left)) move.x--;
            if (Controls.GetButtonDown(m_Up)) move.y++;
            if (Controls.GetButtonDown(m_Down)) move.y--;

            return move;
        }

        private new bool SendMoveEventToSelectedObject()
        {
            float time = Time.unscaledTime;

            if (!AllowMoveEventProcessing(time))
            {
                return false;
            }

            Vector2 movement = GetRawMoveVector();
            // Debug.Log(m_ProcessingEvent.rawType + " axis:" + m_AllowAxisEvents + " value:" + "(" + x + "," + y + ")");
            var axisEventData = GetAxisEventData(movement.x, movement.y, 0.6f);
            if (!Mathf.Approximately(axisEventData.moveVector.x, 0f)
                || !Mathf.Approximately(axisEventData.moveVector.y, 0f))
            {
                ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, axisEventData, ExecuteEvents.moveHandler);
            }
            mm_NextAction = time + 1f / inputActionsPerSecond;
            return axisEventData.used;
        }
    }
}