using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    public ItemData.ItemType type; // 장비의 타입
    public float rate; // 장비의 레벨 별 수치

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

    // 타입에 따라 적절하게 로직을 적용시켜주는 함수
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

    // 장갑의 기능인 연사력을 올리는 함수
    void RateUp()
    {
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();

        foreach (Weapon weapon in weapons)
        {
            switch (weapon.id)
            {
                case 0: // 근접 무기
                    float speed = 150 * Charactor.WeaponSpeed;
                    weapon.speed = speed + (speed * rate);
                    break;
                default: // 원거리 무기
                    speed = 0.5f * Charactor.WeaponRate;
                    weapon.speed = speed * (1f - rate);
                    break;
            }
        }
    }

    // 신발의 기능인 이동 속도를 올리는 함수
    void SpeedUp()
    {
        float speed = 5 * Charactor.Speed; // 플레이어 기본 속도
        GameManager.instance.player.speed = speed + speed * rate;
    }

}
