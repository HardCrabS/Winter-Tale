using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = "RPG/Weapon")]
    public class WeaponConfig : ScriptableObject
    {
        public Transform weaponGrip;

        [SerializeField] GameObject weaponPrefab;
        [SerializeField] AnimationClip attackAnimation;
        [SerializeField] float timeBetweenAnimations = 0.5f;
        [SerializeField] float maxAttackRange = 2f;
        [SerializeField] float additionalDamage = 10;
        [SerializeField] float damageDelay = 0.5f;

        public float DamageDelay { get { return damageDelay; } }
        public float TimeBetweenAnimations
        {
            get { return timeBetweenAnimations; }
        }

        public float MaxAttackRange
        {
            get { return maxAttackRange; }
        }

        public float AdditionalDamage { get { return additionalDamage; } }

        public GameObject WeaponPrefab { get { return weaponPrefab; } }
        public AnimationClip AttackAnimation
        {
            get
            {
                attackAnimation.events = new AnimationEvent[0]; // remove asset events
                return attackAnimation;
            }
        }
    }
}