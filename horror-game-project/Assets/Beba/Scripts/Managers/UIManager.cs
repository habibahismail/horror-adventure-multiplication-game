using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace gameBeba
{
    public class UIManager : MonoBehaviour
    {

        //#region instance declaration

        //private static UIManager _instance;

        //public static UIManager Instance
        //{
        //    get
        //    {
        //        if (_instance == null)
        //        {
        //            GameObject obj = new GameObject();
        //            _instance = obj.AddComponent<UIManager>();
        //            obj.name = typeof(UIManager).ToString();
        //        }

        //        return _instance;
        //    }
        //}
        //#endregion

        [Header("BattleUI")]
        [SerializeField] private GameObject quizUI;
        [SerializeField] private Text instruction;
        [SerializeField] private Text question;
        [SerializeField] private GameObject answerChoiceButton;
        [SerializeField] private GameObject answerChoicePlaceholder;

        [Header("Player Health UI")]
        [SerializeField] private GameObject playerLifeUI;
        [SerializeField] private Image[] hearts;
        [SerializeField] private Sprite emptyHeart;
        [SerializeField] private Sprite fullHeart;

        [Header("Player Action UI")]
        [SerializeField] private Text actionKeyText;
        [SerializeField] private Text actionKeyDesc;
        
        [Header("Code Hint UI")]
        [SerializeField] private GameObject codeHintUI;
        [SerializeField] private Text hintText;
        [SerializeField] private Image bgImage;

        [Header("Subtitles/Dialogues UI")]
        [SerializeField] private Text dialogLine;
        [SerializeField] private Text subtitle;
        [SerializeField] private Text continueButton;

        [Header("Inventory UI")]
        [SerializeField] private InventorySlot[] inventorySlot;
        [SerializeField] private GameObject itemDetailsDisplay;
        [SerializeField] private Text itemNameDisplay;
        [SerializeField] private Text itemDescriptionDisplay;
        [SerializeField] private Image itemImageDisplay;
        [Space]
        [SerializeField] private Canvas subsActionCanvas;
        
        private int currentQuestionIndex = 0;

        private QuestionsGenerator[] currentQuestionList;
        private BattleManager battleManager;
        private PlayerData player;
        private Inventory playerInventory;
        private GameObject[] answerDisplay;

        private void Start()
        {
            player = GameObject.Find("FPSController").GetComponent<PlayerData>();
            battleManager = GameObject.Find("BattleManager").GetComponent<BattleManager>();
            playerInventory = GameObject.Find("FPSController").GetComponent<Inventory>();
            currentQuestionList = new QuestionsGenerator[battleManager.QuestionCount()];
            
            GameEvents.ClearNotification += ClearActionNotification;
        }

        private void ClearActionNotification()
        {
            actionKeyText.text = "";
            actionKeyDesc.text = "";
        }
        
        private void ShowPlayerHealthUI(bool state)
        {
            if (state)
            {
                playerLifeUI.SetActive(true);
            }
            else
            {
                playerLifeUI.SetActive(false);
            }
        }

        private void ShowQuizUI(bool state)
        {

            if (state)
            {
                quizUI.SetActive(true);
                
                if (currentQuestionIndex == 0)
                {
                    DisplayQuestion(currentQuestionIndex);
                }
            }
            else
            {
                quizUI.SetActive(false);
            }
            
        }
        
        private void DisplayQuestion(int index)
        {
            question.text = currentQuestionList[index].question;
            answerDisplay = new GameObject[battleManager.QuestionCount()];
            
            for (int i = 0; i < currentQuestionList[index].answerChoice.Count; i++)
            {
                answerDisplay[i] = Instantiate(answerChoiceButton, answerChoicePlaceholder.transform);
                Button answerButton = answerDisplay[i].GetComponent<Button>();
                answerButton.GetComponentInChildren<Text>().text = currentQuestionList[index].answerChoice[i].ToString();

                int a = currentQuestionList[index].answerChoice[i];
                answerButton.onClick.AddListener(() => battleManager.CheckAnswer(a, currentQuestionList[index].answer));
                
            }
        }

        public void GoToNextQuestion()
        {
            for (int i = 0; i < answerDisplay.Length; i++)
            {
                Destroy(answerDisplay[i]);
            }

            currentQuestionIndex++;
          
            if(currentQuestionIndex < currentQuestionList.Length && player.currentPlayerData.health > 0)
            {
                DisplayQuestion(currentQuestionIndex);
            }else
            {
                battleManager.SetBattleMode(false);
                currentQuestionIndex = 0;
            }
        }

        public void SetCurrentQuestionList(QuestionsGenerator[] q, int count)
        {
            currentQuestionList = q;            
        }

        public void ShowBattleUI(bool state)
        {
            if (state)
            {
                ShowPlayerHealthUI(true);
                ShowQuizUI(true);

            }else
            {
                ShowPlayerHealthUI(false);
                ShowQuizUI(false);
            }
        }

        public void UpdateHealthUI()
        {
            for (int i = 0; i < hearts.Length; i++)
            {
                if (i < player.currentPlayerData.health)
                {
                    hearts[i].sprite = fullHeart;
                }
                else
                {
                    hearts[i].sprite = emptyHeart;
                }
            }
        }

        public void SetActionKeyUI(string keyText,string keyDesc)
        {
            actionKeyText.text = keyText;
            actionKeyDesc.text = keyDesc;
        }
  
        public void SetDialogsSubsText(string line)
        {
            dialogLine.text = line;
        }

        public void TypeDialogByLetter(char letter)
        {
            dialogLine.text += letter;
        }

        public void SetContinueButtonText(string text)
        {
            continueButton.text = text;   
        }

        public string GetCurrentDialogLine()
        {
            string line = dialogLine.text;

            return line;
        }

        public void UpdateInventorySlot(List<Item> items)
        {
            int i = 0;

            for (; i < items.Count && i < inventorySlot.Length; i++)
            {
                inventorySlot[i].item = items[i];

                Button slotButton = inventorySlot[i].GetComponent<Button>();
                Item currentItem = items[i];
                
                if (items[i].quantity > 1)
                {
                    inventorySlot[i].GetComponentInChildren<Text>().text = items[i].quantity.ToString();
                }
                else
                {
                    inventorySlot[i].GetComponentInChildren<Text>().text = "";
                }
            }

            for (; i < inventorySlot.Length; i++)
            {
                inventorySlot[i].item = null;
            }
        }

        public int InventorySlotCount()
        {
            return inventorySlot.Length;
        }

        public void ShowItemDetails(Item item)
        {
            itemDetailsDisplay.SetActive(true);

            itemNameDisplay.text = item.name;
            itemDescriptionDisplay.text = item.description;
            itemImageDisplay.sprite = item.image;
        }

        public void HideItemDetails()
        {
            itemDetailsDisplay.SetActive(false);
        }

        public void DisableSubsDialogMenu(bool state)
        {
            if (state)
            {
                subsActionCanvas.enabled = false;
            }else
            {
                subsActionCanvas.enabled = true;
            }
        }

        
    }
}
