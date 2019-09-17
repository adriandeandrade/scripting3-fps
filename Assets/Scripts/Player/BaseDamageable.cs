using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseDamageable : MonoBehaviour, IDamageable
{
	// Inspector Fields
	[Header("Damageable Configuration")]
	[SerializeField] private float startHealth;

	// Private Variables
	private float currentHealth;

	protected virtual void Start()
	{
        currentHealth = startHealth;
	}

	public void TakeDamage(float amount)
	{
		RecalculateHealth(amount);

		if (currentHealth <= 0)
		{
			OnDeath();
			return;
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
