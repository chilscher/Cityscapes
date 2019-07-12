using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle{
//public class Puzzle : MonoBehaviour {

    public int size = 5;
    public int[,] puzzle;
    public int[] topNums;
    public int[] bottomNums;
    public int[] leftNums;
    public int[] rightNums;
    private System.Random random = new System.Random();

    
    public Puzzle(int size) {
        this.size = size;
        generatePuzzle();
        generateSideNumbers();
    }
    

    /*
    private void Start() {

        generatePuzzle();

        print2dList(puzzle);
        generateSideNumbers();
        
        print("top:");
        printList(topNums);
        print("bottom");
        printList(bottomNums);
        print("left:");
        printList(leftNums);
        print("right");
        printList(rightNums);
        
    }
    */
    

    private void generatePuzzle() {
        int max_height = size;

        try {
            //create empty puzzle, made of all zeroes
            puzzle = new int[size, size];
            for (int i = 0; i < puzzle.GetLength(0); i++) {
                for (int j = 0; j < puzzle.GetLength(0); j++) {
                    puzzle[i, j] = 0;
                }
            }

            for (int i = 0; i < puzzle.GetLength(0); i++) {
                for (int j = 0; j < puzzle.GetLength(0); j++) {
                    puzzle[i, j] = pickValue(i, j);
                }
            }
        }
        catch {
            generatePuzzle();
        }
        
    }

    private int pickValue(int i, int j) {
        //returns the chosen value to be placed on tile i,j
        
        ArrayList freeValues = new ArrayList();
        for (int k = 0; k < size; k++) {
            freeValues.Add(k + 1);
        }
        
        for (int k = 0; k < i; k++) {
            if (puzzle[k,j] != 0) {
                freeValues.Remove(puzzle[k, j]);
            }
        }
        for (int l = 0; l < j; l++) {
            if (puzzle[i, l] != 0) {
                freeValues.Remove(puzzle[i, l]);
            }
        }


        int index = random.Next(freeValues.Count);
        return((int)freeValues[index]);
    }
    /*
    private void printArrayList(ArrayList a) {
        string output = "";
        foreach (int element in a) {
            output += element;
            output += ", ";
        }
        print(output);
    }
    
    private void printList(int[] l) {
        string output = "";
        foreach (int element in l) {
            output += element;
            output += ", ";
        }
        print(output);
    }
    
    
    private void print2dList(int[,] l) {
        string output = "";

        for (int i = 0; i < puzzle.GetLength(0); i++) {
            for (int j = 0; j < puzzle.GetLength(0); j++) {
                output += puzzle[i, j];
            }
            output += "-";
        }
        output = output.Substring(0, (output.Length - 1));
        print(output);
    }
    */
    private void generateSideNumbers() {
        topNums = new int[size];
        bottomNums = new int[size];
        leftNums = new int[size];
        rightNums = new int[size];
        
        for (int index = 0; index < size; index++) {
            
            int[] row = new int[size];
            
            for (int j = 0; j < size; j++) {
                row[j] = puzzle[index, j];
            }
            leftNums[index] = getSideNum(row);
            rightNums[index] = getSideNum(reverseList(row));
        }

        for (int index = 0; index < size; index++) {
            int[] column = new int[size];
            for (int i = 0; i < size; i++) {
                column[i] = puzzle[i, index];
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
        int[] newList = new int[size];
        for (int i = 0; i < size; i++) {
            int oppositeIndex = size - i;
            newList[i] = original[oppositeIndex - 1];
        }
        return newList;
    }

}
