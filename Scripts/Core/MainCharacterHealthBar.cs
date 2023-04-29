using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterHealthBar : MonoBehaviour
{
    [SerializeField] Health healthComponent = null;
    [SerializeField] RectTransform foreground = null;
    void Update()
    {
        float healtFraction = healthComponent.GetFraction();
        foreground.localScale = new Vector3(healtFraction,1,1);
    }
}
