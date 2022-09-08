using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Blitzy.UnityRadialController;
using TMPro;
using DG.Tweening;

public class SimulationSetupMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject simulationSetup;

    [SerializeField]
    GameObject windObj;
    [SerializeField]
    GameObject windObj_speed;
    [SerializeField]
    GameObject windObj_direction;

    [SerializeField]
    GameObject noiseObj;
    [SerializeField]
    GameObject noiseObj_speed;
    [SerializeField]
    GameObject noiseObj_traffic;

    [SerializeField]
    GameObject abmObj;
    [SerializeField]
    GameObject abmObj_time;
    [SerializeField]
    GameObject abmObj_setup;

    [SerializeField]
    GameObject sunObj;
    [SerializeField]
    GameObject sunObj_month;
    [SerializeField]
    GameObject sunObj_daytime;


    [SerializeField]
    GameObject backObj;

    [SerializeField]
    GameObject arrowObj;

    [SerializeField]
    MenuSystem menuSystem;



    RadialController radialController;

    float arrowRot;
    float rotationPoint;

    // this int is for the LinearMenu, booth Noise values are changed 
    // with it, with this value we choose what Value: 0 = speed; 1 = traffic
    // is manipulated
    int noiseValueLinearMenu;

    bool abmInsideSetup = false;

    Transform arrowTransform;

    public bool visible = false;

    public int NoiseValueLinearMenu { get => noiseValueLinearMenu; set => noiseValueLinearMenu = value; }
    public bool AbmInsideSetup { get => abmInsideSetup; set => abmInsideSetup = value; }

    private void Start()
    {
        radialController = GameObject.Find("WheelController").GetComponent<RadialController>();
        arrowTransform = arrowObj.GetComponent<RectTransform>();
    }

    public void Open()
    {
        radialController.rotationResolutionInDegrees = 30;
        radialController.useAutoHapticFeedback = false;
        ToggleVisibility();
    }

    public void Close()
    {
        ToggleVisibility();
        menuSystem.SwitchTo(MenuSystem.Screen.main);
    }

    public void ToggleVisibility()
    {
        if(menuSystem.SimulationName != "none")
        {
            if (!visible)
            {
                if (menuSystem.SimulationName == "wind")
                {
                    simulationSetup.SetActive(true);

                    windObj.SetActive(true);
                    windObj_speed.SetActive(true);
                    windObj_direction.SetActive(true);
                    visible = true;
                }
                else if (menuSystem.SimulationName == "noise")
                {
                    simulationSetup.SetActive(true);

                    noiseObj.SetActive(true);
                    noiseObj_speed.SetActive(true);
                    noiseObj_traffic.SetActive(true);
                    visible = true;
                }
                else if (menuSystem.SimulationName == "abm")
                {
                    simulationSetup.SetActive(true);

                    abmObj.SetActive(true);
                    abmObj_time.SetActive(true);
                    abmObj_setup.SetActive(true);
                    visible = true;
                }
                else if (menuSystem.SimulationName == "sun")
                {
                    simulationSetup.SetActive(true);

                    sunObj.SetActive(true);
                }
            }
            else
            {
                simulationSetup.SetActive(false);
                if (menuSystem.SimulationName == "wind")
                {
                    windObj.SetActive(false);
                    windObj_speed.SetActive(false);
                    windObj_direction.SetActive(false);
                }
                else if (menuSystem.SimulationName == "noise")
                {
                    noiseObj.SetActive(false);
                    noiseObj_speed.SetActive(false);
                    noiseObj_traffic.SetActive(false);
                }
                else if (menuSystem.SimulationName == "abm")
                {
                    abmObj.SetActive(false);
                    abmObj_time.SetActive(false);
                    abmObj_setup.SetActive(false);
                }
                visible = false;
            }
        }
        else
        {
            menuSystem.SwitchTo(MenuSystem.Screen.main);
        }
        
    }
    #region PressFunctions
    public void Press()
    {
        if (simulationSetup)
        {
            if(menuSystem.SimulationName == "noise")
            {
                NoisePress(); 
            }
            else if(menuSystem.SimulationName == "wind")
            {
                WindPress();
            }
            else if(menuSystem.SimulationName == "abm")
            {
                AbmPress();
            }
            else
            {
                Debug.Log("No SimNumber choosed");
                Close();
            }  
        }
    }

    public void WindPress()
    {

        if (rotationPoint == -90 || rotationPoint == 270)
        {
            Close();
        }
        else if (rotationPoint == -30 || rotationPoint == 330)
        {
             //Oben linkes Symbol = direction
             Debug.Log("Wind Direction Change");
             menuSystem.SwitchTo(MenuSystem.Screen.circularValue);
             ToggleVisibility();

        }
        else if (rotationPoint == 30 || rotationPoint == -330)
        {
            //Oben rechtes Symbol = speed
            Debug.Log("Wind Speed Change");
            menuSystem.SwitchTo(MenuSystem.Screen.linearValue);
            ToggleVisibility();
        }
        
    }

    public void NoisePress()
    {

        if (rotationPoint == -90 || rotationPoint == 270)
        {
            Close();
        }

        if (rotationPoint == -30 || rotationPoint == 330)
        {
            //Oben linkes Symbol = traffic
            Debug.Log("Noise Traffic Change");
            menuSystem.SwitchTo(MenuSystem.Screen.linearValue);
            NoiseValueLinearMenu = 1;
            ToggleVisibility();

        }
        if (rotationPoint == 30 || rotationPoint == -330)
        {
            //Oben rechtes Symbol = speed
            Debug.Log("Noise Speed Change");
            menuSystem.SwitchTo(MenuSystem.Screen.linearValue);
            NoiseValueLinearMenu = 0;
            ToggleVisibility();

        }
    }

    public void AbmPress()
    {
       
        if (rotationPoint == -90 || rotationPoint == 270)
        {
            Close();
        }

        if (rotationPoint == -30 || rotationPoint == 330)
        {
            //Oben linkes Symbol = setup
            Debug.Log("ABM Setup");
            menuSystem.SwitchTo(MenuSystem.Screen.abmSetup);
            ToggleVisibility();
        }
        if (rotationPoint == 30 || rotationPoint == -330)
        {
            //Oben rechtes Symbol = time
            Debug.Log("ABM Time Change");
            menuSystem.SwitchTo(MenuSystem.Screen.timeValue);
            ToggleVisibility();
        }

    }

    public void SunPress()
    {
        if (rotationPoint == -90 || rotationPoint == 270)
        {
            Close();
        }

        if (rotationPoint == -30 || rotationPoint == 330)
        {
            //Oben linkes Symbol = month
            Debug.Log("Sun Change Month");
            menuSystem.SwitchTo(MenuSystem.Screen.abmSetup);
            ToggleVisibility();
        }
        if (rotationPoint == 30 || rotationPoint == -330)
        {
            //Oben rechtes Symbol = daytime
            Debug.Log("Sun Daytime");
            menuSystem.SwitchTo(MenuSystem.Screen.timeValue);
            ToggleVisibility();
        }
    }
    #endregion

    public void OnRotation(float deg)
    {
        arrowRot += deg;
        arrowRot = arrowRot % 360;

        arrowTransform.localRotation = Quaternion.Euler(0, 0, -arrowRot);
        rotationPoint = arrowRot;
        Vector3 scaleing = new Vector3(1.5f, 1.5f, 1.5f);

        if (rotationPoint == -90 || rotationPoint == 270)
        {
            backObj.transform.DOScale(scaleing, .3f).SetEase(Ease.OutQuad);
        }
        else if (rotationPoint == -30 || rotationPoint == 330)
        {
            windObj_direction.transform.DOScale(scaleing, .3f).SetEase(Ease.OutQuad);
            noiseObj_traffic.transform.DOScale(scaleing, .3f).SetEase(Ease.OutQuad);
            abmObj_setup.transform.DOScale(scaleing, .3f).SetEase(Ease.OutQuad);
            sunObj_month.transform.DOScale(scaleing, .3f).SetEase(Ease.OutQuad);
        }
        else if (rotationPoint == 30 || rotationPoint == -330)
        {
            windObj_speed.transform.DOScale(scaleing, .3f).SetEase(Ease.OutQuad);
            noiseObj_speed.transform.DOScale(scaleing, .3f).SetEase(Ease.OutQuad);
            abmObj_time.transform.DOScale(scaleing, .3f).SetEase(Ease.OutQuad);
            sunObj_daytime.transform.DOScale(scaleing, .3f).SetEase(Ease.OutQuad);
        }
        else
        {
            backObj.transform.transform.DOScale(Vector3.one, .3f).SetEase(Ease.OutQuad);
            windObj_direction.transform.DOScale(Vector3.one, .3f).SetEase(Ease.OutQuad);
            windObj_speed.transform.DOScale(Vector3.one, .3f).SetEase(Ease.OutQuad);
            noiseObj_traffic.transform.DOScale(Vector3.one, .3f).SetEase(Ease.OutQuad);
            noiseObj_speed.transform.DOScale(Vector3.one, .3f).SetEase(Ease.OutQuad);
            abmObj_setup.transform.DOScale(Vector3.one, .3f).SetEase(Ease.OutQuad);
            abmObj_time.transform.DOScale(Vector3.one, .3f).SetEase(Ease.OutQuad);
            sunObj_daytime.transform.DOScale(Vector3.one, .3f).SetEase(Ease.OutQuad);
            sunObj_month.transform.DOScale(Vector3.one, .3f).SetEase(Ease.OutQuad);
        }
    }

}
