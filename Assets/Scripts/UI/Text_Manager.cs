using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Text_Manager : MonoBehaviour
{
    
    public void Up_Text(float speed,float T)
    {
        StartCoroutine(TextUp(speed,T));
    }

    public void TEXT(string tex)
    {
        GetComponent<Text>().text = tex;
    }

    IEnumerator TextUp(float Speed,float T)
    {
        RectTransform _transform = GetComponent<RectTransform>();
        float NT = 0;
        while(NT < T)
        {
            yield return null;
            NT +=Time.deltaTime;
            _transform.position += Vector3.up * Time.deltaTime * Speed;
        }

        yield return null;
        Destroy(gameObject);
    }
}
