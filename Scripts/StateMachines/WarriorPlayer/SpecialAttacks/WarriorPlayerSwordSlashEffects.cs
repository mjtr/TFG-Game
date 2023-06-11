using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using UnityEngine;


public class WarriorPlayerSwordSlashEffects : MonoBehaviour {

	[SerializeField] private GameObject SlashEffect = null;
    [SerializeField] private GameObject PlaceToPlaySlashEffect = null; 
	[SerializeField] private AudioSource SlashAudioSource = null;
	private GameObject MagicSpiritInstantiate;

	public void SwordSlashAudio(){
		SlashAudioSource.Play();  
	}

	public void SwordSlashEffect(){
		if(PlaceToPlaySlashEffect != null && SlashEffect != null)
        {
			Transform copyEnemyTransform = PlaceToPlaySlashEffect.transform;
            MagicSpiritInstantiate = Instantiate(SlashEffect, copyEnemyTransform);
			Destroy(MagicSpiritInstantiate, 0.1f);
        }
	}
	
}
