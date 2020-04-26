using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSpell : MonoBehaviour
{
    public List<Unit_Manager> U;
    // Start is called before the first frame update

    void Start()
    {

    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Unit" && other.GetComponent<Unit_Manager>().camp == -1)
        {
            U.Add(other.GetComponent<Unit_Manager>());
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.tag == "Unit" && other.GetComponent<Unit_Manager>().camp == -1)
        {
            if(U.Contains(other.GetComponent<Unit_Manager>()))
                U.Remove(other.GetComponent<Unit_Manager>());
        }
    }
}
