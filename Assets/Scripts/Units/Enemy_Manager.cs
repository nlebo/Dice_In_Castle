using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_Manager : Unit_Manager
{
    public Text HP_Text;

    protected override void Start()
    {
        base.Start();
        HP_Text.text = HP.ToString();
    }
    public override void Hit(int _Damage)
    {
        base.Hit(_Damage);
        HP_Text.text = HP.ToString();
    }
}
