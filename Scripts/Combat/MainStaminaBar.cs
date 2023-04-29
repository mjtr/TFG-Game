using UnityEngine;

namespace RPG.Atributes
{
    public class MainStaminaBar : MonoBehaviour
    {
        [SerializeField] Stamina staminaComponent = null;
        [SerializeField] RectTransform foreground = null;
        [SerializeField] Canvas rootCanvas = null;
        void Update()
        {
            float staminaFraction = staminaComponent.GetFraction();
            if(Mathf.Approximately(staminaFraction,0)
            || Mathf.Approximately(staminaFraction,1))
            {
                rootCanvas.enabled = false;
                return;
            }
            rootCanvas.enabled = true;
            foreground.localScale = new Vector3(staminaFraction,1,1);
        }
    }

}