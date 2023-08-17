using System.Collections;
using UnityEditor.Presets;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.PlayerSettings;

namespace UnityXFrame.Core.Simulate
{
    public class XInputModule : StandaloneInputModule
    {
        private KeyCode left = KeyCode.A, right = KeyCode.D, up = KeyCode.W, down = KeyCode.S, click = KeyCode.Space;
        [SerializeField] public RectTransform fakeCursor;

        private float moveSpeed = 5f;

        PointerEventData GeneratePointerData(Vector2 pos, TouchPhase phase)
        {
            var data = GetTouchPointerEventData(new Touch()
            {
                position = pos,
                phase = phase,
                rawPosition = pos

            }, out bool b, out bool bb);
            return data;
        }

        IEnumerator SimulateDrag(Vector2 from, Vector2 to, float dur)
        {
            Input.simulateMouseWithTouches = true;
            float startTime = Time.time;
            float endTime = Time.time + dur;
            ProcessTouchPress(GeneratePointerData(from, TouchPhase.Began), true, false);
            float t = 0f;
            while (t < 1)
            {
                t = Mathf.InverseLerp(startTime, endTime, Time.time);
                // Debug.Log(t);
                Vector2 pos = Vector2.Lerp(from, to, t);
                ProcessDrag(GeneratePointerData(pos, TouchPhase.Moved));
                yield return null;
            }
            ProcessTouchPress(GeneratePointerData(to, TouchPhase.Ended), false, true);
        }

        public void SimulateTouch(Vector2 pos, TouchPhase phase)
        {
            return;
            Input.simulateMouseWithTouches = true;
            PointerEventData pointerData = GeneratePointerData(pos, phase);
            bool pressed = phase == TouchPhase.Began;
            Debug.Log(pressed + " -> " + pointerData);
            if (phase == TouchPhase.Moved)
                ProcessDrag(pointerData);
            else
                ProcessTouchPress(pointerData, pressed, !pressed);
        }

        public void SimulateMouse(Vector2 pos, bool move, bool pressed)
        {
            Input.simulateMouseWithTouches = true;
            var mouseData = GetMousePointerEventData(0);
            var leftButtonData = mouseData.GetButtonState(PointerEventData.InputButton.Left).eventData;
            leftButtonData.buttonData.position = pos;
            if (pressed)
                leftButtonData.buttonState = PointerEventData.FramePressState.Pressed;
            else
                leftButtonData.buttonState = PointerEventData.FramePressState.Released;
            Debug.Log(pressed + " -> " + leftButtonData);
            if (move)
                ProcessDrag(leftButtonData.buttonData);
            else
                ProcessMousePress(leftButtonData);
        }

        private void Update()
        {
            return;
            if (Input.GetMouseButtonDown(0))
            {
                Debug.LogWarning(Input.mousePosition);
            }

            if (Input.GetKey(left))
            {
                fakeCursor.anchoredPosition += new Vector2(-1 * moveSpeed, 0f);
            }

            if (Input.GetKey(right))
            {
                fakeCursor.anchoredPosition += new Vector2(moveSpeed, 0f);
            }

            if (Input.GetKey(down))
            {
                fakeCursor.anchoredPosition += new Vector2(0f, -1 * moveSpeed);
            }

            if (Input.GetKey(up))
            {
                fakeCursor.anchoredPosition += new Vector2(0f, moveSpeed);
            }

            if (Input.GetKeyDown(click))
            {
                SimulateTouch(fakeCursor.anchoredPosition, TouchPhase.Began);
            }

            else if (Input.GetKey(click))
            {
                SimulateTouch(fakeCursor.anchoredPosition, TouchPhase.Moved);
            }

            else if (Input.GetKeyUp(click))
            {
                SimulateTouch(fakeCursor.anchoredPosition, TouchPhase.Ended);
            }
        }
    }
}
