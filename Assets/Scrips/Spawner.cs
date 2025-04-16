using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject coinPrefabs;                          //���� ������
    public GameObject MIssilePrefabs;                       //�̻��� ������

    [Header("���� Ÿ�̹� ����")]
    public float minSpawnInterval = 0.5f;                   //�ּ� �����ð�
    public float maxSpawnInterval = 2.0f;                   //�ִ� ���� �ð�

    [Header("���� ���� Ȯ�� ����")]
    [Range(0, 100)]                                          //����Ƽ UI���� �� �� �ְ� �Ѵ�.
    public int coinSpawnChance = 50;                         //������ ������ Ȯ�� (0~100)

    public float timer = 0.0f;
    public float nextSpawnTIme;                             //���� ���� �ð�

    // Start is called before the first frame update
    void Start()
    {
        SetNextSpawnTime();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;           //�ð��� 0���� ���� �����ȴ�.

        //���� �ð��� �Ǹ� ������Ʈ ����
        if(timer >= nextSpawnTIme)
        {
            SpawnObject();                  //�Լ��� ȣ�� ���ش�
            timer = 0.0f;                   //�ð��� �ʱ�ȭ �����ش�.
            SetNextSpawnTime();             //�ٽ� �Լ��� ����
        }
    }
    void SpawnObject()
    {
        Transform spawnTransform = transform;             //������ ������Ʈ�� ��ġ�� ȸ������ �����´�.

        //Ȯ���� ���� ���� �Ǵ� �̻��� ����
        int randomValue = Random.Range(0, 100);          //0~100�� �������� �̾Ƴ���.
        if (randomValue < coinSpawnChance)
        {
            Instantiate(coinPrefabs, spawnTransform.position, spawnTransform.rotation);         //���� �������� �ش� ��ġ�� �����Ѵ�.
        }
        else
        {
            Instantiate(MIssilePrefabs, spawnTransform.position, spawnTransform.rotation);      //�̻��� �������� �ش� ��ġ�� �����Ѵ�.
        }

        Instantiate(coinPrefabs, spawnTransform.position, spawnTransform.rotation);     //���� �������� �ش� ��ġ�� �����Ѵ�.
    }

    void SetNextSpawnTime()
    {
        //�ּ� - �ִ� ������ ������ �ð� ����
        nextSpawnTIme = Random.Range(minSpawnInterval, maxSpawnInterval);
    }
}
