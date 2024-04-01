using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Dog : MonoBehaviour
{
    private Transform _player;
    [SerializeField] private Animator _animator;
    [SerializeField] private LayerMask detectLayers;

    [Space(15)]
    [SerializeField] private GameObject _icon;
    [SerializeField] private GameObject _exitIcon;

    private const string IsIdle = "IsIdle";
    private const string IsMove = "IsMove";
    private const string IsAwoke = "IsAwoke";

    private NavMeshAgent _agent;
    private float _timer = 5f;

    private Vector3 startPos;

    private bool seePlayer = false;
    private bool catchPlayer = false;
    private bool needToHome = false;
    private bool closed = false;
    private bool canWhoof = false;

    private Coroutine reservPathFounderCoroutine;
    private Coroutine reservGoBackCoroutine;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }
    private void Start()
    {
        startPos = transform.position;
        _player = PlayerMovement.singltone.transform;
    }
    private void Update()
    {
        FollowPlayer();

    }

    private void FollowPlayer()
    {
        if (_agent.pathStatus != NavMeshPathStatus.PathComplete && reservPathFounderCoroutine == null) { reservPathFounderCoroutine = StartCoroutine(PathFounder()); }

        if (catchPlayer)
        {
            if (Vector3.Distance(_player.position, transform.position) > 5f) {
                catchPlayer = false;
            }

            if (Vector3.Distance(_player.position, transform.position) <= 0.9f)
            {
                canWhoof = true;
                _animator.SetBool(IsIdle, true);
                _agent.destination = transform.position;
                return;
            }
            else
            {
                _agent.destination = _player.position;
                _animator.SetBool(IsIdle, false);
                canWhoof = false;
            }
        }
        else if (needToHome)
        {
            if (reservGoBackCoroutine == null) reservGoBackCoroutine = StartCoroutine(GoBack());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;

        seePlayer = true;
        StartCoroutine(CheckPlayer());
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player") return;

        seePlayer = false;
    }

    private IEnumerator Awoke()
    {
        _animator.SetBool(IsAwoke, true);
        _icon.SetActive(true);

        yield return new WaitForSeconds(2f);

        _agent.speed = 0.6f;
        _animator.SetBool(IsMove, true);
        _icon.SetActive(false);

    }

    private IEnumerator Lose()
    {
        _exitIcon.SetActive(true);
        yield return new WaitForSeconds(2f);
        
        _agent.speed = 0f;
        _animator.SetBool(IsIdle, true);
        _exitIcon.SetActive(false);
    }
    private IEnumerator CheckPlayer() {
        while (seePlayer && !catchPlayer)
        {
            yield return new WaitForSeconds(0.2f);
            if (Physics.Raycast(transform.position, _player.position - transform.position, out RaycastHit hit, 1000f, detectLayers))
            {
                if (hit.collider.gameObject == _player.gameObject) {
                    catchPlayer = true;
                    closed = false;

                    if (reservGoBackCoroutine != null) StopCoroutine(reservGoBackCoroutine);
                    reservGoBackCoroutine = null;
                    needToHome = true;

                    StartCoroutine(Awoke());
                    StartCoroutine(Whoof());
                    break;
                }
            }
        }
    }
    private IEnumerator GoBack() {
        StartCoroutine(Lose());
        yield return new WaitForSeconds(5f);
        _agent.speed = 0.4f;
        _animator.SetBool(IsIdle, false);
        _animator.SetBool(IsMove, true);
        while (Vector3.Distance(transform.position, startPos) > 0.1f) {
            yield return new WaitForFixedUpdate();
            if (_agent.pathStatus != NavMeshPathStatus.PathComplete)
            {
                _animator.SetBool(IsIdle, true);
                _agent.SetDestination(transform.position);
            }
            else { _animator.SetBool(IsIdle, false); }
            _agent.SetDestination(startPos);
        }
        transform.position = startPos;
        _agent.speed = 0f;

        _animator.SetBool(IsMove, false);
        yield return new WaitForSeconds(0.85f);
        _animator.SetBool(IsAwoke, false);

        reservGoBackCoroutine = null;
        needToHome = false;
    }
    private IEnumerator Whoof() {
        while (catchPlayer) {
            yield return new WaitForSeconds(1f);
            if (canWhoof) { AI_Controller.DogAlert(transform); _animator.SetTrigger("Whoof"); }
        }
    }
    private IEnumerator PathFounder() {
        yield return new WaitForSeconds(1f);
        if (_agent.pathStatus != NavMeshPathStatus.PathComplete && !closed)
        {
            closed = true;
            if (catchPlayer) {
                catchPlayer = false;
            }
            StartCoroutine(Lose());
        }
        reservPathFounderCoroutine = null;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(startPos, 0.1f);
    }

}
