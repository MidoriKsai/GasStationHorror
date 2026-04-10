using System;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

namespace NPCSystem
{
    public class Car : MonoBehaviour
    {
        [SerializeField]
        private Transform customerSpawnPoint;

        private CustomerData _customerData;
        private NavMeshAgent _agent;
        private Coroutine _moveRoutine;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();

            if (_agent == null)
                Debug.LogError("No NavMeshAgent found", this);
        }

        public void Init(CustomerData customerData)
        {
            _customerData = customerData;
        }

        public Transform GetCustomerSpawnPoint() => customerSpawnPoint;

        public void StartPath(AgentPath path, Action onCompleted)
        {
            if (_moveRoutine != null)
                StopCoroutine(_moveRoutine);

            path.Reset();
            _moveRoutine = StartCoroutine(FollowPath(path, onCompleted));
        }

        private IEnumerator FollowPath(AgentPath path, Action onCompleted)
        {
            _agent.isStopped = false;

            while (path.HasNext())
            {
                Vector3 nextPoint = path.GetNextPosition();
                _agent.SetDestination(nextPoint);

                while (_agent.pathPending)
                    yield return null;

                while (_agent.remainingDistance > _agent.stoppingDistance || _agent.velocity.sqrMagnitude > 0.01f)
                    yield return null;
            }

            onCompleted?.Invoke();
        }
    }
}