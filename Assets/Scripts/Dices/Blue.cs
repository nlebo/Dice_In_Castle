using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blue : Dice_Manager
{
    public GameObject IceRange;
    public float DecreaseSpeed;
    public float DecreaseTime;

    protected override void OnMouseUp()
    {
        if(Drag){
            Drag = false;
            transform.localPosition = Vector3.zero;
            if(CombineDice != null)
            {
                CombineDice.Combine(this);
            }
            else if(Create)
            {
                WarZone();
                Board_Manager.m_Instance.DeleteDice(where);
            }
            else
                IceRange.SetActive(false);
        }
    }
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (Drag)
        {
            if (other.transform == null)
            {
                CombineDice = null;
                Create = false;
            }
            else if (other.transform.tag == "Dice")
            {
                CombineDice = other.transform.GetComponent<Dice_Manager>();
                Create = false;
            }
            else if (other.transform.name == "WarBoard")
            {
                CombineDice = null;
                Create = true;
                IceRange.SetActive(true);
            }
        }
    }

    protected override void OnTriggerExit2D(Collider2D other)
    {
        if (Drag)
        {
            if (other.transform.tag == "Dice")
                CombineDice = null;
            else if (other.transform.name == "WarBoard"){
                Create = false;
                IceRange.SetActive(false);
            }
        }
    }
    public override void WarZone()
    {
        IceSpell Spell = transform.GetChild(0).GetComponent<IceSpell>();

        for(int i =0; i<Spell.U.Count;i++)
        {
            Spell.U[i].Slowing(DecreaseSpeed + Level * 4,DecreaseTime + Level * 0.2f);
        }
    }
}
