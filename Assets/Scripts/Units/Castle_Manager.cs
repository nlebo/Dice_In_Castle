using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Castle_Manager : MonoBehaviour
{
    public int HP,MaxHP;
    public Image HP_Bar;

    public int Camp;
    // Start is called before the first frame update
    void Start()
    {
        HP = MaxHP;

        Board_Manager.Initialize += Init;
    }

    public void Hit(int Damage)
    {
        HP -= Damage;
        HP_Bar.fillAmount = 1 - (float)HP/MaxHP; 

        if(HP<= 0) Death();
    }

    void Death()
    {
        Board_Manager.Initialize();
    }

    public void Init()
    {
        HP = MaxHP;
        HP_Bar.fillAmount = 1 - (float)HP/MaxHP;
    }
}
