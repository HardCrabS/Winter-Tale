using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public abstract class AbilityBehaviour : MonoBehaviour
    {
        protected AbilityConfig config;

        const int PARTICLE_DESTROY_DELAY = 10;
        const string DEFAULT_ATTACK = "DEFAULT ATTACK";
        const string ATTACK_TRIGGER = "Attack";

        AudioSource audioSource;
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public abstract void Use(GameObject target);

        public void SetConfig(AbilityConfig config)
        {
            this.config = config;
        }

        protected void SpawnParticle()
        {
            var particleObject = Instantiate(config.Particle, transform.position, config.Particle.transform.rotation);
            particleObject.transform.SetParent(transform);
            particleObject.GetComponent<ParticleSystem>().Play();
            StartCoroutine(DestroyParticleWhenFinished(particleObject));
        }

        IEnumerator DestroyParticleWhenFinished(GameObject particle)
        {
            var partSystem = particle.GetComponent<ParticleSystem>();
            while(partSystem.isPlaying)
            {
                yield return new WaitForSeconds(PARTICLE_DESTROY_DELAY); 
            }
            Destroy(particle);
        }

        protected void PlayAbilityAnimation()
        {
            var overrideController = GetComponent<Character>().OverrideController;
            Animator animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = overrideController;
            overrideController[DEFAULT_ATTACK] = config.AbilityAnimation;
            animator.SetTrigger(ATTACK_TRIGGER);
        }

        protected void PlayAbilitySound()
        {
            var audioClip = config.RandomAudioAbilityClip;
            audioSource.PlayOneShot(audioClip);
        }
    }
}