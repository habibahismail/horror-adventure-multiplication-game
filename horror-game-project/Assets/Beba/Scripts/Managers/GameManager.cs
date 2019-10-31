using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace gameBeba
{
    [System.Serializable]
    public class GameState
    {
        public bool isCodeGenerated;
    }
    
    public class GameManager : MonoBehaviour
    {

        #region instance declaration

        private static GameManager _instance;

        public static GameManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    _instance = obj.AddComponent<GameManager>();
                    obj.name = typeof(GameManager).ToString();
                }

                return _instance;
            }
        }
        #endregion

        [Header("Menu Properties")]
        public bool isPaused;
        public bool lockPauseMenu;

        [Header("Door Codes Generator")]
        [SerializeField]
        private int codesToGenerate = 0;
        public int codeLength = 0;
        public List<CodesLists> loadedDoorCodes = new List<CodesLists>();

        [Header("Game Properties")]
        public bool isBattling;
        public bool isInteracting;

        private CodesGenerator doorCodes;
        private UIManager ui;
        private GameState currentGameState = new GameState();

        #region Awake
        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            
            if (SaveLoad.SaveExist("GameState"))
            {
                Load();
            }
            else {
                currentGameState.isCodeGenerated = false;
            }
            
            if (!currentGameState.isCodeGenerated)
            {
                doorCodes = new CodesGenerator(codeLength, codesToGenerate);
                currentGameState.isCodeGenerated = true;
                Save();
                GameEvents.OnSaveInitiated();
                loadedDoorCodes = doorCodes.Load();
            }
            else
            {
                doorCodes = new CodesGenerator();
                loadedDoorCodes = doorCodes.Load();
            }

            GameEvents.SaveInitiated += Save;
        }
        #endregion
        
        private void Start()
        {
            isBattling = false;
            isInteracting = false;
            isPaused = false;
           
            //ui = GameObject.Find("UIManager").GetComponent<UIManager>();
            
        }
        
        private void Save()
        {
            SaveLoad.Save<GameState>(currentGameState, "GameState");
            Debug.Log("gameState Saved!");
        }
       
        private void Load()
        {
            if (SaveLoad.SaveExist("GameState"))
            {
                currentGameState = SaveLoad.Load<GameState>("GameState");
                Debug.Log("gameState Loaded!");
            }
        }

        private void InvokeClearNotification()
        {
            GameEvents.OnClearNotificationInitiated();
        }


        public void ClearNotification()
        {
            Invoke("InvokeClearNotification", 2.1f);
           
        }
        
        public void SaveButton()
        {
            GameEvents.OnSaveInitiated();
        }
    }
}

