using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{

   private bool firstTime;
   
   public void StartGame()
   {
      if(firstTime){
         firstTime = false;
         ES3.DeleteFile("SaveFile.es3");
         SceneManager.LoadScene("Pantalla de carga");
      }
      
   }

   public void LoadGame()
   {
      if(firstTime){
         firstTime = false;
         SceneManager.LoadScene("Pantalla de carga");
      }
   }


   public void QuitGame()
   {
      Application.Quit();
   }
   
   private void Start() {
      Cursor.lockState = CursorLockMode.None;
      Cursor.visible = true;
      firstTime = true;
   }
}
