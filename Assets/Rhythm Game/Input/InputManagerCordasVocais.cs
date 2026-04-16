using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManagerCordasVocais : MonoBehaviour
{
    public CordasVocaisInputActions cordasVocaisInputActions;
    public static event Action<int> OnLanePressed;
    private void Awake()
    {
        cordasVocaisInputActions = new CordasVocaisInputActions();
        cordasVocaisInputActions.Player.Enable();

        cordasVocaisInputActions.Player.ArrowLeft.performed += _ => OnLanePressed?.Invoke(1);
        cordasVocaisInputActions.Player.ArrowUp.performed += _ => OnLanePressed?.Invoke(2);
        cordasVocaisInputActions.Player.ArrowDown.performed += _ => OnLanePressed?.Invoke(3);
        cordasVocaisInputActions.Player.ArrowRight.performed += _ => OnLanePressed?.Invoke(4);

    }
     
}
