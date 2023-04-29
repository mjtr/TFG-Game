using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class InputReader : MonoBehaviour, Controls.IPlayerActions
{
    public bool IsAttacking {get; private set;}
    public bool IsBlocking {get; private set;}

    public event Action JumpEvent;
    public event Action DodgeEvent;
    public event Action TargetEvent;
    public event Action RollEvent;


    public Vector2 MovementValue{get; private set;}

    private Controls controls;
    private void Start(){
        controls = new Controls();
        controls.Player.SetCallbacks(this);
        controls.Player.Enable();
    }

    private void onDestroy(){
        controls.Player.Disable();
    }

    public void OnDodge(InputAction.CallbackContext context){
        if(!context.performed){ return; }
        DodgeEvent?.Invoke();
    }

  
    void Controls.IPlayerActions.OnMove(InputAction.CallbackContext context) {
        MovementValue = context.ReadValue<Vector2>();
    }

    void Controls.IPlayerActions.OnLook(InputAction.CallbackContext context)
    {
       
    }

    public void OnTarget(InputAction.CallbackContext context)
    {
        if(!context.performed){return;}
        TargetEvent?.Invoke();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if(context.performed){
            IsAttacking = true;
        }else if(context.canceled){
            IsAttacking = false;
        }
    }

    public void OnBlock(InputAction.CallbackContext context)
    {
        if(context.performed){
            IsBlocking = true;
        }else if(context.canceled){
            IsBlocking = false;
        }
    }

}
