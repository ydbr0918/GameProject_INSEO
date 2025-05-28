using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTurnManager : MonoBehaviour
{

    //전역 변수(모든 공이 공유해서 사용할 수 있음)
    public static bool canPlay = true;                         //공을 칠 수 있는지
    public static bool anyBallMoving = false;                    //어떤 공이라도 움직이고 있는지


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CheckAllBalls();                                   //모든 공의 움직임 확인 호출

        if (!anyBallMoving && !canPlay)
        {
            canPlay = true;
            Debug.Log("턴 종료! 다시 칠 수 있습니다");
        }
    }

    void CheckAllBalls()                           //모든 공이 멈췄는지 확인
    {
        SimpleBallController[] allBalls = FindObjectsOfType<SimpleBallController>();
        anyBallMoving = false;                                               //초기화 시켜준다

        foreach (SimpleBallController ball in allBalls)                  //배열 전체 클래스를 순환하면서
        {
            if (ball.IsMoving())                                       //공이 움직이고 있는지 확인하는 함수를 호출
            {
                anyBallMoving = true;                                      //공이 움직인다고 변수 변경
                break;                                                      //루프문을 빠져나온다
            }
        }


    }

    public static void OnBallHit()
    {
        canPlay = false;
        anyBallMoving = true;
        Debug.Log("턴 시작! 공이 멈출 때 까지 기다리세요");

    }
}
