using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindParticial : MonoBehaviour
{
    public List<Color> colors;
    public Material[] materials;

    public void ChangeColor(int speedValue)
    {
        print("SpeedValue = " +speedValue);
            if (speedValue <= 10)
            {
                materials[1].SetColor("_EmissionColor", colors[0]);
            }
            else if(speedValue > 10 && speedValue <= 20)
            {
                materials[1].SetColor("_EmissionColor", colors[1]);
            }
            else if (speedValue > 20 && speedValue <= 30)
            {
                materials[1].SetColor("_EmissionColor", colors[2]);
            }
            else if (speedValue > 30 && speedValue <= 40)
            {
                materials[1].SetColor("_EmissionColor", colors[3]);
            }
            else if (speedValue > 40 && speedValue <= 50)
            {
                materials[1].SetColor("_EmissionColor", colors[4]);
            }
            else if (speedValue > 50 && speedValue <= 60)
            {
                materials[1].SetColor("_EmissionColor", colors[5]);
            }
            else
            {
                materials[1].SetColor("_EmissionColor", colors[6]);
            }

    }
}
