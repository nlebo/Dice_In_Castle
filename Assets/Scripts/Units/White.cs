using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class White : Unit_Manager
{
    protected override void Start()
    {
        base.Start();
        GetComponent<SpriteRenderer>().flipX = PlayerNum == 0 ? false : true;
    }
}
