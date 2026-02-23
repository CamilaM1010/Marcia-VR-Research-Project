using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    public Transform sitTarget;
    private NavMeshAgent agent;
    private Animator animator;

    private bool isGoingToSit = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        WalkToChair();
    }

    public void WalkToChair()
    {
        isGoingToSit = true;
        agent.SetDestination(sitTarget.position);
        animator.SetBool("isWalking", true);
    }

    void Update()
    {
        if (isGoingToSit && !agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                SitDown();
            }
        }
    }

    void SitDown()
    {
        // Stop movement
        agent.isStopped = true;

        // Completely disable agent BEFORE snapping
        agent.enabled = false;

        // Snap into perfect chair position
        transform.position = sitTarget.position;
        transform.rotation = sitTarget.rotation;

        // Switch animations
        animator.SetBool("isWalking", false);
        animator.SetBool("isSitting", true);

        isGoingToSit = false;
    }

}
