using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioSource[] AmbientMusics;
    public AudioSource[] ActionMusics;
    public AudioSource BossMusic;

    private bool isAttacking = false;

    public bool GetIsMonsterAttacking()
    {
        return isAttacking;
    }

    public void SetIsMonsterAttacking(bool newValue)
    {
        isAttacking = newValue;
    }
}
