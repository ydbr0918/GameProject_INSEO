using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue?dialogueData")]

public class DialogueDataSo : ScriptableObject
{
    [Header("ĳ���� ����")]
    public string characterName = "ĳ����";               //��ȭ â�� ǥ�õ� ĳ���� �̸�
    public Sprite characterImage;                        //ĳ���� �� �̹���

    [Header("��ȭ ����")]
    [TextArea(3,10)]                                             //Inspertor ���� ���� �� �Է� �����ϰ� ����
    public List<string> dialogueLines = new List<string>();      //��ȭ ����� (������� ��µ�)
}
