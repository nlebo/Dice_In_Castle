using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage_UI : MonoBehaviour
{
    public float LiveTime,NT;
    // Start is called before the first frame update
    void Start()
    {
        LiveTime = 1;
    }

    private void OnEnable() {
        NT = 0;
    }

    // Update is called once per frame
    void Update()
    {
        NT+= Time.deltaTime;

        if(NT>= LiveTime)
            gameObject.SetActive(false);
    }
}
