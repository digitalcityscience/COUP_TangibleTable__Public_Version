using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAssign : MonoBehaviour
{

    private TableInterface tableInterface;

    // Start is called before the first frame update
    void Start()
    {
        tableInterface = GameObject.Find("NetworkingManager").GetComponent<TableInterface>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
