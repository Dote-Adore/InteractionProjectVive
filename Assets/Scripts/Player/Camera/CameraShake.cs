using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{

    public bool isShake = false;


    private Animator cameraAnimator = null;
    private Camera camera = null;

    private float targetFOV = 70;
    // Start is called before the first frame update
    void Start()
    {
        if (this.GetComponent<Animator>())
        {
            cameraAnimator = GetComponent<Animator>();
        }
        else
        {
            Debug.LogWarning("canot find Animator in MainCamera, please check!");
        }
        camera = GetComponent<Camera>();

    }
    void Update()
    {
        setFOVEase();
    }

    public void RunShake(bool isShake)
    {
        if (isShake)
        {
            this.isShake = isShake;
            cameraAnimator.SetBool("IsShake", true);
            targetFOV = 75;

        }
        else
        {
            this.isShake = isShake;
            cameraAnimator.SetBool("IsShake", false);
           targetFOV = 70;
        }
    }

    private void setFOVEase()
    {

        if (Mathf.Abs(camera.fieldOfView - targetFOV) < 0.5)
        {
            camera.fieldOfView = targetFOV;
        }
        else
        {
            float currentFOV = camera.fieldOfView;
            camera.fieldOfView = Mathf.Lerp(currentFOV, targetFOV, Time.deltaTime*10);
        }
    }
}
