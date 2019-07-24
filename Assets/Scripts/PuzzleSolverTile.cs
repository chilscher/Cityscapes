using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSolverTile {

    public int value = 0;
    public List<int> prohibitedValues = new List<int>();
    public PuzzleSolverSideTile topHintTile;
    public PuzzleSolverSideTile bottomHintTile;
    public PuzzleSolverSideTile leftHintTile;
    public PuzzleSolverSideTile rightHintTile;
    public bool populated = false;
    public int xValue; //j
    public int yValue; //i

    public void prohibitValue(int x) {
        if ((!prohibitedValues.Contains(x))) {
            prohibitedValues.Add(x);
        }
    }

    public void prohibitValues(List<int> x) {
        foreach (int i in x) {
            prohibitValue(i);
        }
    }

    public void populate(int val) {
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

    
    public void printProhibited() {
        int x = xValue + 1;
        int y = yValue + 1;
        string output = "";
        output += "tile at " + x + ", " + y + " has prohibited values ";

        foreach (int p in prohibitedValues) {
            output += p + ", ";
        }
        output = output.Substring(0, (output.Length - 2));

        if (populated) {
            output = "tile at " + x + ", " + y + " has already been populated! ";
        }
        else if(prohibitedValues.Count == 0) {
            output = "tile at " + x + ", " + y + " has no prohibited values ";
        }

        Debug.Log(output);

    }
    
    

}
