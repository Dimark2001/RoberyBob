using UnityEngine;
using UnityEngine.AI;

public class EnemyAI_CatchState : StateMachineBehaviour
{
    private AI_Controller controller;

    private NavMeshAgent agent;

    [SerializeField] private float stamina = 4f;
    private bool regenerateStamina = false;

    //TARGETS
    private Transform target;
    private Vector3 lastPosTarget;
    private Transform doorTargetPoint;
    //
    private LayerMask detectLayers;
    private GameObject scopeObject;

    private GameObject dataScope;

    private float timerRaycasting = 0;
    private float hearTimer = 0f;

    private bool seePlayer = false;
    private bool canHear = true;
    private bool checkDoor = false;
    private bool canCallPolice = false;
    private bool catching = false;

    private bool isScientist = false;

    private bool startWorkUpdate = false;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (controller == null) {
            controller = animator.transform.GetChild(0).GetChild(0).GetComponent<AI_Controller>();
            agent = controller.agent;
            target = controller.target;
            detectLayers = controller.detectLayers;
            scopeObject = controller.scopeObject;
            canCallPolice = controller.canCallPolice;
            isScientist = controller.isScientist;
        }
        seePlayer = controller.targetOnSeeContact;

        checkDoor = controller.checkDoor;
        if (checkDoor)
        {
            doorTargetPoint = controller.doorTargetPoint;

            if (dataScope != null) { Destroy(dataScope); }
            dataScope = Instantiate(scopeObject, doorTargetPoint.position, scopeObject.transform.rotation);
        }
        if (controller.hearTarget)
        {
            if (controller.goToBath)
            {
                
                controller.hearTarget = false;
                controller.goToBath = false;
                
                if (!seePlayer && canHear)
                {
                    lastPosTarget = AI_Controller.noisePos.position;

                    if (dataScope != null) { Destroy(dataScope); }
                    dataScope = Instantiate(scopeObject, lastPosTarget, scopeObject.transform.rotation);
                    Debug.Log(AI_Controller.noisePos.name);
                }
                else
                {
                    canHear = false;
                }
            }
            else if (!controller.hearNoise)
            {
                
                controller.hearTarget = false;
                if (!seePlayer && canHear)
                {
                    lastPosTarget = target.position;

                    if (dataScope != null) { Destroy(dataScope); }
                    dataScope = Instantiate(scopeObject, lastPosTarget, scopeObject.transform.rotation);
                }
                else
                {
                    canHear = false;
                }
            }
            else {
                controller.hearNoise = false;
                controller.hearTarget = false;
                if (!seePlayer && canHear)
                {
                    if (AI_Controller.noisePos != null) lastPosTarget = AI_Controller.noisePos.position;

                    if (dataScope != null) { Destroy(dataScope); }
                    dataScope = Instantiate(scopeObject, lastPosTarget, scopeObject.transform.rotation);
                }
                else if (!seePlayer) {
                    if (controller.assaultPoint != null) lastPosTarget = controller.assaultPoint.position;
                }
                else
                {
                    canHear = false;
                }
            }
        }
        agent.speed = 1.3f;

        FirstRaycast(animator);
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!startWorkUpdate) return;

        //Скорости
        if (checkDoor)
        {
            if (agent.speed != 0.7f) agent.speed = 0.7f;
        }
        else
        {
            if (catching)
            {
                if (stamina > 0 && !regenerateStamina)
                {
                    if (agent.speed != 1.3f) agent.speed = 1.3f;

                    stamina -= Time.deltaTime;
                }
                else if (stamina <= 0)
                {
                    regenerateStamina = true;
                    controller.ActivateAlertMethod(controller.noStaminaAlertObject);
                }
                if (regenerateStamina)
                {
                    stamina += Time.deltaTime;
                    if (stamina >= 4f)
                    {
                        stamina = 4f;
                        regenerateStamina = false;
                    }
                }

                if (regenerateStamina) agent.speed = 0.3f;
            }
            else {
                if (agent.speed != 0.6f) agent.speed = 0.6f;
            }
        }
        //Видимость цели
        if (timerRaycasting >= 0.2f)
        {
            if (Physics.Raycast(animator.transform.position, target.position - animator.transform.position, out RaycastHit hit, 1000f, detectLayers))
            {
                if (hit.collider.gameObject != target.gameObject)
                {
                    if (dataScope == null)
                    {
                        lastPosTarget = target.position;
                        dataScope = Instantiate(scopeObject, lastPosTarget, scopeObject.transform.rotation);
                    }
                    seePlayer = false;
                }
                else
                {
                    if (controller.targetOnTrigger && !canCallPolice && !PlayerMovement.singltone.playerOnInvise && !PlayerMovement.singltone.playerOnMask) {
                        if (dataScope != null) { Destroy(dataScope); }

                        seePlayer = true;
                        checkDoor = false;
                        catching = true;
                    }
                }
            }
            timerRaycasting = 0;
        }
        else {
            timerRaycasting += Time.deltaTime;
        }
        //Движение
        if (controller.attackPoliceman) {
            agent.SetDestination(controller.policemanTarget.position);
            if (Vector3.Distance(agent.transform.position, controller.policemanTarget.position) < 0.5f)
            {
                controller.attackPoliceman = false;
                Destroy(controller.policemanTarget.gameObject);
                controller.SpawnEffect(controller.catchEffectForBiffen);
                animator.SetBool("isCatching", false);
            }
        }
        else if (seePlayer)
        {
            agent.SetDestination(target.position);

            if (Vector3.Distance(agent.transform.position, target.position) < 0.3f) {
                if (!isScientist) //ПРОИГРЫШ
                {
                    LevelConditions.singltone.Defeat();
                }
                else
                {
                    PlayerMovement.singltone.Stan();
                    animator.SetBool("isCatching", false);
                }
            }
        }
        else if (checkDoor) {
            agent.SetDestination(doorTargetPoint.position);
            if (Vector3.Distance(agent.transform.position, doorTargetPoint.position) < 0.15f)
            {
                controller.CloseTargetedDoor(controller.dataDoorTargetScript);
                controller.checkDoor = false;

                animator.SetBool("isPatrooling", false);
                animator.SetBool("isCatching", false);
            }
        }
        else {
            agent.SetDestination(lastPosTarget);
            if (Vector3.Distance(agent.transform.position, lastPosTarget) < 0.15f) {
                controller.ResetBotColor();
                animator.SetBool("isPatrooling", false);
                animator.SetBool("isCatching", false);
            }
        }
        //Слышимость игрока
        if (controller.hearTarget)
        {
            if (controller.goToBath)
            {
                controller.hearTarget = false;
                controller.goToBath = false;
                
                if (!seePlayer && canHear)
                {
                    lastPosTarget = AI_Controller.noisePos.position;
                    if (dataScope != null) { Destroy(dataScope); }
                    dataScope = Instantiate(scopeObject, lastPosTarget, scopeObject.transform.rotation);
                }
                else
                {
                    canHear = false;
                }
            }
            else if (!controller.hearNoise)
            {
                
                controller.hearTarget = false;
                if (!seePlayer && canHear)
                {
                    lastPosTarget = target.position;

                    if (dataScope != null) { Destroy(dataScope); }
                    dataScope = Instantiate(scopeObject, lastPosTarget, scopeObject.transform.rotation);
                }
                else
                {
                    canHear = false;
                }
            }
            else
            {
                controller.hearNoise = false;
                controller.hearTarget = false;
                if (!seePlayer && canHear)
                {
                    lastPosTarget = AI_Controller.noisePos.position;

                    if (dataScope != null) { Destroy(dataScope); }
                    dataScope = Instantiate(scopeObject, lastPosTarget, scopeObject.transform.rotation);
                }
                else
                {
                    canHear = false;
                }
            }
        }
        //колдаун слуха, если видит цель
        if (!canHear) {
            if (hearTimer >= 4f)
            {
                hearTimer = 0f;
                canHear = true;
            }
            else {
                hearTimer += Time.deltaTime;
            }
        }
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (dataScope != null) { Destroy(dataScope); }
        agent.speed = 0.5f;
        agent.SetDestination(animator.transform.position);
        catching = false;
    }
    private void FirstRaycast(Animator animator) {
        if (Physics.Raycast(animator.transform.position, target.position - animator.transform.position, out RaycastHit hit, 1000f, detectLayers))
        {
            if (hit.collider.gameObject != target.gameObject)
            {
                if (dataScope == null)
                {
                    lastPosTarget = target.position;
                    dataScope = Instantiate(scopeObject, lastPosTarget, scopeObject.transform.rotation);
                }
                seePlayer = false;
            }
            else
            {
                if (controller.targetOnTrigger && !canCallPolice)
                {
                    if (dataScope != null) { Destroy(dataScope); }

                    seePlayer = true;
                    checkDoor = false;
                }
            }
        }
        startWorkUpdate = true;
    }
}
