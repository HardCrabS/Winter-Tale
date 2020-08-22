using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class AreaEffectBehaviour : AbilityBehaviour
    {
        public override void Use(GameObject target)
        {
            var allHits = Physics.SphereCastAll(transform.position, (config as AreaEffectConfig).Radius, Vector3.up);
            foreach (var hit in allHits)
            {
                var enemyComponent = hit.collider.GetComponent<EnemyAI>();
                if (enemyComponent != null)
                {
                    enemyComponent.GetComponent<HealthSystem>().TakeDamage((config as AreaEffectConfig).DamageToEachTarget);
                }
            }
            PlayAbilityAnimation();
            PlayAbilitySound();
            SpawnParticle();
        }
    }
}