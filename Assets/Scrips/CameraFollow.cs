using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 5,- 10);
    public float smoothSpeed = 0.125f;

    private void LateUpdate()                    //ī�޷� �������� ���� LateUpdate ���� ó��
    {
        Vector3 desiredPostion = target.position + offset;
        Vector3 somoothPosition = Vector3.Lerp(transform.position, desiredPostion, smoothSpeed);
        transform.position = somoothPosition;

        transform.LookAt(transform.position);

    }
}
