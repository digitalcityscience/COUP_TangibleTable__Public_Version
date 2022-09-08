using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariable : MonoBehaviour
{
    //Sets a Global Variable for CarSpeed & Volume for every Car in the Noise Animation.
    private static int globalNoiseCarSpeed;
    private static int globalNoiseCarVolume;

    //Sets a Global Variable for WindDirection & WindSpeed
    private static int globalWindDirection;
    private static int globalWindSpeed;

    //Sets a Global Variable for the ABM-Values
    private static int globalABMTime;
    private static bool globalABMBridgeToHC = true;
    private static bool globalABMUnderpassVN = true;
    private static string globalABMStreetOrientation = "horizontal";
    private static string globalABMRoofAmenities = "random";
    private static string globalABMBlocks = "open";

    //Global Bool for the Toggleing of the Simulations
    private static bool resultIsActive;


    //Sets a Global Variable for the ThermalComfort-Values
    private static int globalMonth = 1;

    //getter & setter Methodes for the statics 
    public static int GlobalNoiseCarSpeed { get => globalNoiseCarSpeed; set => globalNoiseCarSpeed = value; }
    public static int GlobalNoiseCarVolume { get => globalNoiseCarVolume; set => globalNoiseCarVolume = value; }
    public static int GlobalWindSpeed { get => globalWindSpeed; set => globalWindSpeed = value; }
    public static int GlobalWindDirection { get => globalWindDirection; set => globalWindDirection = value; }
    public static int GlobalABMTime { get => globalABMTime; set => globalABMTime = value; }
    public static bool GlobalABMBridgeToHC { get => globalABMBridgeToHC; set => globalABMBridgeToHC = value; }
    public static bool GlobalABMUnderpassVN { get => globalABMUnderpassVN; set => globalABMUnderpassVN = value; }
    public static string GlobalABMStreetOrientation { get => globalABMStreetOrientation; set => globalABMStreetOrientation = value; }
    public static string GlobalABMBlocks { get => globalABMBlocks; set => globalABMBlocks = value; }
    public static string GlobalABMAmenities { get => globalABMRoofAmenities; set => globalABMRoofAmenities = value; }
    public static int GlobalMonth { get => globalMonth; set => globalMonth = value; }
    public static bool ResultIsActive { get => resultIsActive; set => resultIsActive = value; }
}
