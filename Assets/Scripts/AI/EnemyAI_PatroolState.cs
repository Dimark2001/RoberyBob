using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI_PatroolState : StateMachineBehaviour
{
    private AI_Controller controller;

    private NavMeshAgent agent;
    private List<Transform> wayPoints;
    private List<Vector3> policeWayPoints;
    private LineRenderer wayRenderer;
    private float timeBeetwenPoints;

    [SerializeField] bool isSwitched = false;
    float timer = 0f;

    bool lastPolicePoint = false;
    private Vector3 lastTarget;

    bool oneWay = false;

    [SerializeField] int pointNow = 0;

    private bool goToHome = false;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (controller == null)
        {
            controller = animator.transform.GetChild(0).GetChild(0).GetComponent<AI_Controller>();
            agent = controller.agent;
            wayPoints = controller.wayPoints;
            timeBeetwenPoints = controller.timeBeetwenPoints;
            wayRenderer = animator.GetComponent<LineRenderer>();

            if (controller.isPolice)
            {
                policeWayPoints = controller.policeWay;
                CallingPoliceAI.singltone.newCall += NewCall;
                pointNow = 0;
            }
            else {
                pointNow = 0;
            }
        }
        if (controller.oneWayTarget != null) { oneWay = true; lastTarget = controller.oneWayTarget.position; controller.oneWayTarget = null;}
        else if (wayPoints.Count == 0 && !controller.isPolice) { lastTarget = controller.startPos; goToHome = true; }
        else if (controller.isPolice) lastTarget = policeWayPoints[pointNow];
        else lastTarget = wayPoints[pointNow].position;

        agent.speed = 0.5f;
        wayRenderer.enabled = true;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (controller.oneWayTarget != null) { oneWay = true; lastTarget = controller.oneWayTarget.position; controller.oneWayTarget = null;}

        if (controller.isPolice) agent.SetDestination(policeWayPoints[pointNow]);
        else
        {
            if (oneWay)
            {
                agent.SetDestination(lastTarget);
            }
            else if (goToHome)
            {
                agent.SetDestination(controller.startPos);
                if (Vector3.Distance(agent.transform.position, controller.startPos) < 0.15f) animator.SetBool("isPatrooling", false);
            }
            else
            {
                agent.SetDestination(wayPoints[pointNow].position);
            }
        }

        if (Vector3.Distance(agent.transform.position, lastTarget) < 1f) {
            if (agent.remainingDistance < agent.stoppingDistance) {
                if (oneWay)
                {
                    oneWay = false;
                    animator.SetBool("isPatrooling", false);
                    controller.spyAction.Invoke();
                    return;
                }

                if (!isSwitched) {
                    isSwitched = true;
                    pointNow++;

                    if (!controller.isPolice)
                    {
                        if (pointNow >= wayPoints.Count)
                        {
                            pointNow = 0;
                        }
                    }
                    else
                    {
                        if (pointNow >= policeWayPoints.Count)
                        {
                            if (controller.isPolice) { Destroy(animator.gameObject); }
                            pointNow = 0;
                        }
                    }
                    if (timeBeetwenPoints > 0f && pointNow == 1) {
                        animator.SetBool("isPatrooling", false);
                    }

                    if (controller.isPolice) lastTarget = policeWayPoints[pointNow];
                    else if(!goToHome) lastTarget = wayPoints[pointNow].position;
                }
            }
        }

        if (isSwitched) {
            if (timer < timeBeetwenPoints)
            {
                timer += Time.deltaTime;
            }
            else {
                isSwitched = false;
                timer = 0f;
            }
        }
        if (agent.hasPath && !oneWay)
        {
            wayRenderer.positionCount = agent.path.corners.Length;
            wayRenderer.SetPositions(agent.path.corners);
        }
        else {
            wayRenderer.positionCount = 0;
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(animator.transform.position);
        wayRenderer.enabled = false;
        goToHome = false;
    }
    private void NewCall() {
        lastPolicePoint = false;
        pointNow = 0;
    }
}
