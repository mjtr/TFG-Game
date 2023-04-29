using System.Collections;
using UnityEngine;

public class VelociraptorScreamMagicResources : MonoBehaviour
{
    [SerializeField] private GameObject MouthEffect = null;
    [SerializeField] private GameObject PlaceToPlayMouthEffect = null; 
	[SerializeField] private GameObject GroundMagicEffect = null;
    [SerializeField] private GameObject PlaceToPlayGroundEffect = null; 

	public void ScreamEffect(){
		if(PlaceToPlayMouthEffect != null && MouthEffect != null)
        {
            Transform copyPlaceTransform = PlaceToPlayMouthEffect.transform;
            GameObject MagicSpiritInstantiate = Instantiate(MouthEffect, copyPlaceTransform);
			Destroy(MagicSpiritInstantiate, 0.6f);
        }
	}

	public void GroundEffect(){
		if(PlaceToPlayGroundEffect != null && GroundMagicEffect != null)
        {
            Transform copyPlaceTransform = PlaceToPlayGroundEffect.transform;
            GameObject MagicSpiritInstantiate = Instantiate(GroundMagicEffect, copyPlaceTransform);
			Destroy(MagicSpiritInstantiate, 0.6f);
        }
	}
	
}
