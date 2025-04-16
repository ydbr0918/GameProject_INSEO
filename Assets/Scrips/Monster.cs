using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Monster : MonoBehaviour
{
    public int Health = 100;        //ü���� ���� �Ѵ�.(����)
    public float Timer = 1.0f;      //Ÿ�̸Ӹ� ���� �Ѵ�.
    public int AttackPoint = 10;    //���ݷ��� ���� �Ѵ�.


    //ù ������ ������ �ѹ� ���� �ȴ�.

    void Start()
    {
        Health += 100;          // ù ������ ������ ����ɶ� 100ü���� �߰� ���� �ش�.
    }

    //�Ź� ������ �� ȣ�� �ȴ�.
    void Update()
    {
        CharacterHealthUp();
        CheckDeath();
    }
    void CharacterHealthUp()
    { 
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CharacterHit(AttackPoint);
        }

        CheckDeath();
    }
   
    void CharacterHealthup()
    {
        if (Timer <= 0)
        {
            Timer = 1.0f;
            Health += 20;
        }
    }
    public void CharacterHit(int Damage)                       //Ŀ���� �������� �޴� �Լ��� ����Ѵ�.
    {
        Health -= Damage;                               //���� ���ݷ¿� ���� ü���� ���� ��Ų��.
    }    
    void CheckDeath()
    {
        if (Health <= 0)                               //ü���� 0���Ϸ� �������� �ı� ��Ų��.
            Destroy(gameObject);                         //�� ������Ʈ�� �ı� �Ѵ�.
    }
   
}
