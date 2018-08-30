using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackSection  {

    public bool straight;
    int length;
    float radius;
    float angle;
    int startPoint;
    int endPoint;

    float maxSpeed;
    float entrySpeed;
    float exitSpeed;


    //Represents a "section" of the track. In the current implmenentation, a section has a length of integer metres, and a angle if applicable of the corner, which is used to work out the radius of the section
    public TrackSection(bool nStraight, int nLength, float nAngle)
    {
        straight = nStraight;
        length = nLength;
        angle = nAngle;
        if (!straight)
        {
            radius = ((float)length)*180f/(angle*Mathf.PI);
        }
        else
        {
            radius = float.MaxValue;
        }
    }

    public int getLength()
    {
        return length;
    }

    public void setStartPoint(int nStartPoint)
    {
        startPoint = nStartPoint;
        endPoint = startPoint + length;
    }

    public float calculateMaxSpeed(Car car)
    {
        float mu = car.getTyre().getCoF();
        float d = car.getCoD();
        float l = car.getLtDRatio() * d;
        float m = car.getMass();
        float g = 9.81f;
        float r = radius;
        float p = car.getPower();


        float terminalSpeed = Mathf.Pow(p / d, 1f / 3f);

        float v2 = mu * g * m * r / (m - mu * l * r);
        //float v2 = mu * m * g / ((m / r) - l);
        float v = Mathf.Sqrt(v2);


        //If the denominator of the equation above is less than 0, then regardless of the speed of the car, the downforce generated and the grip of the tyres allows it to corner at an infinite speed
        //In this case, the car is limited by the drag on the car, so the maximum speed possible is the terminal speed of the car.
        if (m-mu*l*r <= 0)
        {
            return terminalSpeed;
        }
        else
        {
            return Mathf.Min(v, terminalSpeed);
        }


        

        //float x = Mathf.Sqrt(g * m * r * mu) / Mathf.Sqrt(m - mu * l * r);
        //Debug.Log("Speed: " + Mathf.Min(Mathf.Sqrt(r * mu * 9.81f), terminalSpeed));

        
       
    }

    public float calculateTimeToTravel(Car car)
    {

        return length / calculateMaxSpeed(car);


    }
    public float getRadius()
    {
        return radius;
    }
}
