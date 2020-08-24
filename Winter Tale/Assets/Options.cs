using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    [SerializeField] GameObject checkmark1, checkmark2;
    [SerializeField] Slider volumeSlider;
    [SerializeField] AudioSource audioSource;

    private void Start()
    {
        if (checkmark1 != null)
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
        }
        audioSource.volume = GameData.Instance.saveData.volume;
    }
    public void SetVolume()
    {
        audioSource.volume = volumeSlider.value;
        GameData.Instance.saveData.volume = volumeSlider.value;
        GameData.Instance.Save();
    }
}
