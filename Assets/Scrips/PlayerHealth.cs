using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxLives = 3;            //�ִ� �����
    public int currentLives;            //���� �����

    public float invincibleTime = 1.0f;     //�ǰ� �� ���� �ð�(�ݺ� �ǰ� ����)
    public bool isInvincible = false;       //���� ���� ��
    // Start is called before the first frame update
    void Start()
    {
        currentLives = maxLives;            //����� �ʱ�ȭ
    }
    private void OnTriggerEnter(Collider other)     //Ʈ���� ���� �ȿ� ���Գ��� �˻��ϴ� �Լ�
    {
        //���� ����
        if (other.CompareTag("Missile"))            //�̻��ϰ� �浹�ϸ�

        {
            currentLives--;
            Destroy(other.gameObject);              //�̻��� ������Ʈ�� ���ش�.

            if (currentLives <= 0)                  //���� ü���� 0 ������ ���
            {
                GameOver();
            }
        }
    }
            // Update is called once per frame
    void GameOver()       //���� ���� ó��
    {
        gameObject.SetActive(false);                //�÷��̾� ��Ȱ��ȭ
        Invoke("RestarGame", 3.0f);                 //3�� �� ���� �� �����
    }

    void RestarGame()
    {
        //���� �� �����
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
