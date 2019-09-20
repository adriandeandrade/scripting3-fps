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
	[SerializeField] private float runSpeed = 15f;
	[SerializeField] private float jumpMultiplier;
	[SerializeField] private AnimationCurve jumpFallOff;

	[Space]
	[Header("Movement Configuration")]
	[SerializeField] private float gravity = 10f;
	[SerializeField] private float maxVelocityChange = 10f;
	[SerializeField] private Transform groundCheck; // Foot position. Used as a origin point for the overlap sphere checking if we are grounded or not.
	[SerializeField] private Transform camTransform; // The Transform of our camera for the mouse look component.
	[SerializeField] private LayerMask groundMask;

	[Header("Mouse Configuration")]
	[SerializeField] private float horizontalSensitivity;
	[SerializeField] private float verticalSensitivity;
	[SerializeField] private MouseLook mouseLook;

	// Private Variables
	private const float groundedRadius = 0.2f; // The radius of the overlap sphere checking if we are grounded.

	private float horizontal;
	private float vertical;

	private Vector3 direction; // Movement direction based on movement inputs.

	private bool isGrounded = true;
	private bool isJumping = false;

	// Components
	private Rigidbody rBody;
	private Camera cam;
	private CapsuleCollider capsuleCollider;

	private void Awake()
	{
		rBody = GetComponent<Rigidbody>();
		capsuleCollider = GetComponent<CapsuleCollider>();
		cam = Camera.main;

		rBody.freezeRotation = true;
		rBody.useGravity = false;
	}


	private void Start()
	{
		InitializeInput();
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
		// Subscribe Input Events
		Toolbox.instance.GetInputManager().movementControls.performed += OnMovementChanged;
		Toolbox.instance.GetInputManager().movementControls.canceled += OnMovementChanged;
		
		Toolbox.instance.GetInputManager().jumpControl.performed += JumpInput;

		mouseLook = new MouseLook();
		mouseLook.Init(transform, cam.transform);
		mouseLook.XSensitivity = horizontalSensitivity;
		mouseLook.YSensitivity = verticalSensitivity;
	}

	private void OnMovementChanged(InputAction.CallbackContext context)
	{
		var _direction = context.ReadValue<Vector2>();
		direction = new Vector3(_direction.x, 0f, _direction.y);
	}

	private void Movement()
	{
		Vector3 targetVelocity = direction;
		targetVelocity = transform.TransformDirection(targetVelocity); // Here convert direction to worldspace so we actually move in the right direction.

		targetVelocity *= runSpeed;

		Vector3 velocity = rBody.velocity;
		Vector3 velocityChange = (targetVelocity - velocity);

		velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
		velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
		velocityChange.y = 0f;

		rBody.AddForce(velocityChange, ForceMode.VelocityChange);

		if (!isGrounded)
		{
			rBody.AddForce(new Vector3(0f, -gravity * rBody.mass, 0f)); // Add gravity
		}

		mouseLook.UpdateCursorLock();
	}

	private void JumpInput(InputAction.CallbackContext context)
	{
		if (!isJumping)
		{
			isJumping = true;
			StartCoroutine(Jump());
		}
	}

	private void CheckForGround()
	{
		isJumping = true;
		isGrounded = false;
		Collider[] groundColliders = Physics.OverlapSphere(groundCheck.position, groundedRadius, groundMask);

		for (int i = 0; i < groundColliders.Length; i++)
		{
			if (groundColliders[i].gameObject != this.gameObject)
			{
				isJumping = false;
				isGrounded = true;
			}
		}
	}

	private void RotateView()
	{
		mouseLook.LookRotation(transform, camTransform);
	}

	private IEnumerator Jump()
	{
		isJumping = true;
		float timeInAir = 0f;

		do
		{
			float jumpForce = jumpFallOff.Evaluate(timeInAir);
			rBody.velocity = Vector3.up * jumpForce * jumpMultiplier;
			timeInAir += Time.deltaTime;
			yield return null;
		} while (!isGrounded);

		isJumping = false;
	}
}
