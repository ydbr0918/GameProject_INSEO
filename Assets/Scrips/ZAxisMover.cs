using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
//5�� ���� 5�� ���ǵ�� ������ �̵��ϰ� ������� ������Ʈ Ŭ����
public class ZAxisMover : MonoBehaviour         //Z������ �̵��ϴ� Ŭ����
{
    public float speed = 5.0f;                  //�̵��ӵ�
    public float timer = 5.0f;                   //Ÿ�̸� ����


    // Update is called once per frame
    void Update()
    {
        //z�� �������� ������ �̵�
        transform.Translate(0, 0, speed * Time.deltaTime);

        timer -= Time.deltaTime;                //�ð��� ī��Ʈ �ٿ� �Ѵ�.
        if (timer < 0)                          //�ð��� ����Ǹ�
        {
            Destroy(gameObject);                //�ڱ� �ڽ��� �ı��Ѵ�.
        }
    }
}
