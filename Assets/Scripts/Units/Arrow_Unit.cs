using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow_Unit : Unit_Manager
{
    public Projectile_Manager Arrow;
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

            if (Enemy.Count > 0){
                int y = camp == 1 ? 0 : 180;
                Projectile_Manager A = Instantiate(Arrow.gameObject,transform.position + Vector3.up * 0.5f,Arrow.transform.rotation * Quaternion.Euler(0,0,y)).GetComponent<Projectile_Manager>();
                
                A.Shot(transform.position + Vector3.up * 0.5f,Enemy[0].transform.position + Vector3.up * 0.5f,0.2f,Enemy[0],Damage);
            }
            else{
                int y = camp == 1 ? 0 : 180;
                Projectile_Manager A = Instantiate(Arrow.gameObject,transform.position + Vector3.up * 0.5f,Arrow.transform.rotation * Quaternion.Euler(0,0,y)).GetComponent<Projectile_Manager>();
                A.Shot(transform.position + Vector3.up * 0.5f,Castle.transform.position + Vector3.up * 0.5f,0.2f,Castle,Damage);
            }
            ATime = 0;
        }
    }
}
