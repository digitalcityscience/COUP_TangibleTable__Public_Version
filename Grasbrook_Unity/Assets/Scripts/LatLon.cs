using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LatLon
{
    //class that converts latitude / longitude to Unity position and the reverse
    //Got the formula from here
    //https://stackoverflow.com/questions/929103/convert-a-number-range-to-another-range-maintaining-ratio

    //convert a coordinate from one set of ranges to another set of ranges
    private static float convertCoordinate(float oldValue, float oldMin, float oldMax, float newMin, float newMax)
    {
        float oldRange = oldMax - oldMin;
        float newRange = newMax - newMin;
        float returnValue = (((oldValue - oldMin) * newRange) / oldRange) + newMin;
        return returnValue;
    }

    //A LatLon Vector2 includes Latitude as the x value and Longitude as the y value
    //A Unity world coordinate has x as the west/east (longitude) and z as the north/sounth (latitude)

    //This method takes a LatLon Vector2 and translates it into this zone's game world coordinates
    //It does this by taking two points, a Noth West point and South East point in both LatLon and Unity world space positions to do the translation
    public static Vector3 GetUnityPosition(Vector2 latLonPosition, Vector2 northWestLatLon, Vector2 southEastLatLon, Vector3 northWestUnity, Vector3 southEastUnity)
    {
        //check if this zone covers the antimeridian (where 180 and -180 degress longitude meet)
        if (southEastLatLon.y < northWestLatLon.y)
        {
            //Add 360 to any negative longitude positions so that longitude values are lower the further west
            southEastLatLon = new Vector2(southEastLatLon.x, southEastLatLon.y + 360f);
            if (latLonPosition.y < 0f)
            {
                latLonPosition = new Vector2(latLonPosition.x, latLonPosition.y + 360f);
            }
        }
        float newUnityLat = convertCoordinate(latLonPosition.x, southEastLatLon.x, northWestLatLon.x, southEastUnity.z, northWestUnity.z);
        float newUnityLon = convertCoordinate(latLonPosition.y, southEastLatLon.y, northWestLatLon.y, southEastUnity.x, northWestUnity.x);
        Vector3 unityWorldPosition = new Vector3(newUnityLon, newUnityLat, 0);
        return unityWorldPosition;
    }

    public static Vector2 GetLatLonPosition(Vector3 unityPosition, Vector2 northWestLatLon, Vector2 southEastLatLon, Vector3 northWestUnity, Vector3 southEastUnity)
    {
        bool antimeridian = false;
        //check if this zone covers the antimeridian (where 180 and -180 degress longitude meet)
        if (southEastLatLon.y < northWestLatLon.y)
        {
            antimeridian = true;
            //Add 360 to any negative longitude positions so that longitude values are lower the further west
            southEastLatLon = new Vector2(southEastLatLon.x, southEastLatLon.y + 360f);
        }
        float newlat = convertCoordinate(unityPosition.z, southEastUnity.z, northWestUnity.z, southEastLatLon.x, northWestLatLon.x);
        float newlon = convertCoordinate(unityPosition.x, southEastUnity.x, northWestUnity.x, southEastLatLon.y, northWestLatLon.y);
        if (antimeridian)
        {
            if (newlon > 180f)
            {
                newlon = newlon - 360f;
            }
        }
        Vector2 latLonPosition = new Vector2(newlat, newlon);
        return latLonPosition;
    }
}

