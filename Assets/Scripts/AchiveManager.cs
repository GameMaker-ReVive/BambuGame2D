using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class AchiveManager : MonoBehaviour
{
    public GameObject[] lockCharactor;
    public GameObject[] unlockCharactor;
    public GameObject uiNotice;

    enum Achive { UnlockBean, UnlockPotato }
    Achive[] achives; // ���� �����͵��� �����ص� �迭

    WaitForSecondsRealtime wait; // ����ȭ�� ���� ������ ���

    void Awake()
    {
        // Enum.GetValues : �־��� �������� �����͸� ��� �������� �Լ�
        achives = (Achive[])Enum.GetValues(typeof(Achive));

        wait = new WaitForSecondsRealtime(5);

        // �����Ͱ� ���� ��쿡�� �ʱ�ȭ
        if (!PlayerPrefs.HasKey("MyData"))
        {
            Init();
        }
    }

    void Start()
    {
        UnlockCharactor();
    }


    void Init()
    {
        // ������ �ʱ�ȭ
        PlayerPrefs.SetInt("MyData", 1);

        // ���� �ʱ�ȭ
        foreach(Achive achive in achives)
        {
            PlayerPrefs.SetInt(achive.ToString(), 0); // 0 -> false ��� �ǹ̷� �ʱ�ȭ
        }
    }

    void UnlockCharactor()
    {
        for(int index = 0; index < lockCharactor.Length; index++)
        {
            string achiveName = achives[index].ToString();

            // GetInt �Լ��� ����� ���� ���¸� �����ͼ� ��ư Ȱ��ȭ�� ����
            bool isUnlock = PlayerPrefs.GetInt(achiveName) == 1;
            lockCharactor[index].SetActive(!isUnlock);
            unlockCharactor[index].SetActive(isUnlock);
        }
    }

    void LateUpdate()
    {
        foreach(Achive achive in achives)
        {
            CheckAchive(achive);
        }
    }

    void CheckAchive(Achive achive)
    {
        bool isAchive = false;

        // �� ���� �� �ر� ���� ����
        switch(achive)
        {
            case Achive.UnlockBean:
                isAchive = GameManager.instance.kill >= 10;
                break;
            case Achive.UnlockPotato:
                isAchive = GameManager.instance.gameTime == GameManager.instance.maxGameTime;
                break;
        }

        // ���� �޼� �� achive ���� 0 �� ��� 1�� ��ȯ
        if(isAchive && PlayerPrefs.GetInt(achive.ToString()) == 0)
        {
            PlayerPrefs.SetInt(achive.ToString(), 1);

            for(int index = 0; index < uiNotice.transform.childCount; index++)
            {
                // enum Achive ���� ���� ã�� ��ġ�ϴ� �ڽ� ������Ʈ Ȱ��ȭ
                if(index == (int)achive)
                {
                    uiNotice.transform.GetChild(index).gameObject.SetActive(true);
                } else
                {
                    uiNotice.transform.GetChild(index).gameObject.SetActive(false);
                }
            }

            StartCoroutine(NoticeRoutine());
        }
    }

    IEnumerator NoticeRoutine()
    {
        uiNotice.SetActive(true);

        yield return wait;

        uiNotice.SetActive(false);
    }
}
