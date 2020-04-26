using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Spawn : MonoBehaviour
{
    public GameObject[] Enemys;
    public List<GameObject> _E;
    public float FlowTime;
    int count;
    int[] SpawnTime;
    // Start is called before the first frame update
    void Start()
    {
        count = 0;
        
        _E=new List<GameObject>();
        for(int i =0;i<Enemys.Length;i++)
        {
            Unit_Manager E = Instantiate(Enemys[i],transform).GetComponent<Unit_Manager>();
            E.camp = -1;
            E.gameObject.SetActive(false);
            _E.Add(E.gameObject);
            E.GetComponent<SpriteRenderer>().flipX = false;
        }

        SpawnTime = new int[Enemys.Length];

        for(int i=1;i<SpawnTime.Length;i++)
        {
            SpawnTime[i] = SpawnTime[i-1] + Random.Range(3,9);
        }

        Board_Manager.Initialize += Init;
    }

    // Update is called once per frame
    void Update()
    {
        if(Board_Manager.m_Instance.Stop) return;
        
        if(count >= Enemys.Length) return;
        FlowTime += Time.deltaTime;

        if(FlowTime>=SpawnTime[count])
        {
            _E[count].SetActive(true);
            count++;
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
        for(int i =0;i<Enemys.Length;i++)
        {
            Unit_Manager E = Instantiate(Enemys[i],transform).GetComponent<Unit_Manager>();
            E.camp = -1;
            E.gameObject.SetActive(false);
            _E.Add(E.gameObject);
        }

        SpawnTime = new int[Enemys.Length];
        for(int i=1;i<SpawnTime.Length;i++)
        {
            SpawnTime[i] = SpawnTime[i-1] + Random.Range(3,9);
        }
    }
}
