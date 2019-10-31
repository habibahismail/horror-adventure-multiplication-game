using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace gameBeba
{
   
    public class FlashLight : MonoBehaviour
    {
        [SerializeField] private Light flashLight;
        [SerializeField] private AudioSource onFX;
        [SerializeField] private AudioSource offFX;
        [SerializeField] private float followSpeed;
        [SerializeField] private Item flashLightItem;
        [SerializeField] private Item batteryItem;

        private Vector3 offset;
        private GameObject cameratoFollow;
        private Vector3 playerPosition;
        private Inventory playerInventory;

        private bool isOn;
        private bool canUse;

        private void Start()
        {
            isOn = false;
            canUse = false;

            cameratoFollow = Camera.main.gameObject;
            playerPosition = GameObject.Find("FPSController").GetComponent<PlayerData>().PlayerPosition();
            playerInventory = GameObject.Find("FPSController").GetComponent<Inventory>();

            transform.position = playerPosition;
            offset = transform.position - cameratoFollow.transform.position;
 
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (canUse == false)
                {
                    if (playerInventory.ItemList.Contains(flashLightItem))
                    {
                        if (playerInventory.ItemList.Contains(batteryItem))
                        {
                            canUse = true;
                            
                            string text = "It's good that this old flashlight is actually working. \n\n\n\nPress 'F' button to switch it on and off.";
                            playerInventory.UpdateItemDescription(flashLightItem, text);

                        }
                    }
                }

       
                if(canUse == true) {

                    if (isOn == false)
                    {
                        flashLight.enabled = true;
                        isOn = true;
                        onFX.Play();
                    }
                    else if (isOn == true)
                    {
                        flashLight.enabled = false;
                        isOn = false;
                        offFX.Play();
                    }
                }
                
            }

                transform.position = cameratoFollow.transform.position + offset;
                transform.rotation = Quaternion.Slerp(transform.rotation, cameratoFollow.transform.rotation, followSpeed * Time.deltaTime);
            
        }

        public void DisableFlashLight(bool state)
        {
            if (state)
            {
                flashLight.enabled = false;
            }
            else
            {
                if (canUse == true && isOn == true) {

                    flashLight.enabled = true;
                }
            }
        }
    }

}
