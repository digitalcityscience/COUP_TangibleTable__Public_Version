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
    public enum Screen { off, main, simulation, selection, hideSim, linearValue, circularValue, simSetup, steppedValue, execute, toggleResult, timeValue, abmSetup, windSimulation, noiseSimulation, abmSimulation, saveSimulation};


    RadialMenu radialMenu;
    CircularMenu circularMenu;
    LinearMenu linearMenu;
    SelectionMenu selectionMenu;
    RadialController radialController;
    CityPyO_Interface cityPyO_Interface;
    SimulationMenu simulationMenu;
    CalculationModules_Interface calculationModules;
    SimulationSetupMenu simSetupMenue;
    ToggleResult toggleSimResult;
    TimeMenue timeMenu;
    AbmSetupMenu abmSetupMenu;
    SunSeasonMenu sunMonthMenu;
    SwitchSimulations switchSimulations;
    public SaveResultsAsImages saveResultsAsImages;

    Screen activeScreen;

    public Texture SimulationsSetupTexture;
    public Texture RunSimulationTexture;
    public Texture SelectionTexture;
    public Texture ToggleResultTexture;
    public Texture WindSimulationTexture;
    public Texture NoiseSimulationTexture;
    public Texture AbmSimulationTexture;
    public Texture SaveSimulation;

    SpriteRenderer sprite = new SpriteRenderer();

    //string to choose between the Simulation:
    //"none" "noise" "wind" "abm" "sun"
    string simulationName = "none";

    public string SimulationName { get => simulationName; set => simulationName = value; }

    private void Start()
    {
        GameObject menus = GameObject.Find("Menus");
        radialMenu = menus.GetComponent<RadialMenu>();
        circularMenu = menus.GetComponent<CircularMenu>();
        linearMenu = menus.GetComponent<LinearMenu>();
        selectionMenu = menus.GetComponent<SelectionMenu>();
        simSetupMenue = menus.GetComponent<SimulationSetupMenu>();
        switchSimulations = menus.GetComponent<SwitchSimulations>();
        toggleSimResult = menus.GetComponent<ToggleResult>();
        timeMenu = menus.GetComponent<TimeMenue>();
        abmSetupMenu = menus.GetComponent<AbmSetupMenu>();
        sunMonthMenu = menus.GetComponent<SunSeasonMenu>();
        saveResultsAsImages = menus.GetComponent<SaveResultsAsImages>();
       
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

            default:
                break;
        }

    }

    // Open function to set where the main menu icons should be when the menu is opend
    void MainMenuOpen()
    {
        radialController.rotationResolutionInDegrees = 45;
        radialController.useAutoHapticFeedback = true;

        radialMenu.AddEntry("Setup", SimulationsSetupTexture, 0, .15f, Screen.simSetup);
        radialMenu.AddEntry("Calculate", RunSimulationTexture, 45, .2f, Screen.execute);
        radialMenu.AddEntry("Selection", SelectionTexture, 90, .05f, Screen.selection);
        radialMenu.AddEntry("Toggle", ToggleResultTexture, 135, .1f, Screen.toggleResult);
        radialMenu.AddEntry("Wind", WindSimulationTexture, 180, .1f, Screen.windSimulation);
        radialMenu.AddEntry("Noise", NoiseSimulationTexture, 225, .05f, Screen.noiseSimulation);
        radialMenu.AddEntry("Abm", AbmSimulationTexture, 270, .1f, Screen.abmSimulation);
        radialMenu.AddEntry("Save", SaveSimulation, 315, .1f, Screen.saveSimulation);
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

            case Screen.noiseSimulation:
                switchSimulations.SwitchBetweenSimulations("noise");
                SwitchTo(Screen.main);
                break;

            case Screen.windSimulation:
                switchSimulations.SwitchBetweenSimulations("wind");
                SwitchTo(Screen.main);
                break;

            case Screen.abmSimulation:
                switchSimulations.SwitchBetweenSimulations("abm");
                SwitchTo(Screen.main);
                break;

            case Screen.toggleResult:
                ToggleResults();
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

            case Screen.saveSimulation:
                saveResultsAsImages.TakeScreenShot();
                SwitchTo(Screen.off);
                break;
        }

    }
 

    public void Longpress()
    {
        Input.mouseScrollDelta.Set(0, 0);

        print("LongPress");
    }

    // OnRotation gives the rotation value of the surface dial to the diffrent undermenues
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

            case Screen.saveSimulation:
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

    /// <summary>
    /// Connecting function to toggle between an active and deactive simulation result
    /// </summary>
    void ToggleResults()
    {
        if (calculationModules.NoiseResult != null || calculationModules.WindResult != null || calculationModules.AbmResult != null)
        { 
            toggleSimResult.ToggleResultActivitation(SimulationName);
        }
        else
        {
            Debug.Log("There is no Simulations that can be toggled");
        }
        
    }
}

