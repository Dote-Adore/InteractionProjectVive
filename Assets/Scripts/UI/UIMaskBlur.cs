using UnityEngine.UI;
using UnityEngine;

public class UIMaskBlur : MonoBehaviour
{
    public Material uiMaskMaterial;
    public Material defaultMat;
    public float Speed = 4;
    public float BlurStrength = 2;
    public Color colorMask = new Color(0.3f,0.3f,0.3f,1);

    private float tagetBlurStrength = 0;
    private Color targetColor = new Color(1, 1, 1, 1);
    private bool animationDone = true;
    void Start()
    {
        GetComponent<Image>().material = defaultMat;
    }
    void Update()
    {
        if (!animationDone)
        {
            Masking();
        }
    }

    private void Masking()
    {
        float size = 0;
  //      Debug.Log(uiMaskMaterial.GetFloatArray("_DistortionBlurRemapping"));
        Color color = uiMaskMaterial.GetColor("_Color");
        // 如果动画到达一个阈值，则完成动画
        if (Mathf.Abs(size - tagetBlurStrength) < 0.01)
        {
            uiMaskMaterial.SetFloat("_Size", tagetBlurStrength);
            uiMaskMaterial.SetColor("_Color", targetColor);
            animationDone = true;
            if (tagetBlurStrength == 0)
            {
                GetComponent<Image>().material = defaultMat;
            }
            return;

        }
        size = Mathf.Lerp(size, tagetBlurStrength, Time.deltaTime * Speed);
        uiMaskMaterial.SetFloat("_Size", size);
        color = Color.Lerp(color, targetColor, Time.deltaTime * Speed);
        uiMaskMaterial.SetColor("_Color", color);
    }

    public void BlurMaskUI(bool isBlur)
    {
        animationDone = false;
        // 如果需要模糊
        GetComponent<Image>().material = uiMaskMaterial;
        if (isBlur)
        {
            tagetBlurStrength = BlurStrength;
            targetColor = colorMask;
        }
        else
        {
            tagetBlurStrength = 0;
            targetColor = new Color(1, 1, 1, 1);
        }
    }
}
