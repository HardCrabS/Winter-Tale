using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public abstract class AbilityConfig : ScriptableObject
    {
        [Header("Special Ability General")]
        [SerializeField] string buttonName;
        [SerializeField] float energyCost = 10;
        [SerializeField] GameObject particle;
        [SerializeField] AnimationClip animClip;
        [SerializeField] AudioClip[] audioClips;

        protected AbilityBehaviour behaviour;
        public void AttachAbility(GameObject gameObjectToAttachTo)
        {
            var behaviourComponent = GetAbilityBehaviour(gameObjectToAttachTo);
            behaviourComponent.SetConfig(this);
            behaviour = behaviourComponent;
        }

        public abstract AbilityBehaviour GetAbilityBehaviour(GameObject gameObjectToAttachTo);
        public void Use(GameObject target)
        {
            behaviour.Use(target);
        }
        
        public string ButtonName { get { return buttonName; } }
        public float EnergyCost { get { return energyCost; } }
        public AnimationClip AbilityAnimation { get { return animClip; } }
        public GameObject Particle { get { return particle; } }
        public AudioClip RandomAudioAbilityClip
        {
            get 
            { 
                return audioClips.Length > 0? audioClips[Random.Range(0, audioClips.Length)] : null; 
            }
        }
    }
}