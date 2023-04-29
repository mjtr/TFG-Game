using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterStaminaBar : MonoBehaviour
{
    [SerializeField] Stamina staminaComponent = null;
    [SerializeField] RectTransform foreground = null;
    void Update()
    {
        float healtFraction = staminaComponent.GetFraction();
        foreground.localScale = new Vector3(healtFraction,1,1);
    }
}
