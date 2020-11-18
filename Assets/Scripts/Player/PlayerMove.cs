using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // 移动速度
    public float walkSpeed = 1f;
    // 奔跑速度
    public float runSpeed = 1.5f;
    // 是否使用重力
    public bool useGravity;
    public float gravity = 9.8f;
    // 当前找到的物品数量
    private int  currentFindPickupObjNum = 0;
    // 物品数量的ui显示
    public FinedNumsShow numsShowUI;
    public Canvas CongratulationUI;
    public Canvas PauseGameUI;
    
    // 相机的transform
    private Transform cameraTransform;
    private float moveSpeed;
    private CharacterController controller;
    private float gravitySpeed = 0;
    public bool canMove = true;
    private  PickupObj[] allPickupObjs;
    private List<PickupObj> findedPickupObjs;
    private GameObject followOthers = null;
    private float followYOffset = 0;
    void Start()
    {
        moveSpeed = walkSpeed;
        // 获取相机
        foreach(Transform transform in GetComponentsInChildren<Transform>())
        {
            if(transform.name =="Main Camera")
            {
                Debug.Log("Find Camera!");
                cameraTransform = transform;
            }
        }
        if (!cameraTransform)
        {
            Debug.LogError("Can not Find Camera in Children");
        }
        controller = this.GetComponent<CharacterController>();
        
        //获取所有的pickupobj
        allPickupObjs = Object.FindObjectsOfType<PickupObj>();
        findedPickupObjs = new List<PickupObj>();
        Debug.Log("all obj = "+allPickupObjs.Length);
        if (numsShowUI != null)
        {
            numsShowUI.UpdateUIMessage(0,allPickupObjs.Length);
        }
        if (CongratulationUI != null)
        {
            CongratulationUI.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {


        // 如果不能移动
        if (!canMove)
            return;

        // 获取y轴旋转角度
        float yRotation = cameraTransform.rotation.eulerAngles.y;
        // 角度转弧度
        yRotation = yRotation * Mathf.Deg2Rad;
        // cos值
        float cos = Mathf.Cos(yRotation);
        // sin 值
        float sin = Mathf.Sin(yRotation);
        // 向右移动
        moveRight(sin, cos);
        moveFoward(sin, cos);
        SpeedUp();
        // 如果使用重力
        if (useGravity)
        {
            SetGravity();
        }
        PauseGame();
    }

    void LateUpdate()
    {
        if (followOthers != null)
        {
            float y = followOthers.transform.position.y - followYOffset;
            Vector3 currentPosition = this.transform.position;
            this.transform.position = new Vector3(currentPosition.x, y, currentPosition.z);
        }
    }
    private float horizontalValue;

    private float verticalValue;
    // 向右移动
    void moveRight(float sin, float cos)
    {
        horizontalValue = Input.GetAxis("Horizontal") * moveSpeed;
        Vector3 moveDirection = horizontalValue * (-Vector3.forward * sin+Vector3.right * cos);
        controller.Move(moveDirection * moveSpeed*Time.deltaTime);
    }

    // 向前移动
    void moveFoward(float sin, float cos)
    {
        verticalValue = Input.GetAxis("Vertical") * moveSpeed;
        Vector3 moveDirection = verticalValue * (Vector3.forward * cos + Vector3.right * sin);
        controller.Move(moveDirection* moveSpeed * Time.deltaTime);
    }

    //加速
    void SpeedUp()
    {
        float speed = Input.GetAxis("SpeedUp");
        // 如果启动加速键
        if (speed == 1f)
        {
            // 奔跑的时候镜头抖动效果
            if(verticalValue !=0)
                GetComponentInChildren<CameraShake>().RunShake(true);
            else
              GetComponentInChildren<CameraShake>().RunShake(false);
            moveSpeed = runSpeed;
        }
        else
        {
            GetComponentInChildren<CameraShake>().RunShake(false);
            moveSpeed = walkSpeed;
        }
    }
    //设置重力
    void SetGravity()
    {
        if (controller.isGrounded)
        {
            gravitySpeed = 0;
        }
        gravitySpeed -= (gravity * Time.deltaTime);
        Vector3 move = Vector3.zero;
        move.y += gravitySpeed;
        controller.Move(move * Time.deltaTime);
    }

    public void Follow(float Yoffset,GameObject gameObject)
    {
        this.followYOffset = Yoffset;
        followOthers = gameObject;
    }

    public void findAPickUpObj(PickupObj obj)
    {
        // 如果没有找过
        if (!findedPickupObjs.Contains(obj))
        {
            findedPickupObjs.Add(obj);
            currentFindPickupObjNum++;
            if (numsShowUI != null)
            {
             numsShowUI.UpdateUIMessage(currentFindPickupObjNum,allPickupObjs.Length);   
            }
            Debug.Log("当前找到的数量:"+currentFindPickupObjNum+";总数量："+allPickupObjs.Length);
        }
    }

    // 全部找到时的操作
    public void ExecuteFindAllEvent()
    {
        if (currentFindPickupObjNum < allPickupObjs.Length)
        {
            return;
        }

        if (CongratulationUI != null)
        {

            canMove = false;
            CongratulationUI.gameObject.SetActive(true);
            CameraRotation cameraRotate = this.GetComponentInChildren<CameraRotation>();
            cameraRotate.lockCursor = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            if (cameraRotate != null)
            {
                cameraRotate.lockRotation = true;
            }

            RayCast rayCast = this.GetComponentInChildren<RayCast>();
            if (rayCast != null)
            {
                rayCast.canDrawRay = false;
            }

            this.GetComponent<Interact>().disableInteract = true;
        }
    }

    private bool canPause = true; 
    private void PauseGame()
    {
        
        if (Input.GetAxis("Esc").Equals(1f)&&canPause)
        {
            canPause = false;
            Debug.Log("ESC");
            if (PauseGameUI != null)
            {
                PauseGameUI.gameObject.SetActive(true);
                this.GetComponentInChildren<CameraRotation>().lockCursor = false;
                this.GetComponentInChildren<CameraRotation>().lockRotation = true;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                Interact interact = GetComponent<Interact>();
                interact.disableInteract = true;
                canMove = false;
            }

        }

        if (Input.GetAxis("Esc").Equals(0))
            canPause = true;
    }
}
