using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkColor : MonoBehaviour
{
    [SerializeField]
    Color c1;

    [SerializeField]
    Color c2;

    [SerializeField]
    float speed;

    [SerializeField]
    new bool enabled = true;

    new Renderer renderer;

    float time;

    private void OnEnable()
    {
        renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enabled)
        {
            time += Time.deltaTime;
            renderer.material.color = Color.Lerp(c1, c2, (Mathf.Sin((time * speed)) + 1) / 2);
        }
    }
}
