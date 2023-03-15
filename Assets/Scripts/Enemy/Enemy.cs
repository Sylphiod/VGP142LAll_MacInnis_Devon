using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    public int health;
    public UnityEvent<EnemyState, GameObject> onStateChanged;
    Animator eAnim;
    NavMeshAgent agent;

    //Keep reference to player or target if aggro'd.
    public GameObject target;
    public Transform playerTransform;
    public float distance = 20f;
    public GameObject[] collectiblePrefabArray;

    public enum EnemyState
    {
        Chase,
        Patrol
    }

    [SerializeField] EnemyState _currentState;

    public EnemyState currentState
    {
        get { return _currentState; }
        set
        {
            _currentState = value;
            onStateChanged.Invoke(_currentState, gameObject);
        }
    }


    public GameObject[] path;
    public int pathIndex = 0;
    public float distToNextNode;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        eAnim = GetComponent<Animator>();

        if (path.Length <= 0)
        {
            path = GameObject.FindGameObjectsWithTag("PatrolNode");
        }

        if (_currentState == EnemyState.Chase)
        {
            target = GameObject.FindGameObjectWithTag("Player");

            if (target)
                agent.SetDestination(target.transform.position);

        }

        if (distToNextNode <= 0)
        {
            distToNextNode = 0.5f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentState == EnemyState.Patrol)
        {

            if (target)
                Debug.DrawLine(transform.position, target.transform.position, Color.red);

            if (agent.remainingDistance < distToNextNode)
            {
                pathIndex++;

                pathIndex %= path.Length;

                target = path[pathIndex];
            }


        }
        if (_currentState == EnemyState.Chase)
        {
            if (target.gameObject.tag == "PatrolNode")
            {
                target = GameObject.FindGameObjectWithTag("Player");
            }

        }

        if (target)
            agent.SetDestination(target.transform.position);

        float distToPlayer = Vector3.Distance(transform.position, playerTransform.transform.position);

        if (distToPlayer > distance)
        {
            target = path[pathIndex];
            _currentState = EnemyState.Patrol;
            eAnim.SetBool("Attack", false);
        }
        if (distToPlayer < distance)
        {
            _currentState = EnemyState.Chase;
            eAnim.SetBool("Attack", true);
        }

        if (health <= 0)
        {

            Destroy(agent);
            Death();
        }
    }
    private void Death()
    {
        eAnim.SetTrigger("Death");
        Instantiate(collectiblePrefabArray[0], transform.position, transform.rotation);
    }
}
