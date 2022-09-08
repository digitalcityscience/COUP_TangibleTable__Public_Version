using UnityEngine;

/// <summary>
/// Classes for the ABM Simulation
/// SunData get called to save the Information into SunCoordinate & SunResult thats come from the server
/// SunResult is an array because we get 15 pictures from the Server for 15 hours of a day
/// SunScenario saved the inforamtion that will be send to the Server
/// </summary>

public class SunData : MonoBehaviour
{
    public SunResult[] result;
    public SunCoordinates coordinates;
}

public class SunCoordinates
{
    public float[][] bbox_sw_corner;
    public float[][] bbox_coordinates;
    public int img_height;
    public int img_width;
}

public class SunResult
{
    public string image_base64_string;
    public int hour;
}

public class SunScenario
{
    public int sun_time;
    public string season;
    public bool trees;
    public bool groundConcept;
    public bool buildingMaterial;
}
