using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObj : MonoBehaviour
{
    public Vector3 offset = new Vector3(0, 0, 1);
    public Quaternion TargetRelativeRotation = new Quaternion(1,1,1,0);
    // 相机
    public GameObject camera = null;
    public PlayerMove player;
    public float speed = 10f;
    // 是否跟随其他物体
    public GameObject followOther = null;
    // 与其他物体的相对位移
    public Vector3 followOtherOffset = Vector3.zero;
    public UI_Pcikup UIMask = null;    // UI模糊遮罩的panel
    public bool overrideInteactTime = true;
    public float Threshold = 0.005f; //阈值

    public string UI_itemName;
    public string UI_introduction;
    private bool isPickedUp = false;    // 是否被捡起
    private Vector3 initialPosition = Vector3.zero;
    private Quaternion initialRotation; // 初始位置
    private Vector3 targetPosition; //捡起后目标位置
    private bool isPickedUpDone = false;    // 捡起动作是否完成
    private InteractObj InteractObj;

    private Quaternion TargetRotation;
    private Light light = null;
    private Transform[] childrenObj;
    void Start()
    {
        // 如果需要跟随其他物体
        if (followOther !=null)
        {
            followOtherOffset = this.transform.position - followOther.transform.position;
        }
        if (overrideInteactTime)
        {
            InteractObj = GetComponent<InteractObj>();
            InteractObj.isTimeOverrided = true;
        }
        childrenObj = this.GetComponentsInChildren<Transform>();
        // 将所有的mask设置为default
        ChangeLayerMask("Default");
        this.light = player.GetComponent<Interact>().light;
    }

    // Update is called once per frame
    void Update()
    {
        // 如果初始位置为0，则当前没互动，则与被跟踪物体跟随运动
        if (initialPosition.Equals(Vector3.zero))
        {
            if(followOther)
                this.transform.position = followOther.transform.position + followOtherOffset;
            return;
        }
        //如果被捡起来,并且被捡起的动动作还为完成
        if (isPickedUp&&!isPickedUpDone)
        {
            if (Vector3.Distance(transform.position, targetPosition) > Threshold)
            {
               // this.transform.LookAt(Vector3.Lerp(/*transform.forward +*/transform.position, new Vector3(camera.transform.position.x,transform.position.y,camera.transform.position.z), Time.deltaTime * speed*2));
               this.transform.rotation = Quaternion.Lerp(this.transform.rotation,TargetRotation,Time.deltaTime*speed);
               transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * speed);
            }
            else
            {
                transform.position = targetPosition;
                this.transform.rotation = TargetRotation;
                //transform.LookAt(new Vector3(camera.transform.position.x,transform.position.y,camera.transform.position.z));
                isPickedUpDone = true;
                if (overrideInteactTime)
                {
                    InteractObj.canInteract = true;
                }
            }
        }
        // 如果不需要捡起来并且当前位置没有到达初始位置
        else if (!transform.position.Equals(initialPosition) && !isPickedUp)
        {
            transform.rotation = Quaternion.Lerp(this.transform.rotation, initialRotation, Time.deltaTime * speed);
            transform.position = Vector3.Lerp(transform.position, initialPosition, Time.deltaTime * speed);
        }
        // 如果回到初始位置，
        if (Vector3.Distance(this.transform.position,initialPosition)< Threshold)
        {
            player.ExecuteFindAllEvent();
            ChangeLayerMask("Default");
            this.transform.position = initialPosition;
            this.transform.rotation = initialRotation;
            initialPosition = Vector3.zero;
            if (overrideInteactTime)
            {
                InteractObj.canInteract = true;
            }
        }

    }
    // 拾起物体
    public void Pickup()
    {
        // 目标转动
        /*TargetRotation =  Quaternion.LookRotation(
            (new Vector3(camera.transform.position.x, transform.position.y, camera.transform.position.z) -
            transform.position).normalized);*/
        TargetRotation = Quaternion.LookRotation((camera.transform.forward * -1).normalized);
      //  TargetRotation = TargetRelativeRotation*TargetRotation;
        // 取消高亮
        this.GetComponent<InteractObj>().CancelHighLight();
        camera.GetComponent<RayCast>().canDrawRay = false;
        ChangeLayerMask("PickupObj");
        InteractObj.canInteract = false;
        light.enabled = true;
        isPickedUpDone = false;
        targetPosition = camera.transform.TransformPoint(offset);
        // UI遮罩
        UIMask.Blur(true);
        // 取消按键提示
        camera.GetComponent<RayCast>().setCanshowBtn(false);
        UIMask.SetText(UI_itemName, UI_introduction);
        player.canMove = false;
        camera.GetComponent<CameraRotation>().lockRotation = true;
        // 当前被捡起
        isPickedUp = true;
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        player.findAPickUpObj(this);
    }    
    // 放下物体
    public void PickDown()
    {
        light.enabled = false;
        camera.GetComponent<RayCast>().canDrawRay = true;
        // UI遮罩取消
        UIMask.SetText(null,null);
        // 打开按键提示
        camera.GetComponent<RayCast>().setCanshowBtn(true);
        UIMask.Blur(false);
        InteractObj.canInteract = false;
        player.canMove = true;
        camera.GetComponent<CameraRotation>().lockRotation = false;
        isPickedUp = false;
    }

    private void ChangeLayerMask(string LayerName)
    {
        if (LayerName.Equals("PickupObj"))
        {
            foreach (Transform i in childrenObj)
            {
                i.gameObject.layer = 8;
            }
            return;
        }
        else foreach (Transform i in childrenObj)
        {
            i.gameObject.layer = LayerMask.NameToLayer(LayerName);
        }
    }
}
