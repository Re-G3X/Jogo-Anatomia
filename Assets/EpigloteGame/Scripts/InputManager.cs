using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public EpigloteInputActions epigloteInputActions;
    public Animator epigloteAnimator;
    private void Awake()
    {
        epigloteInputActions = new EpigloteInputActions();
        epigloteInputActions.Player.Enable();

        epigloteInputActions.Player.Fechar.performed += Fechar_performed;
        epigloteInputActions.Player.Fechar.canceled += Fechar_canceled;
    }

    private void Fechar_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        epigloteAnimator.SetTrigger("Open");
        Debug.Log("Abrir!");
    }

    private void Fechar_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        epigloteAnimator.SetTrigger("Close");
        Debug.Log("Close!");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
