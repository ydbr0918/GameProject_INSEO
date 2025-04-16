using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [Header("�⺻ �̵� ����")]
    public float moveSpeed = 5f;        //�̵� �ӵ� ���� ����
    public float jumpForce = 7f;        //������ �� ���� �ش�.
    public float turnSpeed = 10f;

    [Header("���� ���� ����")]
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2.0f;

    [Header("���� ���� ����")]
    public float coyoteTime = 0.15f;
    public float coyoteTimeCounter;
    public bool realGrouned = true;

    [Header("�۶��̴� ����")]
    public GameObject gliderObject;
    public float gliderFallSpeed = 1.0f;
    public float gliderMoveSpeed = 7.0f;
    public float gliderMaxTime = 5.0f;
    public float gliderTimeLeft;
    public bool isGliding = false;



    public bool isGrounded = true;      //���� �ִ��� üũ �ϴ� ���� (true/false)

    public int coinCount = 0;           //���� ȹ�� ���� ����
    public int totalCoins = 10;         //�� ���� ȹ�� �ʿ� ���� ����

    public Rigidbody rb;                //�÷��̾� ��ü�� ����
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

        //������ �Է�
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moverVertical = Input.GetAxis("Vertical");

        //�̵� ���� ����
        Vector3 movement = new Vector3(moveHorizontal, 0, moverVertical); //�̵� ���� ����

        if (movement.magnitude > 0.1f)//�Է��� ���� ���� ȸ��
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }

        //GŰ�� �۶��̴� ����(������ ���ȸ� Ȱ��ȭ)
        if (Input.GetKey(KeyCode.G) && !isGrounded && gliderTimeLeft > 0)//GŰ�� �����鼭 ���� ���� �ʰ� �۶��̴� ���� �ð��� ���� ��
        {
            if (!isGliding)                            //�۶��̴� Ȱ��ȭ(������ �ִ� ����)
            {
                //�۶��̴� Ȱ��ȭ �Լ� (�Ʒ� ����)
                EnableGlider();
            }
            //�۶��̴� ��� �ð� ����
            gliderTimeLeft -= Time.deltaTime;

            //�۶��̴� �ð��� �� ��� ��Ȱ��ȭ
            if (gliderTimeLeft <= 0)
            {
                //�۶��̴� ��Ȱ��ȭ �Լ�(�Ʒ� ����)
                DisableGlider();
            }

        }

        else if (isGliding)
        {
            //GŰ�� ���� �۶��̴� ��Ȱ��ȭ
            DisableGlider();
        }

        if (isGliding)
        {
            //�۶��̴� ��� �� �̵�
            ApplyGliderMovement(moveHorizontal, moverVertical);
        }
        else //���� ������ �ڵ���� else�� �ȿ� �ִ´�
        {
            //�ӵ��� ���� �̵�
            rb.velocity = new Vector3(moveHorizontal * moveSpeed, rb.velocity.y, moverVertical * moveSpeed);


            //���� ���� ���� ����
            if (rb.velocity.y < 0)
            {
                rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }
            else if (rb.velocity.y > 0 && !Input.GetButton("jump"))
            {
                rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;


            }
        }



        //���� �Է�
        if (Input.GetButtonDown("Jump") && isGrounded)              //&& �� ���� �����Ҷ� -> (�����̽� ��ư�� �������� �� isGrounded�� ture�϶�)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);  //�������� ������ ����ŭ ��ü�� �ش�.
            isGrounded = false;                                      //������ �ϴ� ���� ������ �������� ������ false��� ���ش�.
            realGrouned = false;
            coyoteTimeCounter = 0;
        }

        //���鿡 ������ �۶��̴� �ð� ȸ�� �� �۶��̴� ��Ȱ��ȭ
        if (isGrounded)
        {
            if (isGliding)
            {
                DisableGlider();
            }
            //���� ���� �� �ð� ȸ��
            gliderTimeLeft = gliderMaxTime;

        }


    }

    //�۶��̴� Ȱ��ȭ �Լ�
    void EnableGlider()
    {
        isGliding = true;

        //�۶��̴� ������Ʈ ǥ��
        if (gliderObject != null)
        {
            gliderObject.SetActive(true);
        }
        //�ϰ� �ӵ� �ʱ�ȭ
        rb.velocity = new Vector3(rb.velocity.x, -gliderFallSpeed, rb.velocity.z);

    }

    //�۶��̴� ��Ȱ��ȭ �Լ�
    void DisableGlider()
    {
        isGliding = false;

        //�۶��̴� ������Ʈ �����
        if (gliderObject != null)
        {
            gliderObject.SetActive(false);
        }
        //��� �����ϵ��� �߷� ����
        rb.velocity=new Vector3(rb.velocity.x,0,rb.velocity.z);

    }
    //�۶��̴� �̵� ����
    void ApplyGliderMovement(float horizontal, float vertical)
    {
        //�۶��̴� ȿ�� : õõ�� �������� ���� �������� �� ������ �̵�
        Vector3 gliderVelocity = new Vector3(
           horizontal * gliderMoveSpeed,             //x��
           -gliderFallSpeed,                         //y��
            vertical * gliderMoveSpeed               //Z��
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

   
    private void OnTriggerEnter(Collider other)     //Ʈ���� ���� �ȿ� ���Գ��� �˻��ϴ� �Լ�
    {
        //���� ����
        if (other.tag == "Coin")                //���� Ʈ���ſ� �浹 Ȯ��
        {
            coinCount++;                            //coinCount = coinCount + 1 ���� ���� 1�� �÷��ش�.
            Destroy(other.gameObject);              //���� ������Ʈ�� ���ش�.
            Debug.Log($"���� ����: {coinCount}/{totalCoins}");

        }

        //������ ���� �� ���� �α� ���
        if (other.gameObject.tag == "Door" && coinCount == totalCoins)
        {
            Debug.Log("���� Ŭ����");
            //���� �Ϸ� ���� �߰� ����
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
