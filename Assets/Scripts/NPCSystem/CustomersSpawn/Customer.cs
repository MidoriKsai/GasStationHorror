using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Customer : MonoBehaviour
{
    private CustomerData _customerData;
    private NavMeshAgent _agent;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();

        if (_agent == null)
        {
            Debug.Log("No NavMeshAgent found");
        }
    }

    public void Init(CustomerData customerData)
    {
        _customerData = customerData;
        Debug.Log(_customerData.customerId);
    }

    public void StartPath(AgentPath path, System.Action onCompleted)
    {
        StopAllCoroutines();
        path.Reset();
        StartCoroutine(FollowPath(path, onCompleted));
    }

    private IEnumerator FollowPath(AgentPath path, System.Action onCompleted)
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