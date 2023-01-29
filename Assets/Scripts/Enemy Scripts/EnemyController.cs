using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    PATROL,
    CHASE,
    ATTACK
}

public class EnemyController : MonoBehaviour
{
    private EnemyAnimator enemyAnim;
    private NavMeshAgent navAgent;

    private EnemyState enemyState;

    public float walkSpeed = 0.5f;
    public float runSpeed = 4f;
    
    public float chaseDistance = 7f;
    private float currentChaseDistance;
    public float attackDistance = 1.8f;
    public float chaseAfterAttackDistance = 2f;

    public float patrolRadiusMin = 20f, patrolRadiusMax = 60f;
    public float patrolForThisTime = 15f;
    private float patrolTimer;

    public float waitBeforeAttack = 2f;
    private float attackTimer;

    private Transform target;

    public GameObject attackPoint;

    private EnemyAudio enemyAudio;


    private void Awake()
    {
        enemyAnim = GetComponent<EnemyAnimator>();
        navAgent = GetComponent<NavMeshAgent>();

        target = GameObject.FindWithTag(Tags.playerTag).transform;

        enemyAudio = GetComponentInChildren<EnemyAudio>();
    }

    void Start()
    {
        enemyState = EnemyState.PATROL;

        patrolTimer = patrolForThisTime;

        attackTimer = waitBeforeAttack;

        currentChaseDistance = chaseDistance;
    }

    void Update()
    {
        if (enemyState == EnemyState.PATROL)
        {
            Patrol();
        }

        if (enemyState == EnemyState.CHASE)
        {
            Chase();
        }

        if (enemyState == EnemyState.ATTACK)
        {
            Attack();
        }
    }

    void Patrol()
    {
        navAgent.isStopped = false;
        navAgent.speed = walkSpeed;

        patrolTimer += Time.deltaTime;

        if (patrolTimer > patrolForThisTime)
        {
            SetNewRandomDestination();

            patrolTimer = 0f;
        }

        if (navAgent.velocity.sqrMagnitude > 0)
        {
            enemyAnim.Walk(true);
        }
        else
        {
            enemyAnim.Walk(false);
        }

        if (Vector3.Distance(transform.position, target.position) <= chaseDistance)
        {
            enemyAnim.Walk(false);

            enemyState = EnemyState.CHASE;

            enemyAudio.PlayScreamSound();
        }
    }

    void Chase()
    {
        navAgent.isStopped = false;
        navAgent.speed = runSpeed;

        navAgent.SetDestination(target.position);

        if (navAgent.velocity.sqrMagnitude > 0)
        {
            enemyAnim.Run(true);
        }
        else
        {
            enemyAnim.Run(false);
        }

        if (Vector3.Distance(transform.position, target.position) <= attackDistance)
        {
            enemyAnim.Run(false);
            enemyAnim.Walk(false);

            enemyState = EnemyState.ATTACK;

            if (chaseDistance != currentChaseDistance)
            {
                chaseDistance = currentChaseDistance;
            }
        }
        else if (Vector3.Distance(transform.position, target.position) > chaseDistance)
        {
            enemyAnim.Run(false);

            enemyState = EnemyState.PATROL;

            patrolTimer = patrolForThisTime;

            if (chaseDistance != currentChaseDistance)
            {
                chaseDistance = currentChaseDistance; 
            }
        }
    }

    void Attack()
    {
        navAgent.velocity = Vector3.zero;
        navAgent.isStopped = true;

        attackTimer += Time.deltaTime;

        if (attackTimer > waitBeforeAttack)
        {
            enemyAnim.Attack();

            attackTimer = 0f;

            enemyAudio.PlayAttackSound();
        }

        if (Vector3.Distance(transform.position, target.position) > attackDistance + chaseAfterAttackDistance)
        {
            enemyState = EnemyState.CHASE;
        }
    }

    void SetNewRandomDestination()
    {
        float randRadius = Random.Range(patrolRadiusMin, patrolRadiusMax);

        Vector3 randDir = Random.insideUnitSphere * randRadius;
        randDir += transform.position;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDir, out navHit, randRadius, -1);

        navAgent.SetDestination(navHit.position);
    }

    void TurnOnAttackPoint()
    {
        attackPoint.SetActive(true);
    }

    void TurnOffAttackPoint()
    {
        if (attackPoint.activeInHierarchy)
        {
            attackPoint.SetActive(false);
        }
    }

    public EnemyState Enemy_State
    {
        get; set;
    }
}
