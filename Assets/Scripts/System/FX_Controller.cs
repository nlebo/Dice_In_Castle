using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FX_Controller : MonoBehaviour
{
    ParticleSystem fx;
    // Start is called before the first frame update
    void Start()
    {
        fx = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!fx.isPlaying) Destroy(gameObject);
    }
}
