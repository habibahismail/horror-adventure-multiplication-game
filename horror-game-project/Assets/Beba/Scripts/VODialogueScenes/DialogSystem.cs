using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace gameBeba {

    public class DialogSystem: MonoBehaviour
    {
        public string[] sentences;
        public float typingSpeed;

        private int index;
        private bool isShowing;

        private PlayerData player;
        private UIManager ui;

        private void Start()
        {
            player = GameObject.Find("FPSController").GetComponent<PlayerData>();
            ui = GameObject.Find("UIManager").GetComponent<UIManager>();

            index = 0;
            isShowing = false;
        }

        private void Update()
        {
            if (isShowing)
            {
                string currentLine = ui.GetCurrentDialogLine();

                if (currentLine == sentences[index])
                {
                    string nextText = "[next]";
                    ui.SetContinueButtonText(nextText);
                }
            }
        }

        IEnumerator Type()
        {
            foreach(char letter in sentences[index].ToCharArray())
            {
                ui.TypeDialogByLetter(letter);
                yield return new WaitForSeconds(typingSpeed);
            }
        }

        private void StopDialog()
        {
            player.EnablePlayer(true);
            StopAllCoroutines();

            string tempText = "";
            ui.SetDialogsSubsText(tempText);

            GameManager.Instance.isInteracting = false;
            isShowing = false;
            index = 0;
        }
        
        private void ClearDialog()
        {
            string tempText = "";
            ui.SetDialogsSubsText(tempText);
        }

        private void ShowNextSentence()
        {
            index++;
            ClearDialog();
            StopAllCoroutines();
            StartCoroutine(Type());
        }

        public void StartDialog(string[] lines)
        {
            player.EnablePlayer(false);
            sentences = lines;
            isShowing = true;

            ClearDialog();
            StartCoroutine(Type());
        }

        public void NextSentence()
        {
            string nextText = "";
            ui.SetContinueButtonText(nextText);

            if (index < sentences.Length - 1)
            {
                ShowNextSentence();
            }
            else
            {
                StopDialog();
            }
        }
    }
}
