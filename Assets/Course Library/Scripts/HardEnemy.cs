using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardEnemy : Enemy
{



    private new float speed = 5f;
    protected override void Smashed()
    {
        if (playerControllerScript.aboutToSmash && player.transform.position.y < 0.3)
        {
            enemyRb.AddForce(-lookDirection * 25) ;
        }
    }

    protected override void Target()
    {
        enemyRb.AddForce(lookDirection.normalized * speed);
    }

}
