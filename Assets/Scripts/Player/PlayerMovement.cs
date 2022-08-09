using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controls movement through player input.
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private PlayerInput playerInput;

    [SerializeField]
    private CharacterController characterController;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private ActorBusy actorBusy;

    private InputAction moveAction;
    private InputAction sneakAction;
    private InputAction runAction;

    private float sneakSpeed = 2f;
    private float walkSpeed = 3f;
    private float runSpeed = 6f;
    private float acceleration = 10f;
    private float turnSpeed = 15f;
    private float gravity = 9.8f;
    private float verticalSpeed;

    private Vector3 targetMovement;

    private bool isSneaking;

    /// <summary>
    /// Gets whether the player is currently sneaking.
    /// </summary>
    /// <returns>Whether the player is sneaking.</returns>
    public bool GetIsSneaking()
    {
        return isSneaking;
    }

    private void ToggleIsSneaking()
    {
        isSneaking = !isSneaking;
        animator.SetBool("Sneaking", isSneaking);
    }

    private float GetNormalizedSpeed()
    {
        return Mathf.Clamp(targetMovement.magnitude / (isSneaking ? sneakSpeed : runSpeed), 0f, 1f);
    }

    private void Start()
    {
        moveAction = playerInput.actions["Move"];
        sneakAction = playerInput.actions["Sneak"];
        runAction = playerInput.actions["Run"];

        sneakAction.started += _ =>
        {
            if (!actorBusy.GetIsBusy()) ToggleIsSneaking();
        };
    }

    private void Update()
    {
        bool isBusy = actorBusy.GetIsBusy();
        bool isRunning = runAction.ReadValue<float>() > 0f;
        Vector2 movement = Vector2.zero;

        if ((isBusy || isRunning) && isSneaking)
        {
            // exit sneak mode if the player is busy or started running
            ToggleIsSneaking();
        }

        if (!isBusy)
        {
            // calculate the player movement vector
            float speed = isSneaking ? sneakSpeed : isRunning ? runSpeed : walkSpeed;
            movement = moveAction.ReadValue<Vector2>() * speed;
        }

        // calculate vertical speed
        if (characterController.isGrounded)
        {
            verticalSpeed = 0f;
        }
        else
        {
            verticalSpeed -= gravity * Time.deltaTime;
        }

        // accelerate towards the movement vector
        targetMovement = Vector3.Lerp(
            targetMovement,
            new Vector3(movement.x, 0f, movement.y),
            acceleration * Time.deltaTime
        );

        // apply movement to player character
        animator.SetFloat("Speed", GetNormalizedSpeed());
        transform.forward = Vector3.Slerp(
            transform.forward,
            (transform.forward - targetMovement * -1f).normalized,
            turnSpeed * Time.deltaTime
        );
        characterController.Move((targetMovement + Vector3.up * verticalSpeed) * Time.deltaTime);
    }
}
