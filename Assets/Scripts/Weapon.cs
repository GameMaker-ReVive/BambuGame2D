using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id; // 무기 종류
    public int prefabId; // 프리펩 ID
    public float damage; // 데미지
    public int count; // 개수
    public float speed; // 속도

    void Update()
    {

    }

    public void init()
    {
        switch (id)
        {
            case 0:
                speed = -150; // 음수여야지 시계방향으로 회전
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
