using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = "RPG/Special Ability/Power Attack")]
    public class PowerAttackConfig : AbilityConfig
    {
        [Header("Power Attack Specific")]
        [SerializeField] float damageToEachTarget = 10;
        public override AbilityBehaviour GetAbilityBehaviour(GameObject gameObjectToAttachTo)
        {
            return gameObjectToAttachTo.AddComponent<PowerAttackBehaviour>();
        }

        public float ExtraDamage { get { return damageToEachTarget; } }
    }
}