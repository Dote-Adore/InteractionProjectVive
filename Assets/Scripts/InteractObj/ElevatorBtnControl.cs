using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
//using UnityEngine.Experimental.Rendering.HDPipeline;
using UnityEngine.Rendering.HighDefinition;
// using UnityEngine.Windows;

public class ElevatorBtnControl : MonoBehaviour
{
    public Material lightOffMat;
    public Material levelNumMat;

    public List<Texture2D> numTexture2Ds=null;
    // 方向Mat，
    public Material dirMat;
    private GameObject showNum;
    private GameObject showDir;
    private GameObject upBtn;
    private GameObject downBtn;
    public float elevatorSpeed = 2.0f;
    public ElevatorEvent elevatorBody;

    private Material numMaterialIns;

    List<Material> lightOffMaterials = new List<Material>();
    List<Material> dirMaterials = new List<Material>();
    List<Material> numMaterials = new List<Material>();

    void Start()
    {
        Transform[] Childern = GetComponentsInChildren<Transform>();

        // 获取需要的子对象
        foreach (var child in Childern)
        {
            switch (child.name)
            {
                case "showNum":
                    showNum = child.gameObject;
                    break;
                case "ShowDir":
                    showDir = child.gameObject;
                    break;
                case "UpBtn":
                    upBtn = child.gameObject;
                    break;
                case "DownBtn":
                    downBtn = child.gameObject;
                    break;
            }
        }
        numMaterialIns = Instantiate(levelNumMat);
        numMaterialIns.name = "NumMat(ins)";

        lightOffMaterials.Add(lightOffMat);
        dirMaterials.Add(dirMat);
        numMaterials.Add(numMaterialIns);

    }

    // 显示向上的按钮
    public void Up()
    {
        // 显示向上按钮
        Renderer upBtnRender = upBtn.GetComponent<Renderer>();

        upBtnRender.materials = numMaterials.ToArray();


        // 显示电梯正在向上
        Renderer dirRender = showDir.GetComponent<Renderer>();
        
        dirRender.materials = dirMaterials.ToArray();


        // 更换数字材质
        Renderer numRender = showNum.GetComponent<Renderer>();
        numRender.materials = numMaterials.ToArray();
        NumChange(true);
        elevatorBody.AudioRun();
    }
    // 更改数字w
    private int texnum = 0;
    private void NumChange(bool isUp)
    {
        StartCoroutine(ChangeNum());
    }


    IEnumerator ChangeNum()
    {
        if (texnum <numTexture2Ds.Count)
        {
            numMaterialIns.EnableKeyword("_EmissiveTex");
            showNum.GetComponent<Renderer>().material.SetTexture("_EmissiveTex", numTexture2Ds[texnum]);
            if (texnum == numTexture2Ds.Count - 1)
            {
                Debug.Log("elevator come");
                LightOff(showDir);
                LightOff(upBtn);
                elevatorBody.OpenDoor();
            }
            texnum++;
            // 如果是最后一个,将箭头取消
            yield return new WaitForSeconds(elevatorSpeed);
            StartCoroutine(ChangeNum());
        }
    }

    private void LightOff(GameObject gameObject)
    {
        gameObject.GetComponent<Renderer>().materials = lightOffMaterials.ToArray();
    }
}
