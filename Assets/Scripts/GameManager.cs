using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player player;
    
    // �������� ����ϰڴٴ� Ű����. �ٷ� �޸𸮿� ������.
    // �ʱ�ȭ ���� �ٸ� �ڵ忡�� ��� ���� (�տ� �ڷ����� �ٿ���� ��)
    public static GameManager instance;

    void Awake()
    {
        instance = this;
    }
}
