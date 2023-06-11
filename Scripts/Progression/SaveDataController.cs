using System;
using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using RPG.Stats;
using UnityEngine;

public class SaveDataController : MonoBehaviour
{
   
    public List<GameObject> misionEnemies = new List<GameObject>();
    public List<GameObject> normalEnemies = new List<GameObject>();

    public GameObject DialogueManager;
    public GameObject Stephen;

    public void SaveSceneData()
    {
        if(ES3.KeyExists("FirstTimeToSaveInScene")){
            ES3.DeleteFile("SaveFile.es3");
        }

        ES3.Save<bool>("FirstTimeToSaveInScene", true);
        SavePlayerData();
        SaveMisionEnemiesData();
        SaveNormalEnemiesData();
    }

    private void SaveMisionEnemiesData()
    {
    
        for (int i = 0; i < misionEnemies.Count; i++)
        {
            GameObject misionEnemyGameObject = misionEnemies[i];
            if(misionEnemyGameObject != null){
                string misionEnemyName = misionEnemyGameObject.name;
                //IncrementOnDestroy misionEnemyIncrementOnDestroy  = misionEnemyGameObject.GetComponent<IncrementOnDestroy>();
                
                string enemyHealthString = misionEnemyName + "health";
                ES3.Save<float>(enemyHealthString, misionEnemyGameObject.GetComponent<Health>().GetHealthPoints());
            }
            
        }

    }

     private void SaveNormalEnemiesData()
    {
    
        for (int i = 0; i < normalEnemies.Count; i++)
        {
            GameObject normalEnemieGameObject = normalEnemies[i];
            if(normalEnemieGameObject != null){
                string normalEnemyName = normalEnemieGameObject.name;
                //IncrementOnDestroy misionEnemyIncrementOnDestroy  = misionEnemyGameObject.GetComponent<IncrementOnDestroy>();
                
                string enemyHealthString = normalEnemyName + "health";
                ES3.Save<float>(enemyHealthString, normalEnemieGameObject.GetComponent<Health>().GetHealthPoints());
            }
            
        }

    }

    private void SavePlayerData()
    {
        GameObject player = GameObject.FindWithTag("Player");   
        
        ES3.Save<float>("PlayerHealth", player.GetComponent<Health>().GetHealthPoints());
        ES3.Save<float>("PlayerExperience", player.GetComponent<Experience>().GetPoints());
        ES3.Save<int>("PlayerLevel", player.GetComponent<BaseStats>().GetLevel());
        ES3.Save<float>("PlayerStaminaPoints", player.GetComponent<Stamina>().GetStaminaPoints());
        ES3.Save<float>("PlayerStaminaRecover", player.GetComponent<Stamina>().GetStaminaRecover());
        
        ES3.Save<Transform>("PlayerTransform", player.transform);

        WarriorPlayerStateMachine warriorPlayerStateMachine = player.GetComponent<WarriorPlayerStateMachine>();
        ES3.Save<bool>("PlayerSpecialAttackRecover", warriorPlayerStateMachine.GetCanUseSpecialAttack());
        ES3.Save<bool>("PlayerCanUseNewComboAttack", warriorPlayerStateMachine.GetCanUseNewComboAttack());

        ES3.Save<Transform>("PlayerRightHand", warriorPlayerStateMachine.GetRightHandTransform());
        ES3.Save<Transform>("PlayerLeftHand", warriorPlayerStateMachine.GetLeftHandTransform());
        ES3.Save<Transform>("CameraMainTransform", Camera.main.transform);

        ES3.Save<ThirdPersonWeaponConfig>("PlayerWeapon", warriorPlayerStateMachine.DefaultWeapon);
    }

}
