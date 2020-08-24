using System;
using UnityEngine;
using UnityEngine.AI;
using RPG.CameraUI;
using System.Collections.Generic;
using System.Collections;

namespace RPG.Characters
{
    [SelectionBase]
    public class Character : MonoBehaviour
    {
        [Header("Animator")]
        [SerializeField] RuntimeAnimatorController animatorController;
        [SerializeField] AnimatorOverrideController overrideController;
        [SerializeField] Avatar avatar;

        [Header("Collider")]
        [SerializeField] Vector3 colliderCenter;
        [SerializeField] float colliderRadius;
        [SerializeField] float colliderHeight;

        [Header("NavMeshAgent")]
        [SerializeField] float agentSpeed = 4;
        [SerializeField] float agentAngularSpeed = 120;
        [SerializeField] float agentAcceleration = 15;
        [SerializeField] float agentRadius = 0.1f;
        [SerializeField] float agentHeight = 0.87f;

        [Header("Movement Properties")]
        [SerializeField] float stoppingDistance = 1;
        [SerializeField] float moveSpeedMultiplier = 0.7f;
        [SerializeField] float animationSpeed = 1;
        [SerializeField] float movingTurnSpeed = 360;
        [SerializeField] float stationaryTurnSpeed = 180;
        [Range(0, 1)] [SerializeField] float animatorForwardCap = 1;

        float turnAmount;
        float forwardAmount;
        bool isAlive = true;

        NavMeshAgent agent;
        Animator animator;
        Rigidbody myRigidbody;

        public AnimatorOverrideController OverrideController { get { return overrideController; } }
        public float AnimSpeedMultiplier { get { return animator.speed; } }
        protected void Awake()
        {
            AddRequiredComponents();
        }

        void AddRequiredComponents()
        {
            animator = gameObject.AddComponent<Animator>();
            if (animator != null)
            {
                animator.runtimeAnimatorController = overrideController;
                animator.avatar = avatar;
            }
            else
            {
                animator = GetComponent<Animator>();
            }

            myRigidbody = gameObject.AddComponent<Rigidbody>();
            myRigidbody.constraints = RigidbodyConstraints.FreezeRotation;

            var myCollider = gameObject.AddComponent<CapsuleCollider>();
            myCollider.center = colliderCenter;
            myCollider.radius = colliderRadius;
            myCollider.height = colliderHeight;

            gameObject.AddComponent<AudioSource>();

            agent = gameObject.AddComponent<NavMeshAgent>();
            agent.speed = agentSpeed;
            agent.angularSpeed = agentAngularSpeed;
            agent.acceleration = agentAcceleration;
            agent.stoppingDistance = stoppingDistance;
            agent.radius = agentRadius;
            agent.height = agentHeight;

            agent.updatePosition = true;
            agent.updateRotation = false;
        }

        protected void Update()
        {
            if (agent.remainingDistance > stoppingDistance && isAlive)
            {
                Move(agent.desiredVelocity);
            }
            else
            {
                Move(Vector3.zero);
            }
        }

        protected void OnAnimatorMove()
        {
            if (Time.deltaTime > 0)
            {
                Vector3 velocity = (animator.deltaPosition * moveSpeedMultiplier) / Time.deltaTime;

                velocity.y = myRigidbody.velocity.y;
                myRigidbody.velocity = velocity;
            }
        }

        void Move(Vector3 move)
        {
            SetForwardAndTurn(move);

            ApplyExtraTurnRotation();
            UpdateAnimator();
        }
        public void SetDestination(Vector3 worldPos)
        {
            agent.SetDestination(worldPos);
        }

        private void SetForwardAndTurn(Vector3 move)
        {
            // convert the world relative moveInput vector into a local-relative
            // turn amount and forward amount required to head in the desired
            // direction.
            if (move.magnitude > 1f) move.Normalize();
            var localMove = transform.InverseTransformDirection(move);
            turnAmount = Mathf.Atan2(localMove.x, localMove.z);
            forwardAmount = localMove.z;
        }

        void UpdateAnimator()
        {
            animator.SetFloat("Forward", forwardAmount * animatorForwardCap, 0.1f, Time.deltaTime);
            animator.SetFloat("Turn", turnAmount, 0.1f, Time.deltaTime);
            animator.speed = animationSpeed;
        }

        void ApplyExtraTurnRotation()
        {
            // help the character turn faster (this is in addition to root rotation in the animation)
            float turnSpeed = Mathf.Lerp(stationaryTurnSpeed, movingTurnSpeed, forwardAmount);
            transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0);
        }

        public virtual void Kill()
        {
            isAlive = false;
        }

        //Isnt using it for now
        public void ProcessDirectMovement()
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            Vector3 m_CamForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 m_Move = v * m_CamForward + h * Camera.main.transform.right;

            //Move(m_Move * agentSpeed * moveSpeedMultiplier);
            SetDestination(transform.position + m_Move * 3);
        }
    }
}