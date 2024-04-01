using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement singltone;

    [HideInInspector] public bool playerOnInvise = false;
    [HideInInspector] public bool playerOnMask = false;

    [HideInInspector] public bool playerOnConveyor = false;
    [HideInInspector] public Vector3 conveyorVector;

    [SerializeField] private float normalSpeed = 5f;

    private bool isStanned = false;

    private Rigidbody rb;
    private Animator anim;

    private float inputX, inputY;

    //RUNNING SYSTEM
    [SerializeField] private float runSpeedKoef = 2f;
    private float runSpeedKoefNow = 1f;
    [SerializeField] private float maxStamina = 5f;
    private float stamina;
    [SerializeField] private GameObject runEffect;
    public bool isRun { get; private set; }
    private bool runInput = false;
    private bool firstRunInput = true;
    [SerializeField] private Image runButtonImage;
    //
    [SerializeField] private LayerMask wallsLayers;
    [SerializeField] private bool isRotatedToWall = false;

    [Space(15)]
    public GameObject teleportingPlayerEffect;

    [HideInInspector] public bool isHide = false;

    private void Awake()
    {
        singltone = this;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        maxStamina *= Bufs.staminaBufCoef;
        stamina = maxStamina;
    }

    void Update()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.LeftShift) || (runInput && firstRunInput))
        {
            PlayerCamouflage.singltone.DisableCamouflage();
            firstRunInput = false;
            isRun = true;
            runSpeedKoefNow = runSpeedKoef;
            anim.SetBool("isRunning", true);
            StartCoroutine(RunEffectCoroutine());
        }
        if ((Input.GetKey(KeyCode.LeftShift) || runInput) && isRun)
        {
            stamina -= Time.deltaTime;
            runButtonImage.fillAmount = 1f / maxStamina * stamina;
            if (stamina < 0)
            {
                isRun = false;
            }
        }
        else
        {
            isRun = false;
            if (stamina < maxStamina)
            {
                runSpeedKoefNow = 1f;

                stamina += Time.deltaTime * 0.5f;
                runButtonImage.fillAmount = 1f / maxStamina * stamina;

                if (stamina > maxStamina) stamina = maxStamina;
            }
            anim.SetBool("isRunning", false);
        }

        Physics.Raycast(transform.position + transform.up * -0.25f, transform.up * 0.25f, out RaycastHit hit, 0.5f, wallsLayers);
        if (hit.collider != null)
        {
            anim.SetBool("nearWall", true);
            if (!isRotatedToWall) {
                isRotatedToWall = true;

                //Vector3 newScale = transform.localScale;
                //newScale.y *= -1;
                //transform.localScale = newScale;

                transform.Rotate(0f, 0f, 180f);
            }
        }
        else
        {
            if (isRotatedToWall)
            {
                isRotatedToWall = false;

                //Vector3 newScale = transform.localScale;
                //newScale.y *= -1;
                //transform.localScale = newScale;
            }
            anim.SetBool("nearWall", false);
        }
    }
    private void FixedUpdate()
    {
        MovementMethod();
    }
    private void MovementMethod()
    {
        Vector3 moveDir = new Vector3(inputX, 0,inputY);
        moveDir = moveDir.normalized;
        if (playerOnConveyor) moveDir += new Vector3(conveyorVector.x, 0f, conveyorVector.z) * 0.5f;

        if (isStanned) moveDir = Vector3.zero;

        rb.velocity = moveDir * normalSpeed * runSpeedKoefNow * Bufs.speedBufCoef;
        if (inputX != 0 || inputY != 0)
        {
            if (!anim.GetBool("nearWall")) {
                float angle = Mathf.Atan2(rb.velocity.z, rb.velocity.x) * Mathf.Rad2Deg - 90f;
                transform.rotation = Quaternion.Euler(new Vector3(90f, 0, angle));
            }

            anim.SetBool("isMove", true);
        }
        else
        {
            anim.SetBool("isMove", false);
        }
    }
    IEnumerator RunEffectCoroutine() {
        
        while (isRun)
        {
            yield return new WaitForSeconds(Random.Range(0.3f, 0.5f));
            if (!isRun) break;
            Vector3 spawnPos = transform.position;
            spawnPos.y = 0.03f;
            Instantiate(runEffect, spawnPos, Quaternion.identity);
        }
    }
    public void Stan() => StartCoroutine(StanCoroutine());
    IEnumerator StanCoroutine() {
        if (isStanned) yield break;

        isStanned = true;
        anim.SetTrigger("Stan");
        AI_Controller.NoiseAlert(transform);
        yield return new WaitForSeconds(4f);
        anim.SetTrigger("StopStan");
        isStanned = false;
    }

    public void DownRunButton() { runInput = true; firstRunInput = true; }
    public void UpRunButton() => runInput = false;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position - transform.up * 0.25f, transform.position + transform.up * 0.25f); 
    }
}
