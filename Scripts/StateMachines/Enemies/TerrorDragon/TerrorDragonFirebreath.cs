using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using UnityEngine;

public class TerrorDragonFirebreath : MonoBehaviour {

	[SerializeField] private GameObject FireBallEffect = null;
    [SerializeField] private GameObject PlaceToPlayFireBallEffect = null; 
    [SerializeField] private AudioSource MagicLaunchAudioSource = null; 

	public void FireBallMagicAudio(){
		MagicLaunchAudioSource.Play();  
	}
	public void FireBallLaunchMagic(){
		if(PlaceToPlayFireBallEffect != null && FireBallEffect != null)
        {
			GameObject newSpell = Instantiate (FireBallEffect, PlaceToPlayFireBallEffect.transform.position, PlaceToPlayFireBallEffect.transform.rotation);
			newSpell.transform.parent = PlaceToPlayFireBallEffect.transform;
			Destroy(newSpell, 3f);
        }
	}

	
	
}
