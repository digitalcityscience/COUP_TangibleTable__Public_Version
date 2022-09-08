using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCollision : MonoBehaviour
{
    BuildingManager bm;
    public int testNumber = 0;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Building"))
        {
            testNumber += 8;
            gameObject.GetComponent<Renderer>().material.color = new Color(1,0,0);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        gameObject.GetComponent<Renderer>().material = bm.buildingMat;
    }
}
