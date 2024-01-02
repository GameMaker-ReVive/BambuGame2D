using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
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
    public bool isLive;

    [Header("# Game Object")]
    public Player player;
    public PoolManager pool;
    public LevelUp uiLevelUp;
    
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

        // �ӽ� ��ũ��Ʈ (ù��° ĳ���� ����)
        uiLevelUp.Select(0);
    }

    void Update()
    {
        if (!isLive)
            return;

        gameTime += Time.deltaTime;

        if(gameTime >= maxGameTime)
        {
            gameTime = maxGameTime;
        }
    }

    public void GetExp()
    {
        exp++;

        if(exp == nextExp[Mathf.Min(level, nextExp.Length - 1)]) // Min �Լ��� ����Ͽ� �ְ� ����ġ�� �״�� ���
        {
            level++;
            exp = 0;
            uiLevelUp.Show();
        }
    }

    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0;
    }

    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1;
    }
}
