using UnityEngine;
using System;

/// <summary>
/// Classes for the Noise Simulation
/// NoiseData get called to save the Information into NoiseResult thats come from the server
/// NoiseScenario saved the inforamtion that will be send to the Server
/// </summary>

public class NoiseData
{
    public NoiseResult result;
}

public class NoiseResult
{
    public float[][] bbox_sw_corner;
    public float[][] bbox_coordinates;
    public string image_base64_string;
    public int img_height;
    public int img_width;
}

public class NoiseScenario
{
    public int noise_max_speed;
    public int noise_traffic_volume_percent;
    public string result_format;
}
