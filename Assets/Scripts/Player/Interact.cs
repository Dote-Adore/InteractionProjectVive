using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Interacttype
{
    UI = 0,
    InterctProp = 1
}

public class Interact : MonoBehaviour
{
    public bool disableInteract = false;
    private bool canInteract = true;
    public Interacttype type = Interacttype.InterctProp;
    private RayCast rayCast;
    private GameObject interactObj;
    public Light light;
    
    // Start is called before the first frame update
    void Start()
    {
        try
        {
            rayCast = GetComponentInChildren<RayCast>();
        }
        catch
        {
            Debug.LogError("未在children中找到rayCast组件，请挂载！");
        }
        // 获取当前交互的物体
            interactObj = rayCast.GetInteractObj();
    }

    // Update is called once per frame
    void Update()
    {
        if (disableInteract == true)
            return;
        interactObj = rayCast.GetInteractObj();
        float interactValue = Input.GetAxis("Interact");
        if (interactValue.Equals(1f) && canInteract)
        {
            canInteract = false;
            // 如果当前物体可以交互
            if (interactObj != null && interactObj.GetComponent<InteractObj>().canInteract)
            {
                Debug.Log("canInteract");
                interactObj.GetComponent<InteractObj>().Interact();
            }
        }
        if (interactValue.Equals(0))
        {
            canInteract = true;
        }
    }
}
