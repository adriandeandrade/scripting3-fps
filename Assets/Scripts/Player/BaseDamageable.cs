using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseDamageable : MonoBehaviour, IDamageable
{
	// Inspector Fields
	[Header("Damageable Configuration")]
	[SerializeField] protected float startHealth;
	[SerializeField] protected MeshRenderer mr;
    [SerializeField] protected Color damageColor;

	// Private Variables
	protected float currentHealth;
    protected Color originalColor;

	// Events
	public delegate void OnTakeDamageAction();
	public static event OnTakeDamageAction OnTakeDamageEvent;

	protected virtual void Awake()
	{
        originalColor = mr.material.GetColor("_BaseColor");
	}

	protected virtual void Start()
	{
		currentHealth = startHealth;
	}

	public void TakeDamage(float amount)
	{
		RecalculateHealth(amount);
        StartCoroutine(OnTakeDamage());

		if (currentHealth <= 0)
		{
			OnDeath();
			return;
		}

		if(OnTakeDamageEvent != null)
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

    IEnumerator OnTakeDamage()
    {
        mr.material.SetColor("_BaseColor", damageColor);
        yield return new WaitForSeconds(0.5f);
        mr.material.SetColor("_BaseColor", originalColor);
        yield break;
    }
}
