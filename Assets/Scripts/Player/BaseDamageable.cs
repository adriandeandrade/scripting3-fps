using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseDamageable : MonoBehaviour, IDamageable
{
	// Inspector Fields
	[Header("Damageable Configuration")]
	[SerializeField] protected float startHealth;

	// Private Variables
	protected float currentHealth;

	// Events
	public delegate void OnTakeDamageAction();
	public static event OnTakeDamageAction OnTakeDamageEvent;

	protected virtual void Start()
	{
		currentHealth = startHealth;
	}

	public virtual void TakeDamage(float amount)
	{
		RecalculateHealth(amount);

		if (currentHealth <= 0)
		{
			OnDeath();
			return;
		}

		OnTakeDamage();
	}

	public void OnTakeDamage()
	{
		if (OnTakeDamageEvent != null)
		{
			OnTakeDamageEvent.Invoke();
		}
	}

	public virtual void RecalculateHealth(float amount)
	{
		currentHealth -= amount;
		// Run whatever happens when you lose health;
	}

	public virtual void OnDeath()
	{
		Destroy(gameObject);
	}
}
