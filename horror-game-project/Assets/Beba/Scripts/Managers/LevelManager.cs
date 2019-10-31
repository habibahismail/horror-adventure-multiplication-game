using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace gameBeba
{

    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private GameObject loadingScreen;
        [SerializeField] private Slider loadingBar;

        private int levelToLoad;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                FadeToNextLevel();
            }
        }

        public void FadeToNextLevel()
        {
            if(SceneManager.sceneCountInBuildSettings <= 2)
            {
                FadeOut(SceneManager.GetActiveScene().buildIndex + 1);
            }else
            {
                Debug.Log("no more scene to load.");
            }
        }

        private void FadeOut(int sceneIndex)
        {
            levelToLoad = sceneIndex;
            animator.SetTrigger("FadeOut");
        }

        public void OnFadeComplete()
        {
            StartCoroutine(LoadAsyncronously());
        }

        private IEnumerator LoadAsyncronously()
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(levelToLoad);
            loadingScreen.SetActive(true);
            
            while (!operation.isDone)
            {
                float progress = Mathf.Clamp01(operation.progress / .9f);

                loadingBar.value = progress;
                yield return null;
            }

        }
    }

}
