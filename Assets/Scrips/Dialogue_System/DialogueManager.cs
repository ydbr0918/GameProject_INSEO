using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [Header("UI 요소-Inspertor 에서 연결")]
    public GameObject DialoguePanel;
    public Image characterImage;
    public TextMeshProUGUI characternameText;
    public TextMeshProUGUI dialogueText;
    public Button nextButton;

    [Header("기본 설정")]
    public Sprite defaultCharacterImage;           //캐릭터 기본 이미지

    [Header("타이핑 효과 설정")]
    public float typingSpeed = 0.05f;                 //글자 하나당 출력속도
    public bool skipTypingOnClick = true;             //클릭 시 타이핑 즉시 완료 여부

    //내부 변수들
    private DialogueDataSo currentDialogue;            //현재 진행중인 대화 이미지
    private int currentLineIndex = 0;              //현재 몇 번째 대화 중인지(0 부터 시작)
    private bool isDialogueActive = false;             //대화 진행중인지 확인하는 그래프
    private bool isTyping = false;                     //현재 타이핑 효과가 진행 중인지 확인
    private Coroutine typingCoroutine;                 //타이핑 효과 코루틴 참조(중지용)

    void Start()
    {
        DialoguePanel.SetActive(false);                                            //대화 창 숨기기
        nextButton.onClick.AddListener(HandleNextInput);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.Space))
        {
            HandleNextInput();                                                   //다음 입력 처리 ( 타이핑 중이면 완료, 아니면 다음 줄)
        }
    }

    IEnumerator TypeText(string textToType)                  //타이핑 할 전체 텍스트
    {
        isTyping = true;                                     //타이핑 시작
        dialogueText.text = "";                              //텍스트 초기화

        for (int i = 0; i < textToType.Length; i++)           //텍스트를 한 글자씩 추가
        {
            dialogueText.text += textToType[i];
            yield return new WaitForSeconds(typingSpeed);      //대기 시간 설정
        }

        isTyping = false;                                    //타이핑 완료
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



    void EndDialogue()                                    //대화를 완전히 종료 하는 함수
    {
        if (typingCoroutine != null)                         //타이핑 효과 정리
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        isDialogueActive = false;                              //대화창 비활성화
        isTyping = false;                                       //타이핑 상태 해제
        DialoguePanel.SetActive(false);                        //대화창 숨기기
        currentLineIndex = 0;                                   //인덱스 초기화
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

    public void SkipDialogue()                    //대화 전체를 바로 스킵하는 함수
    {
        EndDialogue();
    }

    public bool IsDialogueActive()                          //현재 대화가 진행 중인지 확인 하는 함수
    {
        return isDialogueActive;
    }


    public void StartDialogue(DialogueDataSo dialogue)                            //새로운 대화를 시작하는 함수
    {
        if (dialogue == null || dialogue.dialogueLines.Count == 0) return;         //대화 데이터가 없거나

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
