using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tyre  {

    string name;
    float dryCoF;
    float wetCoF;
    float wearCoF;

    float wearRate;

    public Tyre(string nName, float nDryCoF, float nWetCoF)
    {
        name = nName;
        dryCoF = nDryCoF;
        wetCoF = nWetCoF;
    }

    public float getCoF()
    {
        return dryCoF;
    }



	
}
