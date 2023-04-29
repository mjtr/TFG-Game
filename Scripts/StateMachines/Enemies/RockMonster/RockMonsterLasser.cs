using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockMonsterLasser : MonoBehaviour {

	[SerializeField] private GameObject MagicSpiritEffect = null;
    [SerializeField] private GameObject PlaceToPlayBreathEffect = null; 
	[SerializeField] public GameObject LaserWeaponLogic = null;

	private GameObject MagicSpiritInstantiate;
	public void StartBreath(){
		if(PlaceToPlayBreathEffect != null && MagicSpiritEffect != null)
        {
            Transform copyEnemyTransform = PlaceToPlayBreathEffect.transform;
            GameObject MagicSpiritInstantiate = Instantiate(MagicSpiritEffect, copyEnemyTransform);
			Destroy(MagicSpiritInstantiate, 4f);
        }
	}
	
	public void WEnableFireBreathWeaponLogic () 
	{
    	LaserWeaponLogic.SetActive(true);
	}
	
}
