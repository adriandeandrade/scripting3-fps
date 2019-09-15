using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
	// Inspector Variables
	[Space]
	[Header("Motor Configuration")]
	[SerializeField] private float walkSpeed = 10f;
	[SerializeField] private float runSpeed = 15f;

	[Space]
	[Header("Movement Configuration")]
	[SerializeField] private float gravity = 10f;
	[SerializeField] private float maxVelocityChange = 10f;
	[SerializeField] private float jumpHeight = 20f;
	[SerializeField] private float maxDistanceFromWall = 5f;
	[SerializeField] private Transform groundCheck;
    [SerializeField] private Transform camTransform;
	[SerializeField] private LayerMask groundMask;
	[SerializeField] private MouseLook mouseLook;


	// Private Variables
	private const float groundedRadius = 0.2f;
	private bool isGrounded = true;

	// Components
	private Rigidbody rBody;
	private Camera cam;

	private void Awake()
	{
		rBody = GetComponent<Rigidbody>();
		cam = Camera.main;
		rBody.freezeRotation = true;
		rBody.useGravity = false;
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

	private void Movement()
	{
		if (isGrounded)
		{
			float horizontal = Input.GetAxis("Horizontal");
			float vertical = Input.GetAxis("Vertical");

			Vector3 targetVelocity = new Vector3(horizontal, 0f, vertical);
			targetVelocity = transform.TransformDirection(targetVelocity);

			targetVelocity *= walkSpeed;

			Vector3 velocity = rBody.velocity;
			Vector3 velocityChange = (targetVelocity - velocity);

			velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
			velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
			velocityChange.y = 0f;

			rBody.AddForce(velocityChange, ForceMode.VelocityChange);
		}

		rBody.AddForce(new Vector3(0f, -gravity * rBody.mass, 0f));
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
