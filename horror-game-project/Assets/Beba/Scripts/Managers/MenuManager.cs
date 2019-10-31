using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace gameBeba
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private GameObject PausedMenu;
        [SerializeField] private GameObject optionMenu;
        [SerializeField] private GameObject playerMenu;

        private bool optionMenuShown;
        private GameObject subsActionCanvas;
        private GameObject player;
        private PlayerData playerData;
        private Inventory playerInventory;

        private void Start()
        {
            player = GameObject.Find("FPSController");
            playerData = player.GetComponent<PlayerData>();
            playerInventory = player.GetComponent<Inventory>();

            subsActionCanvas = GameObject.Find("SubsActionsCanvas");

            optionMenuShown = false;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && optionMenuShown == false && GameManager.Instance.lockPauseMenu == false)
            {
                GameManager.Instance.isPaused = !GameManager.Instance.isPaused;
                ShowPausedMenu(GameManager.Instance.isPaused);
            }

            if (Input.GetKeyDown(KeyCode.P) && GameManager.Instance.isPaused != true)
            {
                ShowPlayerMenu();
            }
        }

        public void ShowPausedMenu(bool state)
        {
            if (state == true)
            {
                FreezeAllState();
                ToggleOtherMenuForPause(true);
                PausedMenu.SetActive(true);
            }
            else if(state == false)
            {
                UnfreezeAllState();
                ToggleOtherMenuForPause(false);
                PausedMenu.SetActive(false);
                GameManager.Instance.isPaused = false;

                if (optionMenuShown == true)
                {
                    optionMenuShown = false;
                    optionMenu.SetActive(false);
                }
            }
        }

        private void ShowPlayerMenu()
        {
            FreezeAllState();
            playerInventory.RefreshInventory();
            playerMenu.SetActive(true);
            ToggleOtherMenuForPlayerMenu(true);
        }

        private void UnfreezeAllState()
        {
            Time.timeScale = 1;
            AudioListener.pause = false;
            playerData.EnablePlayer(true);
        }

        private void FreezeAllState()
        {
            Time.timeScale = 0;
            AudioListener.pause = true;
            playerData.EnablePlayer(false);
        }
        
        private void ToggleOtherMenuForPause(bool p)
        {
            if (p)
            {
                playerMenu.SetActive(false);
                subsActionCanvas.SetActive(false);
                
            }else
            {
                subsActionCanvas.SetActive(true);
            }
        }

        private void ToggleOtherMenuForPlayerMenu(bool state)
        {
            if (state)
            {
                subsActionCanvas.SetActive(false);
            }
            else
            {
                subsActionCanvas.SetActive(true);
            }
        }
        
        public void ShowOptionsMenu()
        {
            ShowPausedMenu(false);
            
            FreezeAllState();
            ToggleOtherMenuForPause(true);

            optionMenu.SetActive(true);
            optionMenuShown = true;
        }

        public void QuitGameButton()
        {
            Application.Quit();
        }

        public void ClosePlayerMenu()
        {
            UnfreezeAllState();
            playerMenu.SetActive(false);
            ToggleOtherMenuForPlayerMenu(false);
        }        
    }
}


