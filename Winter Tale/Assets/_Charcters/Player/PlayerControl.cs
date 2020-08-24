using System.Collections;
using UnityEngine;
using RPG.CameraUI;

namespace RPG.Characters
{
    public class PlayerControl : MonoBehaviour
    {
        [SerializeField] bool isInDirectMovement = false;
        [SerializeField] GameObject deathButtons;

        SpecialAbilities abilities;
        HealthSystem healthSystem;
        WeaponSystem weaponSystem;
        Character character;

        bool paused = false;

        void Start()
        {
            abilities = GetComponent<SpecialAbilities>();
            healthSystem = GetComponent<HealthSystem>();
            character = GetComponent<Character>();
            weaponSystem = GetComponent<WeaponSystem>();

            SubscribeForMouseClickEvents();
        }

        private void SubscribeForMouseClickEvents()
        {
            CameraRaycaster cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
            cameraRaycaster.onMouseOverDestroyable += OnMouseOverDestroyable;
            if (!isInDirectMovement)
            {
                cameraRaycaster.onMouseOverTerrain += OnMouseOverPotWalkable;
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!paused)
                {
                    paused = true;
                    Time.timeScale = 0;
                    deathButtons.SetActive(true);
                }
                else
                {
                    paused = false;
                    Time.timeScale = 1;
                    deathButtons.SetActive(false);
                }
            }

            if(isInDirectMovement)
            {
                character.ProcessDirectMovement();
            }
            if(healthSystem.healthAsPercentage > Mathf.Epsilon)
            {
                ScanForAbilityKeyDown();
            }
            else
            {
                deathButtons.SetActive(true);
                Destroy(this);
            }
        }

        private void ScanForAbilityKeyDown()
        {
            for (int keyIndex = 1; keyIndex < abilities.GetNumberOfAbilities(); keyIndex++)
            {
                if (Input.GetKeyDown("[" + keyIndex + "]"))
                {
                    abilities.AttemptSpecialAbility(keyIndex);
                }
            }
        }
        void OnMouseOverPotWalkable(Vector3 destination)
        {
            if (Input.GetMouseButton(0) && healthSystem.healthAsPercentage >= Mathf.Epsilon)
            {
                character.SetDestination(destination);
                weaponSystem.StopAttacking();
            }
        }
        void OnMouseOverDestroyable(Destroyable destroyable)
        {
            var currTarget = destroyable.gameObject;
            if (Input.GetMouseButton(0) && IsTargetInRange(currTarget))
            {
                weaponSystem.AttackTarget(currTarget);
            }
            else if (Input.GetMouseButton(0) && !IsTargetInRange(currTarget))
            {
                StartCoroutine(MoveAndAttack(currTarget));
            }
        }
        void OnMouseOverEnemy(EnemyAI enemyToSet)
        {
            var currTarget = enemyToSet.gameObject;
            if(Input.GetMouseButton(0) && IsTargetInRange(currTarget))
            {
                weaponSystem.AttackTarget(currTarget);
            }
            else if(Input.GetMouseButton(0) && !IsTargetInRange(currTarget))
            {
                StartCoroutine(MoveAndAttack(currTarget));
            }
            else if(Input.GetMouseButtonDown(1) && IsTargetInRange(currTarget))
            {
                abilities.AttemptSpecialAbility(0, currTarget);
            }
            else if(Input.GetMouseButtonDown(1) && !IsTargetInRange(currTarget))
            {
                StartCoroutine(MoveAndPowerAttack(currTarget));
            }
        }
        IEnumerator MoveToTarget(GameObject target)
        {
            character.SetDestination(target.transform.position);
            while (!IsTargetInRange(target))
            {
                yield return null;
            }
        }
        IEnumerator MoveAndAttack(GameObject target)
        {
            yield return StartCoroutine(MoveToTarget(target));
            weaponSystem.AttackTarget(target);
        }

        IEnumerator MoveAndPowerAttack(GameObject target)
        {
            yield return StartCoroutine(MoveToTarget(target));
            abilities.AttemptSpecialAbility(0, target);
        }

        bool IsTargetInRange(GameObject target)
        {
            float distnceToTarget = (transform.position - target.transform.position).magnitude;
            if (weaponSystem.GetCurrWeapon() != null)
            {
                return distnceToTarget <= weaponSystem.GetCurrWeapon().MaxAttackRange;
            }
            else
            {
                return false;
            }
        }

        public void SetMovementType(bool movementType)
        {
            isInDirectMovement = movementType;
        }
    }
}