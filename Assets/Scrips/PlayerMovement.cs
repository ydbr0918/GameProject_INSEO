using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [Header("기본 이동 설정")]
    public float moveSpeed = 5f;        //이동 속도 변수 설정
    public float jumpForce = 7f;        //점프의 힘 값을 준다.
    public float turnSpeed = 10f;

    [Header("점프 개선 설정")]
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2.0f;

    [Header("지면 감지 설정")]
    public float coyoteTime = 0.15f;
    public float coyoteTimeCounter;
    public bool realGrouned = true;

    [Header("글라이더 설정")]
    public GameObject gliderObject;
    public float gliderFallSpeed = 1.0f;
    public float gliderMoveSpeed = 7.0f;
    public float gliderMaxTime = 5.0f;
    public float gliderTimeLeft;
    public bool isGliding = false;



    public bool isGrounded = true;      //땅에 있는지 체크 하는 변수 (true/false)

    public int coinCount = 0;           //코인 획득 변수 선언
    public int totalCoins = 10;         //총 코인 획들 필요 변수 선언

    public Rigidbody rb;                //플레이어 강체를 선언
    // Start is called before the first frame update
    void Start()
    {
        if (gliderObject != null)
        {
            gliderObject.SetActive(false);
        }

        gliderTimeLeft = gliderMaxTime;


        coyoteTimeCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGroundedState();

        //움직임 입력
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moverVertical = Input.GetAxis("Vertical");

        //이동 방향 벡터
        Vector3 movement = new Vector3(moveHorizontal, 0, moverVertical); //이동 방향 감지

        if (movement.magnitude > 0.1f)//입력이 있을 떄만 회전
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }

        //G키로 글라이더 제어(누르는 동안만 활성화)
        if (Input.GetKey(KeyCode.G) && !isGrounded && gliderTimeLeft > 0)//G키를 누르면서 땅에 있지 않고 글라이더 남은 시간이 있을 때
        {
            if (!isGliding)                            //글라이더 활성화(누르고 있는 동안)
            {
                //글라이더 활성화 함수 (아래 정의)
                EnableGlider();
            }
            //글라이더 사용 시간 감소
            gliderTimeLeft -= Time.deltaTime;

            //글라이더 시간이 다 디면 비활성화
            if (gliderTimeLeft <= 0)
            {
                //글라이더 비활성화 함수(아래 정의)
                DisableGlider();
            }

        }

        else if (isGliding)
        {
            //G키를 때면 글라이더 비활성화
            DisableGlider();
        }

        if (isGliding)
        {
            //글라이더 사용 중 이동
            ApplyGliderMovement(moveHorizontal, moverVertical);
        }
        else //기존 움직임 코드들을 else문 안에 넣는다
        {
            //속도를 직접 이동
            rb.velocity = new Vector3(moveHorizontal * moveSpeed, rb.velocity.y, moverVertical * moveSpeed);


            //착지 점프 높이 구현
            if (rb.velocity.y < 0)
            {
                rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }
            else if (rb.velocity.y > 0 && !Input.GetButton("jump"))
            {
                rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;


            }
        }



        //점프 입력
        if (Input.GetButtonDown("Jump") && isGrounded)              //&& 두 값을 만족할때 -> (스페이스 버튼일 눌렸을때 와 isGrounded가 ture일때)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);  //위쪽으로 설정한 힘만큼 강체에 준다.
            isGrounded = false;                                      //점프를 하는 순간 땅에서 떨어졌기 때문에 false라고 해준다.
            realGrouned = false;
            coyoteTimeCounter = 0;
        }

        //지면에 있으면 글라이더 시간 회복 및 글라이더 비활성화
        if (isGrounded)
        {
            if (isGliding)
            {
                DisableGlider();
            }
            //지상에 있을 때 시간 회복
            gliderTimeLeft = gliderMaxTime;

        }


    }

    //글라이더 활성화 함수
    void EnableGlider()
    {
        isGliding = true;

        //글라이더 오브젝트 표시
        if (gliderObject != null)
        {
            gliderObject.SetActive(true);
        }
        //하강 속도 초기화
        rb.velocity = new Vector3(rb.velocity.x, -gliderFallSpeed, rb.velocity.z);

    }

    //글라이더 비활성화 함수
    void DisableGlider()
    {
        isGliding = false;

        //글라이더 오브젝트 숨기기
        if (gliderObject != null)
        {
            gliderObject.SetActive(false);
        }
        //즉시 낙하하도록 중력 적용
        rb.velocity=new Vector3(rb.velocity.x,0,rb.velocity.z);

    }
    //글라이더 이동 적용
    void ApplyGliderMovement(float horizontal, float vertical)
    {
        //글라이더 효과 : 천천히 떨어지고 수평 방향으로 더 빠르게 이동
        Vector3 gliderVelocity = new Vector3(
           horizontal * gliderMoveSpeed,             //x축
           -gliderFallSpeed,                         //y축
            vertical * gliderMoveSpeed               //Z축
        );

        rb.velocity=gliderVelocity;
    }
    void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                realGrouned = true;
            }
        }
        void OnCollisionStay(Collision collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                realGrouned = true;
            }
        }
        void OnCollisoinExit(Collision collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                realGrouned = false;
            }
        }

   
    private void OnTriggerEnter(Collider other)     //트리거 영역 안에 들어왔나를 검사하는 함수
    {
        //코인 수집
        if (other.tag == "Coin")                //코인 트리거와 충돌 확인
        {
            coinCount++;                            //coinCount = coinCount + 1 코인 변수 1을 올려준다.
            Destroy(other.gameObject);              //코인 오브젝트를 없앤다.
            Debug.Log($"코인 수집: {coinCount}/{totalCoins}");

        }

        //목적지 도착 시 종료 로그 출력
        if (other.gameObject.tag == "Door" && coinCount == totalCoins)
        {
            Debug.Log("게임 클리어");
            //게임 완료 로직 추가 가능
        }
    }
    void UpdateGroundedState()
    {
        if (realGrouned)
        {
            coyoteTimeCounter = coyoteTime;
            isGrounded = true;
        }
        else
        {
            if (coyoteTimeCounter > 0)
            {
                coyoteTimeCounter -= Time.deltaTime;
                isGrounded = true;
            }
            else
            {
                isGrounded= false;
            }
        }
    }




}
