using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using UnityEngine;


public class WarriorPlayerGreatSwordSlashEffects : MonoBehaviour {

	[SerializeField] private GameObject SlashEffect = null;
    [SerializeField] private GameObject PlaceToPlaySlashEffect = null; 
	[SerializeField] private AudioSource SlashAudioSource = null;
	private GameObject MagicSpiritInstantiate;

	public void GreatSwordSlashAudio(){
		SlashAudioSource.Play();  
	}

	public void GreatSwordSlashEffect(){
		if(PlaceToPlaySlashEffect != null && SlashEffect != null)
        {
			Transform copyEnemyTransform = PlaceToPlaySlashEffect.transform;
            MagicSpiritInstantiate = Instantiate(SlashEffect, copyEnemyTransform);
			Destroy(MagicSpiritInstantiate, 1.5f);
        }
	}
	
}
