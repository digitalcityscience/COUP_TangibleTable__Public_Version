using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CanvasMovement : MonoBehaviour
{
    // Start is called before the first frame update
    Transform myTransform;
    Vector3 myPosition;

    private void Start()
    {
        myTransform = GetComponent<Transform>();
        myPosition = myTransform.position;
    }

    private void Update()
    {
        // myTransform.localPosition = myPosition;

        myTransform.DOLocalMove(myPosition, .3f).SetEase(Ease.OutQuad);
    }

    public void updatePosition(float x, float y)
    {
        myPosition = new Vector3(x, 0.1f, y);
    }
}
