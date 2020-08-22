using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Characters
{
    public class HealthSystem : MonoBehaviour
    {
        [SerializeField] float maxHealthPoints = 100f;
        [SerializeField] Slider healthSlider;
        [SerializeField] AudioClip[] hitSounds;
        [SerializeField] AudioClip[] deathSounds;
        [SerializeField] float deathVanishSeconds = 3;

        const string DEATH_TRIGGER = "Death";

        float currentHealthPoints;

        Animator animator;
        AudioSource audioSource;
        Character character;

        public float healthAsPercentage
        {
            get { return currentHealthPoints / maxHealthPoints; }
        }

        void Start()
        {
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
            character = GetComponent<Character>();

            currentHealthPoints = maxHealthPoints;
        }

        void Update()
        {
            UpdateHealthBar();
        }

        void UpdateHealthBar()
        {
            if(healthSlider)
            {
                healthSlider.value = healthAsPercentage;
            }
        }

        public void TakeDamage(float damage)
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0, maxHealthPoints);
            int randIndex = UnityEngine.Random.Range(0, hitSounds.Length);
            audioSource.PlayOneShot(hitSounds[randIndex]);
            if (currentHealthPoints == 0)
            {
                StartCoroutine(KillCharacter());
            }
        }

        IEnumerator KillCharacter()
        {
            if (character != null)
            {
                character.Kill();
                animator.SetTrigger(DEATH_TRIGGER);
            }

            audioSource.clip = deathSounds[UnityEngine.Random.Range(0, deathSounds.Length)];
            audioSource.Play();

            Destroy(healthSlider.gameObject);
            yield return new WaitForSeconds(audioSource.clip.length);

            var playerComponent = GetComponent<PlayerControl>();
            if(playerComponent)
            {
                //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            else
            {
                Destroy(gameObject, deathVanishSeconds);
            }
        }

        public void AddToHealth(float amount)
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints + amount, 0, maxHealthPoints);
        }
    }
}