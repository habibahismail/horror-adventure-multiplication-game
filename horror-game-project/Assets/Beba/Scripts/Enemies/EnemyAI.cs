using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace gameBeba
{
    /*
     * TODO: 
     * interaction when student is disabled.
     */

    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] protected Transform[] waypoints;
        [Space]
        [SerializeField] protected float facePlayerFactor = 20f;
        [SerializeField] protected float patrolStartWaitTime = 1f;
        [SerializeField] protected float disableTime = 20f;
        [Space]
        [SerializeField] protected float chaseRadius = 20f;
        [SerializeField] protected float lineOfSightRadius = 45f;
        [SerializeField] protected float memoryStartTime = 10f;
        [Space]
        [SerializeField] private Material eyeMaterialEnabled;
        [SerializeField] private Material eyeMaterialDisabled;

        protected GameObject player;
        protected Animator anim;
        protected NavMeshAgent enemy;
        protected BattleManager battleManager;
        protected Material[] currentMaterials;
        
        protected float distance;
        protected float patrolWaitTime;
        protected int randomSpot;
        
        protected bool canSeePlayer = false;
        protected float fieldOfViewAngle = 160f;

        protected bool aiMemorizePlayer = false;
        protected float increasingMemoryTime;

        private bool isMoving = false;
        private bool playerIsInLOS = false;
        private bool isCollideWithPlayer = false;
        private bool isDisabled = false;

        private float _disableTime;

        protected void Start()
        {
            enemy = GetComponent<NavMeshAgent>();
            anim = GetComponent<Animator>();
            player = GameObject.Find("FPSController");
            battleManager = GameObject.Find("BattleManager").GetComponent<BattleManager>();
            currentMaterials = this.GetComponentInChildren<SkinnedMeshRenderer>().materials;

            patrolWaitTime = patrolStartWaitTime;
            _disableTime = disableTime;
            GenerateRandomSpot();
        }

        protected void Update()
        {
            distance = Vector3.Distance(transform.position, player.transform.position);

            if (isDisabled)
            {
                _disableTime -= Time.deltaTime;
                Debug.Log(_disableTime);

                if(_disableTime < 0)
                {
                    ReenableEnemy();
                    isDisabled = false;
                    _disableTime = disableTime;
                }
            }

            if(distance <= lineOfSightRadius)
            {
                CheckLOS();
            }


            if (enemy.isActiveAndEnabled)
            {
                if(playerIsInLOS == false && aiMemorizePlayer == false)
                {
                    Patrol();
                    StopCoroutine(AiMemory());
                }
                else if(playerIsInLOS == true){

                    aiMemorizePlayer = true;
                    FacePlayer();
                    ChasePlayer();
                }
                else if ( aiMemorizePlayer == true && playerIsInLOS == false)
                {
                    ChasePlayer();
                    StartCoroutine(AiMemory());
                }

                if (isCollideWithPlayer)
                {
                    player.GetComponent<PlayerData>().SetPlayerLookAtTarget(this.transform);
                    SetEnemyToMove(false);
                }

                if (GameManager.Instance.isInteracting)
                {
                    SetEnemyToMove(false);
                }
                else
                {
                    SetEnemyToMove(true);
                }
            }

        }

        protected void Patrol()
        {
            enemy.SetDestination(waypoints[randomSpot].position);

            if (!isMoving)
            {
                anim.SetBool("isMoving", true);
                isMoving = true;
            }

            if(Vector3.Distance(transform.position, waypoints[randomSpot].position) < 2f)
            {
                if (patrolWaitTime <= 0)
                {
                    GenerateRandomSpot();
                    patrolWaitTime = patrolStartWaitTime;
                }
                else
                {
                    patrolWaitTime -= Time.deltaTime;

                    if (isMoving)
                    {
                        anim.SetBool("isMoving", false);
                        isMoving = false;
                    }
                }
            }

        }
        
        protected void ChasePlayer()
        {
            distance = Vector3.Distance(transform.position, player.transform.position);

            if (distance <= chaseRadius)
            {
                Vector3 dirToPlayer = transform.position - player.transform.position;
                Vector3 newPosition = transform.position - dirToPlayer;

                enemy.SetDestination(newPosition);

                if (!isMoving)
                {
                    anim.SetBool("isMoving", true);
                    isMoving = true;
                }
            }
            
        }

        protected void FacePlayer()
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * facePlayerFactor);
        }

        private void GenerateRandomSpot()
        {
            randomSpot = Random.Range(0, waypoints.Length);
        }

        protected void CheckLOS()
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            float angle = Vector3.Angle(direction, transform.forward);

            if(angle < fieldOfViewAngle * 0.5f)
            {
                RaycastHit raycastHit;

                if(Physics.Raycast(transform.position,direction,out raycastHit, lineOfSightRadius))
                {
                    if(raycastHit.collider.tag == "Player")
                    {
                        playerIsInLOS = true;
                        aiMemorizePlayer = true;
                    }
                    else
                    {
                        playerIsInLOS = false;
                    }
                }
            }
        }

        IEnumerator AiMemory()
        {
            increasingMemoryTime = 0;

            while(increasingMemoryTime < memoryStartTime)
            {
                increasingMemoryTime += Time.deltaTime;
                aiMemorizePlayer = true;
                yield return null;
            }

            aiMemorizePlayer = false;
        }

        protected void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                isCollideWithPlayer = true;
                battleManager.SetCurrentEnemy(this.gameObject);
                SetBattleModeOn();
                SetEnemyToMove(false);
            }
        }

        private void SetEnemyToMove(bool state)
        {
            if (state)
            {
                enemy.enabled = true;

                anim.SetBool("isMoving", true);
                isMoving = true;
            }
            else
            {
                enemy.enabled = false;

                anim.SetBool("isMoving", false);
                isMoving = false;
            }
        }

        private void SetBattleModeOn()
        {
            GameManager.Instance.isBattling = true;
        }

        public void DisableEnemy()
        {
            SetEnemyToMove(false);

            this.GetComponent<BoxCollider>().isTrigger = false;

            currentMaterials = this.GetComponentInChildren<SkinnedMeshRenderer>().materials;
            currentMaterials[1] = eyeMaterialDisabled;
            this.GetComponentInChildren<SkinnedMeshRenderer>().materials = currentMaterials;

            isCollideWithPlayer = false;
            isDisabled = true;
        }

        private void ReenableEnemy()
        {
            SetEnemyToMove(true);

            this.GetComponent<BoxCollider>().isTrigger = true;

            currentMaterials[1] = eyeMaterialEnabled;
            this.GetComponentInChildren<SkinnedMeshRenderer>().materials = currentMaterials;
            
        }

    }

}
