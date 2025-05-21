using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    public int fruitType;                               //���� Ÿ��(0:���, 1:��� ����, 2:���ڳ�) int�� index����

    public bool hasMered = false;                       //������ ���������� Ȯ���ϴ� �÷���

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasMered)                          //�̹� ������ ������ ����
            return;

        Fruit otherFruit = collision.gameObject.GetComponent<Fruit>();             //�ٸ� ���ϰ� �浹�ߴ��� Ȯ��

        if (otherFruit != null && !otherFruit.hasMered && otherFruit.fruitType == fruitType)  //�浹�� ���� �����̰� Ÿ���� ���ٸ�(�������� �ʾ��� ���)
        {
            hasMered = true;                   //���ƴٰ� ǥ��
            otherFruit.hasMered = true;

            Vector3 mergePosition = (transform.position + otherFruit.transform.position) / 2f;        //�� ������ �߰� ��ġ ���

            //���� �Ŵ������� Merge �����Ȱ��� ȣ��
            //���� �Ŵ������� Merge �����Ȱ��� ȣ��
            FruitGame gameManager = FindObjectOfType<FruitGame>();
            if (gameManager != null)
            {
                gameManager.MergeFruits(fruitType, mergePosition);  //�Լ��� �����ϰ� ȣ���Ѵ�.
            }

         
            //������ ����
            Destroy(otherFruit.gameObject);
            Destroy(gameObject);
        }
    }

}
