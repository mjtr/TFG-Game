using System.Collections;
using UnityEngine;

public class CrabMonsterHealMagicResources : MonoBehaviour
{
    [SerializeField] private GameObject CastingEffect = null;
    [SerializeField] private GameObject PlaceToPlayCastingEffect = null; 
	[SerializeField] private GameObject LaunchEffect = null;
    [SerializeField] private GameObject PlaceToPlayLaunchEffect = null; 
	[SerializeField] private AudioSource MagicCastingAudioSource = null;
    [SerializeField] private AudioSource MagicLaunchAudioSource = null; 
	private GameObject MagicSpiritInstantiate;

	public void PlayCrabMonsterCastingMagicAudio(){
		MagicCastingAudioSource.Play();  
	}

	public void PlayCrabMonsterLaunchMagicAudio(){
		MagicLaunchAudioSource.Play();  
	}
	public void CrabMonsterCastingMagic(){
		if(PlaceToPlayCastingEffect != null && CastingEffect != null)
        {
			Transform copyEnemyTransform = PlaceToPlayCastingEffect.transform;
            MagicSpiritInstantiate = Instantiate(CastingEffect, copyEnemyTransform);
        }
	}
	public void CrabMonsterMainMagicAttack(){
		if(MagicSpiritInstantiate != null)
		{
			Destroy(MagicSpiritInstantiate);
		}

		if(PlaceToPlayLaunchEffect != null && LaunchEffect != null)
        {
			Transform copyEnemyTransform = PlaceToPlayLaunchEffect.transform;
            MagicSpiritInstantiate = Instantiate(LaunchEffect, copyEnemyTransform);
			Destroy(MagicSpiritInstantiate, 1.5f);
        }

		GetComponent<Health>().Heal(10000f); 
	}
	
}
