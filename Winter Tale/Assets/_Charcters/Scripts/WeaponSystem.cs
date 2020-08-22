using UnityEngine.Assertions;
using UnityEngine;
using System.Collections;
using System;

namespace RPG.Characters
{
    public class WeaponSystem : MonoBehaviour
    {
        [SerializeField] float baseDamage = 10;
        [SerializeField] WeaponConfig currWeaponConfig;
        [Range(0.1f, 1)] [SerializeField] float criticalHitChance = 0.5f;
        [SerializeField] float criticalHitMultiplier = 1.25f;
        [SerializeField] ParticleSystem critHitParticle;

        float lastHitTime = 0;

        GameObject currTarget;
        GameObject weaponObject;
        Animator animator;
        Character character;
        HealthSystem characterHS, targetHS;

        const string DEFAULT_ATTACK = "DEFAULT ATTACK";

        void Start()
        {
            animator = GetComponent<Animator>();
            character = GetComponent<Character>();
            characterHS = GetComponent<HealthSystem>();

            PutWeaponInHands(currWeaponConfig);
        }

        void Update()
        {
            bool targetIsDead, targetIsOutOfRange;

            if (currTarget == null)
            {
                targetIsDead = false;
                targetIsOutOfRange = false;
            }
            else
            {
                targetIsDead = targetHS.healthAsPercentage <= Mathf.Epsilon;
                targetIsOutOfRange = Vector3.Distance(transform.position, currTarget.transform.position) >= currWeaponConfig.MaxAttackRange;
            }

            bool characterIsDead = characterHS.healthAsPercentage <= Mathf.Epsilon;

            if (characterIsDead || targetIsDead || targetIsOutOfRange)
            {
                StopAllCoroutines();
            }
        }

        public void AttackTarget(GameObject targetToAttack)
        {
            if (currWeaponConfig != null)
            {
                SetAttackAnimation();
            }
            currTarget = targetToAttack;
            targetHS = currTarget.GetComponent<HealthSystem>();
            StartCoroutine(AttackTargetRepeatedly());
        }

        public void StopAttacking()
        {
            StopAllCoroutines();
        }
        public WeaponConfig GetCurrWeapon()
        {
            return currWeaponConfig;
        }
        private void SetAttackAnimation()
        {
            var animatorOverrideController = character.OverrideController;
            animator.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController[DEFAULT_ATTACK] = currWeaponConfig.AttackAnimation;
        }

        public void PutWeaponInHands(WeaponConfig weaponToUse)
        {
            Transform dominantHand = GetDominantHand();

            currWeaponConfig = weaponToUse;
            Destroy(weaponObject);
            if (weaponToUse != null && weaponToUse.WeaponPrefab != null)
            {
                weaponObject = Instantiate(weaponToUse.WeaponPrefab, dominantHand);
                weaponObject.transform.localPosition = weaponToUse.weaponGrip.position;
                weaponObject.transform.localRotation = weaponToUse.weaponGrip.rotation;
                weaponObject.transform.localScale = weaponToUse.weaponGrip.localScale;
            }
        }

        private Transform GetDominantHand()
        {
            var dominantHands = GetComponentsInChildren<DominantHand>();
            int numberOfDominantHands = dominantHands.Length;
            Assert.AreNotEqual(numberOfDominantHands, 0, "No DominantHand found on " + gameObject.name + ", please add one");
            Assert.IsFalse(numberOfDominantHands > 1, "Multiple DominantHand scripts on " + gameObject.name + ", please remove one");

            return dominantHands[0].transform;
        }

        IEnumerator AttackTargetRepeatedly()
        {
            bool attackerIsAlive = characterHS.healthAsPercentage >= Mathf.Epsilon;
            bool targetIsAlive = targetHS.healthAsPercentage >= Mathf.Epsilon;

            while (attackerIsAlive && targetIsAlive)
            {
                var animClip = currWeaponConfig.AttackAnimation;
                float animClipTime = animClip.length / character.AnimSpeedMultiplier;
                float timeToWait = animClipTime + currWeaponConfig.TimeBetweenAnimations;

                bool timeToHitAgain = Time.time - lastHitTime > timeToWait;
                if (timeToHitAgain)
                {
                    AttackTargetOnce();
                    lastHitTime = Time.time;
                }
                yield return new WaitForSeconds(timeToWait);
            }
        }

        private void AttackTargetOnce()
        {
            transform.LookAt(currTarget.transform);
            TriggerAttackAnimation();
            float damageDelay = currWeaponConfig.DamageDelay;
            StartCoroutine(DamageAfterDelay(damageDelay));
        }

        IEnumerator DamageAfterDelay(float delay)
        {
            yield return new WaitForSecondsRealtime(delay);
            targetHS.TakeDamage(CalculateDamage());
        }

        float CalculateDamage()
        {
            float damage = baseDamage + currWeaponConfig.AdditionalDamage;
            float currCritChance = UnityEngine.Random.Range(0f, 1.0f);

            if (currCritChance <= criticalHitChance)
            {
                damage *= criticalHitMultiplier;
                if (critHitParticle != null)
                    critHitParticle.Play();
            }

            return damage;
        }
        void TriggerAttackAnimation()
        {
            animator.SetTrigger("Attack");
        }
    }
}