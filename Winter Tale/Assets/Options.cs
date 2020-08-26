using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject optionsPanel;
    [SerializeField] GameObject checkmark1, checkmark2;
    [SerializeField] Slider volumeSlider;
    [SerializeField] AudioSource audioSource;

    bool paused = false;

    private void Start()
    {
        if (GameData.Instance.saveData.isDirectMovement)
        {
            checkmark1.SetActive(true);
            checkmark2.SetActive(false);
        }
        else
        {
            checkmark1.SetActive(false);
            checkmark2.SetActive(true);
        }
        volumeSlider.value = GameData.Instance.saveData.volume;
        volumeSlider.onValueChanged.AddListener(delegate { SetVolume(); });

        audioSource.volume = GameData.Instance.saveData.volume;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!paused)
            {
                paused = true;
                Time.timeScale = 0;
                optionsPanel.SetActive(false);
                pausePanel.SetActive(true);
            }
            else
            {
                ResumeGame();
            }
        }
    }
    public void ResumeGame()
    {
        paused = false;
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }
    public void SetVolume()
    {
        audioSource.volume = volumeSlider.value;
        GameData.Instance.saveData.volume = volumeSlider.value;
        GameData.Instance.Save();
    }
}
