using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Atributes
{
    public class ThirdPersonStaminaDisplay : MonoBehaviour
    {
        Stamina stamina;
        private void Awake()
        {
            stamina = GameObject.FindWithTag("Player").GetComponent<Stamina>();
        }

        private void Update()
        {
            GetComponent<Text>().text = String.Format("{0:0}/{1:0}", stamina.GetStaminaPoints(), stamina.GetMaxStaminaPoints());
        }
    }
}

