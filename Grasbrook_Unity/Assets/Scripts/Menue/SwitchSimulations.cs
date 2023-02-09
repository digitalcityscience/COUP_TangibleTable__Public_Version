using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchSimulations : MonoBehaviour
{
    MenuSystem menuSystem;
    LegendChanger legendChanger;

    [SerializeField]
    GameObject noiseAnimation;
    [SerializeField]
    GameObject cityBlock;
    [SerializeField]
    GameObject bridgeHC;

    // Start is called before the first frame update
    void Start()
    {
        menuSystem = GameObject.Find("WheelController").GetComponent<MenuSystem>();
        legendChanger = GameObject.Find("LegendBox").GetComponent<LegendChanger>();
    }

    public void SwitchBetweenSimulations(string simName)
    {
        if(simName == "noise")
        {
            menuSystem.SimulationName = "noise";
            legendChanger.ChangeLegendInforamtion("noise", 0, 3);
            noiseAnimation.SetActive(true);
            bridgeHC.SetActive(true);
            cityBlock.SetActive(false);
        }
        else if(simName == "wind")
        {
            menuSystem.SimulationName = "wind";
            legendChanger.ChangeLegendInforamtion("wind", 0, 3);
            noiseAnimation.SetActive(false);
            bridgeHC.SetActive(true);
            cityBlock.SetActive(false);
        }
        else if(simName == "abm")
        {
            menuSystem.SimulationName = "abm";
            legendChanger.ChangeLegendInforamtion("abm", 0, 5);
            noiseAnimation.SetActive(false);
            cityBlock.SetActive(false);
            GlobalVariable.GlobalABMBlocks = "open";
        }
    }
}
