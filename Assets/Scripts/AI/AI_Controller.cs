using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Controller : MonoBehaviour
{
    private Animator anim;
    private Transform mainParent;

    public NavMeshAgent agent { get; private set; }
    public Transform target;

    [SerializeField] private float hearDistance = 20f;
    [HideInInspector] public bool hearTarget = false;
    [SerializeField] private GameObject hearAlertObject;

    public bool targetOnTrigger { get; private set; }
    public bool targetOnSeeContact = false;
    public LayerMask detectLayers;
    public GameObject scopeObject;

    [Space(15)]
    [Header("Patrol System")]
    public bool canPatrol = false;
    public float timeBeetwenPoints = 1f;
    public List<Transform> wayPoints;

    [Space(15)]
    [Header("Just rotate")]
    public bool canRotate = false;
    public float timeBetweenRotatings;
    public float[] angles;

    [Space(15)]
    [Header("Doors settings")]
    [SerializeField] private bool canCheckOpenDoors;
    public Transform doorTargetPoint { get; private set; }
    public DoorComponent dataDoorTargetScript { get; private set; }
    public bool checkDoor;
    private bool checkDoorColdown = false;
    [SerializeField] private GameObject doorAlertObject;

    [Space(15)]
    [Header("Police settings")]
    public bool isPolice = false;
    public List<Vector3> policeWay;
    [Space(5)]
    public Transform assaultPoint;

    [Space(15)]
    [Header("Other settings")]
    public GameObject noStaminaAlertObject;
    [Space(5)]
    public bool canCallPolice;
    [SerializeField] private GameObject callPoliceAlertObject;
    [SerializeField] private GameObject dogAlertObject;
    [SerializeField] private GameObject bathAlertObject;
    [SerializeField] private GameObject donutAlertObject;
    [Space(5)]
    [SerializeField] private Transform bathPoint;
    private SpriteRenderer spriteRenderer;

    [Space(5)]
    public bool isBiffen = false;
    [HideInInspector] public bool attackPoliceman = false;
    private bool policerOnTrigger = false;
    private Coroutine reservFindPolicerCoroutine;
    [HideInInspector] public Transform policemanTarget;
    public GameObject catchEffectForBiffen;

    private bool rottenDonutOnTrigger = false;
    private Coroutine reservRottenDonutCoroutine;
    [HideInInspector] public Transform oneWayTarget;
    [HideInInspector] public bool goToBath = false;
    [Space(5)]
    [SerializeField] private AI_Controller mainBiffen;

    [Space(15)]
    public bool isScientist = false;


    private Coroutine reservSeeContactCoroutine;
    private Coroutine reservSeeContactDoorCoroutine;
    private PlayerMovement targetMoveScript;
    //
    private LevelConditions levelConditions;

    private static Action dogAlarmEvent;
    private static Action noiseAlarmEvent;
    public static Transform dogPos;
    public static Transform noisePos;
    public static bool withDistance = true;
    [HideInInspector] public bool hearNoise = false;

    [HideInInspector] public Action spyAction;

    //START VARS
    [HideInInspector] public Quaternion startAngle;
    [HideInInspector] public Vector3 startPos;
    private void Awake()
    {
        dogAlarmEvent = null;
        noiseAlarmEvent = null;
    }
    private void Start()
    {
        dogAlarmEvent += GoToDog;
        noiseAlarmEvent += GoToNoise;

        mainParent = transform.parent.parent;

        startPos = mainParent.position;
        startAngle = mainParent.rotation;

        agent = mainParent.GetComponent<NavMeshAgent>();
        anim = mainParent.GetComponent<Animator>();
        spriteRenderer = mainParent.GetChild(0).GetComponent<SpriteRenderer>();

        targetMoveScript = PlayerMovement.singltone;
        target = PlayerMovement.singltone.gameObject.transform;

        if (canPatrol)
        {
            anim.SetBool("isPatrooling", true);
        }

        hearDistance *= Bufs.stealthBufCoef;
        StartCoroutine(HearSystemCoroutine());

        levelConditions = FindObjectOfType<LevelConditions>();

        if (assaultPoint != null)
        {
            startPos = assaultPoint.position;
            startAngle = assaultPoint.rotation;

            hearNoise = true;
            hearTarget = true;
            anim.SetBool("isCatching", true);
        }

        if (isBiffen) { 
            mainParent.gameObject.SetActive(false);
        }
    }
    private void Update()
    {
        if (PlayerMovement.singltone.isHide && targetOnTrigger) {
            targetOnTrigger = false;
        }
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player")) {
            targetOnTrigger = true;
            if (reservSeeContactCoroutine == null) {
                reservSeeContactCoroutine = StartCoroutine(SeeContactCoroutine());
            }
        }
        if (col.gameObject.CompareTag("RottenDonut"))
        {
            rottenDonutOnTrigger = true;
            if (reservRottenDonutCoroutine == null)
            {
                reservRottenDonutCoroutine = StartCoroutine(SeeRottenDonutCoroutine(col.transform));
            }
        }
        if (canCheckOpenDoors)
        {
            if (col.gameObject.CompareTag("Door") && !checkDoor)
            {
                DoorComponent resDoor = col.GetComponent<DoorComponent>();
                if (resDoor.GetState() != 0 && resDoor.GetWhoOpenDoor() == "Player")
                {
                    dataDoorTargetScript = resDoor;
                    if (reservSeeContactDoorCoroutine != null) StopCoroutine(reservSeeContactDoorCoroutine);
                    reservSeeContactDoorCoroutine = StartCoroutine(SeeContactDoorCoroutine(col.transform));
                }
            }
        }
        if (isBiffen && col.gameObject.CompareTag("Enemy")) {
            policerOnTrigger = true;
            if (reservFindPolicerCoroutine == null) {
                policemanTarget = col.transform;
                reservFindPolicerCoroutine = StartCoroutine(SeePolicemanContactCoroutine());
            }
        }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            targetOnTrigger = false;
        }
        if (col.gameObject.CompareTag("RottenDonut"))
        {
            rottenDonutOnTrigger = false;
        }
        if (col.gameObject.CompareTag("Door") && dataDoorTargetScript != null && col.gameObject == dataDoorTargetScript.gameObject)
        {
            if(reservSeeContactDoorCoroutine != null) StopCoroutine(reservSeeContactDoorCoroutine);
            reservSeeContactDoorCoroutine = null;
        }
        if (isBiffen && col.gameObject.CompareTag("Enemy"))
        {
            policerOnTrigger = false;
        }
    }
    private IEnumerator SeeContactDoorCoroutine(Transform door)
    {
        while (door.gameObject == dataDoorTargetScript.gameObject) {
            yield return new WaitForSeconds(0.25f);
            Vector3 newMainParentPos = mainParent.position;
            newMainParentPos.y = 1f;
            if (Physics.Raycast(mainParent.position, door.GetChild(0).position - mainParent.position, out RaycastHit hit, 1000f, detectLayers))
            {
                //Debug.DrawRay(mainParent.position, door.GetChild(0).position - mainParent.position, Color.red, 100f);
                if (hit.collider.gameObject == door.gameObject) // || hit.collider.transform.parent.gameObject == door.gameObject
                {
                    if (!checkDoor)
                    {
                        doorTargetPoint = dataDoorTargetScript.GetFarPoint(transform);
                        checkDoor = true;
                        checkDoorColdown = true;
                        anim.SetBool("isCatching", true);
                        levelConditions.seeStar = 0;

                        StartCoroutine(ActivateAlert(doorAlertObject));
                        break;
                    }
                }
                
            }
        }
        reservSeeContactDoorCoroutine = null;
    }
    private IEnumerator SeeRottenDonutCoroutine(Transform donut)
    {
        while (rottenDonutOnTrigger)
        {
            yield return new WaitForSeconds(0.25f);
            if (Physics.Raycast(mainParent.position, donut.position - mainParent.position, out RaycastHit hit, 1000f, detectLayers))
            {
                if (hit.collider.gameObject.CompareTag("RottenDonut"))
                {
                    oneWayTarget = donut;
                    StartCoroutine(ActivateAlert(donutAlertObject));
                    anim.SetBool("isPatrooling", true);
                    break;
                }
            }
        }
        reservRottenDonutCoroutine = null;
    }
    private IEnumerator SeeContactCoroutine() {
        while (targetOnTrigger) {
            yield return new WaitForSeconds(0.25f);
            if (Physics.Raycast(mainParent.position, target.position - mainParent.position, out RaycastHit hit, 1000f, detectLayers))
            {
                if (hit.collider.gameObject == target.gameObject && !PlayerMovement.singltone.playerOnInvise &&  !PlayerMovement.singltone.playerOnMask)
                {
                    if (canCallPolice)
                    {
                        CallingPoliceAI.singltone.CallPolice();
                        StartCoroutine(ActivateAlert(callPoliceAlertObject));
                        levelConditions.seeStar = 0;
                    }
                    else {
                        anim.SetBool("isCatching", true);
                        targetOnSeeContact = true;
                        levelConditions.seeStar = 0;

                        if (!isBiffen && mainBiffen != null) {
                            if (!mainBiffen.gameObject.activeInHierarchy) FindObjectOfType<BiffenController>().Catch(mainParent.transform);
                            else mainBiffen.GoToNoiseWithoutDistance(mainParent.transform);
                        }
                        break;
                    }
                }
                else {
                    targetOnSeeContact = false;
                }
            }
        }
        targetOnSeeContact = false;

        reservSeeContactCoroutine = null;
    }
    private IEnumerator SeePolicemanContactCoroutine()
    {
        while (policerOnTrigger)
        {
            yield return new WaitForSeconds(0.25f);
            if (Physics.Raycast(mainParent.position, policemanTarget.position - mainParent.position, out RaycastHit hit, 1000f, detectLayers))
            {
                if (hit.collider.gameObject == policemanTarget.gameObject)
                {
                    anim.SetBool("isCatching", true);
                    attackPoliceman = true;
                    break;
                }
            }
        }

        reservFindPolicerCoroutine = null;
    }
    private IEnumerator HearSystemCoroutine() {
        while (true) {
            yield return new WaitForSeconds(0.2f);
            if (Vector3.Distance(mainParent.transform.position, target.transform.position) < hearDistance && targetMoveScript.isRun) {
                if (!targetOnSeeContact && !anim.GetBool("isCatching"))
                {
                    StartCoroutine(ActivateAlert(hearAlertObject));
                }
                anim.SetBool("isCatching", true);
                hearTarget = true;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (mainParent != null) {
            Gizmos.DrawLine(mainParent.position, target.position);
            Gizmos.DrawWireSphere(mainParent.position, hearDistance);
        }
        
    }
    public void CloseTargetedDoor(DoorComponent door) { StartCoroutine(CloseTargetedDoorWithTimer(door)); }
    IEnumerator CloseTargetedDoorWithTimer(DoorComponent door) {
        StartCoroutine(ColdownCheckingDoors());
        yield return new WaitForSeconds(4.6f);
        door.Close();
    }
    public void ActivateAlertMethod(GameObject alert) => StartCoroutine(ActivateAlert(alert));
    IEnumerator ActivateAlert(GameObject alert) {
        if (!alert.activeInHierarchy) {
            alert.SetActive(true);
            yield return new WaitForSeconds(2.25f);
            alert.SetActive(false);
        }
    }
    public IEnumerator ColdownCheckingDoors()
    {
        if (canCheckOpenDoors) {
            yield return new WaitForSeconds(5f);
            checkDoorColdown = false;
        }
    }
    public static void DogAlert(Transform dog) {
        dogPos = dog;
        dogAlarmEvent.Invoke();
    }
    public static void NoiseAlert(Transform noise, bool withDistance = true)
    {
        noisePos = noise;
        AI_Controller.withDistance = withDistance;
        noiseAlarmEvent.Invoke();
    }
    private void GoToDog() {
        if (Vector3.Distance(mainParent.position, dogPos.position) <= hearDistance) {
            if (!targetOnSeeContact && !anim.GetBool("isCatching"))
            {
                StartCoroutine(ActivateAlert(dogAlertObject));
            }
            hearNoise = true;
            hearTarget = true;
            anim.SetBool("isCatching", true);
            
        }
    }
    private void GoToNoise()
    {
        if (!withDistance || Vector3.Distance(mainParent.position, noisePos.position) <= hearDistance)
        {
            if (!targetOnSeeContact && !anim.GetBool("isCatching"))
            {
                StartCoroutine(ActivateAlert(hearAlertObject));
            }
            hearNoise = true;
            hearTarget = true;
            anim.SetBool("isCatching", true);
        }
    }
    public void GoToNoiseWithoutDistance(Transform noisePos)
    {
        AI_Controller.noisePos = noisePos;
        if (!targetOnSeeContact && !anim.GetBool("isCatching"))
        {
            StartCoroutine(ActivateAlert(hearAlertObject));
        }
        hearNoise = true;
        hearTarget = true;
        anim.SetBool("isCatching", true);
    }
    public void GoToBath()
    {
        noisePos = bathPoint;
        spriteRenderer.color = new Color(0.5117139f, 1f, 0.3726415f, 1f);
        if (!targetOnSeeContact && !anim.GetBool("isCatching"))
        {
            StartCoroutine(ActivateAlert(bathAlertObject));
        }
        goToBath = true;
        hearNoise = false;
        hearTarget = true;
        anim.SetBool("isCatching", true);
    }
    public void ResetBotColor() {
        spriteRenderer.color = Color.white;
    }
    public void Teleportating(Transform tpPos) {
        anim.SetTrigger("tp");
        mainParent.position = tpPos.position;
        StartCoroutine(TeleportatingCoroutine());
    }
    IEnumerator TeleportatingCoroutine() {
        float timer = 0f;
        while (timer < 5f)
        {
            yield return null;
            timer += Time.deltaTime;
            mainParent.Rotate(0, Time.deltaTime * timer * 250f, 0);
        }
        Destroy(mainParent.gameObject);
    }
    public void SetOneWay(Transform oneWayPos) {
        oneWayTarget = oneWayPos;
        anim.SetBool("isPatrooling", true);
    }
    public void SpawnEffect(GameObject effect) {
        Instantiate(effect, mainParent.position, Quaternion.identity);
    }
}
