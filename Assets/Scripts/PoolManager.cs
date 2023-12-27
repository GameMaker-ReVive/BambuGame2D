using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameObject[] prefabs;

    List<GameObject>[] pools;

    // �����յ��� ������ ����
     void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];

        for( int index = 0; index < pools.Length; index++ )
        {
            pools[index] = new List<GameObject> ();
            
        }   
    }

    // Ǯ ����� �ϴ� ����Ʈ��
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
        //select�� ������ pools[]�� �������߰�
        if (!select) {
           select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }
        
        return select;
    }
}
