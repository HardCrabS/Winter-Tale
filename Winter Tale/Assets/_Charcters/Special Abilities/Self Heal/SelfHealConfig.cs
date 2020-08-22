using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = "RPG/Special Ability/Self Heal")]
    public class SelfHealConfig : AbilityConfig
    {
        [Header("Self Heal Specific")]
        [SerializeField] float extraHealth = 50;
        public override AbilityBehaviour GetAbilityBehaviour(GameObject gameObjectToAttachTo)
        {
            return gameObjectToAttachTo.AddComponent<SelfHealBehaviour>();
        }
        public float ExtraHealth { get { return extraHealth; } }
    }
}