using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Experimental.Rendering.HDPipeline;
using UnityEngine.Rendering.HighDefinition;

public class ElevatorTrigger : MonoBehaviour
{
    public ElevatorEvent elevatorEvent;
    private bool enableTrigger = true;


    private bool isPlayerIn = false;
    private float playerYOffset = 0;
    private GameObject player;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        // 如果player进入
        if (other.gameObject.name.Equals("Player"))
        {
            // 关门
            elevatorEvent.CloseDoor();
            this.GetComponent<BoxCollider>().enabled = false;
            playerYOffset = elevatorEvent.transform.position.y - other.transform.position.y;
            other.GetComponent<PlayerMove>().useGravity = false;
            other.GetComponent<PlayerMove>().Follow(playerYOffset,elevatorEvent.gameObject);
        }
    }
}
