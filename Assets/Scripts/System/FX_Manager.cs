using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FX_Manager : MonoBehaviour
{
    public GameObject Hit;
    public GameObject Create_Dice;
    public GameObject Freeze;
    public GameObject Bomb;
    public static FX_Manager m_Instance;
    // Start is called before the first frame update
    private void Awake() {
        m_Instance = this;    
    }
}
