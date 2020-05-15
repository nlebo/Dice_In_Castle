using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;


public class Unit_Manager : MonoBehaviourPun , IPunObservable
{
    // Start is called before the first frame update
    public enum State{Idle,Attack};
    public State _State;
    protected int compulsion_State;
    public int camp;
    public int PBoard_Num;
    public float MoveSpeed, Attack_Range;

    public LayerMask lay;
    public int Damage;
    public int HP;
    public int LV;

    public List<Unit_Manager> Enemy;
    public Castle_Manager Castle;
    public float ATime,Attack_Delay;
    protected bool _hit;
    protected float hitTime,hitNTime;
    protected int hitEnemy;
    protected bool icy;
    public Animator Anim;
    public int PlayerNum;
    Color PrevColor;
    protected virtual void Start()
    {
        Enemy = new List<Unit_Manager>();
        _State = State.Idle;
        Damage += LV * 2;
        HP += LV * 2;
        hitTime = 0.5f;
        hitEnemy = 0;
        Board_Manager.Initialize += Init;

        // int PE = PlayerNum == PhotonNetwork.LocalPlayer.ActorNumber ? 0 : 1;
        // transform.position = WarBoard_Manager.m_Instance.SpawnPoints[PE].position;
        // transform.parent = WarBoard_Manager.m_Instance.SpawnPoints[PE];
        camp = PlayerNum == 0 ? 1 : -1;
        GetComponent<SpriteRenderer>().flipX = PlayerNum == 0 ? true : false;
        compulsion_State = 0;
        Anim = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if(Board_Manager.m_Instance.Stop) return;
        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position + Vector3.up * 0.5f,transform.right * camp, Attack_Range,lay);
        
        _State = State.Idle;
        Enemy.Clear();
        Castle = null;

        for(int i=0;i<hit.Length;i++)
        {
            if(hit[i].transform.gameObject.layer == 8 && hit[i].transform.GetComponent<Unit_Manager>().camp != camp)
            {
                Enemy.Add(hit[i].transform.GetComponent<Unit_Manager>());
                _State = State.Attack;


                hitEnemy++;
            }
            else if(hit[i].transform.gameObject.layer == 9 && hit[i].transform.GetComponent<Castle_Manager>().Camp != camp)
            {
                Castle = hit[i].transform.GetComponent<Castle_Manager>();
                _State = State.Attack;
            }
        }


        // if(!PhotonNetwork.IsMasterClient)
        //     _State = compulsion_State == 0 ? State.Idle : _State;

        // else if(_State == State.Idle ? (compulsion_State == 1) : (compulsion_State == 0))
        // {   
        //     compulsion_State = compulsion_State == 0 ? 1 : 0;
        //     photonView.RPC("RPCCompulsion",RpcTarget.Others,compulsion_State);
        // }

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

        if (_State == State.Idle)
        {
            if(!Anim.GetBool("Walk")) Anim.SetBool("Walk",true);
            ATime = 0;
            transform.Translate(transform.right * MoveSpeed * Time.deltaTime * camp);
        }

        else if(_State == State.Attack)
        {
            if(Anim.GetBool("Walk")) Anim.SetBool("Walk",false);
            Attack();
        }

        
    }
    // [PunRPC]
    // public void RPCCompulsion(int input)
    // {
    //     compulsion_State = input;
    // }

    virtual public void Attack()
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
                Enemy[0].Hit(Damage);
            else{
                Castle.Hit(Damage);
                Death();
            }
            ATime = 0;
        }
    }

    virtual public void Hit(int _Damage)
    {
        // if(!PhotonNetwork.IsMasterClient) return;

       
        //photonView.RPC("RPCHit",RpcTarget.All,_Damage);
        PrevColor = GetComponent<SpriteRenderer>().color;
        HP -= _Damage;
        //transform.Translate(transform.right * MoveSpeed * -camp);
        _hit = true;
        hitNTime = 0;

        if(HP - _Damage <= 0){ 
            //photonView.RPC("RPCDeath",RpcTarget.All);
            Death();
        }
        else
             Instantiate(FX_Manager.m_Instance.Hit,transform.position + FX_Manager.m_Instance.Hit.transform.position,Quaternion.identity);
    }

    [PunRPC]
    public void RPCHit(int _Damage)
    {
        // PrevColor = GetComponent<SpriteRenderer>().color;
        // HP -= _Damage;
        // //transform.Translate(transform.right * MoveSpeed * -camp);
        // _hit = true;
        // hitNTime = 0;
    }

    [PunRPC]
    public void RPCDeath()
    {
        Death();
    }

    virtual protected void Death()
    {
        if(camp == -1) Board_Manager.m_Instance.UpCoin(1);
        Board_Manager.Initialize -= Init;
        WarBoard_Manager.m_Instance.UnitCount[camp == 1 ? 0 : 1]--;
        if (camp == 1){
            UI_Manager.m_Instance.UpdateText(UI_Manager.m_Instance.MaxNowUnit, $"({WarBoard_Manager.m_Instance.UnitCount[0]}/{WarBoard_Manager.m_Instance.MaxUnit[0]})");
            WarBoard_Manager.m_Instance.DeathUnit(PBoard_Num);
        }
        Instantiate(FX_Manager.m_Instance.Bomb,transform.position + FX_Manager.m_Instance.Hit.transform.position,Quaternion.identity);
        Destroy(gameObject);
    }

    public void Slowing(float decrease,float T)
    {
        //photonView.RPC("RPCSlow",RpcTarget.All,decrease,T);
        StartCoroutine(SlowDown(decrease,T));
    }
    public IEnumerator SlowDown(float decrease, float T)
    {
        MoveSpeed -= MoveSpeed * (decrease/100);
        Attack_Delay += Attack_Delay * (decrease/100); 
        icy = true;
        GetComponent<SpriteRenderer>().color = new Color(179.0f/255,194.0f/255,1,1);
        yield return new WaitForSeconds(T);

        icy = false;
        MoveSpeed += MoveSpeed * (decrease/100);
        Attack_Delay -= Attack_Delay * (decrease/100);
        GetComponent<SpriteRenderer>().color = Color.white;
        yield return null;
    }

    // [PunRPC]
    // public void RPCSlow(float decrease,float T)
    // {
    //     StartCoroutine(SlowDown(decrease,T));
    // }
    public void Init()
    {
        Board_Manager.Initialize -= Init;
        Destroy(gameObject);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting && PhotonNetwork.IsMasterClient) stream.SendNext(HP);
        else if(stream.IsReading && !PhotonNetwork.IsMasterClient) HP = (int)stream.ReceiveNext();

    }
}
