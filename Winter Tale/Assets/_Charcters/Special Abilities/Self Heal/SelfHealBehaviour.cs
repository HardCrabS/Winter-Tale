using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class SelfHealBehaviour : AbilityBehaviour
    {
        public override void Use(GameObject target)
        {
            var healthSystem = GetComponent<HealthSystem>();
            healthSystem.AddToHealth((config as SelfHealConfig).ExtraHealth);
            PlayAbilityAnimation();
            PlayAbilitySound();
            SpawnParticle();
        } 
    }
}