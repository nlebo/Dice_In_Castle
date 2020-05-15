using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Spawn : MonoBehaviour
{
    public GameObject[] Enemys;
    public List<GameObject> _E;
    public float FlowTime;
    public float SpawnTime;
    RoundManager Round;

    public static Enemy_Spawn m_Instance;

    [SerializeField]
    int M_HP;
    int count;
    // Start is called before the first frame update
    void Start()
    {
        Round = RoundManager.m_Instance;
        count = 0;
        _E=new List<GameObject>();

        Board_Manager.Initialize += Init;
        m_Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(Board_Manager.m_Instance.Stop) return;
        
        if(count >= _E.Count) return;
        FlowTime += Time.deltaTime;

        if(FlowTime>=SpawnTime)
        {
            _E[count].SetActive(true);
            count++;
            FlowTime = 0;
        }

        
    }

    public void CreateUnit(int camp, int ENum, int Level, int HP)
    {
        Unit_Manager E = Instantiate(Enemys[ENum], transform).GetComponent<Unit_Manager>();
        E.PlayerNum = 2;
        E.camp = camp;
        E.LV = Level;
        E.HP = HP;
        E.gameObject.SetActive(false);
        E.GetComponent<SpriteRenderer>().flipX = false;
        _E.Add(E.gameObject);
    }

    public void RoundStart()
    {
        _E.Clear();
        count = 0;
        FlowTime = 0;

        int round = Round.Round;

        if(round > 1 && round % 2 == 1) M_HP = M_HP + (int)(M_HP / 3);
        for(int i=0; i< 2 + (round - 1);i++)
        {
            CreateUnit(-2,0,round/10,M_HP);
        }

    }
    public void Init()
    {
        count = 0;
        
        FlowTime = 0;
        _E.Clear();
        for(int i =0;i<transform.childCount;i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

    }
}
