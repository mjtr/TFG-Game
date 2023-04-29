using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulEaterDragonFirebreath : MonoBehaviour {

	[SerializeField] private GameObject FireBreathEffect = null;
    [SerializeField] private GameObject PlaceToPlayBreathEffect = null; 
	[SerializeField] public GameObject FireBreathWeaponLogic = null;

	private GameObject effectInstantiate;
	public void StartBreath(){
		if(FireBreathEffect != null && PlaceToPlayBreathEffect != null)
        {
            Transform copyEnemyTransform = PlaceToPlayBreathEffect.transform;
            GameObject effect = Instantiate(FireBreathEffect, copyEnemyTransform);
			effectInstantiate = effect;
        }
	}

	public void EndBreath()
	{
		FireBreathWeaponLogic.SetActive(false);
		if(effectInstantiate != null)
		{
			Destroy(effectInstantiate, 0.5f);
		}
		
	}
	
	public void WEnableFireBreathWeaponLogic () 
	{
    	FireBreathWeaponLogic.SetActive(true);
	}
	
}
