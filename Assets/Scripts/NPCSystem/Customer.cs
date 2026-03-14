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
    
    public void StartPath(CustomerPath path)
    {
        Debug.Log("StartPath started");
        StartCoroutine(FollowPath(path));
    }
    

    private IEnumerator FollowPath(CustomerPath path)
    {
        _agent.isStopped = false;

        while (true)
        {
            Vector3 nextPoint = path.GetNextPosition();
            Debug.Log("Go to: " + nextPoint);
            
            _agent.SetDestination(nextPoint);
            

            yield return null;

            while (_agent.pathPending)
                yield return null;

            while (_agent.remainingDistance > _agent.stoppingDistance || _agent.velocity.sqrMagnitude > 0.01f)
                yield return null;
        }
    }
    
}
