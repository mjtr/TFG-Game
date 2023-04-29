using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Atributes;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{
    public class ThirdPersonEnemyHealthDisplay : MonoBehaviour
    {
        PlayerStateMachine stateMachine;
        private void Awake()
        {
            stateMachine = GameObject.FindWithTag("Player").GetComponent<PlayerStateMachine>();
        }

        private void Update()
        {
            if(stateMachine.Targeter.CurrentTarget == null)
            {
                GetComponent<Text>().text = "N/A";   
                return;
            }
            Health health = stateMachine.Targeter.CurrentTarget.GetTargetHealth();
            // GetComponent<Text>().text = String.Format("{0:0}%", health.GetPercentage()); //Para mostrar la vida con porcentajes
            GetComponent<Text>().text = String.Format("{0:0}/{1:0}", health.GetHealthPoints(), health.GetMaxHealthPoints());

        }
    }
}

