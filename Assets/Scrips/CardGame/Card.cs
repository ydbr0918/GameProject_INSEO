using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
    public int cardValue;                  //ī�� ��(ī�� �ܰ�)
    public Sprite cardImage;               //ī�� �̹���
    public TextMeshPro cardText;           //ī�� �ؽ�Ʈ

    public void InitCard(int value, Sprite image)          //ī�� ���� �ʱ�ȭ �Լ�
    {
        cardValue = value;
        cardImage = image;

        //ī�� �̹��� ����
        GetComponent<SpriteRenderer>().sprite = image;              //�ش� �̹����� ī�忡 ǥ�� �Ѵ�.

        //ī�� �ؽ�Ʈ ����(�ִ°��)
        if (cardText != null)
        {
            cardText.text=cardValue.ToString();           //ī�� ���� ǥ�� �Ѵ�
        }
    }
}
