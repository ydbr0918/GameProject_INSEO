using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueNPC : MonoBehaviour
{

    public DialogueDataSo myDialogue;
    private DialogueManager dialogueManager;
    // Start is called before the first frame update
    void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();

        if (dialogueManager == null)
        {
            Debug.LogError("���̾� �α� �޴����� �����ϴ�.");
        }
    }

    
    // Update is called once per frame
    void OnMouseDown()                                         //���콺�� NPC�� Ŭ�� ���� ��
    {
        if (dialogueManager == null)                                 //�Ŵ����� ������ ���� ����
        if (dialogueManager.IsDialogueActive()) return;          //�̹� ��ȭ ���̸� ���� ����
        if(myDialogue == null)return;                                //��ȭ �����Ͱ� ������ ���� ����

        dialogueManager.StartDialogue(myDialogue);                    //��� ������ �����Ǹ� �� ��ȭ ����
    }
}
