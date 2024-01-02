using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptble Object/ItemData")] // CreateAssetMenu : Ŀ���� �޴��� �����ϴ� �Ӽ�
public class ItemData : ScriptableObject
{
    // ������ Ÿ�� (����, ���Ÿ�, �尩, �Ź�, ��)
    public enum ItemType { Melee, Range, Glove, Shoe, Heal } // enum ������ �����ʹ� ���� ���·ε� ��� ����

    [Header("# Main Info")]
    public ItemType itemType; // ������ Ÿ��
    public int itemId; // ������ id
    public string itemName; // ������ �̸�
    [TextArea] // �ν����Ϳ� �ؽ�Ʈ�� ���� �� ���� �� �ְ� ���ִ� �Ӽ�
    public string itemDesc; // ������ ����
    public Sprite itemIcon; // ������ ������


    [Header("# Level Data")]
    public float baseDamage; // �⺻ (0 ����) ������
    public int baseCount; // �⺻ ����(����), ���Ÿ�(����)
    public float[] damages; // ���� �� ������
    public int[] counts; // ���� �� count

    [Header("# Weapon")]
    public GameObject projectile; // ����ü (������)
    public Sprite hand; // ������ ȹ�� �� �� ��������Ʈ ����
}
