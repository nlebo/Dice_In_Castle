﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit_Manager : MonoBehaviour
{
    // Start is called before the first frame update
    public enum State{Idle,Attack};
    public State _State;
    public int camp;
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
    }

    // Update is called once per frame
    protected virtual void Update()
    {
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
            else
                Castle.Hit(Damage);
            ATime = 0;
        }
    }

    virtual public void Hit(int Damage)
    {
        PrevColor = GetComponent<SpriteRenderer>().color;
        HP -= Damage;
        //transform.Translate(transform.right * MoveSpeed * -camp);
        _hit = true;
        hitNTime = 0;
        if(HP <= 0) Death();
    }

    virtual protected void Death()
    {
        if(camp == -1) Board_Manager.m_Instance.UpCoin(1);
        Board_Manager.Initialize -= Init;
        Destroy(gameObject);
    }

    public void Slowing(float decrease,float T)
    {
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

    public void Init()
    {
        Board_Manager.Initialize -= Init;
        Destroy(gameObject);
    }
}