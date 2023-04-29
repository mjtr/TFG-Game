using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public class ForceReceived : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private  NavMeshAgent agent; 
    [SerializeField] private float drag = 0.3f;

    private Vector3 dampingVelocity;
    private Vector3 impact;
    private float verticalVelocity;

    public Vector3 Movement => impact + Vector3.up * verticalVelocity;

   private void Update()
   {
        if(controller == null){return;}
        
        if(verticalVelocity < 0f && controller.isGrounded)
        {
            verticalVelocity = Physics.gravity.y * Time.deltaTime;
        }else
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }

        impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVelocity, drag);

        //Para hacer que el enemigo no se mueva tanto cuando le golpeemos
       /* if(agent != null && impact == Vector3.zero)
        {
            agent.enabled = true;
        }*/
        
        if(agent != null && impact.sqrMagnitude < 0.2f * 0.2f)
        {
            impact = Vector3.zero;
            agent.enabled = true;
        }
   }
    
    public void AddForce(Vector3 force){
        impact += force;

        if(agent != null)
        {
            agent.enabled = false;
        }

    }

    public void Jump(float jumForce)
    {
        verticalVelocity += jumForce;
    }

    public void Reset()
    {
        impact = Vector3.zero;
        verticalVelocity = 0f;
    }
}
