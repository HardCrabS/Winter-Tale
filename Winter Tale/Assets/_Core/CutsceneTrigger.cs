using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class CutsceneTrigger : MonoBehaviour
    {
        [SerializeField] GameObject timeline;
        [SerializeField] GameObject enterFence, exitFence;
        [SerializeField] MusicPlaylist musicPlaylist;

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<PlayerControl>())
            {
                StartCoroutine(musicPlaylist.FadeOutAtBossFight(2));
                timeline.SetActive(true);
                enterFence.SetActive(true);
                exitFence.SetActive(true);
                Destroy(GetComponent<BoxCollider>());
            }
        }
    }
}