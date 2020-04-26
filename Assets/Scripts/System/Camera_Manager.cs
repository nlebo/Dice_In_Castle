using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Manager : MonoBehaviour
{
    public static Camera_Manager m_Instance;
    Input_Manager MyInput;

    public Vector3 bPos,Pos;
    public bool down;
    // Start is called before the first frame update
    void Start()
    {
        m_Instance = this;
        MyInput = Input_Manager.m_Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(MyInput.TPos, Vector2.zero);
            if (hit.transform != null && hit.transform.name == "WarBoard")
            {
                bPos = Input_Manager.m_Instance.TPos;
                Pos = transform.position;
                down = true;
            }
        }
        if (Input.GetMouseButtonUp(0))
            down = false;

        if(down)
        {
            float tx = bPos.x - MyInput.TPos.x;

            if(Pos.x + tx >= 0 && Pos.x + tx <= 7.5f) 
                transform.position = Pos + new Vector3(tx,0,0);
            
        }

        
    }
}
