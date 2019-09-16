using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
	// Inspector Variables
	[Space]
	[Header("Motor Configuration")]
	[SerializeField] private float walkSpeed = 10f;
	[SerializeField] private float runSpeed = 15f;

	[Space]
	[Header("Input Asset")]
	[SerializeField] private InputActionAsset playerControls; // Player controls

	[Space]
	[Header("Movement Configuration")]
	[SerializeField] private float gravity = 10f;
	[SerializeField] private float maxVelocityChange = 10f;
	[SerializeField] private Transform groundCheck;
	[SerializeField] private Transform camTransform;
	[SerializeField] private LayerMask groundMask;
	[SerializeField] private MouseLook mouseLook;

	// Private Variables
	private const float groundedRadius = 0.2f;
	private float horizontal;
	private float vertical;
	private Vector3 direction;
	private bool isGrounded = true;

	// Components
	private Rigidbody rBody;
	private Camera cam;

    // Input Actions
	private InputAction movement;

	private void Awake()
	{
		InitializeInput();

		rBody = GetComponent<Rigidbody>();
		cam = Camera.main;
		rBody.freezeRotation = true;
		rBody.useGravity = false;
	}

	private void OnEnable()
	{
		movement.Enable();
	}

	private void Start()
	{
		mouseLook = new MouseLook();
		mouseLook.Init(transform, cam.transform);
	}

	private void Update()
	{
		RotateView();
	}

	private void FixedUpdate()
	{
		Movement();
		CheckForGround();
	}

     private void InitializeInput()
    {
        InputActionMap playerActionMap = playerControls.GetActionMap("Player");
        movement = playerActionMap.GetAction("Movement");

        // Subscribe Input Events
        movement.performed += OnMovementChanged;
		movement.canceled += OnMovementChanged;
    }

	private void OnMovementChanged(InputAction.CallbackContext context)
	{
		var _direction = context.ReadValue<Vector2>();
		direction = new Vector3(_direction.x, 0f, _direction.y);
	}

	private void Movement()
	{
		if (isGrounded)
		{
			Vector3 targetVelocity = direction;
			targetVelocity = transform.TransformDirection(targetVelocity); // Here convert direction to worldspace soo we actually move in the right direction.

			targetVelocity *= walkSpeed;

			Vector3 velocity = rBody.velocity;
			Vector3 velocityChange = (targetVelocity - velocity);

			velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
			velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
			velocityChange.y = 0f;

			rBody.AddForce(velocityChange, ForceMode.VelocityChange);
		}

		rBody.AddForce(new Vector3(0f, -gravity * rBody.mass, 0f)); // Add gravity
        mouseLook.UpdateCursorLock();
	}

	private void CheckForGround()
	{
		isGrounded = false;
		Collider[] groundColliders = Physics.OverlapSphere(groundCheck.position, groundedRadius, groundMask);

		for (int i = 0; i < groundColliders.Length; i++)
		{
			if (groundColliders[i].gameObject != this.gameObject)
			{
				isGrounded = true;
			}
		}
	}

	private void RotateView()
	{
		mouseLook.LookRotation(transform, camTransform);
	}
}
