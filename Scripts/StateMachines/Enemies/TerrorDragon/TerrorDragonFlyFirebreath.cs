using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using UnityEngine;

public class TerrorDragonFlyFirebreath : MonoBehaviour {

	[SerializeField] private GameObject FireBallEffect = null;
    [SerializeField] private GameObject PlaceToPlayFireBallEffect = null; 
    [SerializeField] private AudioSource MagicLaunchAudioSource = null; 

	public void FlyFireBallMagicAudio(){
		MagicLaunchAudioSource.Play();  
	}
	public void FlyFireBallLaunchMagic(){
		if(PlaceToPlayFireBallEffect != null && FireBallEffect != null)
        {
			GameObject newSpell = Instantiate (FireBallEffect, PlaceToPlayFireBallEffect.transform.position, PlaceToPlayFireBallEffect.transform.rotation);
			newSpell.transform.parent = PlaceToPlayFireBallEffect.transform;
			Destroy(newSpell, 2f);
        }
	}
	
	
}
