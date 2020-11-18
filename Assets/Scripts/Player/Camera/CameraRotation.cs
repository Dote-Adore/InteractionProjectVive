using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
public class CameraRotation : MonoBehaviour
{
    // 是否锁定鼠标
    public bool lockCursor;
    public bool lockRotation;
    // X speed
    public float sensitivityX = 5F;
    // Y speed
    public float sensitivityY = 5f;
    // x mini rotation;
    public float minimumX = -60f;
    // y max rotation;
    public float maxmumX = 60f;
    private float rotationX = 0f;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // 锁定鼠标位置
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        if (!lockRotation)
        {
            // x轴转向
            float rotationY = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;
            rotationX += Input.GetAxis("Mouse Y") * sensitivityY;
            // 限制y轴转向
            rotationX = Mathf.Clamp(rotationX, minimumX, maxmumX);
            // 赋予新值
            transform.localEulerAngles = new Vector3(-rotationX, rotationY, transform.localRotation.z);
        }
    }
}
