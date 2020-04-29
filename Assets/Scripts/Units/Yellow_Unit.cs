using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Yellow_Unit : Unit_Manager
{
    bool Arrive;
    List<int> Miner;

    protected override void Start()
    {
        base.Start();
        Miner = new List<int>();
    }
    protected override void Update()
    {
         RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position + Vector3.up * 0.5f,transform.right * camp, Attack_Range,lay);

        Miner.Clear();
        Enemy.Clear();

        _State = State.Idle;
        if(camp == 1 ? transform.position.x >= WarBoard_Manager.m_Instance.OreStop[1].position.x : transform.position.x <= WarBoard_Manager.m_Instance.OreStop[0].position.x )
        {
            Arrive = true;
        }

        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].transform.gameObject.tag == "Miner" && hit[i].transform.GetComponent<Unit_Manager>().camp != camp)
            {
                Enemy.Add(hit[i].transform.GetComponent<Unit_Manager>());
                Miner.Add(Enemy.Count-1);
                _State = State.Attack;
            }
            else if(hit[i].transform.gameObject.tag == "MinerGuard" && hit[i].transform.GetComponent<Unit_Manager>().camp != camp)
            {
                Enemy.Add(hit[i].transform.GetComponent<Unit_Manager>());
                _State = State.Attack;
            }

        }

        if(!PhotonNetwork.IsMasterClient)
            _State = compulsion_State == 0 ? State.Idle : _State;

        else if(_State == State.Idle ? (compulsion_State == 1) : (compulsion_State == 0))
        {   
            compulsion_State = compulsion_State == 0 ? 1 : 0;
            photonView.RPC("RPCCompulsion",RpcTarget.Others,compulsion_State);
        }

        if(_hit)
        {
            if(hitNTime == 0) GetComponent<SpriteRenderer>().color = Color.red;
            hitNTime += Time.deltaTime;

            if(hitNTime>= hitTime)
            {
                hitNTime = 0;
                _hit = false;
                GetComponent<SpriteRenderer>().color = icy ? new Color(179.0f / 255, 194.0f / 255, 1, 1) : Color.white;
            }
        }

        if (_State == State.Idle && !Arrive)
        {
            transform.Translate(transform.right * MoveSpeed * Time.deltaTime * camp);
        }
        else if(_State == State.Idle)
        {
            ATime = 0;
        }

        else if(_State == State.Attack)
        {
            Attack();
        }
    }

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
            {
                if (Miner.Count > 0) Enemy[Miner[0]].Hit(Damage);

                else
                    Enemy[0].Hit(Damage - 3);
            }
            ATime = 0;
        }
    }

}
