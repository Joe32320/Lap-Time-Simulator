using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car {

    string name;
    float power; //550kW
    float mass; //605kg
    float liftToDragRatio; //3
    float coD;
    float maxBrakeForce;
    Tyre tyre;





    public Car(string nName, float nPower, float nMass, float ltD, float nCoD, float brakeG)
    {
        name = nName;
        power = nPower*1000;
        mass = nMass;
        liftToDragRatio = ltD;
        coD = nCoD;
        maxBrakeForce = brakeG * 9.81f * mass;
    }

    public float getPower()
    {
        return power;
    }

    public float getMass()
    {
        return mass;
    }

    public float getLtDRatio()
    {
        return liftToDragRatio;
    }

    public float getCoD()
    {
        return coD;
    }

    public Tyre getTyre()
    {
        return tyre;
    }

    public string getName()
    {
        return name;
    }

    public void setTyres(Tyre nTyre)
    {
        tyre = nTyre;
    }

    public float getBrakeForce()
    {
        return maxBrakeForce;
    }
	
}
