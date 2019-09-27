using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
	[SerializeField] private Image healthBar;

	// Private Variables
	private bool canSeePlayer;
	private bool doTracking = true;
	private float nextFireTime;
	private float currentFireDelay;
	private TurretStates currentState;

	// Components
	private Player target;
	private Animator animator;

	private void OnEnable()
	{
		BaseDamageable.OnTakeDamageEvent += UpdateHealth;
	}

	protected void Awake()
	{
		target = FindObjectOfType<Player>();
		animator = GetComponent<Animator>();
	}

	protected override void Start()
	{
		base.Start();

		currentState = TurretStates.Idle;
		nextFireTime = Time.time + fireRate;
	}

	private void Update()
	{
		if (doTracking)
		{
			RaycastToTarget();
			UpdateStates();
		}
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
		animator.SetTrigger("Shoot");
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
		directionToTarget.y = partToRotate.transform.localPosition.y;
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

	private void UpdateHealth()
	{
		Debug.Log("Turret Took damage");

		if (healthBar != null)
		{
			healthBar.fillAmount = currentHealth / startHealth;
		}
	}
}
