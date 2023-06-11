using System;
using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using RPG.Stats;
using UnityEngine;

public class LoadDataController : MonoBehaviour
{

    public List<GameObject> misionEnemies = new List<GameObject>();
    public List<GameObject> normalEnemies = new List<GameObject>();
    public GameObject DialogueManager;
    public GameObject Stephen;

    private GameObject player;

    void Awake()
    {
        if(ES3.KeyExists("FirstTimeToSaveInScene")){
            player = GameObject.FindWithTag("Player");
            Transform transformSaved = ES3.Load<Transform>("PlayerTransform");
            player.transform.SetPositionAndRotation(transformSaved.position, transformSaved.rotation);
            player.transform.localScale = transformSaved.localScale;
            player.transform.SetLocalPositionAndRotation(transformSaved.localPosition, transformSaved.localRotation);
        }
     
    }

    void Start()
    {
        if(ES3.KeyExists("FirstTimeToSaveInScene")){
            LoadPlayerData();
            LoadMisionEnemiesData();
            LoadNormalEnemiesData();
        }
        
    }

    private void LoadMisionEnemiesData()
    {
        for (int i = 0; i < misionEnemies.Count; i++)
        {
            GameObject misionEnemyGameObject = misionEnemies[i];
            string misionEnemyName = misionEnemyGameObject.name;

            bool hasEnemyHealth = ES3.KeyExists(misionEnemyName + "health");
            if(hasEnemyHealth){
                float enemyHealth = ES3.Load<float>(misionEnemyName + "health");
                misionEnemyGameObject.GetComponent<Health>().RestoreHealthPoints(enemyHealth);
                
            }else{
                Destroy(misionEnemyGameObject);
            }
           
        }
    }

    private void LoadNormalEnemiesData()
    {
        for (int i = 0; i < normalEnemies.Count; i++)
        {
            GameObject normalEnemyGameObject = normalEnemies[i];
            string normalEnemyName = normalEnemyGameObject.name;

            bool hasEnemyHealth = ES3.KeyExists(normalEnemyName + "health");
            if(hasEnemyHealth){
                float enemyHealth = ES3.Load<float>(normalEnemyName + "health");
                normalEnemyGameObject.GetComponent<Health>().RestoreHealthPoints(enemyHealth);
                
            }else{
                Destroy(normalEnemyGameObject);
            }
           
        }
    }

    private void LoadPlayerData()
    {          
        WarriorPlayerStateMachine warriorPlayerStateMachine =  player.GetComponent<WarriorPlayerStateMachine>();
        
        if(warriorPlayerStateMachine != null){
            warriorPlayerStateMachine.Health.RestoreLevel(ES3.Load<int>("PlayerLevel"));
            player.GetComponent<Experience>().RestoreExperience(ES3.Load<float>("PlayerExperience"));
            warriorPlayerStateMachine.Stamina.RestoreStamina(ES3.Load<float>("PlayerStaminaPoints"),ES3.Load<float>("PlayerStaminaRecover"));
            warriorPlayerStateMachine.Health.RestoreHealthPoints(ES3.Load<float>("PlayerHealth"));
            warriorPlayerStateMachine.Targeter.RestoreTargets(new List<Target>());
            warriorPlayerStateMachine.RestoreWeapon(ES3.Load<ThirdPersonWeaponConfig>("PlayerWeapon"));
            warriorPlayerStateMachine.RestoreSpecialAttack(ES3.Load<bool>("PlayerSpecialAttackRecover"));

            if(ES3.Load<bool>("PlayerCanUseNewComboAttack"))
            {
                warriorPlayerStateMachine.AddNewComboAttack();
            }

            Transform playerTransformSaved = ES3.Load<Transform>("PlayerTransform");
            gameObject.transform.SetPositionAndRotation(playerTransformSaved.position, playerTransformSaved.rotation);
            gameObject.transform.localScale = playerTransformSaved.localScale;

           // warriorPlayerStateMachine.RestoreHandsTransform(ES3.Load<Transform>("PlayerRightHand"),ES3.Load<Transform>("PlayerLeftHand"));

            Transform cameraMainTransform = ES3.Load<Transform>("CameraMainTransform");
            Camera.main.transform.SetPositionAndRotation(cameraMainTransform.position, cameraMainTransform.rotation);
            Camera.main.transform.SetLocalPositionAndRotation(cameraMainTransform.localPosition, cameraMainTransform.localRotation);


        }else{
            Debug.Log("El warriorPlayerStateMachine es nulo");
        }
       
    }
    
}
