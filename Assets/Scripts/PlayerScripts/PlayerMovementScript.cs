using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    private Transform cameraTransform;
    private float moveSpeed = 5f;
    private CharacterController characterController;
    private float rotationSpeed = 5f;
    private Vector3 playerVelocity;
    private float gravityValue = -9.81f;
    private Animator animator;
    private bool groundedPlayer = false;
    private float lastSlideTime;
    public bool rolling = false;
    private float dodgeForce = 200f;
    private bool isRunning = false;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        cameraTransform = Camera.main.transform;
    }

    private void AnimatePlayer()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (!isRunning)
            {
                animator.SetBool("Running", gameObject.GetComponent<Rigidbody>().velocity.magnitude > 8);
                isRunning = true;
            }
        }
        else
        {
            if (isRunning)
            {
                animator.SetBool("Running", false);
                isRunning = false;
            }
        }

        animator.SetBool("PistolWalking",
            Input.GetKey(KeyCode.W) ||
            Input.GetKey(KeyCode.S) ||
            Input.GetKey(KeyCode.A) ||
            Input.GetKey(KeyCode.D));
    }

    private void Update()
    {
        AnimatePlayer();
        CheckSlideAnimation();

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
        characterController.Move(moveDirection * (Input.GetKey(KeyCode.LeftShift) ? moveSpeed * 2 : moveSpeed) * Time.deltaTime);
        
        if(GetComponent<Rigidbody>().velocity.magnitude > 8f || Input.GetMouseButton(1))
        {
            // todo: rotate player body
            float targetAngle = cameraTransform.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0, targetAngle, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }        

        playerVelocity.y += gravityValue * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);
    }

    private void CheckSlideAnimation()
    {
        // check slide buffer
        if (Time.time - lastSlideTime > 5f)
        {
            GetComponent<Collider>().isTrigger = false;
        }

        // check if slide animation is playing
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Slide"))
        {
            // check if slide animation is finished
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                // transition to running animation
                animator.Play("Idle", 0, 0.0f);
            }
        }
        else
        {
            rolling = false;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                characterController.Move(transform.forward * Time.deltaTime * dodgeForce);
                animator.Play("Slide", 0, 0.0f);
                GetComponent<Collider>().isTrigger = true;
                rolling = true;
                lastSlideTime = Time.time;
            }
        }
    }
}
