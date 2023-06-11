using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using UnityEngine;

public class SpiderScreamResources : MonoBehaviour {

	[SerializeField] private GameObject MagicEffect = null;
	[SerializeField] private GameObject CastingMagicEffect = null;
	[SerializeField] private GameObject TelaraniaMagicEffect = null;
    [SerializeField] private GameObject PlaceToPlayLaunchMagicEffect = null; 
	[SerializeField] private GameObject PlaceToPlayCastingMagicEffect = null; 
	[SerializeField] private GameObject PlaceToPlayTelaraniaMagicEffect = null; 

    [SerializeField] private AudioSource MagicLaunchAudioSource = null; 
	[SerializeField] private AudioSource MagicCastingAudioSource = null; 

	public float timeToDestroyLaunchMagic = 3f;

	public void WaveScreamLaunchMagicAudio(){
		MagicLaunchAudioSource.Play();  
	}

	public void WaveScreamCastingMagicAudio(){
		MagicCastingAudioSource.Play();  
	}

	public void WaveScreamLaunchMagic(){
		if(MagicEffect != null)
        {
			GameObject newSpell = Instantiate (MagicEffect, PlaceToPlayLaunchMagicEffect.transform.position, PlaceToPlayLaunchMagicEffect.transform.rotation);
			Destroy(newSpell, timeToDestroyLaunchMagic);
        }
	}

	public void WaveScreamCastMagic(){
		if(CastingMagicEffect != null)
        {
			GameObject newSpell = Instantiate (CastingMagicEffect, PlaceToPlayCastingMagicEffect.transform.position, PlaceToPlayCastingMagicEffect.transform.rotation);
			Destroy(newSpell, 5f);
        }
	}

	public void TelaraniaLaunchMagic()
	{
		if(TelaraniaMagicEffect != null)
        {
			GameObject newSpell = Instantiate (TelaraniaMagicEffect, PlaceToPlayTelaraniaMagicEffect.transform.position, PlaceToPlayTelaraniaMagicEffect.transform.rotation);
			Destroy(newSpell, 2.5f);
        }
	}

	
	
}
