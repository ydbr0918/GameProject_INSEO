using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [Header("UI ���-Inspertor ���� ����")]
    public GameObject DialoguePanel;
    public Image characterImage;
    public TextMeshProUGUI characternameText;
    public TextMeshProUGUI dialogueText;
    public Button nextButton;

    [Header("�⺻ ����")]
    public Sprite defaultCharacterImage;           //ĳ���� �⺻ �̹���

    [Header("Ÿ���� ȿ�� ����")]
    public float typingSpeed = 0.05f;                 //���� �ϳ��� ��¼ӵ�
    public bool skipTypingOnClick = true;             //Ŭ�� �� Ÿ���� ��� �Ϸ� ����

    //���� ������
    private DialogueDataSo currentDialogue;            //���� �������� ��ȭ �̹���
    private int currentLineIndex = 0;              //���� �� ��° ��ȭ ������(0 ���� ����)
    private bool isDialogueActive = false;             //��ȭ ���������� Ȯ���ϴ� �׷���
    private bool isTyping = false;                     //���� Ÿ���� ȿ���� ���� ������ Ȯ��
    private Coroutine typingCoroutine;                 //Ÿ���� ȿ�� �ڷ�ƾ ����(������)

    void Start()
    {
        DialoguePanel.SetActive(false);                                            //��ȭ â �����
        nextButton.onClick.AddListener(HandleNextInput);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.Space))
        {
            HandleNextInput();                                                   //���� �Է� ó�� ( Ÿ���� ���̸� �Ϸ�, �ƴϸ� ���� ��)
        }
    }

    IEnumerator TypeText(string textToType)                  //Ÿ���� �� ��ü �ؽ�Ʈ
    {
        isTyping = true;                                     //Ÿ���� ����
        dialogueText.text = "";                              //�ؽ�Ʈ �ʱ�ȭ

        for (int i = 0; i < textToType.Length; i++)           //�ؽ�Ʈ�� �� ���ھ� �߰�
        {
            dialogueText.text += textToType[i];
            yield return new WaitForSeconds(typingSpeed);      //��� �ð� ����
        }

        isTyping = false;                                    //Ÿ���� �Ϸ�
    }

    private void CompleteTyping()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        isTyping = false;

        if (currentDialogue != null && currentLineIndex < currentDialogue.dialogueLines.Count)
        {
            dialogueText.text = currentDialogue.dialogueLines[currentLineIndex];
        }
    }

    void ShowCurrentLine()
    {
        if (currentDialogue != null && currentLineIndex < currentDialogue.dialogueLines.Count)
        {
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }
        }
        string currentText = currentDialogue.dialogueLines[currentLineIndex];
        typingCoroutine = StartCoroutine(TypeText(currentText));
    }



    void EndDialogue()                                    //��ȭ�� ������ ���� �ϴ� �Լ�
    {
        if (typingCoroutine != null)                         //Ÿ���� ȿ�� ����
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        isDialogueActive = false;                              //��ȭâ ��Ȱ��ȭ
        isTyping = false;                                       //Ÿ���� ���� ����
        DialoguePanel.SetActive(false);                        //��ȭâ �����
        currentLineIndex = 0;                                   //�ε��� �ʱ�ȭ
    }

    public void ShowNextLine()
    {
        currentLineIndex++;

        if (currentLineIndex >= currentDialogue.dialogueLines.Count)
        {
            EndDialogue();
        }
        else
        {
            ShowCurrentLine();
        }
    }

    public void HandleNextInput()
    {
        if (isTyping && skipTypingOnClick)
        {
            CompleteTyping();
        }
        else if (!isTyping)
        {
            ShowNextLine();
        }
    }

    public void SkipDialogue()                    //��ȭ ��ü�� �ٷ� ��ŵ�ϴ� �Լ�
    {
        EndDialogue();
    }

    public bool IsDialogueActive()                          //���� ��ȭ�� ���� ������ Ȯ�� �ϴ� �Լ�
    {
        return isDialogueActive;
    }


    public void StartDialogue(DialogueDataSo dialogue)                            //���ο� ��ȭ�� �����ϴ� �Լ�
    {
        if (dialogue == null || dialogue.dialogueLines.Count == 0) return;         //��ȭ �����Ͱ� ���ų�

        currentDialogue = dialogue;
        currentLineIndex = 0;
        isDialogueActive = true;


        DialoguePanel.SetActive(true);
        characternameText.text = dialogue.characterName;

        if (characterImage != null)
        {
            if (dialogue.characterImage != null)
            {
                characterImage.sprite = dialogue.characterImage;
            }
            else
            {
                characterImage.sprite = defaultCharacterImage;
            }
        }


        ShowCurrentLine();
    }
    // Start is called before the first frame update
   


}
