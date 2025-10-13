using UnityEngine;

[RequireComponent (typeof(PlayerMovement))]
[RequireComponent (typeof(Animator))]
[RequireComponent (typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Reference")]
    private PlayerMovement characterMovement;
    private Animator animator;
    private CharacterController character;

    private void Awake()
    {
        characterMovement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
        character = GetComponent<CharacterController>();
    }

    private void Start()
    {
        characterMovement?.Init(animator, character); 
    }

    private void Update()
    {
        characterMovement?.OnUpdate();
    }
}
