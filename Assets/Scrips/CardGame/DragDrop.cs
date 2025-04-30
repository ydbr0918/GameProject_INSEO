using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DragDrop : MonoBehaviour
{
    public bool isDragging = false;                //�巡�� ������ �Ǻ��ϴ� bool��
    public Vector3 startPosition;                  //�巡�� ���� ��ġ
    public Transform startParent;                 //�巡�� ���� �� �ִ� ����

    private GameManager gameManager;              //���ӸŴ����� �����Ѵ�
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;           //���� ��ġ�� �θ� ����
        startParent = transform.parent;

        gameManager=FindObjectOfType<GameManager>();           //���� �Ŵ��� ����
    }

    // Update is called once per frame
    void Update()
    {
        if (isDragging)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            transform.position = mousePos;
        }
    }
    private void OnMouseDown()                  //���콺 Ŭ�� �� �巡�� ����
    {
        isDragging = true;

        startPosition = transform.position;        //���� ��ġ�� �θ� ����
        startParent = transform.parent;

        GetComponent<SpriteRenderer>().sortingOrder = 10;      //�巡�� ���� ī�尡 �ٸ� ī�庸�� �տ� ���̵��� �Ѵ�.
    }
    void OnMouseUp()                     //���콺 ��ư ���� ��
    {
        isDragging = false;
        GetComponent<SpriteRenderer>().sortingOrder = 1;                //�巡�� ���� ī�尡 �ٸ� ī�庸�� �տ� ���̵��� �Ѵ�

        ReturnToOriginalPosition();

    }
    //���� ��ġ�� ���ư��� �Լ�
    void ReturnToOriginalPosition()
    {
        transform.position = startPosition;
        transform.SetParent(startParent);

        if (gameManager != null)
        {
            if (startParent == gameManager.handArea)
            {
                gameManager.ArrangeHand();
            }
        }
    }
    bool IsOverArea(Transform area)
    {
        if (area == null)
        {
            return false;
        }

        //������ �ݶ��̴��� ������
        Collider2D areaCollider = area.GetComponent<Collider2D>();
        if (areaCollider == null)
        
           return false;

        //ī�尡 ���� �ȿ� �ִ��� Ȯ��
        return areaCollider.bounds.Contains(transform.position);
    }
}
