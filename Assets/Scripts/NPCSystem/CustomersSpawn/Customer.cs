using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Customer : MonoBehaviour
{
    [SerializeField]
    private Transform _dialogPoint;

    [SerializeField]
    private Animator _animator;

    public CustomerData currentCustomerData;

    private NavMeshAgent _agent;

    private bool _isInteracting;

    private static readonly int IsWalking =
        Animator.StringToHash("IsWalking");

    private static readonly int Interact =
        Animator.StringToHash("Interact");

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();

        if (_agent == null)
        {
            Debug.LogError("No NavMeshAgent found");
        }

        if (_animator == null)
        {
            _animator = GetComponentInChildren<Animator>();
        }

        if (_animator == null)
        {
            Debug.LogError("No Animator found");
        }
        
        _animator.applyRootMotion = false;
    }

    private void Update()
    {
        UpdateAnimations();
    }

    public void Init(CustomerData customerData)
    {
        currentCustomerData = customerData;

        Debug.Log(currentCustomerData.customerId);
    }

    public Transform GetDialogPoint() => _dialogPoint;

    public void StartPath(
        AgentPath path,
        System.Action onCompleted)
    {
        StopAllCoroutines();

        path.Reset();

        StartCoroutine(
            FollowPath(path, onCompleted));
    }

    private IEnumerator FollowPath(
        AgentPath path,
        System.Action onCompleted)
    {
        _agent.isStopped = false;

        while (path.HasNext())
        {
            Vector3 nextPoint =
                path.GetNextPosition();

            _agent.SetDestination(nextPoint);

            while (_agent.pathPending)
                yield return null;

            while (_agent.remainingDistance >
                   _agent.stoppingDistance ||
                   _agent.velocity.sqrMagnitude > 0.1f)
            {
                yield return null;
            }
        }

        _agent.isStopped = true;

        _agent.ResetPath();

        _agent.velocity = Vector3.zero;
        

        onCompleted?.Invoke();
    }

    private void UpdateAnimations()
    {
        if (_isInteracting)
            return;

        bool isWalking =
            _agent.velocity.sqrMagnitude > 0.1f;

        _animator.SetBool(
            IsWalking,
            isWalking);
    }

    public void PlayInteractAnimation()
    {
        StartCoroutine(
            InteractRoutine());
    }

    private IEnumerator InteractRoutine()
    {
        _isInteracting = true;

        _animator.SetBool(
            IsWalking,
            false);

        _animator.SetTrigger(
            Interact);

        yield return new WaitForSeconds(2f);

        _isInteracting = false;
    }
}