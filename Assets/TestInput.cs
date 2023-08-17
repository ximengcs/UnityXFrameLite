using Assets.Scripts.Test;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class TestInput : MonoBehaviour
{
    public RectTransform Target;
    public PlayerInput Map;

    void Start()
    {
        InputAction action = Map.currentActionMap.FindAction("LeftBtn");
        InputAction space = Map.currentActionMap.FindAction("Space");
        InputAction w = Map.currentActionMap.FindAction("W");
        InputAction s = Map.currentActionMap.FindAction("S");
        InputAction a = Map.currentActionMap.FindAction("A");
        InputAction d = Map.currentActionMap.FindAction("D");
        w.performed += (context) => Target.anchoredPosition += new Vector2(0f, 5);
        s.performed += (context) => Target.anchoredPosition += new Vector2(0f, -1 * 5);
        a.performed += (context) => Target.anchoredPosition += new Vector2(-5, 0);
        d.performed += (context) => Target.anchoredPosition += new Vector2(5, 0);

        action.performed += InnerBtnLeft;
        space.performed += InnerSpace;
    }

    private void InnerBtnLeft(CallbackContext context)
    {
        Debug.LogWarning(context);
    }

    private void InnerSpace(CallbackContext context)
    {
        Debug.LogWarning(context);
        GetComponent<MouseSimulation>().Trigger(Target.anchoredPosition);
    }
}
