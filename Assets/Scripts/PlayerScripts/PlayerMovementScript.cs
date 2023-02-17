using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    private Transform cameraTransform;
    private float moveSpeed = 5f;
    private CharacterController characterController;
    private Transform playerTransform;
    private float rotationSpeed = 5f;
    private Vector3 playerVelocity;
    private float gravityValue = -9.81f;
    private Animator animator;
    private bool groundedPlayer = false;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        playerTransform = transform;
        cameraTransform = Camera.main.transform;
    }

    private void AnimatePlayer()
    {
        animator.SetBool("PistolWalking", 
            Input.GetKey(KeyCode.W) ||
            Input.GetKey(KeyCode.S) ||
            Input.GetKey(KeyCode.A) ||
            Input.GetKey(KeyCode.D)
        );
    }

    private void Update()
    {
        AnimatePlayer();

        groundedPlayer = characterController.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveDirection = 
            horizontal * cameraTransform.right.normalized + vertical * cameraTransform.forward.normalized;
        moveDirection.y = 0;
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
        
        if(GetComponent<Rigidbody>().velocity.magnitude > 8f)
        {
            // todo: rotate player body
            float targetAngle = cameraTransform.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0, targetAngle, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }        

        playerVelocity.y += gravityValue * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);
    }
}
