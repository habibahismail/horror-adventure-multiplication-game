using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

namespace gameBeba
{
    [System.Serializable]
    public struct DialogLines
    {
        public string line;
        public float waitTime;
    }

    public class SceneWithSubtitle : MonoBehaviour
    {
        public Text dialogPlaceholder;
        public float typingSpeed;
        public bool lockPlayerMovement;
        public DialogLines[] lines;

        private AudioSource sceneAudio;
        private int index;
        FirstPersonController player;
        

        private void Start()
        {
            if (lockPlayerMovement) {
                player = GameObject.Find("FPSController").GetComponent<FirstPersonController>();
                player.enabled = false;
            }
            
            sceneAudio = GetComponent<AudioSource>();
            dialogPlaceholder.text = "";
            sceneAudio.Play();
            StartCoroutine(DisplaySubs());
        }

        
        IEnumerator Type()
        {
            foreach (char letter in lines[index].line )
            {
                dialogPlaceholder.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }
        }

        IEnumerator DisplaySubs()
        {
           
            if(index < lines.Length)
            {
                dialogPlaceholder.text = "";
                StartCoroutine(Type());
                yield return new WaitForSeconds(lines[index].waitTime);
                StopAllCoroutines();
                index++;
                StartCoroutine(DisplaySubs());
                
            }
            else
            {
                dialogPlaceholder.text = "";
              
                if (lockPlayerMovement)
                {
                    Invoke("EnableController", 1);
                }
                
            }
            
        }

        private void EnableController()
        {
            player.enabled = true;
        }

    }
}

