using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("# Player Info")] // �ν������� �Ӽ����� �̻ڰ� ���н����ִ� Ÿ��Ʋ
    public int level;
    public int kill;
    public int exp;
    public int health;
    public int maxHealth = 100;
    public int[] nextExp = { 10, 30, 60, 100, 150, 210, 280, 360, 450, 600 }; // �� ������ �ʿ� ����ġ�� ������ �迭 ���� ���� �� �ʱ�ȭ

    [Header("# Game Control")]
    public float gameTime; // ���ӽð�
    public float maxGameTime = 2 * 10f; // �ִ���ӽð�

    [Header("# Game Object")]
    public Player player;
    public PoolManager pool;
    
    // �������� ����ϰڴٴ� Ű����. �ٷ� �޸𸮿� ������.
    // �ʱ�ȭ ���� �ٸ� �ڵ忡�� ��� ���� (�տ� �ڷ����� �ٿ���� ��)
    public static GameManager instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        health = maxHealth;
    }

    void Update()
    {
        Debug.Log(instance);
        gameTime += Time.deltaTime;

        if(gameTime >= maxGameTime)
        {
            gameTime = maxGameTime;
        }
    }

    public void GetExp()
    {
        exp++;

        if(exp == nextExp[level])
        {
            level++;
            exp = 0;
        }
    }
}
