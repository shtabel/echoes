using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] generators;   // array of generators

    [SerializeField]
    DoorController theDoor;

    [SerializeField]
    RocketSpawner rs;   // rocket spawner

    int counter;        // counter of generators

    // Start is called before the first frame update
    void Start()
    {
        counter = generators.Length;
        //rs = FindObjectOfType<RocketSpawner>();
    }

    public virtual void MinusGenerator()    // if we have destroyed a generator/запитали генератор
    {
        //Debug.Log("Minus generator");
        counter--;
        //Debug.Log("Generatoes left:" + counter);
        if (counter <= 0)   // if we have destroyed all generators
        {
            PuzzleSolved();
        }
        else
        {
            if (rs != null)
            {
                rs.SpawnRocket(); // spawn new rocket
            }
        }            
    }

    public virtual void PlusGenerator()    // if we have destroyed a generator
    {
        //Debug.Log("Minus generator");
        counter++;
        //Debug.Log("Generatoes left:" + counter);
    }

    void PuzzleSolved()
    {
        //Debug.Log("Puzzle Solved");
        
        // OPEN THE DOOR
        theDoor.OpenTheDoor();

        GeneratorBlink();
    }

    void GeneratorBlink()
    {
        for (int i = 0; i < generators.Length; i++)
        {
            if (generators[i].GetComponent<Generatorv2Controller>())
            {
                generators[i].GetComponent<Generatorv2Controller>().Fade(false);
            }
        }
    }
}
