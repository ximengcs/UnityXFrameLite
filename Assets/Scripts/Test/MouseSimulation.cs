using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine;

namespace Assets.Scripts.Test
{
    public class MouseSimulation : MonoBehaviour
    {
        private Mouse m_Mouse;

        private void OnEnable()
        {
            m_Mouse = InputSystem.AddDevice<Mouse>();
        }

        private void OnDisable()
        {
            InputSystem.RemoveDevice(m_Mouse);
            m_Mouse = null;
        }

        public void Trigger(Vector2 position)
        {
            Debug.LogWarning("trigger " + position);
            InputState.Change(m_Mouse, new MouseState
            {
                position = position,
                delta = Vector2.zero,
            }.WithButton(MouseButton.Left, true));
        }
    }
}
