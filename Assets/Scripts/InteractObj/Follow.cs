using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public GameObject TargetObj;

    private InteractObj interactObj;
    private Vector3 offset;
    void Start()
    {
        interactObj = GetComponent<InteractObj>();
        offset = this.transform.position - TargetObj.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (interactObj.IsFirstInteraction()&&interactObj.canInteract)
        {
            transform.position = TargetObj.transform.position + offset;
        }
    }
}
