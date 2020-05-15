using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Red : Unit_Manager
{
    override public void Attack()
    {
         ATime += Time.deltaTime;
        if (ATime >= Attack_Delay)
        {
            if (Enemy == null && Castle == null)
            {
                _State = State.Idle;
                return;
            }

            if (Enemy.Count > 0){

                for(int i=0;i<Enemy.Count;i++){
                    Enemy[i].Hit(Damage);
                }
            }
            if(Castle != null){
                Castle.Hit(Damage);
                Death();
            }
            ATime = 0;
        }
    }

}
