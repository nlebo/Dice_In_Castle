using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreparedUnit : MonoBehaviour
{

    public int Num;
    bool drag;
    Vector3 basePos;

    public Image sprite;
    public GameObject OriginalDice;
    public int Where;



    RectTransform RT;
    // Start is called before the first frame update
    void Start()
    {
        drag = false;
        RT = GetComponent<RectTransform>();
        sprite = GetComponent<Image>();

        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(!drag) return;

        transform.position = (Vector2)Input_Manager.m_Instance.TPos;
        
    }

    public void OnClick()
    {
        drag = true;
        basePos = RT.localPosition;
    }

    public void UpClick()
    {

        Vector3 ViewPos = Camera.main.WorldToViewportPoint(transform.position);
        RT.localPosition = basePos;
        drag = false;
        if(ViewPos.y <= 0.4f)
        {
            ReturnDice();
        }
        
        
       

        
    }

    public void ReturnDice()
    {
        OriginalDice.GetComponent<SpriteRenderer>().color += new Color(0,0,0,0.5f);
        OriginalDice.GetComponent<Dice_Manager>().Active = true;
        gameObject.SetActive(false);
        WarBoard_Manager.m_Instance.DropUnit(Num);
         
    }
}
