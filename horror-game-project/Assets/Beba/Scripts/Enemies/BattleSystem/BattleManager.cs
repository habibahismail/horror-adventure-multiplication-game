using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace gameBeba
{

    public class BattleManager : MonoBehaviour
    {
        private QuestionsGenerator[] questions; 
        private UIManager ui;
        private GameObject player;
        private GameObject currentEnemy;
        private PlayerData playerData;
        private FlashLight flashlight;

        private bool battleUIon;
        private bool battleEnded;
        private int questionCount = 3;

        private void Start()
        {
            player = GameObject.Find("FPSController");
            playerData = player.GetComponent<PlayerData>();

            ui = GameObject.Find("UIManager").GetComponent<UIManager>();
            flashlight = GameObject.Find("FlashLight").GetComponent<FlashLight>();
            
            battleUIon = false;
            battleEnded = false;
            
        }

        private void Update()
        {
            if (GameManager.Instance.isBattling && battleUIon == false)
            {
                playerData.EnablePlayer(false);
                GameManager.Instance.lockPauseMenu = true;
                GameManager.Instance.isInteracting = true;

                SetBattleMode(true);

                ui.ShowBattleUI(true);
                ui.UpdateHealthUI();

                flashlight.DisableFlashLight(true);


            }

            if (battleUIon == false && battleEnded == true && GameManager.Instance.isPaused == false &&
                GameManager.Instance.isInteracting == false )
            {
                if (playerData.currentPlayerData.health > 0) {

                    playerData.EnablePlayer(true);
                    currentEnemy.GetComponent<Enemy>().DisableEnemy(20f);
                    ui.ShowBattleUI(false);
                }
                else
                {
                    Debug.Log("GameOver!");
                }
            }
        }

        private void StartBattleQuiz()
        {
            questions = new QuestionsGenerator[questionCount];

            for (int i = 0; i < questionCount; i++)
            {
                QuestionsGenerator temp = new QuestionsGenerator();
                questions[i] = temp;
            }
            
            ui.SetCurrentQuestionList(questions, questionCount);
        }

        public void CheckAnswer(int playerAnswer, int realAnswer)
        {
            if (playerAnswer == realAnswer)
            {
                ui.GoToNextQuestion();

            }else
            {
                playerData.currentPlayerData.health--;
                ui.UpdateHealthUI();
                ui.GoToNextQuestion();
            }
        }

        public int QuestionCount()
        {
            return questionCount;
        }

        public void SetCurrentEnemy(GameObject enemy)
        {
            currentEnemy = enemy;
        }

        public void SetBattleMode(bool state)
        {
            battleUIon = state;
            
            if(battleUIon == true)
            {
                battleEnded = false;
                StartBattleQuiz();

            }else
            {
                battleEnded = true;
                GameManager.Instance.isBattling = false;
                GameManager.Instance.isInteracting = false;
                GameManager.Instance.lockPauseMenu = false;
                flashlight.DisableFlashLight(false);
            }
        }
    }

}
