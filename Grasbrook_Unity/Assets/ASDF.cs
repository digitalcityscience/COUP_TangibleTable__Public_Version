using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ASDF : MonoBehaviour
{
   public List<GameObject> objects;
    // Start is called before the first frame update
    void Start()
    {
        foreach(var x  in objects)
        Instantiate(x);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
