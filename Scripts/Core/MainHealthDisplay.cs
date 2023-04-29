using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Atributes
{
    public class MainHealthDisplay : MonoBehaviour
    {
        Health health;
        private void Awake()
        {
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
        }

        private void Update()
        {
           // GetComponent<Text>().text = String.Format("{0:0}%", health.GetPercentage()); //Para mostrar la vida en porcentajes
            GetComponent<Text>().text = String.Format("{0:0}/{1:0}", health.GetHealthPoints(), health.GetMaxHealthPoints());
        }
    }
}

