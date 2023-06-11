using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonFirebreath : MonoBehaviour {
	[SerializeField] private GameObject FireBallEffect = null;
    [SerializeField] private GameObject PlaceToPlayFireBallEffect = null; 
	
	public void FireBreathLaunchMagic(){
		if(PlaceToPlayFireBallEffect != null && FireBallEffect != null)
        {
			GameObject newSpell = Instantiate (FireBallEffect, PlaceToPlayFireBallEffect.transform.position, PlaceToPlayFireBallEffect.transform.rotation);
			newSpell.transform.parent = PlaceToPlayFireBallEffect.transform;
			Destroy(newSpell, 3f);
        }
	}

	
}
