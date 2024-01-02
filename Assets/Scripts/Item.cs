using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public ItemData data;
    public int level;
    public Weapon weapon;
    public Gear gear;

    Image icon;
    Text textLevel;
    Text textName;
    Text textDesc;

    void Awake()
    {
        icon = GetComponentsInChildren<Image>()[1]; // GetComponentsInChildren �� ù��° Image ������Ʈ�� �ڱ��ڽ��̹Ƿ� �ι�° ���� ������.
        icon.sprite = data.itemIcon; // ItemData �� �����ص� itemIcon ���� �����ͼ� ��������Ʈ ����

        Text[] texts = GetComponentsInChildren<Text>();
        textLevel = texts[0]; // icon �� �޸� GetComponentsInChildren �� ù��° Text ������Ʈ�� Text Level �̱⿡ ù��° ���� ������.
        textName = texts[1];
        textDesc = texts[2];
        textName.text = data.itemName;
    }

    void OnEnable()
    {
        textLevel.text = "Lv." + (level + 1);

        switch(data.itemType)
        {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100, data.counts[level]);
                break;
            case ItemData.ItemType.Glove:
            case ItemData.ItemType.Shoe:
                textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100);
                break;
            default:
                textDesc.text = string.Format(data.itemDesc);
                break;

        }
    }

    public void OnClick()
    {
        switch(data.itemType)
        {
            case ItemData.ItemType.Melee :
            case ItemData.ItemType.Range:
                // ������ 0 �� �� (���� ���� ��) �⺻ ������Ʈ ����
                if(level == 0)
                {
                    GameObject newWeapon = new GameObject();
                    // AddComponent �Լ� ��ȯ ���� �̸� ������ ������ ����
                    weapon = newWeapon.AddComponent<Weapon>(); // AddComponent<T> : ���ӿ�����Ʈ�� T ������Ʈ�� �߰��ϴ� �Լ�
                    weapon.Init(data);
                }
                else
                {
                    float nextDamage = data.baseDamage;
                    int nextCount = 0;

                    nextDamage += data.baseDamage * data.damages[level];
                    nextCount += data.counts[level];

                    weapon.LevelUp(nextDamage, nextCount);
                }

                level++;
                break;
            case ItemData.ItemType.Glove:
            case ItemData.ItemType.Shoe:
                if (level == 0)
                {
                    GameObject newGear = new GameObject();
                    // AddComponent �Լ� ��ȯ ���� �̸� ������ ������ ����
                    gear = newGear.AddComponent<Gear>(); // AddComponent<T> : ���ӿ�����Ʈ�� T ������Ʈ�� �߰��ϴ� �Լ�
                    gear.Init(data);
                } else
                {
                    float nextRate = data.damages[level];
                    gear.LevelUp(nextRate);
                }

                level++;
                break;
            case ItemData.ItemType.Heal:
                GameManager.instance.health = GameManager.instance.maxHealth;
                // ��ȸ�� �������� �������� �ʿ� ���⿡ level++ �ڵ带 ����
                break;
        }

        // ���� ������ �ִ� ������ ���� ��
        if(level == data.damages.Length)
        {
            // ��ư Ŭ�� ���ѵǰ� ���� -> interactable ��Ȱ��ȭ 
            GetComponent<Button>().interactable = false;
        }
    }
}
