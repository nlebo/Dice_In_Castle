using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PBoard_Manager : MonoBehaviour
{

    public Animator Anim;
    public static UnityAction _Disable;
    public static UnityAction _Enable;
    // Start is called before the first frame update
    void Awake()
    {
        Anim = GetComponent<Animator>();
        _Disable += Disable;
        _Enable += Enable;
    }
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnEnable() {

        Anim.SetTrigger("Active");
    }

    public void Enable()
    {
        gameObject.SetActive(true);
        transform.localPosition = new Vector3(0,0,10);
        Anim.Play("PBoard_Idle");
        Anim.SetTrigger("Active");
    }

    public void Disable()
    {
        
        Anim.SetTrigger("Disable");
    }

    public void Del()
    {
        transform.localPosition = new Vector3(0,0,10);
        gameObject.SetActive(false);
    }
}
