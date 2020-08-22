using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Weapons;

namespace RPG.Characters
{
    [RequireComponent(typeof(HealthSystem))]
    [RequireComponent(typeof(Character))]
    [RequireComponent(typeof(WeaponSystem))]
    [SelectionBase]
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] Transform patrolPath;
        [SerializeField] float chaseRadius = 2f;
        [SerializeField] float waypointTolerance = 0.1f;
        [SerializeField] float timeToWaitAtWaypoint = 0.5f;

        float currWeaponRange;
        float distanceToPlayer;

        GameObject player;
        WeaponSystem weaponSystem;
        HealthSystem healthSystem;
        Character character;

        enum State { idle, chasing, attacking, patroling }
        State state = State.idle;

        void Start()
        {
            player = FindObjectOfType<PlayerControl>().gameObject;
            weaponSystem = GetComponent<WeaponSystem>();
            character = GetComponent<Character>();
            healthSystem = GetComponent<HealthSystem>();

            var overrideController = GetComponent<Character>().OverrideController;
            Animator animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = overrideController;

            if (weaponSystem.GetCurrWeapon() != null)
                currWeaponRange = weaponSystem.GetCurrWeapon().MaxAttackRange;
        }
        void Update()
        {
            if (healthSystem.healthAsPercentage <= Mathf.Epsilon)
            {
                StopAllCoroutines();
                return;
            }
            distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

            bool inAttackRange = distanceToPlayer <= currWeaponRange;
            bool inChaseRange = distanceToPlayer > currWeaponRange
                            && distanceToPlayer <= chaseRadius;
            bool outsideChaseRange = distanceToPlayer > chaseRadius;


            if (outsideChaseRange && state != State.patroling)
            {
                StopAllCoroutines();
                StartCoroutine(Patrol());
            }
            if (inChaseRange && state != State.chasing)
            {
                StopAllCoroutines();
                StartCoroutine(ChasePlayer());
            }
            if (inAttackRange && state != State.attacking)
            {
                StopAllCoroutines();
                Attack();
            }
        }

        void Attack()
        {
            state = State.attacking;
            weaponSystem.AttackTarget(player);
        }

        IEnumerator Patrol()
        {
            state = State.patroling;
            int nextWaypointIndex = 0;
            while (patrolPath != null)
            {
                Vector3 destinationWaypoint = patrolPath.GetChild(nextWaypointIndex).position;
                character.SetDestination(destinationWaypoint);
                nextWaypointIndex = GetNextWaypointIndex(destinationWaypoint, nextWaypointIndex);
                yield return new WaitForSeconds(timeToWaitAtWaypoint);
            }
        }

        int GetNextWaypointIndex(Vector3 nextWaypointPos, int currIndex)
        {
            int index = currIndex;
            if (Vector3.Distance(transform.position, nextWaypointPos) <= waypointTolerance)
            {
                index = (currIndex + 1) % patrolPath.childCount;
            }
            return index;
        }

        IEnumerator ChasePlayer()
        {
            state = State.chasing;
            while (distanceToPlayer >= currWeaponRange)
            {
                character.SetDestination(player.transform.position);
                yield return new WaitForEndOfFrame();
            }
        }

        public void MultiplyChaseRadius(float multiplier)
        {
            chaseRadius *= multiplier;
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, currWeaponRange);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseRadius);
        }
    }
}