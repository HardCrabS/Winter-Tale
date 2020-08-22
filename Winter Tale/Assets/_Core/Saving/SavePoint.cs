using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;

public class SavePoint : MonoBehaviour
{
    [SerializeField] Transform spawnPoint;
    [SerializeField] AudioClip audioClip;
    [SerializeField] GameObject saveFire;

    AudioSource audioSource;

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<PlayerControl>();
        if (player)
        {
            SaveData saveData = new SaveData();

            saveData.spawnPoint = new Position(spawnPoint.position.x, spawnPoint.position.y, spawnPoint.position.z);
            GameData.Instance.saveData = saveData;
            GameData.Instance.Save();

            PlayEffects();
            Destroy(this);
        }
    }

    void PlayEffects()
    {
        audioSource.PlayOneShot(audioClip);
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 0.7f, transform.position.z);
        Instantiate(saveFire, pos, saveFire.transform.rotation);
    }
}
