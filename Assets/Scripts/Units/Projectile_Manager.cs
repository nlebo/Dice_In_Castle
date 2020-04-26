using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Manager : MonoBehaviour
{
    public void Shot(Vector3 Start,Vector3 Goal,float ArriveTime,Unit_Manager U,int Damage)
    {
        StartCoroutine(_Shot(Start,Goal,ArriveTime,U,Damage));
    }
    public void Shot(Vector3 Start,Vector3 Goal,float ArriveTime,Castle_Manager U,int Damage)
    {
        StartCoroutine(_Shot(Start,Goal,ArriveTime,U,Damage));
    }


    IEnumerator _Shot(Vector3 Start,Vector3 Goal,float ArriveTime,Unit_Manager U,int Damage)
    {
        float NT = 0;
        while(NT < ArriveTime)
        {
            transform.position = new Vector3(Mathf.Lerp(Start.x,Goal.x,NT/ArriveTime),transform.position.y,transform.position.z);
            NT += Time.deltaTime;
            yield return null;
        }

        transform.position = Goal;
        U.Hit(Damage);
        yield return null;
        Destroy(gameObject);
    }
    IEnumerator _Shot(Vector3 Start,Vector3 Goal,float ArriveTime,Castle_Manager U,int Damage)
    {
        float NT = 0;
        while(NT < ArriveTime)
        {
            transform.position = new Vector3(Mathf.Lerp(Start.x,Goal.x,NT/ArriveTime),transform.position.y,transform.position.z);
            NT += Time.deltaTime;
        }

        transform.position = Goal;
        U.Hit(Damage);
        yield return null;
        Destroy(gameObject);
    }
}
