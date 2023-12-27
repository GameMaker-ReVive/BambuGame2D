using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameObject[] prefabs;

    List<GameObject>[] pools;

    // 프리팹들을 보관할 변수
     void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];

        for( int index = 0; index < pools.Length; index++ )
        {
            pools[index] = new List<GameObject> ();
            
        }   
    }

    // 풀 담당을 하는 리스트들
    public GameObject Get(int index)
    {
        GameObject select = null;

        foreach ( GameObject item in pools[index] ) {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }
        //select가 없으면 pools[]에 프리팹추가
        if (!select) {
           select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }
        
        return select;
    }
}
