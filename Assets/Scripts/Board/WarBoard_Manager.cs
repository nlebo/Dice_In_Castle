using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarBoard_Manager : MonoBehaviour
{
    public static WarBoard_Manager m_Instance;
    public Unit_Manager[] Units;
    public Transform[] SpawnPoints;
    Camera_Manager Cam;
    // Start is called before the first frame update
    void Start()
    {
        m_Instance = this;
        Cam = Camera_Manager.m_Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateUnit(int What,int PE,int Lv)
    {
        Unit_Manager U = Instantiate(Units[What].gameObject,SpawnPoints[PE]).GetComponent<Unit_Manager>();
        U.camp = PE == 0 ? 1 : -1;
        U.GetComponent<SpriteRenderer>().flipX = PE == 0 ? true : false;
        U.LV = Lv;
    }

}
