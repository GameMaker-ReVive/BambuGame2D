using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // ������
    public GameObject[] prefabs;

    // Ǯ ����� �ϴ� ����Ʈ��
    List<GameObject>[] pools;

    void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];

        for(int i = 0; i < pools.Length; i++) {
            pools[i] = new List<GameObject>();
        }
    }

    public GameObject Get(int index)
    {
        GameObject select = null;

        // ������ Ǯ�� ��� (��Ȱ��ȭ ��) �ִ� ���� ������Ʈ ����
        foreach(GameObject item in pools[index])
        {
            if(!item.activeSelf)
            {
                // �߰��ϸ� select ������ �Ҵ�
                select = item;
                select.SetActive(true);
                break;
            }
        }

        // ��� ������Ʈ ��� ���� �� (�� ã���� ��) ������Ʈ�� ���� �����ؼ� select ������ �Ҵ�
        if(select == null)
        {
            select = Instantiate(prefabs[index], transform);
            // transform -> ������Ʈ ������ġ (= �θ� ������Ʈ -> �ڱ��ڽ�(PoolManager))

            // ������ ������Ʈ�� �ش� ������Ʈ Ǯ ����Ʈ�� �߰�
            pools[index].Add(select);
        }


        return select;
    }
}
