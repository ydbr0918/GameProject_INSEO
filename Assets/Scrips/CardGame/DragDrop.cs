
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Rendering;

public class DragDrop : MonoBehaviour                   //ī�� �巡�� �� ��� ó���� ���� Ŭ���� 
{
    public bool isDragging = false;                     //�巡�� ������ �Ǻ��ϴ� Bool ��
    public Vector3 startPosition;                       //�巡�� ���� ��ġ 
    public Transform startParent;                       //�巡�� ���� �� �ִ� ���� (Area)

    private GameManager gameManager;                    //���ӸŴ����� ���� �Ѵ�. 

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;         //���� ��ġ�� �θ� ����
        startParent = transform.parent;

        gameManager = FindObjectOfType<GameManager>();          //���� �Ŵ��� ���� 
    }

    // Update is called once per frame
    void Update()
    {
        if (isDragging)      //�巡�� ���̸� ���콺 ��ġ�� ī�� �̵�
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            transform.position = mousePos;
        }
    }

    void OnMouseDown()          //���콺 Ŭ�� �� �巡�� ����
    {
        isDragging = true;

        startPosition = transform.position;         //���� ��ġ�� �θ� ����
        startParent = transform.parent;

        GetComponent<SpriteRenderer>().sortingOrder = 10;           //�巡�� ���� ī�尡 �ٸ� ī�庸�� �տ� ���̵��� �Ѵ�. 
    }

    void OnMouseUp()                //���콺 ��ư ���� �� 
    {
        isDragging = false;
        GetComponent<SpriteRenderer>().sortingOrder = 1;           //�巡�� ���� ī�尡 �ٸ� ī�庸�� �տ� ���̵��� �Ѵ�. 

        if (gameManager == null)                                 //�Ŵ����� ������ �Լ��� ���� �Ѵ�. 
        {
            RetrunToOriginalPosition();
            return;
        }

        bool wasInMergeArea = startParent == gameManager.mergeArea;         //���� ī�尡 ��� �������� �Դ��� Ȯ�� 

        if (IsOverArea(gameManager.handArea))                            //���� ���� ���� ī�带 ���Ҵ��� Ȯ��
        {
            Debug.Log("���� �������� �̵�");

            if (wasInMergeArea)                                          //������������ �Դٸ� MoveToHand �Լ� ȣ�� 
            {
                for (int i = 0; i < gameManager.mergeCount; i++)         //ī�带 ���� �������� �����ϰ� ���з� �̵� 
                {
                    if (gameManager.mergeCards[i] == gameObject)            //�ڵ� �迭�� ���� ���콺 ���ϴ� ������Ʈ�� ���� ���
                    {
                        for (int j = i; j < gameManager.mergeCount - 1; j++)
                        {
                            gameManager.mergeCards[j] = gameManager.mergeCards[j + 1];      //�ش� ī�带 �����ϰ� �迭 �ڿ��� ������ ��ĭ�� �̵�
                        }
                        gameManager.mergeCards[gameManager.mergeCount - 1] = null;      //�� ���� ī�带 null �� ����
                        gameManager.mergeCount--;                                       //ī�� ���� ���δ�. 

                        transform.SetParent(gameManager.handArea);          //���п� ī�� �߰�
                        gameManager.handCards[gameManager.handCount] = gameObject;
                        gameManager.handCount++;

                        gameManager.ArrangeHand();                      //���� ����
                        gameManager.ArrangeMerge();
                        break;
                    }
                }
            }
            else
            {
                gameManager.ArrangeHand();          //�̹� ���п� �ִ� ī���� ���� �� ���� 
            }
        }
        else if (IsOverArea(gameManager.mergeArea))                   //���� ���� ���� ī�带 ���Ҵ��� Ȯ�� 
        {
            if (gameManager.mergeCount >= gameManager.maxMergeSize)  //���� ������ ���� á���� Ȯ��
            {
                Debug.Log("���� ������ ���� á���ϴ�!");
                RetrunToOriginalPosition();
            }
            else
            {
                gameManager.MoveCardToMerge(gameObject);            //���� �������� �̵�
            }
        }
        else
        {
            RetrunToOriginalPosition();                     //�ƹ� ������ �ƴϸ� ���� ��ġ�� ���ư��� 
        }

        if (wasInMergeArea)              //���� ������ ���� ��� ��ư ���� ������Ʈ 
        {
            if (gameManager.mergeButton != null)
            {
                bool canMerge = (gameManager.mergeCount == 2 || gameManager.mergeCount == 3);
                gameManager.mergeButton.interactable = canMerge;
            }
        }

    }


    void RetrunToOriginalPosition()                  //���� ��ġ�� ���ư��� �Լ� 
    {
        transform.position = startPosition;
        transform.SetParent(startParent);

        if (gameManager != null)
        {
            if (startParent == gameManager.handArea)
            {
                gameManager.ArrangeHand();
            }
            if (startParent == gameManager.mergeArea)
            {
                gameManager.ArrangeMerge();
            }
        }
    }

    bool IsOverArea(Transform area)         //ī�尡 Ư�� ���� ���� �ִ��� Ȯ�� 
    {
        if (area == null)
        {
            return false;
        }

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);        //���� ���콺 ��ġ�� ��������
        mousePosition.z = 0;                                                                //2D �̱� ������

        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePosition, Vector2.zero);            //����ĳ��Ʈ ���� (���콺 ��ġ���� �Ʒ� ��������)

        foreach (RaycastHit2D hit in hits)                   //����ĳ��Ʈ�� ������ ��� �ݶ��̴� Ȯ��
        {
            if (hit.collider != null && hit.collider.transform == area)      //�ݶ��̴��� ���� ������Ʈ�� ã�� �ִ� �������� Ȯ�� 
            {
                Debug.Log(area.name + " ���� ������");
                return true;
            }
        }

        return false;
    }

}
