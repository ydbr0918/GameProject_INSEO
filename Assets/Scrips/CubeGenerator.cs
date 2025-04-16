using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeGenerator : MonoBehaviour
{
    public GameObject cubePrefab;                   //������ ť�� ������
    public int totalCubes = 10;                     //�� ������ ť�� ����
    public float cubeSpeacing = 1.0f;               //ť�� ����
    // Start is called before the first frame update
    void Start()
    {
        GenCube();                              //�Լ��� ȣ�� �Ѵ�.
    }

    // Update is called once per frame
    public void GenCube()
    {
        Vector3 myPosition = transform.position;            //��ũ��Ʈ�� ���� ������Ʈ�� ��ġ (x,y,z)

        GameObject firestCube = Instantiate(cubePrefab, myPosition, Quaternion.identity);   //ù��° ť�� ����(�� ��ġ)

        for (int i = 1; i < totalCubes; i++)
        {
            //�� ��ġ���� z������ ���� �������� ������ ��ġ�� ����
            Vector3 position = new Vector3(myPosition.x, myPosition.y, myPosition.z + (i * cubeSpeacing));
            Instantiate(cubePrefab, position, Quaternion.identity);
        }
    }
}
