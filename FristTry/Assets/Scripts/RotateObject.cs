﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RotateObject : MonoBehaviour
{
    public GameObject myPrefab;

    public int minDist = 200;
    public int maxDist = 600;
    public int stepDist = 200;

    public int minElev = 0;
    public int maxElev = 90;
    public int stepElev = 15;

    public int minAzim = 0;
    public int maxAzim = 180;
    public int stepAzim = 180;

    public static void CartesianToSpherical(Vector3 cartCoords, out float outRadius, out float outPolar, out float outElevation)
    {
        if (cartCoords.x == 0)
            cartCoords.x = Mathf.Epsilon;
        outRadius = Mathf.Sqrt((cartCoords.x * cartCoords.x)
                        + (cartCoords.y * cartCoords.y)
                        + (cartCoords.z * cartCoords.z));
        outPolar = Mathf.Atan(cartCoords.z / cartCoords.x);
        if (cartCoords.x < 0)
            outPolar += Mathf.PI;
        outElevation = Mathf.Asin(cartCoords.y / outRadius);
    }

    public static void SphericalToCartesian(float radius, float polar, float elevation, out Vector3 outCart)
    {
        float a = radius * Mathf.Cos(elevation);
        outCart.x = a * Mathf.Cos(polar);
        outCart.y = radius * Mathf.Sin(elevation);
        outCart.z = a * Mathf.Sin(polar);
    }

    // Start is called before the first frame update
    void Start()
    {
        Vector3 outCart;
        print(minAzim);
        print(maxAzim);
        print(stepAzim);
        for (int rr = minDist; rr <= maxDist; rr = rr + stepDist)
            for (int pp = minAzim; pp <= maxAzim; pp = pp + stepAzim)
                for (int ee = minElev; ee <= maxElev; ee = ee + stepElev)
                {
                    float eef = (float)(3.14 / 180.0) * (float)(90 - ee);
                    float ppf = (float)(3.14 / 180.0) * (float)pp;
                    SphericalToCartesian((float)rr, ppf, eef, out outCart);
                    //print(outCart);
                    // Instantiate at position <outCart> and zero rotation.
                    Instantiate(myPrefab, outCart, Quaternion.identity);

                    // TODO: Place camera at position <outCart> and set direction to (0, 0, 0)
                    //       Capture RGB image, depthmap, ground truth map

                    //Rotate Objects
                    //for (int aa = minAngleRot; ee <= maxAngleRot; ee = ee + stepAngleRot)

                }


    }

}

