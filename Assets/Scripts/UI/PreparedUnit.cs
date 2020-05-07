using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreparedUnit : MonoBehaviour
{

    bool drag;
    Vector3 basePos,baseTPos;

    RectTransform RT;
    // Start is called before the first frame update
    void Start()
    {
        drag = false;
        RT = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!drag) return;

        RT.position = basePos + (Input.mousePosition - baseTPos);
    }

    public void OnClick()
    {
        drag = true;
        basePos = RT.position;
        baseTPos = Input.mousePosition;
    }

    public void UpClick()
    {
        drag = false;
        RT.position = basePos;
    }
}
