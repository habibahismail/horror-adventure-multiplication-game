using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace gameBeba
{
    public class SceneTrigger : MonoBehaviour
    {
        [SerializeField] private GameObject sceneToTrigger;
        
        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Player")
            {
                sceneToTrigger.SetActive(true);
            }
        }
    }
}

