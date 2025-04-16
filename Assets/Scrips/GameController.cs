using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public float GameTimer = 0.0f;                                      //���� Ÿ�̸Ӹ� ���� �Ѵ�. 
    public GameObject MonsterGO;                                        //���� ���� ������Ʈ�� ���� �Ѵ�. 

    // Update is called once per frame
    void Update()
    {
        GameTimer -= Time.deltaTime;       //�ð��� �� �����Ӹ��� ���� ��Ų��. (deltaTime �����Ӱ��� �ð� ������ �ǹ��Ѵ�.)

        if (GameTimer <= 0)                  //���� Timer�� ��ġ�� 0���Ϸ� ������ ���
        {
            GameTimer = 3.0f;               //�ٽ� 3�ʷ� ���� �����ش�.

            GameObject Temp = Instantiate(MonsterGO);               //�ش� ���� ������Ʈ�� ���� ���� �����ش�. 
            Temp.transform.position = new Vector3(Random.Range(-10, 10), Random.Range(-4, 4), 0.0f);
            //X -10 ~ 10 Y -4 ~ 4 �� ������ �������� ��ġ ��Ų��. 
        }

        if (Input.GetMouseButtonDown(0))                                //���콺 ��ư�� ������
        {
            RaycastHit hit;                                                     //Ray ����
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);        //ī�޶󿡼� ���̸� ���� �����Ѵ�.
            //3D ���ӿ��� ������Ʈ�� ���� �� �� ����Ѵ�. (ȭ�鿡 ���̴� ��ü�� �����ϱ� ���ؼ� ���)

            if (Physics.Raycast(ray, out hit))                                  //Hit �� ������Ʈ�� �����Ѵ�.
            {
                if (hit.collider != null)                                       //Hit �� ������Ʈ�� ���� ���
                {
                    //Debug.Log(hit.collider.name);                               //�α׷� �����ش�.
                    hit.collider.gameObject.GetComponent<Monster>().CharacterHit(50);   //���Ϳ��� ������ 50�� �ش�. 
                }
            }
        }
    }
}
