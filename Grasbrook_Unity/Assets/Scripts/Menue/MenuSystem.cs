using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Blitzy.UnityRadialController;


/// <summary>
/// Main Menu Class
/// This class is the connection between all menues, it give the rotation values and the press event to the other menues
/// </summary>
public class MenuSystem : MonoBehaviour
{
    public enum Screen { off, main, simulation, selection, hideSim, linearValue, circularValue, simSetup, steppedValue, execute, toggleSim, timeValue, abmSetup, sunMonth};


    RadialMenu radialMenu;
    CircularMenu circularMenu;
    LinearMenu linearMenu;
    SelectionMenu selectionMenu;
    RadialController radialController;
    CityPyO_Interface cityPyO_Interface;
    SimulationMenu simulationMenu;
    CalculationModules_Interface calculationModules;
    SimulationSetupMenu simSetupMenue;
    HideActiveSimulation hideActiveSimulation;
    ShowSimulation showSimulation;
    TimeMenue timeMenu;
    AbmSetupMenu abmSetupMenu;
    SunSeasonMenu sunMonthMenu;

    Screen activeScreen;

    public Texture simulationsSetup;
    public Texture runsimulation;
    public Texture selection;
    public Texture choosesimulation;
    public Texture hideSimulation;
    public Texture lastSimulation;

    // "none" "noise" "wind" "abm" "sun"
    string simulationName = "none";

    public string SimulationName { get => simulationName; set => simulationName = value; }

    private void Start()
    {

        GameObject menus = GameObject.Find("Menus");
        radialMenu = menus.GetComponent<RadialMenu>();
        circularMenu = menus.GetComponent<CircularMenu>();
        linearMenu = menus.GetComponent<LinearMenu>();
        selectionMenu = menus.GetComponent<SelectionMenu>();
        simulationMenu = menus.GetComponent<SimulationMenu>();
        simSetupMenue = menus.GetComponent<SimulationSetupMenu>();
        hideActiveSimulation = menus.GetComponent<HideActiveSimulation>();
        showSimulation = menus.GetComponent<ShowSimulation>();
        timeMenu = menus.GetComponent<TimeMenue>();
        abmSetupMenu = menus.GetComponent<AbmSetupMenu>();
        sunMonthMenu = menus.GetComponent<SunSeasonMenu>();


        radialController = GameObject.Find("WheelController").GetComponent<RadialController>();
        calculationModules = GameObject.Find("NetworkingManager").GetComponent<CalculationModules_Interface>();
        activeScreen = Screen.off;
           
    }

    //Main Press function for the surface dial and the Menues
    public void Press()
    {
        switch(activeScreen)
        {
            case Screen.off:
                MainMenuOpen();
                activeScreen = Screen.main;
                break;

            case Screen.main:
                RadialMenuEntry rme = radialMenu.Press();
                SwitchTo(rme.GetLink());
                break;

            case Screen.simSetup:
                simSetupMenue.Press();
                break;

            case Screen.abmSetup:
                abmSetupMenu.Press();
                break;

            case Screen.linearValue:
                linearMenu.Close();
                SwitchTo(Screen.simSetup);
                break;

            case Screen.selection:
                selectionMenu.Close();
                SwitchTo(Screen.main);
                break;

            case Screen.simulation:
                simulationMenu.Press();
                break;

            case Screen.circularValue:
                circularMenu.Close();
                SwitchTo(Screen.simSetup);
                break;

            case Screen.timeValue:
                timeMenu.Close();
                SwitchTo(Screen.simSetup);
                break;

            case Screen.sunMonth:
                sunMonthMenu.Press();
                break;

            default:
                break;
        }

    }

    // Open function to set where the main menu icons should be when the menu is opend
    void MainMenuOpen()
    {
        radialController.rotationResolutionInDegrees = 30;
        radialController.useAutoHapticFeedback = true;

        radialMenu.AddEntry("Sim Setup", simulationsSetup, 30, .15f, Screen.simSetup);
        radialMenu.AddEntry("Run Simulation", runsimulation, 90, .2f, Screen.execute);
        radialMenu.AddEntry("Selection", selection, 140, .05f, Screen.selection);
        radialMenu.AddEntry("Choose Simulation", choosesimulation, 220, .05f, Screen.simulation);
        radialMenu.AddEntry("Active Sim", lastSimulation, 270, .1f, Screen.toggleSim);
        radialMenu.AddEntry("Hide Sim", hideSimulation, 330, .1f, Screen.hideSim);
        
    }

   // function to switch between the diffrent menus
    public void SwitchTo(Screen nextLink)
    {
        radialMenu.Close();

        activeScreen = nextLink;
        switch (nextLink)
        {
            case Screen.simSetup:
                simSetupMenue.Open();
                break;

            case Screen.linearValue:
                linearMenu.Open(SimulationName);
                break;

            case Screen.timeValue:
                timeMenu.Open();
                break;

            case Screen.execute:
                ExecuteSimulation();
                SwitchTo(Screen.off);
                break;

            case Screen.hideSim:
                HideSimulation();
                SwitchTo(Screen.main);
                break;

            case Screen.toggleSim:
                ShowSimumlation();
                SwitchTo(Screen.main);
                break;

            case Screen.main:
                MainMenuOpen();
                break;

            case Screen.simulation:
                simulationMenu.Open();
                break;

            case Screen.selection:
                selectionMenu.Open();
                break;

            case Screen.circularValue:
                circularMenu.Open();
                break;

            case Screen.abmSetup:
                abmSetupMenu.Open();
                break;

            case Screen.sunMonth:
                sunMonthMenu.Open();
                break;
        }

    }
 

    public void Longpress()
    {
        Input.mouseScrollDelta.Set(0, 0);
        print("LongPress");
    }

    // OnrRotation gives the rotation value of the surface dial to the diffrent undermenues
    public void OnRotation(float numDegs)
    {
        switch (activeScreen)
        {
            case Screen.off:
                break;

            case Screen.main:
                radialMenu.OnRotation(numDegs);
                break;

            case Screen.simSetup:
                simSetupMenue.OnRotation(numDegs);
                break;

            case Screen.linearValue:
                linearMenu.OnRotation(numDegs);
                break;

            case Screen.selection:
                selectionMenu.OnRotation(numDegs);
                break;

            case Screen.simulation:
                simulationMenu.OnRotation(numDegs);
                break;

            case Screen.circularValue:
                circularMenu.OnRotation(numDegs);
                break;

            case Screen.timeValue:
                timeMenu.OnRotation(numDegs, SimulationName);
                break;

            case Screen.abmSetup:
                abmSetupMenu.OnRotation(numDegs);
                break;

            case Screen.sunMonth:
                sunMonthMenu.OnRotation(numDegs);
                break;

            default:
                break;
        }
    }

    //start the menu calculation
    public void ExecuteSimulation()
    {
        calculationModules.ActivateSimulation(SimulationName);
    }

    private void HideSimulation()
    {
        if(calculationModules.NoiseResult != null || calculationModules.WindResult != null || calculationModules.AbmResult != null)
        {
            hideActiveSimulation.HideOldSimulation(SimulationName);
        }
        else
        {
            Debug.Log("There is no Simulations that can be hided");
        }
    }

    void ShowSimumlation()
    {
        if (calculationModules.NoiseResult != null || calculationModules.WindResult != null || calculationModules.AbmResult != null)
        {
            showSimulation.ShowOldSimulations(SimulationName);
        }
        else
        {
            Debug.Log("There is no Simulations that can be activated");
        }
        
    }
}

