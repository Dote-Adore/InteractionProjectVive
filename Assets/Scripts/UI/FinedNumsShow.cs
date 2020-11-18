using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinedNumsShow : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateUIMessage(int currentNum, int TotalNums)
    {
        Text content = this.GetComponent<Text>();
        content.text = "目前找出：" + currentNum + "/" + TotalNums;
    }
}
