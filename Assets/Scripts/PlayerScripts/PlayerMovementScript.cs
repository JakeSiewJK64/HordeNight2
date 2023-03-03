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
        animator.SetBool("PistolWalking", WASDCheck());
        animator.SetBool("Running", isRunning);
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

        isRunning = Input.GetKey(KeyCode.LeftShift);

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveDirection = 
            horizontal * cameraTransform.right.normalized + vertical * cameraTransform.forward.normalized;

        moveDirection.y = 0;
        
        characterController.Move(moveDirection * (Input.GetKey(KeyCode.LeftShift) ? moveSpeed * 2 : moveSpeed) * Time.deltaTime);
        
        if(WASDCheck() || Input.GetMouseButton(1) || Input.GetMouseButton(0))
        {
            // todo: rotate player body
            float targetAngle = cameraTransform.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0, targetAngle, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }        

        playerVelocity.y += gravityValue * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);
    }

    private bool WASDCheck()
    {
        return Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);
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
                rolling = false;
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

    public bool GetRunning()
    {
        return isRunning;
    }

    public bool GetSliding()
    {
        return rolling;
    }
}
