using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id; // ���� ����
    public int prefabId; // ������ ID
    public float damage; // ������
    public int count; // ����
    public float speed; // �ӵ�

    void Update()
    {

    }

    public void init()
    {
        switch (id)
        {
            case 0:
                speed = -150; // ���������� �ð�������� ȸ��
                Batch();
                break;
            default:
                break;
        }
    }

    void Batch()
    {
        for(int index = 0; index < count; index++)
        {
            Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        }
    }
}
