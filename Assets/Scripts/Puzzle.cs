using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle{

    public int size = 5;
    public int[,] solution;
    public int[] topNums;
    public int[] bottomNums;
    public int[] leftNums;
    public int[] rightNums;
    private System.Random random = new System.Random();

    


    public Puzzle(int size) {
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
        this.size = (int)Mathf.Sqrt(predeterminedSolution.Length);
        solution = predeterminedSolution;
        //Debug.Log(predeterminedSolution[0, 3]);
        generateSideNumbers();
        hasUniqueSolution();
    }

    private void generatePuzzle() {

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

            /*
            if (!hasUniqueSolution()) {
                generatePuzzle();
            }
            */
        }
        catch {
            //if the puzzle generation failed, try again
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
            if (solution[k,j] != 0) {
                freeValues.Remove(solution[k, j]);
            }
        }
        for (int l = 0; l < j; l++) {
            if (solution[i, l] != 0) {
                freeValues.Remove(solution[i, l]);
            }
        }


        int index = random.Next(freeValues.Count);
        return((int)freeValues[index]);
    }

    private void generateSideNumbers() {
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
        int[] newList = new int[size];
        for (int i = 0; i < size; i++) {
            int oppositeIndex = size - i;
            newList[i] = original[oppositeIndex - 1];
        }
        return newList;
    }

    private bool hasUniqueSolution() {
        solvePuzzle();
        return true;
    }

    private void solvePuzzle() {
        PuzzleSolver p = new PuzzleSolver();
        p.solvePuzzle(topNums, bottomNums, leftNums, rightNums, solution);
        //puzzleSolver.GetComponent<PuzzleSolver>();
    }

    public void getUniqueSolution() {
        //returns the unique solution, if there is one, or returns the most complete the puzzle can be without a unique solution
        
        PuzzleSolver p = new PuzzleSolver();
        solution = p.getUniqueSolution(topNums, bottomNums, leftNums, rightNums, solution);

    }

    public bool isPuzzleValid() {
        //returns true if the puzzle has exactly one unique solution
        PuzzleSolver p = new PuzzleSolver();
        return p.isPuzzleValid(topNums, bottomNums, leftNums, rightNums, solution);
    }

}
