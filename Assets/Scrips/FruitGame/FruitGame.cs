using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using TMPro.EditorUtilities;
using UnityEngine;

public class FruitGame : MonoBehaviour
{

    public GameObject[] fruitPrefabs;                        //���� ������ �迭 ����

    public float[] fruitSizes = { 0.5f, 0.7f, 0.9f, 1.1f, 1.3f, 1.5f, 1.7f, 1.9f };   //���� ũ�� ����

    public GameObject currentFruit;              //���� ��� �ִ� ����
    public int currentFruitType;

    public float fruitStartHeight = 6.0f;              //���� ���۽� ���� ����(����Ʈ���� ���� ����)
    public float gameWidth = 5.0f;                     //������ �ʺ�
    public bool isGameOver = false;                    //���� ����
    public Camera mainCamera;                          //ī�޶� ����(���콺 ��ġ ��ȯ�� �ʿ�)

    public float fruitTimer;                            //�� �ð� ������ ���� Ÿ�̸�

    public float gameHeight;                            //���� ���� ����


    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;                    //���� ī�޶� ���� ��������
        SpawnNewFruit();                             //���� ���� �� ù ���� ����
        fruitTimer = -3.0f;
        gameHeight = fruitStartHeight + 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver) return;   //���� ������ ���� 

        if (fruitTimer >= 0)                           //Ÿ�̸��� �ð��� 0���� Ŭ ���
        {
            fruitTimer -= Time.deltaTime;
        }

        if (fruitTimer < 0 && fruitTimer > -2)       //Ÿ�̸� �ð��� 0�� -2 ���̿� ������ �� �Լ��� ȣ�� �ϰ� �ٸ� �ð���� ������
        {
            CheckGameOver();
            SpawnNewFruit();
            fruitTimer = -3.0f;                //Ÿ�̸� �ð��� -3���� ������
        }


        if (currentFruit != null)    //���� ������ ������ ���� ���� ó��
        {
            Vector3 mousePosition = Input.mousePosition;        //���콺 ��ġ�� �޾ƿ´�.
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);  //���콺 ��ġ�� ���� ��ǥ�� ��ȯ

            Vector3 newPosition = currentFruit.transform.position;         //���� ��ġ ������Ʈ
            newPosition.x = worldPosition.x;

            float halfFruitSize = fruitSizes[currentFruitType] / 2f;
            if (newPosition.x < -gameWidth / 2 + halfFruitSize)
            {
                newPosition.x = -gameWidth / 2 + halfFruitSize;
            }
            if (newPosition.x > gameWidth / 2 + halfFruitSize)
            {
                newPosition.x = -gameWidth / 2 + halfFruitSize;
            }

            currentFruit.transform.position = newPosition;                        //���� ��ǥ ����
        }

        if (Input.GetMouseButtonDown(0) && fruitTimer == -3.0f)                //���콺 �� Ŭ���ϸ� ������ ����߸���.&&Ÿ�̸� �ð��� -3.0f
        {
            DropFruit();
        }



    }


    void SpawnNewFruit()                               //���� ���� �Լ�
    {
        if (!isGameOver)                                //���� ������ �ƴ� ���� �� ���� ����
        {
            currentFruitType = Random.Range(0, 3);        //0~2 ������ ���� ���� Ÿ��

            Vector3 mousePosition = Input.mousePosition;                      //���콺 ��ġ�� �޾ƿ´�.
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);         //���콺 ������ ���� ��ǥ�� ��ȯ

            Vector3 spawnPosition = new Vector3(worldPosition.x, fruitStartHeight, 0);        //X��ǥ�� ����ϰ� �������� ������ ��� �Ѵ�.

            float halfFruitSize = fruitSizes[currentFruitType] / 2;

            //X�� ��ġ�� ���� ������ ����� �ʵ��� ����
            spawnPosition.x = Mathf.Clamp(spawnPosition.x, -gameWidth / 2 + halfFruitSize, gameWidth / 2 - halfFruitSize);

            currentFruit = Instantiate(fruitPrefabs[currentFruitType], spawnPosition, Quaternion.identity);     //���� ����
            currentFruit.transform.localScale = new Vector3(fruitSizes[currentFruitType], fruitSizes[currentFruitType], 1);   //���� ũ�� ����

            Rigidbody2D rb = currentFruit.GetComponent<Rigidbody2D>();
            if (rb != null)
            {

                rb.gravityScale = 0f;
            }
        }
    }

    void DropFruit()
    {
        Rigidbody2D rb = currentFruit.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 1f;                //�߷��� ���� ������ ���� ��Ų��.

            currentFruit = null;                 //���� ��� �ִ� ���� ����

            fruitTimer = 1.0f;                   //Ÿ�̸� �ʱ�ȭ
        }
    }

    public void MergeFruits(int fruitType, Vector3 position)
    {
        if (fruitType < fruitPrefabs.Length - 1)
        {
            GameObject newFruit = Instantiate(fruitPrefabs[fruitType + 1], position, Quaternion.identity);
            newFruit.transform.localScale = new Vector3(fruitSizes[fruitType + 1], fruitSizes[fruitType + 1], 1.0f);

            //���� �߰� ���� ���
        }
    }


    public void CheckGameOver()       //���� ���� üũ �Լ�
    {
        Fruit[] allFruits = FindObjectsOfType<Fruit>();          //Scene�� �ִ� ��� ���� ������Ʈ�� �پ��ִ� ������Ʈ�� �����´�. ���� ���ӿ��縸 ��� ����� ��

        float gameOverHeight = gameHeight;

        for (int i = 0; i < allFruits.Length; i++)
        {
            if (allFruits[i] != null)
            {
                Rigidbody2D rb = allFruits[i].GetComponent<Rigidbody2D>();

                //������ ���� �����̰� ���� ��ġ�� �ִٸ�
                if (rb != null && rb.velocity.magnitude < 0.1f && allFruits[i].transform.position.y > gameOverHeight)
                {
                    isGameOver = true;
                    Debug.Log("���� ����");

                    break;
                }
            }
        }
    }
}
