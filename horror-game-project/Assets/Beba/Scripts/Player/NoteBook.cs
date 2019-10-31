using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace gameBeba
{

    public class NoteBook : MonoBehaviour
    {
        [SerializeField] private GameObject noteBookCanvas;
        [SerializeField] private Book noteBook;
        [SerializeField] private GameObject nextButton;
        [SerializeField] private GameObject previousButton;

        [SerializeField] private AudioClip[] pageFlipSound;

        private PlayerData player;
        private UIManager ui;
        private AudioSource audioSource;

        private void Start()
        {
            player = GetComponent<PlayerData>();
            ui = GameObject.Find("UIManager").GetComponent<UIManager>();
            audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.N))
            {
                noteBookCanvas.SetActive(true);
                player.EnablePlayer(false);
                GameManager.Instance.lockPauseMenu = true;
                GameManager.Instance.isInteracting = true;
                ui.DisableSubsDialogMenu(true);
            }

            if(noteBook.currentPage == 0)
            {
                previousButton.SetActive(false);
            }else
            {
                previousButton.SetActive(true);
            }

            if(noteBook.currentPage > noteBook.TotalPageCount-1)
            {
                nextButton.SetActive(false);
            }
            else
            {
                nextButton.SetActive(true);
            }
        }

        public void CloseNoteBook()
        {
            noteBookCanvas.SetActive(false);
            player.EnablePlayer(true);
            GameManager.Instance.lockPauseMenu = false;
            GameManager.Instance.isInteracting = false;
            ui.DisableSubsDialogMenu(false);
        }

        public void PlayFlippingSound()
        {
            int index = Random.Range(0, 3);

            switch (index)
            {
                case 0: audioSource.PlayOneShot(pageFlipSound[0]); break;
                case 1: audioSource.PlayOneShot(pageFlipSound[1]); break;
                case 2: audioSource.PlayOneShot(pageFlipSound[2]); break;
            }
           
        }

    }

}
