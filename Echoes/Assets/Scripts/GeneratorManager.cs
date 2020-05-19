﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorManager : MonoBehaviour
{
    [SerializeField]
    GeneratorController[] generators;   // array of generators

    [SerializeField]
    DoorController theDoor;

    RocketSpawner rs;   // rocket spawner

    int counter;        // counter of generators

    // Start is called before the first frame update
    void Start()
    {
        counter = generators.Length;
        rs = FindObjectOfType<RocketSpawner>();
    }

    public void MinusGenerator()    // if we have destroyed a generator
    {
        counter--;
        if (counter <= 0)   // if we have destroyed all generators
        {
            PuzzleSolved();
        }
        else
            rs.SpawnRocket(); // spawn new rocket
    }

    void PuzzleSolved()
    {
        //Debug.Log("Puzzle Solved");
        // OPEN THE DOOR
        theDoor.OpenTheDoor();
    }
}