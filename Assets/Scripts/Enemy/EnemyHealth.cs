using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : HealthSystem
{
    public override void Die()
    {
        base.Die();
        StartCoroutine(ClearDeadBody());
    }

    // TODO: Create object pool and use zombies from pool
    private IEnumerator ClearDeadBody() {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
