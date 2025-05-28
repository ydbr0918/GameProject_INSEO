using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTurnManager : MonoBehaviour
{

    //���� ����(��� ���� �����ؼ� ����� �� ����)
    public static bool canPlay = true;                         //���� ĥ �� �ִ���
    public static bool anyBallMoving = false;                    //� ���̶� �����̰� �ִ���


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CheckAllBalls();                                   //��� ���� ������ Ȯ�� ȣ��

        if (!anyBallMoving && !canPlay)
        {
            canPlay = true;
            Debug.Log("�� ����! �ٽ� ĥ �� �ֽ��ϴ�");
        }
    }

    void CheckAllBalls()                           //��� ���� ������� Ȯ��
    {
        SimpleBallController[] allBalls = FindObjectsOfType<SimpleBallController>();
        anyBallMoving = false;                                               //�ʱ�ȭ �����ش�

        foreach (SimpleBallController ball in allBalls)                  //�迭 ��ü Ŭ������ ��ȯ�ϸ鼭
        {
            if (ball.IsMoving())                                       //���� �����̰� �ִ��� Ȯ���ϴ� �Լ��� ȣ��
            {
                anyBallMoving = true;                                      //���� �����δٰ� ���� ����
                break;                                                      //�������� �������´�
            }
        }


    }

    public static void OnBallHit()
    {
        canPlay = false;
        anyBallMoving = true;
        Debug.Log("�� ����! ���� ���� �� ���� ��ٸ�����");

    }
}
