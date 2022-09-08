using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Blitzy.UnityRadialController;
using TMPro;
using DG.Tweening;

public class SimulationMenu : MonoBehaviour
{


    [SerializeField]
    private GameObject simulationChooser;

    [SerializeField]
    private GameObject noiseObject;

    [SerializeField]
    private GameObject windObject;

    [SerializeField]
    private GameObject backObject;

    [SerializeField]
    private GameObject pedestrianObject;

    [SerializeField]
    private GameObject thermalComfortObject;

    [SerializeField]
    private GameObject arrow;

    [SerializeField]
    private MenuSystem menuSystem;

    RadialController radialController;

    [SerializeField]
    LegendChanger legendChanger;

    [SerializeField]
    GameObject noiseAnimation;

    [SerializeField]
    GameObject cityBlock;
    [SerializeField]
    GameObject bridgeHC;

    float arrowRot;
    float rotationPoint;

    Transform arrowTransform;

    private void Start()
    {
        radialController = GameObject.Find("WheelController").GetComponent<RadialController>();
        arrowTransform = arrow.GetComponent<RectTransform>();

    }

    public void Open()
    {
        radialController.rotationResolutionInDegrees = 30;
        radialController.useAutoHapticFeedback = false;
        simulationChooser.SetActive(true);
    }

    public void Close()
    {
        simulationChooser.SetActive(false);
        menuSystem.SwitchTo(MenuSystem.Screen.main);
    }

    public void Press()
    {
        
        if (simulationChooser)
        {
            if (rotationPoint == -90 || rotationPoint == 270)
            {
                Close();
            }
            if (rotationPoint == -30 || rotationPoint == 330)
            {
                Debug.Log("Noise Simulation");
                menuSystem.SimulationName = "noise";
                Close();
                legendChanger.ChangeLegendInforamtion("noise", 0, 3);
                noiseAnimation.SetActive(true);
                bridgeHC.SetActive(true);
                cityBlock.SetActive(false);
            }
            if (rotationPoint == 30 || rotationPoint == -330)
            {
                Debug.Log("Wind Simulation");
                menuSystem.SimulationName = "wind";
                Close();
                legendChanger.ChangeLegendInforamtion("wind", 0, 3);
                noiseAnimation.SetActive(false);
                cityBlock.SetActive(false);
            }
            if (rotationPoint == 90 || rotationPoint == -270)
            {
                Debug.Log("Pedestrians Simulation");
                menuSystem.SimulationName = "abm";
                Close();
                legendChanger.ChangeLegendInforamtion("abm", 0, 5);
                noiseAnimation.SetActive(false);
                cityBlock.SetActive(false);
                GlobalVariable.GlobalABMBlocks = "open";
            }
            if (rotationPoint == 150 || rotationPoint == -210)
            {
                Debug.Log("Thermal Comfort Simulation");
                menuSystem.SimulationName = "sun";
                Close();
                legendChanger.ChangeLegendInforamtion("sun", 0, 5);
                noiseAnimation.SetActive(false);
                cityBlock.SetActive(false);
            }
        }
        
    }

    public void OnRotation(float deg)
    {
        arrowRot += deg;
        arrowRot = arrowRot % 360;

        arrowTransform.localRotation = Quaternion.Euler(0, 0, -arrowRot);
        rotationPoint = arrowRot;
        Vector3 scaleing = new Vector3(1.5f, 1.5f, 1.5f);

        if (rotationPoint == -90 || rotationPoint == 270)
        {
            backObject.transform.DOScale(scaleing, .3f).SetEase(Ease.OutQuad);
        }
        else if(rotationPoint == -30 || rotationPoint == 330)
        {
            noiseObject.transform.DOScale(scaleing, .3f).SetEase(Ease.OutQuad);
        }
        else if(rotationPoint == 30 || rotationPoint == -330) 
        {
            windObject.transform.DOScale(scaleing, .3f).SetEase(Ease.OutQuad);
        }
        else if (rotationPoint == 90 || rotationPoint == -270)
        {
            pedestrianObject.transform.DOScale(scaleing, .3f).SetEase(Ease.OutQuad);
        }
        else if (rotationPoint == 150 || rotationPoint == -210)
        {
            thermalComfortObject.transform.DOScale(scaleing, .3f).SetEase(Ease.OutQuad);
        }
        else
        {
            backObject.transform.DOScale(Vector3.one, .3f).SetEase(Ease.OutQuad);
            noiseObject.transform.DOScale(Vector3.one, .3f).SetEase(Ease.OutQuad);
            windObject.transform.DOScale(Vector3.one, .3f).SetEase(Ease.OutQuad);
            pedestrianObject.transform.DOScale(Vector3.one, .3f).SetEase(Ease.OutQuad);
            thermalComfortObject.transform.DOScale(Vector3.one, .3f).SetEase(Ease.OutQuad);
        }
    }
}
