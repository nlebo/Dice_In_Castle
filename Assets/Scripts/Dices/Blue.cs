using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blue : Dice_Manager
{
    public GameObject IceRange;
    public float DecreaseSpeed;
    public float DecreaseTime;

    protected override void Update()
    {
        if (Drag)
        {
            hit = null;
            transform.position = new Vector3(MyInput.TPos.x, MyInput.TPos.y, transform.position.z);

            hit = Physics2D.RaycastAll(transform.position, Vector2.zero);
            CombineDice = null;
            Create = false;
            IceRange.SetActive(false);
            for (int i = 0; i < hit.Length; i++)
            {
                if(hit[i].transform == transform){}
                else if (hit[i].transform.tag == "Dice")
                {
                    CombineDice = hit[i].transform.GetComponent<Dice_Manager>();
                    Create = false;
                }
                else if (hit[i].transform.name == "WarBoard")
                {
                    CombineDice = null;
                    Create = true;
                    IceRange.SetActive(true);
                }
            }
        }
    }
    protected override void OnMouseUp()
    {
        if(Drag){
            Drag = false;
            
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

                transform.localPosition = Vector3.zero;
        }
    }
    public override bool WarZone()
    {
        IceSpell Spell = transform.GetChild(0).GetComponent<IceSpell>();

        for(int i =0; i<Spell.U.Count;i++)
        {
            Spell.U[i].Slowing(DecreaseSpeed + Level * 4,DecreaseTime + Level * 0.2f);
        }
        Instantiate(FX_Manager.m_Instance.Freeze,transform.position,FX_Manager.m_Instance.Freeze.transform.rotation);
        return true;
    }
}
