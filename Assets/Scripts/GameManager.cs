using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player player;
    public PoolManager pool;

    public float gameTime; // ���ӽð�
    public float maxGameTime = 2 * 10f; // �ִ���ӽð�
    
    // �������� ����ϰڴٴ� Ű����. �ٷ� �޸𸮿� ������.
    // �ʱ�ȭ ���� �ٸ� �ڵ忡�� ��� ���� (�տ� �ڷ����� �ٿ���� ��)
    public static GameManager instance;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        gameTime += Time.deltaTime;

        if(gameTime >= maxGameTime)
        {
            gameTime = maxGameTime;
        }
    }
}
