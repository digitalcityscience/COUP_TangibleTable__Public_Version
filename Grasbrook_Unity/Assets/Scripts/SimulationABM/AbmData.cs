using UnityEngine;

/// <summary>
/// Classes for the ABM Simulation
/// AbmData get called to save the Information into AbmCoordinate & AbmResult thats come from the server
/// AbmResult is an array because we get 15 pictures from the Server for 15 hours of a day
/// AbmScenario saved the inforamtion that will be send to the Server
/// </summary>

public class AbmData : MonoBehaviour
{
    public AbmResult[] results;
    public AbmCoordinates coordinates;
}

public class AbmCoordinates
{
    public float[][] bbox_sw_corner;
    public float[][] bbox_coordinates;
    public int img_height;
    public int img_width;
}

public class AbmResult
{
    public string image_base64_string;
    public int hour;
}

public class AbmScenario
{
    public int abm_time;
    public string blocks;
    public bool bridge_hafencity;
    public string main_street_orientation;
    public string roof_amenities;
    public bool underpass_veddel_north;
}
