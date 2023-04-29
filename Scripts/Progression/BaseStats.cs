using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Stats
{
    [Serializable]
    public class BaseStats : MonoBehaviour
    {   
        [Range(1,99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;
        [SerializeField] GameObject levelUpParticleEffect = null;
        [SerializeField] UnityEvent levelUpSound = null;
        [SerializeField] bool shouldUseModifiers = false;
        public event Action OnLevelUp;
        LazyValue<int> currentLevel;
        Experience experience;

        private void Awake()
        {
            experience = GetComponent<Experience>();
            currentLevel = new LazyValue<int>(CalculateLevel);
        }
        private void Start()
        {
            currentLevel .ForceInit();
        }

        private void OnEnable()
        {
            if(experience != null)
            {
                experience.onExperienceGained += UpdateLevel;
            }
        }

        private void OnDisable() 
        {
            if(experience != null)
            {
                experience.onExperienceGained -= UpdateLevel;
            }
        }

        private void UpdateLevel()
        {
           int newLevel = CalculateLevel();
           if(newLevel > currentLevel.value)
           {
                currentLevel.value = newLevel;
                LevelUpEffect();
                OnLevelUp();
                this.IncrementStaminaRecover(0.005f);

                if(newLevel == 20)
                {
                    WarriorPlayerStateMachine warriorPlayerStateMachine =  GameObject.FindWithTag("Player").GetComponent<WarriorPlayerStateMachine>();
                    warriorPlayerStateMachine.SetCanUseSpecialAttack();
                    string controlsMessage = "¡Enhorabuena!, ahora puedes usar ataques especiales\n\nPara poder usarlo tienes tienes que usar la barra espaciadora\n\nPero cuidado, que consumirás mucha energía\n\n";
                    PixelCrushers.DialogueSystem.DialogueManager.ShowAlert(controlsMessage,5f);
                }
           }
        }

        private void IncrementStaminaRecover(float value)
        {
            WarriorPlayerStateMachine warriorPlayerStateMachine =  GameObject.FindWithTag("Player").GetComponent<WarriorPlayerStateMachine>();
            warriorPlayerStateMachine.Stamina.SetStaminaRecover(warriorPlayerStateMachine.Stamina.GetStaminaRecover() + value);
        }

        private void LevelUpEffect()
        {
            if(levelUpParticleEffect == null){return;}
            Transform copyCharacterTransform = transform;
            Vector3 newPosition = copyCharacterTransform.position + new Vector3(0f, 1f, 0f);
            copyCharacterTransform.position = newPosition;
            levelUpSound?.Invoke();
            Instantiate(levelUpParticleEffect, copyCharacterTransform);
        }

        public float GetStat(Stat stat)
        {
            return (GetBaseStat(stat) + GetAdditiveModifier(stat)) * (1 + GetPercentageModifier(stat)/100);
        }

        private float GetBaseStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel());
        }

        public int GetLevel()
        {
            return currentLevel.value;
        }

        public float GetAdditiveModifier(Stat stat)
        {
            if(!shouldUseModifiers){return 0;}
            
            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifiers in provider.GetAdditiveModifiers(stat))
                {
                    total += modifiers;
                }
            }
            return total;
        }

        private float GetPercentageModifier(Stat stat)
        {
            if(!shouldUseModifiers){return 0;}
            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifiers in provider.GetPercentageModifiers(stat))
                {
                    total += modifiers;
                }
            }
            return total;
        }

        private int CalculateLevel()
        {   
            if(experience == null){return startingLevel;}

            float currentXP = experience.GetPoints();
            int penultimateLevel = progression.GetLevels(Stat.ExperienceToLevelUp, characterClass);

            for (int level = 1; level < penultimateLevel; level++)
            {
                float XPToLevelUp = progression.GetStat(Stat.ExperienceToLevelUp,characterClass, level);
                if(XPToLevelUp > currentXP){
                    return level;
                }
            }
            return penultimateLevel +1 ;
        }

      
    }
}

