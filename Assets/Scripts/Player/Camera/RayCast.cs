using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 发射射线，进行交互
public class RayCast : MonoBehaviour
{
    // 可交互的距离
    public float interactDistance = 1f;
    public bool canDrawRay = true;
    public UI_Pcikup ui_pickup;
    private GameObject pickGameObj = null;
    private Ray ray;
    private RaycastHit hit;
    private bool canShowBtn = true;
    public Material HighLightMat;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (canDrawRay)
            CastObject();
        // 如果存在可交互物体

        // 是否可以显示按钮
        if (canShowBtn)
        {
            if (pickGameObj)
                SetButtonPrompt(true);
            else
                SetButtonPrompt(false);
        }
        else
        {
            SetButtonPrompt(false);
        }
    }

    void CastObject()
    {
        // 从屏幕中心发射一条射线
        ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        if (Physics.Raycast(ray, out hit))
        {
            GameObject currentHitObj = hit.collider.gameObject;
            // 如果超出可交互范围之外
            if (Vector3.Distance(transform.position, currentHitObj.transform.position) > interactDistance)
            {
                // if(pickGameObj == currentHitObj)
                if (pickGameObj != null)
                {
                    pickGameObj.GetComponent<InteractObj>().CancelInteract();
                    pickGameObj.GetComponent<InteractObj>().CancelHighLight();
                    pickGameObj = null;
                }
                return;
            }

            // 如果当前指向的是可交互物体
            if (currentHitObj.tag == "InteractObj")
            {
                // 如果之前没有指向可交互的物体
                if (pickGameObj == null)
                {
                    pickGameObj = currentHitObj;
                    // 准备与新物体交互
                    pickGameObj.GetComponent<InteractObj>().ReadyToInteract();
                    // 高亮
                    pickGameObj.GetComponent<InteractObj>().Highlight(HighLightMat);
                }
                // 如果现在的物体和之前的物体不同
                else if (pickGameObj != currentHitObj)
                {
                    // 取消之前物体交互
                    pickGameObj.GetComponent<InteractObj>().CancelInteract();
                    pickGameObj.GetComponent<InteractObj>().CancelHighLight();
                    pickGameObj = currentHitObj;
                    pickGameObj.GetComponent<InteractObj>().ReadyToInteract();
                    pickGameObj.GetComponent<InteractObj>().Highlight(HighLightMat);

                }
            }
            // 如果当前指向的是不可交互物体
            else if (pickGameObj != null)
            {
                // 取消之前的交互
                pickGameObj.GetComponent<InteractObj>().CancelInteract();
                pickGameObj.GetComponent<InteractObj>().CancelHighLight();

                pickGameObj = null;

            }
        }
        // 如果没有碰到任何东西
        else if (pickGameObj != null)
        {
            pickGameObj.GetComponent<InteractObj>().CancelInteract();
            pickGameObj.GetComponent<InteractObj>().CancelHighLight();

            pickGameObj = null;
        }
    }

    private void SetButtonPrompt(bool Isvisiable)
    {
        ui_pickup.ButtonPrompt(Isvisiable);
    }

    public GameObject GetInteractObj()
    {
        if (pickGameObj != null && pickGameObj.tag == "InteractObj")
        {
            return pickGameObj;
        }
        else
            return null;
    }


    public void setCanshowBtn(bool canShowBtn)
    {
        this.canShowBtn = canShowBtn;
    }
}
