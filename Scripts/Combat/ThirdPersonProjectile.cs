using System;
using RPG.Atributes;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class ThirdPersonProjectile : MonoBehaviour
    {
        [SerializeField] float launchSpeed = 1.2f;
        [SerializeField] bool isHoming = false; //Para que el proyectil siga al personaje
        [SerializeField] bool isThrowUp = false; //Para que el proyectil se lance primero hacia arriba
        [SerializeField] float maxLifeTime = 10f;//Como máximo el proyectil vivirá 10 segundos
        [SerializeField] float  lifeAfterImpact = 0f;
        [SerializeField] float proyectileDamage = 5f;
        [SerializeField] float forceY = 25.0f;
        [SerializeField] bool isFromPlayer = false;
        [SerializeField] GameObject[]  destroyOnHit = null;  
        [SerializeField] GameObject hitEffect = null; // asociaremos al proyectil un efecto cuando colisione
        [SerializeField] GameObject explosionParticle;
        [SerializeField] UnityEvent onHit;
        //[SerializeField] Transform spawnTransform;
       
        Health HealthTarget = null;
        Transform targetTransform = null;
        GameObject instigator = null;
        private Rigidbody proyectileRigidbody;

        private void Start()
        {
            if(!isFromPlayer)
            {
                HealthTarget = GameObject.FindWithTag("Player").GetComponent<Health>();
                targetTransform = GameObject.FindGameObjectWithTag("Player").transform;
            }
  
            proyectileRigidbody =  GetComponent<Rigidbody>();
            if(isThrowUp)
            {
                proyectileRigidbody.AddForce(new Vector3(0, forceY, 0));
            }
           
           // transform.LookAt(GetTargetLocation());//Hacemos que al comenzar a lanzar el proyectil éste apunte hacia el target
        }

        void Update()
        {

            if(isFromPlayer){
                if (launchSpeed == 0f){return;}
                proyectileRigidbody.AddForce(transform.forward * launchSpeed, ForceMode.Impulse);
                gameObject.transform.Translate(Vector3.back * launchSpeed);
                return;
            }

            if(!isFromPlayer && targetTransform == null){return;}
            if(isHoming && !isFromPlayer)
            {
                transform.LookAt(targetTransform);
            }
           
            if(proyectileRigidbody == null){return;}
            proyectileRigidbody.AddForce(transform.forward * launchSpeed);
            return;
            
            //transform.Translate(Vector3.forward * launchSpeed * Time.deltaTime); //Esto es para que el proyectil gire sobre si mismo*/
        }


        public void SetTarget(Health target,GameObject instigator, float damage)//Instigator es para la experiencia
        {
            this.HealthTarget = target;
            this.proyectileDamage = damage;
            this.instigator = instigator;

            Destroy(gameObject, maxLifeTime);
        }

        private Vector3 GetTargetLocation()
        {
            return HealthTarget.transform.position;
        }

        private void OnTriggerEnter(Collider other)
        {

            if(isFromPlayer)
            {
                if(other.GetComponent<Health>() == null)
                {
                    Destroy(gameObject, maxLifeTime);
                    return;
                }  

                instigator = GameObject.FindWithTag("Player");
                Health EnemyHealth = other.GetComponent<Health>();
                if(EnemyHealth.CheckIsDead()){return;}
                if(EnemyHealth == instigator.GetComponent<Health>()){return;}
                SetTarget(EnemyHealth, instigator, proyectileDamage);
            
            }else{

                if(instigator == null){
                    SetTarget(HealthTarget, gameObject, proyectileDamage);
                }   

                GameObject weHit = other.gameObject;
                if (weHit.name == "SecondEnviroment" || weHit.name == "Enviroment" || weHit.name == "SecondTerrain"){
                    StartExplosionEffect();
                    return;
                }

                if(other.GetComponent<Health>() == null){return;}
                if(HealthTarget != null && other.GetComponent<Health>() != HealthTarget ){return;}
                
            
                if(HealthTarget.CheckIsDead()){return;}

            }

            if(HealthTarget.GetIsInvulnerable())
            {
                HealthTarget.FindAndPlayPlayerBlockEvents();
            }
            else
            {
                HealthTarget.TakeDamage(instigator,proyectileDamage, true);
            }


            StartExplosionEffect();

            launchSpeed = 0f;

            onHit?.Invoke();

            if(hitEffect != null)
            {
                Instantiate(hitEffect,GetTargetLocation(), transform.rotation);
            }

            foreach(GameObject toDestroy in destroyOnHit)
            {
                Destroy(toDestroy);
            }

           // Destroy(gameObject, lifeAfterImpact);
            
        }

        public void setLaunchSpeed(float newValue)
        {
            launchSpeed = newValue;
        }

        private void StartExplosionEffect()
        {
            if(explosionParticle != null)
            {
                Vector3 collisionPosition = transform.position;
                //collisionPosition.y += 0.5f; // Si queremos que se instacion un poco más arriba de donde colisiona
                GameObject newParticle = Instantiate(explosionParticle, collisionPosition, Quaternion.identity);
                Destroy (newParticle, 2.0f);
            }
            Destroy(gameObject);
        }
    }
}

