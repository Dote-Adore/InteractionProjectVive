using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class playerOutELevatorEvent : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Equals("Player"))
        {
            other.transform.parent = null;
            this.GetComponent<BoxCollider>().enabled = false;
            other.GetComponent<PlayerMove>().useGravity = true;
            other.GetComponent<PlayerMove>().Follow(0,null);
        }
    }
}
