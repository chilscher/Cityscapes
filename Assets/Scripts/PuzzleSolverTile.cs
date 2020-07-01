//for Cityscapes, copyright Cole Hilscher 2020

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSolverTile {
    //an object used by PuzzleSolver to store the value and crossed-out values for a specific square of the Cityscapes Puzzle
    //all of these functions are just used to store data
    public int value = 0; //the tile's value, only filled if the value is definitely determined
    public List<int> prohibitedValues = new List<int>(); //the list of values that this tile cannot hold

    //the 4 hints that have this tile as part of their designated rows
    public PuzzleSolverSideTile topHintTile;
    public PuzzleSolverSideTile bottomHintTile;
    public PuzzleSolverSideTile leftHintTile;
    public PuzzleSolverSideTile rightHintTile;

    public bool populated = false;

    //the "coordinates" of this tile within the puzzle
    public int xValue; //j
    public int yValue; //i

    public void prohibitValue(int x) {
        //add a value to the list of impossible, prohibited values for this tile
        if ((!prohibitedValues.Contains(x))) {
            prohibitedValues.Add(x);
        }
    }

    public void prohibitValues(List<int> x) {
        //add several values to the prohibited list
        foreach (int i in x) {
            prohibitValue(i);
        }
    }

    public void populate(int val) {
        //add val to this tile, as a known solution
        if (!populated) {
            value = val;
            populated = true;
            prohibitedValues = new List<int>();
        }
    }

    public void populateLastValue() {
        //uses the last unprohibited value to populate the tile
        int maxValue = prohibitedValues.Count + 1;
        for (int i = 0; i<maxValue; i++) {
            int v = i + 1;
            if (!prohibitedValues.Contains(v)) {
                populate(v);
                break;
            }
        }
    }
}
