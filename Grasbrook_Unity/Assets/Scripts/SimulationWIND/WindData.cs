using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Classes for the Wind Simulation
/// WindData get called to save the Information into WindResult thats come from the server
/// WindScenario saved the inforamtion that will be send to the Server
/// </summary>

[Serializable]
public class WindData
{
    public WindResult results;
    public bool grouptaskProcessed = false;
    public int tasksTotal = 7;
    public int tasksCompleted = 0;
}

[Serializable]
public class WindResult
{
    public float[][] bbox_sw_corner;
    public float[][] bbox_coordinates;
    public string image_base64_string;
    public int img_height;
    public int img_width;
}

public class WindScenario
{
    public int wind_speed;
    public int wind_direction;
    public string result_format;
    public float[] custom_roi;
}
