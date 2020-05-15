using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice_Manager : MonoBehaviour
{
    public enum Kind{White,Yellow,Red,Blue,Black,Arrow,MinerGuard}

    public Kind _Kind;
    public int kind_id;
    public Sprite[] scales;
    public int Level;
    public int where;
    protected Input_Manager MyInput;
    protected bool Drag = false;
    protected bool Create = false;
    public bool Active = true;
    protected Dice_Manager CombineDice;

    protected RoundManager roundManager;

    protected RaycastHit2D[] hit;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        MyInput = Input_Manager.m_Instance;
        roundManager = RoundManager.m_Instance;

        if(Level >= 1)
        switch(_Kind)
        {
            case Kind.Red:
            GetComponent<SpriteRenderer>().color = Color.red;
            break;
            case Kind.Blue:
            GetComponent<SpriteRenderer>().color = Color.blue;
            break;
            case Kind.Black:
            GetComponent<SpriteRenderer>().color = Color.black;
            break;
        }
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (Drag && Active)
        {
            hit = null;
            transform.position = new Vector3(MyInput.TPos.x, MyInput.TPos.y, transform.position.z);

            hit = Physics2D.RaycastAll(transform.position, Vector2.zero);
            CombineDice = null;
            Create = false;
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
                }
            }
        }
    }

    private void OnMouseDown() {

        if(Active)
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
                if(WarZone()){
                //Board_Manager.m_Instance.DeleteDice(where);
                GetComponent<SpriteRenderer>().color -= new Color(0,0,0,0.5f);
                Active = false;
                }
            }
        }
    }

    public virtual bool WarZone()
    {
        if(!roundManager.RoundReady) return false;

        return WarBoard_Manager.m_Instance.AddSpawnList(kind_id,0,Level,gameObject,where);
        //return WarBoard_Manager.m_Instance.AddSpawnList(kind_id,0,Level);
    }
    public void Combine(Dice_Manager dice)
    {
        if(!Active) return; 
        if(dice.Level == Level && dice._Kind == _Kind && Level < 5){
            Level++;
            Board_Manager.m_Instance.CreateDice(where,Level);
            Board_Manager.m_Instance.DeleteDice(dice.where);
        }
    }

}
