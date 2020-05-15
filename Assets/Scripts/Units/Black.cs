using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Black : Unit_Manager
{
    public int RandomRange;

    public override void Attack()
    {
        ATime += Time.deltaTime;
        if (ATime >= Attack_Delay)
        {
            if (Enemy == null && Castle == null)
            {
                _State = State.Idle;
                return;
            }

            if (Enemy.Count > 0)
                Enemy[0].Hit(Damage + Random.Range(0,RandomRange + 1));
            else{
                Castle.Hit(Damage + Random.Range(0,RandomRange + 1));
                //Death();
            }
            ATime = 0;
        }
    }
}
