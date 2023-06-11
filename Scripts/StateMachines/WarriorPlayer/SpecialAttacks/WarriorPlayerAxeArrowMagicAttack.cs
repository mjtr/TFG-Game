using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using UnityEngine;

public class WarriorPlayerAxeArrowMagicAttack : MonoBehaviour {

	[SerializeField] private GameObject CastingEffect = null;
    [SerializeField] private GameObject PlaceToPlayCastingEffect = null; 
	[SerializeField] private AudioSource MagicCastingAudioSource = null;
    [SerializeField] private AudioSource MagicLaunchAudioSource = null; 
	private GameObject MagicSpiritInstantiate;

	public void PlayAxeCastingMagicAudio(){
		MagicCastingAudioSource.Play();  
	}

	public void PlayAxeLaunchMagicAudio(){
		MagicLaunchAudioSource.Play();  
	}
	public void AxeCastingMagic(){
		if(PlaceToPlayCastingEffect != null && CastingEffect != null)
        {
			Transform copyEnemyTransform = PlaceToPlayCastingEffect.transform;
            MagicSpiritInstantiate = Instantiate(CastingEffect, copyEnemyTransform);
        }
	}
	public void AxeMainMagicAttack(){
		if(MagicSpiritInstantiate != null)
		{
			MagicSpiritInstantiate.GetComponent<ThirdPersonProjectile>().setLaunchSpeed(0.75f);	
		}
		Destroy(MagicSpiritInstantiate, 2f);
	}
	
}
