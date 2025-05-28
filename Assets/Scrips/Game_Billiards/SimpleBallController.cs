using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBallController : MonoBehaviour
{

    [Header("기본 설정")]
    public float power = 10f;                      //타격 힘
    public Sprite arrowSprite;                     //화살표 이미지


    private Rigidbody rb;                          //공의 물리
    private GameObject arrow;                      //화살표 오브젝트
    private bool isDragging = false;               //드래그 중인지 확인 하는 Bool
    private Vector3 startPos;                      //드래그 시작 위치


    // Start is called before the first frame update
    void Start()
    {
        SetupBall();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        UpdateArrow();
    }

    void SetupBall()                                   //공 설정하기
    {
        rb = GetComponent<Rigidbody>();               //물리 컴포넌트 가져오기
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();          //없을 경우 붙여준다
        }

        //물리 설정
        rb.mass = 1;
        rb.drag = 1;
    }

    public bool IsMoving()                                 //공이 움직이고 있는지 확인
    {
        return rb.velocity.magnitude > 0.2f;               //공이 속도를 가지고 있으면 움직인다고 판단
    }

    void HandleInput()
    {
        if(!SimpleTurnManager.canPlay)return;
        if (SimpleTurnManager.anyBallMoving) return;

        if (IsMoving()) return;                          //공이 움직이고 있으면 조작 불가

        if (Input.GetMouseButtonDown(0))
        {
            StartDrag();
        }

        if (Input.GetMouseButtonUp(0) && isDragging)        //드래그 중이였는데 마우스 버튼 업 했을때
        {
            Shoot();
        }

    }

    void Shoot()                                     //공 발사 하기
    {
        Vector3 mouseDelta = Input.mousePosition - startPos;               //마우스 이동 거리롤 힘 계산
        float force = mouseDelta.magnitude * 0.01f * power;

        if (force < 5) force = 5;                                                     //최소 힘 보정 값 설정

        Vector3 direction = new Vector3(-mouseDelta.x,0, -mouseDelta.y).normalized;           //방향 계산

        rb.AddForce(direction * force, ForceMode.Impulse);                        //공에 힘 적용

        SimpleTurnManager.OnBallHit();

        //공 발사 이후 변수들 정리
        isDragging = false;
        Destroy(arrow);
        arrow = null;

        Debug.Log("발사! 힘:" + force);



    }

    void CreateArrow()                   //화살표 만들기
    {
        if (arrow != null)
        {
            Destroy(arrow);                 //기존에 화살표가 있을 경우 제거
        }

        arrow = new GameObject("Arrow");        //빈 오브젝트 생성->new GameObject 새 화살표 만들기
        SpriteRenderer sr = arrow.AddComponent<SpriteRenderer>();  //새로 만든 오브젝트에 렌더러를 붙인다.

        sr.sprite = arrowSprite;
        sr.color = Color.green;
        sr.sortingOrder = 10;

        arrow.transform.position = transform.position + Vector3.up;
        arrow.transform.localScale = Vector3.one;
    }

    void UpdateArrow()
    {
        if (!isDragging || arrow == null) return;
        {

            Vector3 mouseDelta = Input.mousePosition - startPos;
            float distance = mouseDelta.magnitude;

            float size = Mathf.Clamp(distance * 0.01f, 0.5f, 2.0f);
            arrow.transform.localScale = Vector3.one * size;

            SpriteRenderer sr = arrow.GetComponent<SpriteRenderer>();
            float colorRatio = Mathf.Clamp01(distance * 0.005f);
            sr.color = Color.Lerp(Color.green, Color.red, colorRatio);

            if (distance > 10f)
            {
                Vector3 direction = new Vector3(-mouseDelta.x,0, -mouseDelta.y);

                float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                arrow.transform.rotation = Quaternion.Euler(90, angle, 0);
            }
        }






    }

    void StartDrag()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject == gameObject)
            {
                isDragging = true;
                startPos = Input.mousePosition;
                CreateArrow();
                Debug.Log("드래그 시작");
            }
        }
    }
}
