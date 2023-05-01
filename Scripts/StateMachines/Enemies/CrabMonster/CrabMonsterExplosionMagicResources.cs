using System.Collections;
using UnityEngine;

public class CrabMonsterExplosionMagicResources : MonoBehaviour
{
    [SerializeField] private GameObject ExplosionEffect = null;
    [SerializeField] private GameObject PlaceToPlayExplosionEffect = null; 
    [SerializeField] private AudioSource ExplosionEffectSound = null; 
	[SerializeField] private GameObject LaunchEffect = null;
    [SerializeField] private GameObject PlaceToPlayLaunchEffect = null; 

    private GameObject MagicInstanciate;

	public void CastingExplosionEffect(){
		if(PlaceToPlayExplosionEffect != null && ExplosionEffect != null)
        {
            Transform copyPlaceTransform = PlaceToPlayExplosionEffect.transform;
            MagicInstanciate = Instantiate(ExplosionEffect, copyPlaceTransform);
            Destroy(MagicInstanciate, 1.7f);
        }
	}

    public void LaunchExplosionEffect(){
		if(PlaceToPlayLaunchEffect != null && LaunchEffect != null)
        {
            Transform copyPlaceTransform = PlaceToPlayLaunchEffect.transform;
            MagicInstanciate = Instantiate(LaunchEffect, copyPlaceTransform);
            Destroy(MagicInstanciate, 0.5f);
        }
	}
	public void PlayExplosionAudio(){
		ExplosionEffectSound.Play();
	}

	
}
