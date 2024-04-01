using UnityEditor.Animations;
using UnityEngine;

public class PlayerCamouflage : MonoBehaviour
{
    public static PlayerCamouflage singltone;

    private PlayerMovement _playerMovement;
    private Animator _animator;

    [SerializeField] private AnimatorController baseAnimatorController;
    [SerializeField] private AnimatorController camouflageAnimatorController;

    [SerializeField] private GameObject camouflageEffect;

    private bool camouflageIsActive = false;

    private void Awake()
    {
        singltone = this;
    }
    private void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _animator = GetComponent<Animator>();
        
    }
    public void EnableCamouflage() {
        if (camouflageIsActive) { return; }
        camouflageIsActive = true;
        _animator.runtimeAnimatorController = camouflageAnimatorController;
        _playerMovement.playerOnMask = true;
        GameObject dataEffect = Instantiate(camouflageEffect, transform.position, Quaternion.identity);
    }
    public void DisableCamouflage()
    {
        if (!camouflageIsActive) { return; }
        camouflageIsActive = false;
        _animator.runtimeAnimatorController = baseAnimatorController;
        _playerMovement.playerOnMask = false;
        GameObject dataEffect = Instantiate(camouflageEffect, transform.position, Quaternion.identity);
    }
}
