using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : HealthSystem
{
	public override void Start()
	{
		healthBar = GameObject.Find("Player Health Bar").GetComponent<HealthBar>();
		base.Start();
	}
}
