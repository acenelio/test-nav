using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NavGame.Core;

namespace NavGame.Core
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class LocomotionAnimator : MonoBehaviour
    {
        public float smoothness = 0.1f;

        protected NavMeshAgent agent;
        protected Animator animator;

        protected virtual void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponentInChildren<Animator>();
        }

        protected virtual void Update()
        {
            float speedPercent = agent.velocity.magnitude / agent.speed;
            animator.SetFloat("speedPercent", speedPercent, smoothness, Time.deltaTime);
        }
    }
}
