using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TurretStates
{
	Idle, Rotating, Engaging
}

public class Turret1 : BaseDamageable
{
	// Inspector Fields
	[Header("Turret Configuration")]
	[SerializeField] private float rotateSpeed = 100f;
	[SerializeField] private float maxAttackDistance = 20f;
	[SerializeField] private float fireRate;
	[SerializeField] private float fireDelay = 1f;
	[SerializeField] private Transform firePoint;
	[SerializeField] private Transform partToRotate;
	[SerializeField] private Transform gun;

	// Private Variables
	private bool canSeePlayer;
	private float nextFireTime;
	private float currentFireDelay;
	private TurretStates currentState;
	private Color originalColor;

	// Components
	private Player target;
	private MeshRenderer gunMr;

	private void Awake()
	{
		target = FindObjectOfType<Player>();
		gunMr = gun.GetComponent<MeshRenderer>();
		originalColor = gunMr.material.GetColor("_BaseColor");
	}

	protected override void Start()
	{
		base.Start();

		currentState = TurretStates.Idle;
		nextFireTime = Time.time + fireRate;
	}

	private void Update()
	{
		RaycastToTarget();
		UpdateStates();
	}

	private void SetState(TurretStates newState)
	{
		switch (newState)
		{
			case TurretStates.Idle:
				currentState = TurretStates.Idle;
				InitIdleState();
				break;

			case TurretStates.Engaging:
				currentState = TurretStates.Engaging;
				InitEngagingState();
				break;

			case TurretStates.Rotating:
				currentState = TurretStates.Rotating;
				InitRotatingState();
				break;
		}
	}

	private void UpdateStates()
	{
		switch (currentState)
		{
			case TurretStates.Idle:
				UpdateIdleState();
				break;

			case TurretStates.Engaging:
				UpdateEngagingState();
				break;

			case TurretStates.Rotating:
				UpdateRotatingState();
				break;
		}
	}

	private void InitIdleState()
	{

	}

	private void InitEngagingState()
	{
		currentFireDelay = fireDelay;
	}

	private void InitRotatingState()
	{

	}

	private void UpdateIdleState()
	{
		if (CheckDistanceToTarget())
		{
			SetState(TurretStates.Rotating);
		}
	}

	private void UpdateEngagingState()
	{
		if (CheckDistanceAndCanSeeTarget())
		{
			if (currentFireDelay <= 0)
			{
				if (Time.time > nextFireTime)
				{
					Shoot();
				}
			}
			else
			{
				currentFireDelay -= Time.deltaTime;
			}
		}
		else
		{
			SetState(TurretStates.Idle);
		}
	}

	private void UpdateRotatingState()
	{
		RotateTurret();
		RotateGun();

		if (CheckDistanceAndCanSeeTarget())
		{
			SetState(TurretStates.Engaging);
		}
		else
		{
			SetState(TurretStates.Idle);
		}
	}

	private void Shoot()
	{
		ShootDamageRay();
		nextFireTime = Time.time + fireRate;
		gunMr.material.SetColor("_BaseColor", Color.red);
		Invoke("SetOriginalColor", 0.5f);
	}

	private void SetOriginalColor()
	{
		gunMr.material.SetColor("_BaseColor", originalColor);
	}

	private void ShootDamageRay()
	{
		RaycastHit hit;
		if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, maxAttackDistance))
		{
			if (hit.collider != null)
			{
				Player player = hit.collider.GetComponent<Player>();

				if (player != null)
				{
					hit.collider.GetComponent<IDamageable>().TakeDamage(2f);
				}
				else
				{
					Debug.Log("Turret missed!");
				}
			}
		}
	}

	private void RotateTurret()
	{
		Vector3 directionToTarget = target.transform.position - partToRotate.transform.position;
		directionToTarget.y = partToRotate.transform.position.y;
		Quaternion desiredRot = Quaternion.LookRotation(directionToTarget, Vector3.up);
		partToRotate.localRotation = Quaternion.RotateTowards(partToRotate.localRotation, desiredRot, rotateSpeed * Time.deltaTime);
	}

	private void RotateGun()
	{
		Vector3 aim;
		Vector3 directionToTarget = target.transform.position - gun.transform.position;

		aim.x = 0f;
		aim.y = directionToTarget.y;
		directionToTarget.y = 0f;
		aim.z = directionToTarget.magnitude;
		Quaternion desiredRot = Quaternion.LookRotation(aim, Vector3.up);
		gun.transform.localRotation = Quaternion.RotateTowards(gun.transform.localRotation, desiredRot, rotateSpeed * Time.deltaTime);
	}

	private void RaycastToTarget()
	{
		canSeePlayer = false;

		RaycastHit hit;
		if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, Mathf.Infinity))
		{
			if (hit.collider != null)
			{
				Player player = hit.collider.GetComponent<Player>();

				if (player != null)
				{
					canSeePlayer = true;
				}
				else
				{
					canSeePlayer = false;
				}
			}
		}
	}

	private float GetDistanceToTarget()
	{
		return Vector3.Distance(target.transform.position, transform.position);
	}

	private bool CheckDistanceAndCanSeeTarget()
	{
		if (GetDistanceToTarget() <= maxAttackDistance && canSeePlayer)
			return true;
		else
			return false;
	}

	private bool CheckDistanceToTarget()
	{
		return GetDistanceToTarget() <= maxAttackDistance;
	}
}
