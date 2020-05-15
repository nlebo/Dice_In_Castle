using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yellow : Dice_Manager
{
    public int UPCOIN;
    
    protected override void Start()
    {   
        base.Start();
        RoundManager.R_Finish += UpCoin;
    }

    public void UpCoin()
    {
        Board_Manager.m_Instance.UpCoin(UPCOIN + (Level * 2) );
    }

    private void OnDestroy() {
        RoundManager.R_Finish -= UpCoin;
    }

    public override bool WarZone()
    {
        return false;
    }
}
