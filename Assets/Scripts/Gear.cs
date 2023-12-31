using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    public ItemData.ItemType type; // ����� Ÿ��
    public float rate; // ����� ���� �� ��ġ

    public void Init(ItemData data)
    {
        // Basic Set
        name = "Gear " + data.itemId;
        transform.parent = GameManager.instance.player.transform;
        transform.localPosition = Vector3.zero;

        // Property Set
        type = data.itemType;
        rate = data.damages[0];
        ApplyGear();
    }

    public void LevelUp(float rate)
    {
        this.rate = rate;
        ApplyGear();
    }

    // Ÿ�Կ� ���� �����ϰ� ������ ��������ִ� �Լ�
    void ApplyGear()
    {
        switch (type)
        {
            case ItemData.ItemType.Glove:
                RateUp();
                break;
            case ItemData.ItemType.Shoe:
                SpeedUp();
                break;
        }
    }

    // �尩�� ����� ������� �ø��� �Լ�
    void RateUp()
    {
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();

        foreach (Weapon weapon in weapons)
        {
            switch (weapon.id)
            {
                case 0: // ���� ����
                    weapon.speed = 150 + (150 * rate);
                    break;
                default: // ���Ÿ� ����
                    weapon.speed = 0.5f * (1f - rate);
                    break;
            }
        }
    }

    // �Ź��� ����� �̵� �ӵ��� �ø��� �Լ�
    void SpeedUp()
    {
        float speed = 5; // �÷��̾� �⺻ �ӵ�
        GameManager.instance.player.speed = speed + speed * rate;
    }

}