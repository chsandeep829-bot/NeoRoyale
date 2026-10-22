using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public Camera viewCamera;
    [Min(0f)] public float walkSpeed = 5f;
    [Min(0f)] public float sprintSpeed = 8f;
    [Min(0f)] public float jumpHeight = 1.4f;
    public float gravity = -20f;
    [Min(0.1f)] public float sensitivity = 2f;
    [Min(0.5f)] public float cameraDistance = 4f;
    [Min(0f)] public float cameraHeight = 1.6f;

    private CharacterController controller;
    private float verticalVelocity;
    private float yaw;
    private float pitch = 12f;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        if (!viewCamera) viewCamera = Camera.main;
        yaw = transform.eulerAngles.y;
    }

    private void Start()
    {
        LockCursor(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            LockCursor(false);

        if (Input.GetMouseButtonDown(0))
            LockCursor(true);

        if (Cursor.lockState == CursorLockMode.Locked)
        {
            yaw += Input.GetAxis("Mouse X") * sensitivity;
            pitch -= Input.GetAxis("Mouse Y") * sensitivity;
            pitch = Mathf.Clamp(pitch, -20f, 60f);
        }

        Vector3 input = transform.right * Input.GetAxisRaw("Horizontal")
                      + transform.forward * Input.GetAxisRaw("Vertical");
        input = Vector3.ClampMagnitude(input, 1f);

        float speed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;
        controller.Move(input * speed * Time.deltaTime);

        if (controller.isGrounded && verticalVelocity < 0f)
            verticalVelocity = -2f;

        if (controller.isGrounded && Input.GetKeyDown(KeyCode.Space))
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);

        verticalVelocity += gravity * Time.deltaTime;
        controller.Move(Vector3.up * verticalVelocity * Time.deltaTime);

        if (input.sqrMagnitude > 0.01f)
            transform.rotation = Quaternion.Euler(0f, yaw, 0f);
    }

    private void LateUpdate()
    {
        if (!viewCamera) return;

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 focus = transform.position + Vector3.up * cameraHeight;
        Vector3 position = focus - rotation * Vector3.forward * cameraDistance;
        viewCamera.transform.SetPositionAndRotation(position, rotation);
    }

    private static void LockCursor(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }
}