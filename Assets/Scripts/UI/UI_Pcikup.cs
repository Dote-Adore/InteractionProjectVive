using System.Collections;
using UnityEngine.UI;
using UnityEngine;

// 当物体拿起时显示的界面
public class UI_Pcikup : MonoBehaviour
{
    public UIMaskBlur blurMask;
    public Text UI_itemName;
    public Text UI_introduction;
    // 按键提示
    public RectTransform buttonPrompt;
    public float secondanimDelay = 0.3f;

    private Animation itemNameAnim;
    private Animation introductionAnim;
    void Start()
    {
        itemNameAnim = UI_itemName.GetComponent<Animation>();
        introductionAnim = UI_introduction.GetComponent<Animation>();

    }
    public void Blur(bool isBlur)
    {
        blurMask.BlurMaskUI(isBlur);
    }

    public void SetText(string itemName, string introduction)
    {
        if (itemName == null)
        {
            itemNameAnim["ItemNameAnim"].speed = -1;
            introductionAnim["IntroductionAnim"].speed = -1;
            itemNameAnim.Play("ItemNameAnim");
            introductionAnim.GetComponent<Animation>().Play("IntroductionAnim");
            return;
        }
        itemNameAnim["ItemNameAnim"].speed = 1;
        introductionAnim["IntroductionAnim"].speed = 1;
        UI_itemName.text = itemName;
        UI_introduction.text = introduction;
        itemNameAnim.Play("ItemNameAnim");
        introductionAnim.Play("IntroductionAnim");
        // DelayAnim();

    }
    

    // 按键提示
    public void ButtonPrompt(bool isVisable)
    {
        buttonPrompt.gameObject.SetActive(isVisable);
        // Debug.Log("Button Visable:" + isVisable);
    }

    IEnumerator DelayAnim()
    {
        yield return new WaitForSeconds(secondanimDelay);

    }
}
