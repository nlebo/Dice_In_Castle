using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice_Manager : MonoBehaviour
{
    public enum Kind{White,Yellow,Red,Blue,Black,Arrow}

    public Kind _Kind;
    public int kind_id;
    public Sprite[] scales;
    public int Level;
    public int where;
    Input_Manager MyInput;
    protected bool Drag = false;
    protected bool Create = false;
    protected Dice_Manager CombineDice;
    // Start is called before the first frame update
    void Start()
    {
        MyInput = Input_Manager.m_Instance;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Drag)
        {
            transform.position = new Vector3(MyInput.TPos.x,MyInput.TPos.y,transform.position.z);
        }
    }

    private void OnMouseDown() {
        Drag = true;
    }

    protected virtual void OnMouseUp() {
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
        }
    }

    public virtual void WarZone()
    {
        WarBoard_Manager.m_Instance.CreateUnit(kind_id,0,Level);
    }
    public void Combine(Dice_Manager dice)
    {
        if(dice.Level == Level && dice._Kind == _Kind && Level < 5){
            Level++;
            Board_Manager.m_Instance.CreateDice(where,Level);
            Board_Manager.m_Instance.DeleteDice(dice.where);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
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
            }
        }
    }
    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if (Drag)
        {
            if (other.transform.tag == "Dice")
                CombineDice = null;
            else if (other.transform.name == "WarBoard")
                Create = false;
        }
    }
}
