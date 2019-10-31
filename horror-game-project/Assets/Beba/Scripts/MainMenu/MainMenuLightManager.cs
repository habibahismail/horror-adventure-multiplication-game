using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace gameBeba
{

    public class MainMenuLightManager : MonoBehaviour
    {
        [SerializeField] private Light flickeringLight01;
        [SerializeField] private Light flickeringLight02;
        [SerializeField] private Light flickeringLight03;

        [SerializeField] private GameObject HMpos01;
        [SerializeField] private GameObject HMpos02;
        [SerializeField] private GameObject HMpos03;

        private float timer = 5.0f;
        private float defaultIntensity = 8.0f;
        private Light currentLight;
        private GameObject currentHMpos;

        private void Start()
        {
            currentHMpos = new GameObject();
            currentLight = flickeringLight01;
            StartCoroutine(LightFlicker(currentLight));
        }


        IEnumerator LightFlicker(Light light)
        {
            float waitTime = Random.Range(3,38);

            Debug.Log("wait time: " + waitTime);
            yield return new WaitForSeconds(waitTime);

            light.GetComponent<FlickeringLightEffect>().enabled = true;
            currentHMpos.SetActive(true);
            Debug.Log("light flicker: " + light.GetComponent<FlickeringLightEffect>().enabled);

            waitTime = Random.Range(3, 8);
            Debug.Log("wait time: " + waitTime);
            yield return new WaitForSeconds(waitTime);

            StopAllCoroutines();
            Debug.Log("stopall");
            light.intensity = defaultIntensity;

            light.GetComponent<FlickeringLightEffect>().enabled = false;
            currentHMpos.SetActive(false);
            Debug.Log("light flicker: " + light.GetComponent<FlickeringLightEffect>().enabled);
            
            currentLight = ChangeRandomLight();
            StartCoroutine(LightFlicker(currentLight));
            
        }

        private Light ChangeRandomLight()
        {
            int randomLightIndex;
            Light chosenLight = null;

            randomLightIndex = Random.Range(0, 3);

            Debug.Log("randomLightIndex: " + randomLightIndex);

            switch (randomLightIndex)
            {
                case 0: chosenLight = flickeringLight01; currentHMpos = HMpos01; Debug.Log(chosenLight); break;
                case 1: chosenLight = flickeringLight02; currentHMpos = HMpos02; Debug.Log(chosenLight); break;
                case 2: chosenLight = flickeringLight03; currentHMpos = HMpos03; Debug.Log(chosenLight); break;
            }

            return chosenLight;
        }
    }

}
