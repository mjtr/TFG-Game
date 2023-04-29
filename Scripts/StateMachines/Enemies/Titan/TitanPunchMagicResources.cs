using System.Collections;
using UnityEngine;

public class TitanPunchMagicResources : MonoBehaviour
{
    [SerializeField] private GameObject PunchEffect = null;
    [SerializeField] private GameObject PlaceToPlayPunchEffect = null; 
    [SerializeField] private AudioSource PunchEffectSound = null; 

	public void PunchExplosionEffect(){
		if(PlaceToPlayPunchEffect != null && PunchEffect != null)
        {
            Transform copyPlaceTransform = PlaceToPlayPunchEffect.transform;
            GameObject MagicSpiritInstantiate = Instantiate(PunchEffect, copyPlaceTransform);
			Destroy(MagicSpiritInstantiate, 1.5f);
        }
	}
	public void PlayPunchAudio(){
		PunchEffectSound.Play();
	}
	
}
