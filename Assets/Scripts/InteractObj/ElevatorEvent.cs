using System.Collections;
using System.Collections.Generic;
// using Boo.Lang;
using UnityEngine;
using UnityEngine.Audio;

public class ElevatorEvent : MonoBehaviour
{
    public Texture2D[] numTexture2Ds = null;
    public System.Collections.Generic.List<Texture2D> DirTex = null;
    public GameObject ScreenNum = null;
    public GameObject SrceenDir = null;
    public bool isUp = false;
    public int currentLevel = 3;
    public int targetLevel = 0;
    public Material[] lightOffMat = null;
    public float elevatorSpeed = 2.0f;
    public AudioSource audio;


    private Animator animator = null;
    public float DoorAnimTime = 5.0f;
    private Material ScreenNumMat = null;


    private bool canCloseDoor = true;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OpenDoor()
    {
        animator.SetBool("OpenDoor",true);
    }
    // 关电梯门
    public void CloseDoor()
    {
     if(!canCloseDoor)
         return;
        animator.SetBool("OpenDoor", false);
    }
    public void Down()
    {
        animator.SetBool("Down",true);
        Renderer ScreenNum_Ren= ScreenNum.GetComponent<Renderer>();
        ScreenNumMat = ScreenNum_Ren.materials[0];
        isUp = false;
        StartCoroutine(ChangeNum());
    }


    IEnumerator ChangeNum()
    {
        // 往下开
        if (!isUp)
        {
            if (currentLevel >= targetLevel)
            {
                ScreenNumMat.EnableKeyword("_EmissiveTex");
                Debug.Log("haha");
                ScreenNum.GetComponent<Renderer>().material.SetTexture("_EmissiveTex", numTexture2Ds[currentLevel]);
                if (currentLevel == targetLevel)
                {
                    LightOff(SrceenDir);
                    canCloseDoor = false;
                    OpenDoor();
                }
                currentLevel--;
                yield return  new WaitForSeconds(elevatorSpeed);
                StartCoroutine(ChangeNum());
            }
        }
    }

    private void LightOff(GameObject gameObject)
    {
        gameObject.GetComponent<Renderer>().materials = lightOffMat;
    }

    public void RunElevator()
    {
        if(currentLevel>targetLevel)
            Down();
    }

    public void AudioRun()
    {
        audio.Play();
    }
}
