//for Cityscapes, copyright Cole Hilscher 2020

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle{
    //contains the solution of a puzzle, as well as all of the side hints, all in the form of ints
    public int size = 5;
    public int[,] solution;
    public int[] topNums;
    public int[] bottomNums;
    public int[] leftNums;
    public int[] rightNums;

    public Puzzle(int size) {
        //randomly create a puzzle with defined size
        //specifically, creates one puzzle at random, checks if it is solvable, and repeats until it gets a valid puzzle
        this.size = size;
        bool cont = true;
        while (cont) {
            generatePuzzle();
            generateSideNumbers();
            if (isPuzzleValid()) {
                cont = false;
            }
        }
    }

    public Puzzle(int[,] predeterminedSolution) {
        //create a puzzle object based off of a predetermined solution
        this.size = (int)Mathf.Sqrt(predeterminedSolution.Length);
        solution = predeterminedSolution;
        generateSideNumbers();
    }

    private void generatePuzzle() {
        //create a puzzle by randomly picking valid values
        try {
            //create empty puzzle, made of all zeroes
            solution = new int[size, size];
            for (int i = 0; i < size; i++) {
                for (int j = 0; j < size; j++) {
                    solution[i, j] = 0;
                }
            }

            //attempt to make the actual puzzle
            for (int i = 0; i < size; i++) {
                for (int j = 0; j < size; j++) {
                    solution[i, j] = pickValue(i, j);
                }
            }
        }
        catch {
            //if the puzzle generation failed, try again
            generatePuzzle();
        }
    }

    private int pickValue(int i, int j) {
        //returns the chosen value to be placed on tile i,j
        //the value returned is chosen from a list of random possible values
        ArrayList freeValues = new ArrayList();
        for (int k = 0; k < size; k++) {
            freeValues.Add(k + 1);
        }
        for (int k = 0; k < i; k++) {
            if (solution[k,j] != 0) {
                freeValues.Remove(solution[k, j]);
            }
        }
        for (int l = 0; l < j; l++) {
            if (solution[i, l] != 0) {
                freeValues.Remove(solution[i, l]);
            }
        }
        int index = StaticVariables.rand.Next(freeValues.Count);
        return((int)freeValues[index]);
    }

    private void generateSideNumbers() {
        //given a solution puzzle, creates the side hint numbers to match
        topNums = new int[size];
        bottomNums = new int[size];
        leftNums = new int[size];
        rightNums = new int[size];
        
        for (int index = 0; index < size; index++) {
            
            int[] row = new int[size];
            
            for (int j = 0; j < size; j++) {
                row[j] = solution[index, j];
            }
            leftNums[index] = getSideNum(row);
            rightNums[index] = getSideNum(reverseList(row));
        }

        for (int index = 0; index < size; index++) {
            int[] column = new int[size];
            for (int i = 0; i < size; i++) {
                column[i] = solution[i, index];
            }
            topNums[index] = getSideNum(column);
            bottomNums[index] = getSideNum(reverseList(column));
        }
    }

    private int getSideNum(int[] nums) {
        //gets the skyscraper viewing number for a single row/column
        int count = 0;
        int highest = 0;
        foreach (int height in nums) {
            if (height > highest) {
                count++;
                highest = height;
            }
        }
        return count;
    }

    private int[] reverseList(int[] original) {
        //returns a list backwards
        int[] newList = new int[size];
        for (int i = 0; i < size; i++) {
            int oppositeIndex = size - i;
            newList[i] = original[oppositeIndex - 1];
        }
        return newList;
    }

    public bool isPuzzleValid() {
        //returns true if the puzzle has less than 7 unique solutions
        PuzzleSolver p = new PuzzleSolver();
        return p.isPuzzleValid(topNums, bottomNums, leftNums, rightNums);
    }

}
