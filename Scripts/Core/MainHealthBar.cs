using UnityEngine;

namespace RPG.Atributes
{
    public class MainHealthBar : MonoBehaviour
    {
        [SerializeField] Health healthComponent = null;
        [SerializeField] RectTransform foreground = null;
        [SerializeField] Canvas rootCanvas = null;
        void Update()
        {
            if(healthComponent == null){return;}
            float healtFraction = healthComponent.GetFraction();
            if(Mathf.Approximately(healtFraction,0)
            || Mathf.Approximately(healtFraction,1))
            {
                rootCanvas.enabled = false;
                return;
            }
            rootCanvas.enabled = true;
            foreground.localScale = new Vector3(healtFraction,1,1);
        }
    }

}