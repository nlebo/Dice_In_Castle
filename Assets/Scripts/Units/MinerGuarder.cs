using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MinerGuarder : Unit_Manager
{
    bool Arrive;

    protected override void Update()
    {
        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position + (Vector3.up * 0.5f) + (Vector3.right * camp * Attack_Range), transform.right * camp, Attack_Range * 2, lay);

        _State = State.Idle;
        Enemy.Clear();

        if (camp == 1 ? transform.position.x >= WarBoard_Manager.m_Instance.OreT.position.x - 1 : transform.position.x <= WarBoard_Manager.m_Instance.OreT.position.x + 1)
        {
            Arrive = true;
        }

        if (Arrive)
        {
            for (int i = 0; i < hit.Length; i++)
            {
                if (hit[i].transform.gameObject.layer == 8 && hit[i].transform.gameObject.tag != "Miner" && hit[i].transform.GetComponent<Unit_Manager>().camp != camp)
                {
                    Enemy.Add(hit[i].transform.GetComponent<Unit_Manager>());
                    _State = State.Attack;


                    hitEnemy++;
                }
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
            ATime = 0;
            transform.Translate(transform.right * MoveSpeed * Time.deltaTime * camp);
        }

        else if(_State == State.Attack && Arrive)
        {
            Attack();
        }
    }
}
