using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerScript : MonoBehaviour {

    public TextAsset carFile;
    public List<TextAsset> trackFiles;



	// Use this for initialization
	void Start () {
        
        Tyre soft = new Tyre("Soft", 1.6f, 1);
        Tyre medium = new Tyre("Medium", 1.5f, 1);
        Tyre hard = new Tyre("Hard", 1.4f, 1);

        List<Car> carList = loadCars();
        foreach(Car car in carList)
        {
            car.setTyres(medium);
        }

        List<Track> tracks = loadTracks();

        foreach(Car car in carList)
        {
            Simulation.Run(car, tracks[0]);
        }


        //Car mercedes = new Car(medium);
        //Simulation.Run(mercedes, silverstone);

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void RaceLap(Track track, Car car)
    {
        track.Race(car);
    }

    List<Car> loadCars()
    {
        string text = carFile.text;
        string[] lines = text.Split(new char[] { '\n' });

        List<Car> carList = new List<Car>();

        for(int i = 1; i < lines.Length; i++)
        {
            string line = lines[i];
            line.Replace("\r", "");
            string[] data = line.Split(new char[] { ',' });

            string teamName = data[0];
            float mass = float.Parse(data[1]);
            float power = float.Parse(data[2]);
            float coD = float.Parse(data[3]);
            float ltD = float.Parse(data[4]);
            float brakeG = float.Parse(data[5]);

            Car car = new Car(teamName, power, mass, ltD, coD, brakeG);
            carList.Add(car);
        }

        /*foreach(Car car in carList)
        {
            print(car.getName() + " : " + car.getPower() + " : " + car.getMass());
        }*/
        return carList;
    }

    List<Track> loadTracks()
    {
        List<Track> tracks = new List<Track>();

        foreach(TextAsset file in trackFiles)
        {
            string text = file.text;
            text.Replace("\r", "");
            string[] lines = text.Split(new char[] { '\n' });
            string[] firstLineData = lines[0].Split(new char[] { ',' });
            string trackName = firstLineData[1];

            List<TrackSection> sections = new List<TrackSection>();

            for(int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];
                string[] data = line.Split(new char[] { ',' });

                int sectionLength = int.Parse(data[2]);
                int sectionRadius = int.Parse(data[3]);

                bool isStraight = sectionRadius == 0;

                TrackSection section = new TrackSection(isStraight, sectionLength, sectionRadius);
                sections.Add(section);
            }
            Track track = new Track(trackName, sections);
            tracks.Add(track);
        }

        return tracks;
    }








    /*
    void dump()
    {
        List<TrackSection> sections = new List<TrackSection>();

        TrackSection section = new TrackSection(true, 450, 0); sections.Add(section);
        section = new TrackSection(false, 100, 75); sections.Add(section);
        section = new TrackSection(true, 75, 0); sections.Add(section);
        section = new TrackSection(false, 140, 45); sections.Add(section);
        section = new TrackSection(true, 180, 0); sections.Add(section);
        section = new TrackSection(false, 50, 90); sections.Add(section);
        section = new TrackSection(true, 90, 0); sections.Add(section);
        section = new TrackSection(false, 75, 135); sections.Add(section);
        section = new TrackSection(true, 135, 0); sections.Add(section);
        section = new TrackSection(false, 50, 45); sections.Add(section);
        section = new TrackSection(true, 625, 0); sections.Add(section);
        section = new TrackSection(false, 130, 135); sections.Add(section);
        section = new TrackSection(true, 70, 0); sections.Add(section);
        section = new TrackSection(false, 180, 180); sections.Add(section);
        section = new TrackSection(true, 125, 0); sections.Add(section);
        section = new TrackSection(false, 220, 45); sections.Add(section);
        section = new TrackSection(true, 400, 0); sections.Add(section);
        section = new TrackSection(false, 150, 80); sections.Add(section);
        section = new TrackSection(true, 420, 0); sections.Add(section);
        section = new TrackSection(false, 65, 20); sections.Add(section);
        section = new TrackSection(true, 40, 0); sections.Add(section);
        section = new TrackSection(false, 45, 30); sections.Add(section);
        section = new TrackSection(true, 90, 0); sections.Add(section);
        section = new TrackSection(false, 100, 45); sections.Add(section);
        section = new TrackSection(true, 40, 0); sections.Add(section);
        section = new TrackSection(false, 125, 90); sections.Add(section);
        section = new TrackSection(true, 80, 0); sections.Add(section);
        section = new TrackSection(false, 50, 15); sections.Add(section);
        section = new TrackSection(true, 725, 0); sections.Add(section);
        section = new TrackSection(false, 210, 90); sections.Add(section);
        section = new TrackSection(true, 350, 0); sections.Add(section);
        section = new TrackSection(false, 30, 90); sections.Add(section);
        section = new TrackSection(true, 35, 0); sections.Add(section);
        section = new TrackSection(false, 80, 90); sections.Add(section);
        section = new TrackSection(false, 190, 90); sections.Add(section);

        Track silverstone = new Track("Silverstone", sections);
    }
    */
}
