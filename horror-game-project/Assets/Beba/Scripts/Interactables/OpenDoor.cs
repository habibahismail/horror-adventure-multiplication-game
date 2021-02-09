using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace gameBeba
{
    [System.Serializable]
    public struct DoorLockedResponds
    {
        public string line;
        public float time;
    }

    public class OpenDoor : MonoBehaviour
    {
        public GameObject door01;
        public GameObject door02;
        public AudioSource openFX;
        public AudioSource lockedFX;
        public Door door;
        public DoorLockedResponds[] responds;
        
        private float distance;
        private string objectHitName;
        private bool isOpen;
        private bool isLocked;
        private bool isSolved;
        private bool noLock;
        private int index = 0;
        private int currentDoorIndex = 0;
        private string actionKeyText;
        private string actionKeyDesc;
        private string dialogResponseText;

        private List<CodesLists> doorCodes;
        private UIManager ui;


        private void Start()
        {
            doorCodes = GameManager.Instance.loadedDoorCodes;
            ui = GameObject.Find("UIManager").GetComponent<UIManager>();
            
            switch (door)
            {
                case Door.Door01: currentDoorIndex = 0; isLocked = true; break;
                case Door.Door02: currentDoorIndex = 1; isLocked = true; break;
                case Door.Door03: currentDoorIndex = 2; isLocked = true; break;
                case Door.NotLocked: isLocked = false; noLock = true; break;
                default: isLocked = false; noLock = true; break;
            }

            isSolved = IsDoorSolved(currentDoorIndex);

            if ( isSolved && !noLock)
            {
                isOpen = true;
                isLocked = false;

                this.GetComponent<BoxCollider>().enabled = false;

                Animation door1Animation = door01.GetComponent<Animation>();
                Animation door2Animation = door02.GetComponent<Animation>();

                door1Animation["doorOpenAnimation"].speed = 15;
                door2Animation["doorOpenAnim_another"].speed = 15;

                door1Animation.Play("doorOpenAnimation");
                door2Animation.Play("doorOpenAnim_another");
            }
            else
            {
                isOpen = false;
            }
        }

        private void FixedUpdate()
        {
            distance = PlayerRaycast.distanceFromTarget;
            objectHitName = PlayerRaycast.objectHit;
        }

        void OnMouseOver()
        {
            if (distance <= 2.5 && isOpen == false && objectHitName == gameObject.name)
            {
                actionKeyText = "[E]";
                actionKeyDesc = "_Open Door_";
                
            }else
            {
                actionKeyText = "";
                actionKeyDesc = "";
            }

            ui.SetActionKeyUI(actionKeyText, actionKeyDesc);

            if (Input.GetButtonDown("Action"))
            {
                
                if (distance <= 2.5)
                {
                    if (isOpen == false && isLocked == false)
                    {
                        actionKeyText = "";
                        actionKeyDesc = "";

                        isOpen = true;

                        this.GetComponent<BoxCollider>().enabled = false;
                        door01.GetComponent<Animation>().Play("doorOpenAnimation");
                        door02.GetComponent<Animation>().Play("doorOpenAnim_another");
                        openFX.Play();
                        
                        ui.SetActionKeyUI(actionKeyText, actionKeyDesc);
                    }
                    else
                    {
                        lockedFX.Play();
         
                        dialogResponseText = responds[index].line;
                        ui.SetDialogsSubsText(dialogResponseText);
                        Invoke("DialogResponseOff", responds[index].time);

                        Debug.Log("[OpenDoor] Index before = " + index);

                        if (index < responds.Length - 1)
                        {
                            index++;
                        }
                        else
                        {
                            index = 0;
                        }

                        Debug.Log("[OpenDoor] Index after = " + index);

                    }
                }
            }
        }

        void OnMouseExit()
        {
            actionKeyText = "";
            actionKeyDesc = "";

            ui.SetActionKeyUI(actionKeyText, actionKeyDesc);
        }

        void DialogResponseOff()
        {
            dialogResponseText= "";
            ui.SetDialogsSubsText(dialogResponseText);
        }

        bool IsDoorSolved(int index)
        {
            bool isSolved = doorCodes[index].isSolved;
            return isSolved;
        }

        public void SetDoorToUnlocked()
        {
            isLocked = false;
        }
        
    }


}
