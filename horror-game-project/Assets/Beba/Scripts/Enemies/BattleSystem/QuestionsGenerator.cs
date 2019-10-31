using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace gameBeba
{
    public class QuestionsGenerator
    {
        public string question;
        public int answer;
        public List<int> answerChoice = new List<int>();

        private int wrongAnswerCount = 2;

        public QuestionsGenerator()
        {
            GenerateQuestion();
            GenerateAnswerChoices(wrongAnswerCount);
        }
        
        private void GenerateQuestion()
        {
            int no1 = Random.Range(1,10);
            int no2 = Random.Range(1,10);

            answer = no1 * no2;
            question = no1 + " x " + no2;
            
        }

        private void GenerateAnswerChoices(int count)
        {
            answerChoice.Add(answer);

            for (int i = 0; i < count; i++)
            {
                GenerateRandomWrongAnswer();
            }

            ShuffleAnswerChoicesList();
        }

        private void GenerateRandomWrongAnswer()
        {
            int answerTemp;
            bool noStop = true;
            
            while (noStop)
            {
                answerTemp = Random.Range(answer - 4, answer + 5);

                if(answerTemp < 0) { answerTemp = 0;  }
              
                if (!answerChoice.Contains(answerTemp))
                {
                    answerChoice.Add(answerTemp);
                    noStop = false;
                }
            }            
        }
        
        private void ShuffleAnswerChoicesList()
        {
            for (int i = 0; i < answerChoice.Count; i++)
            {
                int temp = answerChoice[i];
                int randomIndex = Random.Range(i, answerChoice.Count);
                answerChoice[i] = answerChoice[randomIndex];
                answerChoice[randomIndex] = temp;
            }
        }
        
    }

}
