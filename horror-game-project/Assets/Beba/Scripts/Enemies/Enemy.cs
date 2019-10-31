using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace gameBeba
{
    /* to-do
     * 
     * convert this class to abstract class. another class for prefectbear and student bunny.
     * implement patrolling in base class
     * 
     * refactor code
     */

    public class Enemy : MonoBehaviour
    {
        [SerializeField] private float distance;
        private bool isCollideWithPlayer;

        private GameObject player;
        private Animator anim;
        private NavMeshAgent enemy;
        private BattleManager battleManager;
        private Material[] currentMaterials;
        private float _startChasingDistance;
        
        [SerializeField] private float startChasingDistance = 1.0f;
        [SerializeField] private float moveSpeed;
        [SerializeField] private Material eyeMaterialEnabled;
        [SerializeField] private Material eyeMaterialDisabled;
        
        private void Start()
        {
            player = GameObject.Find("FPSController");
            battleManager = GameObject.Find("BattleManager").GetComponent<BattleManager>();
            _startChasingDistance = startChasingDistance;

            enemy = GetComponent<NavMeshAgent>();
            anim = GetComponent<Animator>();

            isCollideWithPlayer = false;

            enemy.speed = 0f;
        }

        private void Update()
        {

            distance = Vector3.Distance(transform.position, player.transform.position);
            
            if (distance < _startChasingDistance)
            {
                Debug.Log(enemy.speed + "update() start chasing");
                Vector3 dirToPlayer = transform.position - player.transform.position;
                Vector3 newPosition = transform.position - dirToPlayer;

                enemy.SetDestination(newPosition);
                enemy.speed = moveSpeed;
            }else
            {
                enemy.speed = 0f;
            }

            if (enemy.speed > 0f)
            {
                anim.SetBool("isMoving", true);
            }
            else if (enemy.speed == 0f )
            {
                anim.SetBool("isMoving", false);
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

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                isCollideWithPlayer = true;
                battleManager.SetCurrentEnemy(this.gameObject);
                SetBattleModeOn();
            }
        }

        private void SetEnemyToMove(bool state)
        {
            if (state)
            {
                enemy.speed = moveSpeed;
                anim.SetBool("isMoving", true);
                _startChasingDistance = startChasingDistance;
            }
            else
            {
                enemy.speed = 0;
                anim.SetBool("isMoving", false);
                _startChasingDistance = 0f;

            }
        }
        
        private void SetBattleModeOn()
        {
            GameManager.Instance.isBattling = true;
        }

        public void DisableEnemy(float disableTime)
        {
            SetEnemyToMove(false);
            this.GetComponent<BoxCollider>().isTrigger = false;

            currentMaterials = this.GetComponentInChildren<SkinnedMeshRenderer>().materials;
            currentMaterials[1] = eyeMaterialDisabled;
            this.GetComponentInChildren<SkinnedMeshRenderer>().materials = currentMaterials;

            isCollideWithPlayer = false;
        }
    }

}
