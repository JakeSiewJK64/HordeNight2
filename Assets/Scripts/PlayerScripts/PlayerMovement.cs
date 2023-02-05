using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    private CharacterController controller;
    private bool sprinting = false;
    private float playerSpeed = 2;
    private float rotationSpeed = 2;
    Transform playerTransform;

    private void Start()
    {
        controller = gameObject.AddComponent<CharacterController>();
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        playerTransform = transform;

        Vector3 forward = playerTransform.forward;
        forward.y = 0;
        forward = forward.normalized;

        Vector3 right = playerTransform.right;
        right.y = 0;
        right = right.normalized;

        Vector3 moveDirection = forward * vertical + right * horizontal;

        controller.Move(moveDirection * playerSpeed * Time.deltaTime);

        playerTransform.Rotate(Vector3.up * rotationSpeed * horizontal * Time.deltaTime);
    }
}
