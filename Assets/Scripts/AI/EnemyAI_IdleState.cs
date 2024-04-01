using UnityEngine;

public class EnemyAI_IdleState : StateMachineBehaviour
{
    private AI_Controller controller;

    private bool switchToPatrolling = false;
    private float timer = 0f;

    public bool needRotate = false;
    //
    private float timeBetweenRotatings;
    private float[] angles;

    private int angleID_Now = 0;
    private float rotateTimer = 0f;
    private bool waitRotate = false;
    private bool angleIsConfirmed = false;
    //
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (controller == null)
        {
            controller = animator.transform.GetChild(0).GetChild(0).GetComponent<AI_Controller>();

            timeBetweenRotatings = controller.timeBetweenRotatings;
            angles = controller.angles;
        }
        if (controller.canPatrol)
        {
            switchToPatrolling = true;
        }

        if (Vector3.Distance(animator.transform.position, controller.startPos) < 0.15f) { needRotate = true; }
        timer = 0f;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (timer >= 3f)
        {
            needRotate = false;
            if (switchToPatrolling)
            {
                animator.SetBool("isPatrooling", true);
            }
            else if (Vector3.Distance(animator.transform.position, controller.startPos) > 0.15f)
            {
                animator.SetBool("isPatrooling", true);
            }
        }
        else
        {
            if (!controller.canRotate) {
                if (!switchToPatrolling && needRotate) animator.transform.rotation = Quaternion.Lerp(animator.transform.rotation, controller.startAngle, timer * 2f);
            }
            timer += Time.deltaTime;
        }

        if (controller.canRotate) {
            if (rotateTimer >= timeBetweenRotatings)
            {
                animator.transform.rotation = controller.startAngle;
                rotateTimer = 0;
                waitRotate = !waitRotate;

                if (!waitRotate)
                {
                    angleID_Now++;
                    
                    if (angleID_Now >= angles.Length) {
                        angleID_Now = 0;
                    }

                    controller.startAngle = Quaternion.Euler(controller.startAngle.x, angles[angleID_Now], controller.startAngle.z);

                }
            }
            else
            {
                if (!waitRotate) rotateTimer += Time.deltaTime;
                else rotateTimer += Time.deltaTime;
            }
            if (!waitRotate)
            {
                animator.transform.rotation = Quaternion.Lerp(animator.transform.rotation, controller.startAngle, rotateTimer * 0.5f);
            }
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        needRotate = false;
    }
}
