using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yellow_Unit : Unit_Manager
{
    public float MTime,Mining_Delay;
    public int GetGold;
    bool Mining;
    protected override void Update()
    {
         RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position + Vector3.up * 0.5f,transform.right * camp, Attack_Range,lay);
        
        _State = State.Idle;

        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].transform.gameObject.layer == 10)
            {
                Mining = true;
                _State = State.Attack;
            }

        }

        if(_hit)
        {
            if(hitNTime == 0) GetComponent<SpriteRenderer>().color = Color.red;
            hitNTime += Time.deltaTime;

            if(hitNTime>= hitTime)
            {
                hitNTime = 0;
                _hit = false;
                GetComponent<SpriteRenderer>().color = icy ? new Color(179.0f/255,194.0f/255,1,1) : Color.white;
            }
        }

        if(_State == State.Idle)
        transform.Translate(transform.right * MoveSpeed * Time.deltaTime * camp);

        else if(_State == State.Attack)
        {
            Attack();
        }
    }

    public override void Attack()
    {
        MTime += Time.deltaTime;
        if(MTime >= Mining_Delay)
        {
            Board_Manager.m_Instance.UpCoin(GetGold + LV * GetGold);
            MTime = 0;
        }
    }
}
