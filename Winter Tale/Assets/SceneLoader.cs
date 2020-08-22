using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] GameObject loadPanel;
    [SerializeField] Slider loadSlider;
    [SerializeField] Button loadGameButton;

    private void Start()
    {
        if(loadGameButton != null)
        {
            var pos = GameData.Instance.saveData.spawnPoint;
            if(pos.x == 0 && pos.y == 0)
            {
                loadGameButton.interactable = false;
            }
        }
    }

    public void StartNewGame()
    {
        GameData.Instance.EraseGameData();
        StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void LoadGame()
    {
        StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void ReloadScene()
    {
        StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex));
    }

    IEnumerator LoadScene(int sceneIndex)
    {
        loadPanel.SetActive(true);
        yield return null;

        AsyncOperation asyncOperation 
            = SceneManager.LoadSceneAsync(sceneIndex);

        while (!asyncOperation.isDone)
        {
            loadSlider.value = asyncOperation.progress;
            if (asyncOperation.progress >= 0.9f)
            {
    
            }

            yield return null;
        }
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
