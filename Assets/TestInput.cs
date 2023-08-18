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
    private InputAction w;
    private InputAction s;
    private InputAction a;
    private InputAction d;

    void Start()
    {
        InputAction action = Map.currentActionMap.FindAction("LeftBtn");
        InputAction space = Map.currentActionMap.FindAction("Space");
        w = Map.currentActionMap.FindAction("W");
        s = Map.currentActionMap.FindAction("S");
        a = Map.currentActionMap.FindAction("A");
        d = Map.currentActionMap.FindAction("D");

        action.performed += InnerBtnLeft;
        space.performed += InnerSpace;
    }

    private void Update()
    {
        if (w.ReadValue<float>() > 0)
        {
            Target.anchoredPosition += new Vector2(0f, 5);
        }
        if (s.ReadValue<float>() > 0)
        {
            Target.anchoredPosition += new Vector2(0f, -1 * 5);
        }
        if (a.ReadValue<float>() > 0)
        {
            Target.anchoredPosition += new Vector2(-5, 0);
        }
        if (d.ReadValue<float>() > 0)
        {
            Target.anchoredPosition += new Vector2(5, 0);
        }
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
