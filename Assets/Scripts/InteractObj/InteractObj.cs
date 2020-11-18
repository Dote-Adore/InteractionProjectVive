using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;


// 交互类型
public enum InteractObjType
{
    Reapetdly = 0, Once = 1, Twice = 2, Pickup = 3
}

public class InteractObj : MonoBehaviour
{
    public InteractObjType type = InteractObjType.Twice;
    public bool canInteract;
    public bool shouldDestroy;
    public float interactTime = 0.5f;
    private bool isFirstInteraction = true;

    // 当交互时同时调用的事件
    public UnityEvent InteractEvent;
    //交互时间是否被重写
    public bool isTimeOverrided = false;
    private PickupObj pickupObj;
    Animator animator;

    private Renderer[] renderers;
    private Material HighlightMat =null;
    void Start()
    {
        if (GetComponent<Animator>())
        {
            animator = GetComponent<Animator>();
        }
        if (GetComponent<PickupObj>())
        {
            type = InteractObjType.Pickup;
            pickupObj = GetComponent<PickupObj>();
        }
        if (GetComponent<Outline>())
            GetComponent<Outline>().SetOutline(false);

        // 获取包括子类的renderers
        renderers = GetComponentsInChildren<Renderer>();

    }

    void OnEnable()
    {
    //    Debug.Log("Interact Obj on enable:" + gameObject.name);
    }
    public void Interact()
    {
        // 如果当前不可交互
        if (!canInteract)
            return;
        canInteract = false;
        switch (type)
        {
            // 两次交互
            case InteractObjType.Twice:
                InteractTwice();
                // 延时执行
                if (!isTimeOverrided)
                    StartCoroutine(InteractDown());
                break;
            case InteractObjType.Pickup:
                Pickup();
                // 延时执行
                if (!isTimeOverrided)
                    StartCoroutine(InteractDown());
                break;
            case InteractObjType.Once:
                OnceInteract();
                break;
        }
    }
    public void ReadyToInteract()
    {
        Debug.Log("ready to interact");
        // outline的宽度
        if (GetComponent<Outline>())
            GetComponent<Outline>().SetOutline(true);
    }
    public void CancelInteract()
    {
        if (GetComponent<Outline>())
            GetComponent<Outline>().SetOutline(false);
    }

    IEnumerator InteractDown()
    {
        yield return new WaitForSeconds(interactTime);
        canInteract = true;
    }

    private void InteractTwice()
    {
        Debug.Log("Interact " + this.name);
        if (isFirstInteraction)
        {
            isFirstInteraction = false;
            animator.SetBool("Interaction", true);
        }
        else
        {
            isFirstInteraction = true;
            animator.SetBool("Interaction", false);
        }
    }

    // 捡起
    private void Pickup()
    {
        Debug.Log("Pickup");
        if (isFirstInteraction)
        {
            CancelInteract();
            isFirstInteraction = false;
            pickupObj.Pickup();
        }
        else
        {
            ReadyToInteract();
            pickupObj.PickDown();
            isFirstInteraction = true;
        }
    }

    public bool IsFirstInteraction()
    {
        return isFirstInteraction;
    }

    // 添加高亮材质
    public void Highlight(Material HighlightMat)
    {
        if (this.HighlightMat == null)
        {
            this.HighlightMat = HighlightMat;
        }
        foreach (var renderer in renderers)
        {
            var materials = renderer.sharedMaterials.ToList();
            materials.Add(HighlightMat);
            renderer.materials = materials.ToArray();
        }
    }

    // 取消高亮材质
    public void CancelHighLight()
    {

        foreach (var renderer in renderers)
        {
            var materials = renderer.sharedMaterials.ToList();

            materials.Remove(HighlightMat);
            renderer.materials = materials.ToArray();
        }
    }

    public void OnceInteract()
    {
        canInteract = false;
        this.tag = "Untagged";
        // 取消高亮
        CancelHighLight();
        InteractEvent.Invoke();
        // 只能执行一次
    }
}