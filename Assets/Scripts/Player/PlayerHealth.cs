using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : HealthSystem
{
	public override void TakeDamage(int damage)
	{
		currentHealth = maxHealth;
	}
}
