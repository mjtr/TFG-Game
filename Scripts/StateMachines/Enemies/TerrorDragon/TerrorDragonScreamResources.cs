using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using UnityEngine;

public class TerrorDragonScreamResources : MonoBehaviour {

	[SerializeField] private GameObject MagicEffect = null;
	[SerializeField] private GameObject CastingMagicEffect = null;
    [SerializeField] private GameObject PlaceToPlaMagicEffect = null; 
    [SerializeField] private AudioSource MagicLaunchAudioSource = null; 

	private Vector3 position;

	public void WaveScreamMagicAudio(){
		MagicLaunchAudioSource.Play();  
	}
	public void WaveScreamLaunchMagic(){
		GameObject player = GameObject.FindWithTag("Player");
		if(MagicEffect != null)
        {
			GameObject newSpell = Instantiate (MagicEffect, position, player.transform.rotation);
			Destroy(newSpell, 3f);
        }
	}

	public void WaveScreamCastMagic(){
		GameObject player = GameObject.FindWithTag("Player");
		if(CastingMagicEffect != null)
        {
			position = player.transform.position;
			position.y = position.y + 0.3f;
			GameObject newSpell = Instantiate (CastingMagicEffect, position, player.transform.rotation);
			Destroy(newSpell, 5f);
        }
	}

	
	
}
