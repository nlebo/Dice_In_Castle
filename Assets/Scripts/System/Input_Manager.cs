using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Input_Manager : MonoBehaviour
{
    public static Input_Manager m_Instance;
    public Vector3 TPos;
    // Start is called before the first frame update
    void Start()
    {
        m_Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        TPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
    }
}
