using UnityEngine;
using UnityEngine.AI;

public class PoliceAI : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float waitTime = 2f;
    public bool loop = true;
    public float hearingRadius = 20f;  // How far the cop can hear gunshots
    public float chaseSpeed = 6f;
    public float patrolSpeed = 3.5f;

    private int currentPointIndex = 0;
    private NavMeshAgent agent;
    private bool waiting = false;
    private float waitTimer = 0f;

    private enum State { Patrolling, Chasing }
    private State currentState = State.Patrolling;

    private Transform player;
    private bool isStunned = false;
    private float stunTimer = 0f;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (patrolPoints.Length == 0)
        {
            Debug.LogWarning("No patrol points assigned for " + gameObject.name);
            return;
        }

        GoToNextPoint();
    }

    void Update()
    {
        // 1. Handle stun first
        if (isStunned)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0f)
            {
                isStunned = false;
                agent.isStopped = false; // resume movement
                Debug.Log(name + " recovered from stun!");
            }

            // Play stunned animation
            animator.SetBool("Stunned", true);
            animator.SetBool("isMoving", false);
            animator.SetBool("isSprinting", false);
            return; // skip the rest of Update while stunned
        }
        else
        {
            // Stun is over
            animator.SetBool("Stunned", false);
        }

        // 2. Handle AI behavior
        if (currentState == State.Patrolling)
        {
            PatrolBehavior();
        }
        else if (currentState == State.Chasing)
        {
            ChaseBehavior();
        }

        // 3. Update animator movement
        bool isMoving = agent.desiredVelocity.magnitude > 0.1f && !agent.isStopped && !waiting;
        animator.SetBool("isMoving", isMoving);
        animator.SetBool("isSprinting", currentState == State.Chasing && isMoving);
    }


    void PatrolBehavior()
    {
        agent.speed = patrolSpeed;

        if (waiting)
        {
            animator.SetBool("isMoving", false);
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0f)
            {
                waiting = false;
                GoToNextPoint();
            }
        }
        else
        {
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                waiting = true;
                animator.SetBool("isMoving", false);
                waitTimer = waitTime;
            }
        }
    }

    void ChaseBehavior()
    {
        agent.speed = chaseSpeed;

        if (player != null)
            agent.SetDestination(player.position);

        // Optional: return to patrol if player gets far away
        if (Vector3.Distance(transform.position, player.position) > hearingRadius * 1.5f)
        {
            currentState = State.Patrolling;
            waiting = true;
            waitTimer = waitTime;
            agent.isStopped = false;
            GoToNextPoint();
        }

    }

    void GoToNextPoint()
    {
        if (patrolPoints.Length == 0) return;

        agent.destination = patrolPoints[currentPointIndex].position;
        currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
    }

    public void OnPlayerShot(Vector3 shotPosition)
    {
        float distance = Vector3.Distance(transform.position, shotPosition);
        if (distance <= hearingRadius)
        {
            Debug.Log(gameObject.name + " heard a gunshot!");
            currentState = State.Chasing;
        }
    }

    public void Stun(float duration)
    {
        if (!isStunned)
        {
            isStunned = true;
            stunTimer = duration;
            agent.isStopped = true;
            Debug.Log(name + " is stunned!");
        }
    }
}
