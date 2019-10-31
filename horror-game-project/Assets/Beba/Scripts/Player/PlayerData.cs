using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

namespace gameBeba
{
    [System.Serializable]
    public class SavedPlayerData
    {
        public int health;
        [HideInInspector] public float[] playerPosition = new float[3];
        [HideInInspector]  public float[] playerRotation = new float[3];

        public int answeredRight;
        public int answeredWrong;
        public int totalQuestionAnswered;
        public int longestStreak;

    }

    public class PlayerData : MonoBehaviour
    {
        [SerializeField] private GameObject mouseCursor;
        private Vector3 loadedPlayerPosition;
        private Vector3 loadedPlayerRotation;

        [Header("Player Data")]
        public SavedPlayerData currentPlayerData;
        private int maxHealth = 5;
        private FirstPersonController player;

        private void Awake()
        {
            if (SaveLoad.SaveExist("PlayerData"))
            {
                Load();

                loadedPlayerPosition = new Vector3(currentPlayerData.playerPosition[0], 
                    currentPlayerData.playerPosition[1], currentPlayerData.playerPosition[2]);

                loadedPlayerRotation = new Vector3(currentPlayerData.playerRotation[0],
                    currentPlayerData.playerRotation[1], currentPlayerData.playerRotation[2]);
                
                transform.position = loadedPlayerPosition;
                transform.rotation = Quaternion.Euler(loadedPlayerRotation);
            }
            else
            {
                currentPlayerData.health = maxHealth;
                currentPlayerData.answeredRight = 0;
                currentPlayerData.answeredWrong = 0;
                currentPlayerData.totalQuestionAnswered = 0;
                currentPlayerData.longestStreak = 0;
            }
        }

        private void Start()
        {
            player = GameObject.Find("FPSController").GetComponent<FirstPersonController>();

            GameEvents.SaveInitiated += Save;
        }
        
        private void SetPlayerPositionForSaving(Vector3 currentPosition, Quaternion rotation)
        {
            currentPlayerData.playerPosition[0] = currentPosition.x;
            currentPlayerData.playerPosition[1] = currentPosition.y;
            currentPlayerData.playerPosition[2] = currentPosition.z;

            currentPlayerData.playerRotation[0] = rotation.eulerAngles.x;
            currentPlayerData.playerRotation[1] = rotation.eulerAngles.y;
            currentPlayerData.playerRotation[2] = rotation.eulerAngles.z;
        }

        private void Save()
        {
            SetPlayerPositionForSaving(transform.position, transform.rotation);
            SaveLoad.Save<SavedPlayerData>(currentPlayerData, "PlayerData");
            Debug.Log("playerData saved!");
        }

        private void Load()
        {
            if (SaveLoad.SaveExist("PlayerData"))
            {
                currentPlayerData = SaveLoad.Load<SavedPlayerData>("PlayerData");
            }
        }
        
        public Vector3 PlayerPosition()
        {
            return loadedPlayerPosition;
        }
        
        public void EnablePlayer(bool state)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Debug.Log("player enable:" + state);
            }

            if (state == true)
            {
                player.enabled = true;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                mouseCursor.SetActive(false);
            }
            else if(state == false)
            {
                player.enabled = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = false;
                mouseCursor.SetActive(true);
            }
        }

        public void SetPlayerLookAtTarget(Transform target)
        {
            Vector3 direction = (target.transform.position - transform.position).normalized;
            Quaternion rotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5f);
        }

    }

}

