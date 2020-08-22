using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoorTrigger : MonoBehaviour
{
    [SerializeField] SceneLoader sceneLoader;
    [SerializeField] GameObject creditsTimeline;
    [SerializeField] GameObject gameCanvas;
    [SerializeField] GameObject creditsCanvas;
    [SerializeField] GameObject campfireFire;

    IEnumerator CheckForInput()
    {
        while(true)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                sceneLoader.LoadMainMenu();
            }
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<RPG.Characters.PlayerControl>())
        {
            creditsTimeline.SetActive(true);
            creditsCanvas.SetActive(true);
            gameCanvas.SetActive(false);
            campfireFire.SetActive(true);
            StartCoroutine(CheckForInput());
        }
    }
}
