using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

namespace gameBeba
{
    public enum Door { Door01, Door02, Door03, NotLocked }

    public class KeypadLockSystem : MonoBehaviour
    {
        public Text actionKeyText;
        public Text actionKeyDesc;
        public Text tryAgainText;

        public Camera[] cameras;
        public PostProcessProfile postProcessProfile;
        public OpenDoor doorTrigger;
        public AudioSource passcodeErrorFX;
        public AudioSource doorUnlockedFX;
        public Door door;
        public string doorCode;
      
        [HideInInspector]
        public int maxCodeLength;
        [HideInInspector]
        public int currentCodeLength;

        private int currentDoorIndex = 0;
        private bool doorStillLocked;
        private bool keyPadView;
        private float distance;

        private List<CodesLists> doorCodes;
        private PlayerData player;
        private DepthOfField dof;

        private AudioListener cameraOneAudioLis;
        private AudioListener cameraTwoAudioLis;

        private void Awake()
        {
            doorCodes = GameManager.Instance.loadedDoorCodes;

            switch (door)
            {
                case Door.Door01: doorCode = doorCodes[0].theCode; currentDoorIndex = 0; break;
                case Door.Door02: doorCode = doorCodes[1].theCode; currentDoorIndex = 1; break;
                case Door.Door03: doorCode = doorCodes[2].theCode; currentDoorIndex = 2; break;
                default:  break;
            }

            if (doorCodes[currentDoorIndex].isSolved)
            {
                doorStillLocked = false;
            }else
            {
                doorStillLocked = true;
            }
        }

        private void Start()
        {
            keyPadView = false;
            tryAgainText.raycastTarget = false;
            dof = postProcessProfile.GetSetting<DepthOfField>();
            player = GameObject.Find("FPSController").GetComponent<PlayerData>();

            cameraOneAudioLis = cameras[0].GetComponent<AudioListener>();
            cameraTwoAudioLis = cameras[1].GetComponent<AudioListener>();

            maxCodeLength = GameManager.Instance.codeLength;
            currentCodeLength = 0;
        }

        void Update()
        {
            distance = PlayerRaycast.distanceFromTarget;
        }

        private void OnMouseOver()
        {
            if (distance <= 1.8f && doorStillLocked == true && keyPadView == false)
            {
                actionKeyText.text = "[E]";
                actionKeyDesc.text = "_Access the Keypad Lock Panel_";

                if (Input.GetButtonDown("Action"))
                {
                    KeypadView();
                }
            }
            else
            {
                actionKeyText.text = "";
                actionKeyDesc.text = "";
            }
        }

        void OnMouseExit()
        {
            actionKeyText.text = "";
            actionKeyDesc.text = "";
        }

        void KeypadView()
        {

            actionKeyText.text = "";
            actionKeyDesc.text = "";

            tryAgainText.raycastTarget = true;
            tryAgainText.text = "[x] try again later.";

            dof.enabled.value = false;

            cameras[0].enabled = false;
            cameras[1].enabled = true;

            cameraOneAudioLis.enabled = false;
            cameraTwoAudioLis.enabled = true;
            
            player.EnablePlayer(false);

            this.GetComponent<BoxCollider>().enabled = false;

            GameManager.Instance.lockPauseMenu = true;
            keyPadView = true;

        }

        public void SetDoorStillLocktoFalse()
        {
            doorStillLocked = false;
            doorTrigger.SetDoorToUnlocked();
            GameManager.Instance.loadedDoorCodes[currentDoorIndex].isSolved = true;
            doorUnlockedFX.Play();
            Invoke("ExitKeypadMode", 1.5f);
        }

        public void ExitKeypadMode()
        {
            tryAgainText.text = "";
            tryAgainText.raycastTarget = false;

            dof.enabled.value = true;

            cameras[0].enabled = true;
            cameras[1].enabled = false;

            cameraOneAudioLis.enabled = true;
            cameraTwoAudioLis.enabled = false;
            
            player.EnablePlayer(true);

            this.GetComponent<BoxCollider>().enabled = true;

            GameManager.Instance.lockPauseMenu = false;
            keyPadView = false;
        }

        public bool ReturnDoorLockStatus()
        {
            return doorCodes[currentDoorIndex].isSolved;
        }

    }

}

