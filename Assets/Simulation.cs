using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Simulation {

    

    public Simulation()
    {

    }

    public static void Run(Car car, Track track)
    {
        string teleFileName = car.getName() + "telemetry.txt";

        StreamWriter writer = new StreamWriter(teleFileName);

        List<float[]> lapTrace = new List<float[]>();

        float curV = 0;
        float curT = 0;
        int distanceCovered = 0;
        bool completesLap = true;

        float maxV = 0;

        int count = 0;


        for(int i = 0; i < track.getLength(); i++)
        {

            bool brake = shouldBrake(i, curV, track, car);
            float[] tickData = simulateSTick(1, curV, curT, car, track.getCheckpointPassed(i), !brake);
            curV = tickData[0];
            curT += tickData[1];
            maxV = (maxV > curV) ? maxV : curV;

            lapTrace.Add(tickData);

            if (track.getCheckpointPassed(i).calculateMaxSpeed(car) < curV && !track.getCheckpointPassed(i).straight)
            {
                completesLap = false;
                count += 1;
                Debug.Log("Violation");
                Debug.Log(curV + ":" + track.getCheckpointPassed(i).calculateMaxSpeed(car) + ":" + i + ":" + track.getCheckpointPassed(i).getLength());
            }
        }

        //Debug.Log("Completes Lap?: " + completesLap.ToString() + ":" + count);

        int minutes = Mathf.FloorToInt(curT / 60);
        int seconds = Mathf.FloorToInt((curT - (minutes * 60)));
        int millseconds = Mathf.FloorToInt((curT - (minutes * 60)-seconds)*1000);
        //Debug.Log("Lap Time: " + curT);
        Debug.Log(car.getName() + " Lap Time: " + minutes + ":" + seconds + ":" + millseconds + " Max V: " + maxV);

        printTelemetry(writer, lapTrace, car);



    }


    //Basic idea is to calculate what happens if the car starts braking a metre from where it is now, if while braking, it violates
    //the maximum speed of the track section it is on, then the car should start braking now, as for the car to be accerlating previously, this test must not have been violated.
    static bool shouldBrake(int currentPosition, float curSpeed, Track track, Car car)
    {
        float[] nextTick = simulateSTick(1, curSpeed, 0f, car, track.getCheckpointPassed(currentPosition), true);
        nextTick = simulateSTick(1, nextTick[0], 0f, car, track.getCheckpointPassed(currentPosition + 1), true);
        nextTick = simulateSTick(1, nextTick[0], 0f, car, track.getCheckpointPassed(currentPosition + 2), true);
        nextTick = simulateSTick(1, nextTick[0], 0f, car, track.getCheckpointPassed(currentPosition + 3), true);
        nextTick = simulateSTick(1, nextTick[0], 0f, car, track.getCheckpointPassed(currentPosition + 4), true);

        float simulatedSpeed = nextTick[0];
        int simulatedPosition = currentPosition + 5;

        int count = 0;

        while(simulatedSpeed > 0 && count < 900)
        {
            float[] brakeTick = simulateSTick(1, simulatedSpeed, 0f, car, track.getCheckpointPassed(simulatedPosition), false);

            if(brakeTick[0] > track.getCheckpointPassed(simulatedPosition).calculateMaxSpeed(car))
            {
                //Debug.Log("Is braking");
                return true;
            }

            simulatedPosition += 1;
            simulatedSpeed = brakeTick[0];
            count += 1;
        }


        return false;



        
    }


    //This simulates the car 
    static float[] simulateSTick(int deltaS, float vCur, float tCur, Car car, TrackSection section, bool accelating)
    {
        float p = car.getPower();
        float mu = car.getTyre().getCoF();
        float m = car.getMass();
        float g = 9.81f;
        float d = car.getCoD();
        float l = car.getLtDRatio() * d;
        float r = section.getRadius();
        float fBrakeMax = car.getBrakeForce();


        float fDrag = d * vCur * vCur; //Drag generated
        float fDown = l * vCur * vCur; //Downforce generated
        float fCur = m * vCur * vCur / r; //Force needed to corner
        float fTyre = mu * ((m * g) + fDown); //Total force available from tyres
        float fAcc = Mathf.Min(p / vCur, Mathf.Max(fTyre - fCur, 0)); //Force available to push the car forward
        float fBrake = Mathf.Min(Mathf.Max(fTyre - fCur, 0), fBrakeMax); //Force avaiable to stop the car with brakes

        float terminalSpeed = Mathf.Pow(p / d, 1f / 3f);

        float fForward = 0;
        float accForward = 0;

        if (accelating)
        {
            fForward = fAcc - fDrag;
            accForward = fForward / m;
        }
        else
        {
            fForward = -fBrake - fDrag;
            accForward = fForward / m;
        }

        float vNewSqu = Mathf.Max(vCur * vCur + (2 * accForward*deltaS),0.1f);

        float vNew = Mathf.Min(Mathf.Sqrt(vNewSqu),terminalSpeed);

        float t = 1 / (0.5f * vNew + vCur);

        //float t = (vNew - vCur) / accForward;

        //Debug.Log(vNew);

        float[] tickData = { vNew, t };

        return tickData;

    }

    static void printTelemetry(StreamWriter writer, List<float[]> lapData, Car car)
    {

        writer.WriteLine(car.getName());
        for(int i = 0; i < lapData.Count; i++)
        {
            writer.WriteLine(i.ToString() + "," + lapData[i][0] + "," + lapData[i][1]);
        }
        writer.Flush();
    }



    /*
    static float simulateTick(float deltaT, float vCur, float sCur, Car car, TrackSection section, bool accerlating)
    {

        float p = car.getPower();
        float mu = car.getTyre().getCoF();
        float m = car.getMass();
        float g = 9.81f;
        float d = car.getCoD();
        float l = car.getLtDRatio() * d;
        float r = section.getRadius();


        float fDrag = d * vCur * vCur; //Drag generated
        float fDown = l * vCur * vCur; //Downforce generated
        float fCur = m * vCur * vCur / r; //Force needed to corner
        float fTyre = mu * ((m * g) + fDown); //Total force available from tyres
        float fAcc = Mathf.Min(p / vCur, Mathf.Max(fTyre - fCur, 0)); //Force available to push the car forward
        float fBrake = Mathf.Max(fTyre - fCur, 0); //Force avaiable to stop the car

        float terminalSpeed = Mathf.Pow(p / d, 1f / 3f);

        float fForward = fAcc - fDrag;
        float accForward = fForward / m;

        float vNew = Mathf.Min(terminalSpeed, (accForward * deltaT) + vCur);
        Debug.Log(vNew + ":" + (vNew-vCur)+":" + fCur);


        float sNew = sCur + ((vCur + vNew) * deltaT / 2);

        return vNew;





    }
    */
    
    
	
}
