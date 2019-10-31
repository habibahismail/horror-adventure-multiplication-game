using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace gameBeba
{
    public class KeypadButtonPress : MonoBehaviour
    {
        public Text codeDisplay;
        public int numberPressed;
        public KeypadLockSystem lockSystem;
        AudioSource soundFX;
        
        private string theCode;
        private bool canEnterInput;
        private bool canCheckPasscode;

        private void Start()
        {
            if (lockSystem.ReturnDoorLockStatus()) {

                codeDisplay.text = "Door Unlocked.";

            }
            else
            {
                soundFX = GetComponent<AudioSource>();
                theCode = lockSystem.doorCode;
                canCheckPasscode = false;

                Invoke("ClearCodeDisplayText", 5.0f);

            }

        }
        


        public void OnMouseOver()
        {
            if (Input.GetMouseButtonDown(0) && canEnterInput == true)
            {
                if ( lockSystem.currentCodeLength < lockSystem.maxCodeLength )
                {
                    this.GetComponent<Animation>().Play("pushButton");
                    soundFX.Play();

                    lockSystem.currentCodeLength++;
                    codeDisplay.text = codeDisplay.text + numberPressed;
                    
                    if(lockSystem.currentCodeLength == lockSystem.maxCodeLength)
                    {
                        canEnterInput = false;
                        canCheckPasscode = true;
                        
                    }
                    
                }
                

                if(canCheckPasscode) {

                    if (CodeChecker(codeDisplay.text))
                    {
                        codeDisplay.text = "Door Unlocked.";
                        lockSystem.SetDoorStillLocktoFalse();

                    }
                    else
                    {
                        codeDisplay.text = "Wrong Passcode.";
                        lockSystem.passcodeErrorFX.Play();
                        canCheckPasscode = false;
                        Invoke("ClearCodeDisplayText", 1.5f);

                    }

                }


            }   
        }

        bool CodeChecker(string currentCode)
        {
           
            if (currentCode == theCode  ){

                return true;
            }
            else
            {
                return false;
            }
        }

        void ClearCodeDisplayText()
        {
            codeDisplay.text = "";
            canEnterInput = true;
            lockSystem.currentCodeLength = 0;

        }

    }
}


