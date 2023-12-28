using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    RectTransform rect;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    void FixedUpdate()
    {
        // ���� ��ǥ�� ��ũ�� ��ǥ�� �ٸ��⿡ Camera.main.WorldToScreenPoint �� ����Ͽ� ���� ī�޶� ������ �� ��ǥ�� ���� ��.
        // WorldToScreenPoint : ���� ���� ������Ʈ ��ġ�� ��ũ�� ��ǥ�� ��ȯ
        rect.position = Camera.main.WorldToScreenPoint(GameManager.instance.player.transform.position);
    }
}
