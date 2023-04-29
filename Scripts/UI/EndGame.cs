using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class EndGame : MonoBehaviour
{
    public void EndActualGame()
    {
        SceneManager.LoadScene("InterfazJuegoCompletado");
    }

     public void DeadCharacterEndGame()
    {
        SceneManager.LoadScene("InterfazPersonajeHaMuerto");
    }

}
