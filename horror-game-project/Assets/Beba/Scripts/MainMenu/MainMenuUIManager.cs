using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace gameBeba
{
    /* to-do
     * 
     * create menu for options (disable head bob, sound control, brightness)
     * hints and tips while loading screen.
     * image background for loading screen. // low priority.
     * 
     */

    public class MainMenuUIManager : MonoBehaviour
    {
        [SerializeField] private GameObject mouseCursorObject;
        [SerializeField] private GameObject mainMenuCanvas;
        [SerializeField] private GameObject pressAnyKeyText;
        [SerializeField] private Button continueButton;

        private bool canUseMenu;

        private void Start()
        {
            canUseMenu = false;

            if (SaveLoad.SaveExist("GameState"))
            {
                continueButton.GetComponent<Button>().interactable = true;
            }
        }

        private void Update()
        {
            if (Input.anyKey && canUseMenu == true)
            {
                pressAnyKeyText.SetActive(false);
                mainMenuCanvas.SetActive(true);
                mouseCursorObject.SetActive(true);
            }
        }

        public void OnCameraMovementStop()
        {
            pressAnyKeyText.SetActive(true);
            canUseMenu = true;
        }
    }

}
