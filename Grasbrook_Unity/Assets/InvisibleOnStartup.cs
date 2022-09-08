using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleOnStartup : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        gameObject.GetComponent<Renderer>().enabled = false;
    }
}
