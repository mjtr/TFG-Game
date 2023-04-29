using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Atributes
{
    public class MainStaminaDisplay : MonoBehaviour
    {
        Stamina playerStamina;
        private void Awake()
        {
            playerStamina = GameObject.FindWithTag("Player").GetComponent<Stamina>();
        }

        private void Update()
        {
           // GetComponent<Text>().text = String.Format("{0:0}%", health.GetPercentage()); //Para mostrar la estamina en porcentajes
            GetComponent<Text>().text = String.Format("{0:0}/{1:0}", playerStamina.GetMaxStaminaPoints(), playerStamina.GetMaxStaminaPoints());
        }
    }
}

