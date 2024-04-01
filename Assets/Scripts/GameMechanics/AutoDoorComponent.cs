using UnityEngine;

public class AutoDoorComponent : MonoBehaviour
{
    private Animator _animator;
    [SerializeField] private bool lockedDoor = false;
    [SerializeField] private GameObject lockCanvas;
    private void Start()
    {
        _animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider col)
    {
        if ((col.gameObject.tag == "Player" || col.gameObject.tag == "Enemy") && !lockedDoor){
            _animator.SetBool("Open", true);
        }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player" || col.gameObject.tag == "Enemy")
        {
            _animator.SetBool("Open", false);
        }
    }
    public void UnlockingDoor()
    {
        lockedDoor = false;
        lockCanvas.SetActive(false);
    }
}
