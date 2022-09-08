using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Blitzy.UnityRadialController;
public class LinearMenu : MonoBehaviour
{
    Transform arrowTransform;

    [SerializeField]
    GameObject linearValue;

    [SerializeField]
    GameObject arrow;

    [SerializeField]
    GameObject labelBox;

    [SerializeField]
    GameObject label;

    [SerializeField]
    GameObject valueBox;

    TextMeshProUGUI valueText;
    TextMeshProUGUI labelText;

    [SerializeField]
    WindSimulation windSimulation;
    [SerializeField]
    NoiseSimulation noiseSimulation;
    [SerializeField]
    AbmSimulation abmSimulation;

    [SerializeField]
    SimulationSetupMenu simulationSetup;

    [SerializeField]
    ParticleSystem windTrails;

    [SerializeField]
    WindParticial windParticial;

    LegendChanger legendChanger;

    RadialController radialController;

    NoiseAnimation poolSpawner;

    float min = 0;
    float max = 70;

    float arrowRot = -90;
    string simName;


    private void Start()
    {
        arrowTransform = arrow.GetComponent<RectTransform>();
        valueText = valueBox.GetComponent<TextMeshProUGUI>();
        labelText = label.GetComponent<TextMeshProUGUI>();
        radialController = GameObject.Find("WheelController").GetComponent<RadialController>();
        legendChanger = GameObject.Find("LegendBox").GetComponent<LegendChanger>();
        poolSpawner = GameObject.Find("CarPool").GetComponent<NoiseAnimation>();
    }

    public void Open(string sim)
    {
        simName = sim;
        radialController.rotationResolutionInDegrees = 1;
        radialController.useAutoHapticFeedback = false;

        linearValue.SetActive(true);
        labelBox.SetActive(true);

        UpdateValue(arrowRot);
        string valueString = UpdateValue(arrowRot).ToString() + "km/h";
        valueText.text = valueString;
    }

    public void Close()
    {
        linearValue.SetActive(false);
        labelBox.SetActive(false);
        windTrails.gameObject.SetActive(false);
    }


    public void OnRotation(float deg)
    {
        arrowRot += deg;

        if (arrowRot < -90)
        {
            arrowRot = -90;
        }
        else
        {
            if (arrowRot > 90)
            {
                arrowRot = 90;
            }
        }

        UpdateValue(arrowRot);
    }


    float map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }

    int UpdateValue(float rotation)
    {
        
        float mappedValue = map(rotation, -90, 90, min, max);

        arrowTransform.localRotation = Quaternion.Euler(0, 0, -rotation);


        // 1 = noise ; 2 = wind ; 3 = ABM
        //Noise
        if(simName == "noise")
        {
            //0 = speed, 1 = traffic
            if(simulationSetup.NoiseValueLinearMenu == 0)
            {
                int valueKey = 0;
                min = 0;
                max = 70;
                string labelString = "Traffic speed:";
                string valueString = GlobalVariable.GlobalNoiseCarSpeed.ToString() + "km/h";
                valueText.text = valueString;
                labelText.text = labelString;
                GlobalVariable.GlobalNoiseCarSpeed = (int)mappedValue;
                noiseSimulation.noiseScenario.noise_max_speed = (int)mappedValue;
                legendChanger.ChangeLegendInforamtion(simName, (int)mappedValue, valueKey);
            }
            else if (simulationSetup.NoiseValueLinearMenu == 1)
            {
                int valueKey = 1;
                min = 10;
                max = 90;
                string labelString = "Traffic volume:";
                string valueString = GlobalVariable.GlobalNoiseCarVolume.ToString() + "%";
                valueText.text = valueString;
                labelText.text = labelString;
                GlobalVariable.GlobalNoiseCarVolume = (int)mappedValue;
                poolSpawner.ToggleCarsActivity();
                noiseSimulation.noiseScenario.noise_traffic_volume_percent = (int)mappedValue;
                legendChanger.ChangeLegendInforamtion(simName, (int)mappedValue, valueKey);

            }
            
        }
        //Wind Speed 
        else if (simName == "wind")
        {
            int valueKey = 1;
            min = 0;
            max = 70;
            string labelString = "Wind speed:";
            string valueString = GlobalVariable.GlobalWindSpeed.ToString() + "km/h";
            valueText.text = valueString;
            labelText.text = labelString;
            windSimulation.windScenario.wind_speed = (int)mappedValue;
            GlobalVariable.GlobalWindSpeed = (int)mappedValue;
            windParticial.ChangeColor((int)mappedValue);
            legendChanger.ChangeLegendInforamtion(simName, (int)mappedValue, valueKey);
            windTrails.gameObject.SetActive(true);
        }
        
        return (int)mappedValue;

    }

}

