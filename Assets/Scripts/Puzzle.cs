//for Cityscapes, copyright Cole Hilscher 2024

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Puzzle{
    //contains the solution of a puzzle, as well as all of the side hints, all in the form of ints
    public int size = 5;
    public int[,] solution;
    public int[] topNums;
    public int[] bottomNums;
    public int[] leftNums;
    public int[] rightNums;
    public int[,] startingSolution;

    public Puzzle(int size) {
        //randomly create a puzzle with defined size
        this.size = size;        
        GeneratePuzzle();
        GenerateSideNumbers();
        GenerateStartingSolution();
        AddPermanentBuildingsBasedOnSize();
    }

    public Puzzle(int[,] predeterminedSolution, int predeterminedPermanentBuilding) {
        //create a puzzle object based off of a predetermined solution
        this.size = (int)Mathf.Sqrt(predeterminedSolution.Length);
        solution = predeterminedSolution;
        GenerateSideNumbers();
        if (predeterminedPermanentBuilding == 0){
            startingSolution = new int[size, size];
            startingSolution[0,0] = solution[0,0];
        }
    }

    private void GeneratePuzzle() {
        solution = new int[size, size];

        //create a list of steps to follow
        Step[] allSteps = new Step[size * size];
        Step previousStep = null;
        for (int i = 0; i < size; i++) {
            int n = i + 1;
            for (int j = 0; j < size; j++) {
                //create the step for this part of the puzzle generation
                int stepIndex = (size * i) + j;
                allSteps[stepIndex] = new Step(n);
                Step step = allSteps[stepIndex];
                step.previousStep = previousStep;
                if (previousStep != null)
                    previousStep.nextStep = step;
                step.index = stepIndex;
                previousStep = step;
                //set up the empty puzzle at the same time
                solution[i, j] = 0;
            }
        }

        bool cont = true;
        Step currentStep = allSteps[0];
        while(cont){
            if (!currentStep.hasSearchedForPossibleSpaces){
                currentStep.possibleSpaces = CreateListOfAvailableSpaces(currentStep.numberToPlace);
                currentStep.hasSearchedForPossibleSpaces = true;
            }
            Space space = currentStep.PickRandomSpace();
            if (space != null){
                solution[space.x, space.y] = currentStep.numberToPlace;
                currentStep.selectedSpace = space;
                currentStep.possibleSpaces.Remove(space);
                currentStep = currentStep.nextStep;
                if (currentStep == null)
                    cont = false;
            }
            else{
                currentStep.hasSearchedForPossibleSpaces = false;
                //there are no available spaces to pick
                currentStep = currentStep.previousStep;
                //back out the last change
                solution[currentStep.selectedSpace.x, currentStep.selectedSpace.y] = 0;
                currentStep.selectedSpace = null;
            }
        }
    }

    private void GenerateStartingSolution(){
        //removes each number from the puzzle until it is no longer uniquely solveable
        startingSolution = new int[size, size];
        List<Space> spacesToCheck = new List<Space>();
        for (int i = 0; i < size; i++) {
            for (int j = 0; j < size; j++) {
                startingSolution[i,j] = solution[i,j];
                spacesToCheck.Add(new Space(i,j));
            }
        }
        while(spacesToCheck.Count > 0){
            Space space = spacesToCheck[StaticVariables.rand.Next(spacesToCheck.Count)];
            spacesToCheck.Remove(space);
            startingSolution[space.x, space.y] = 0;
            if (!IsPuzzleValid())
                startingSolution[space.x,space.y] = solution[space.x,space.y];
        }
    }

    private void AddPermanentBuildingsBasedOnSize(){
        int numToAdd = 0;
        if (size == 3)
            numToAdd = 1;
        if (size == 4)
            numToAdd = 2;
        if (size == 5)
            numToAdd = 2;
        if (size == 6)
            numToAdd = 1;

        for (int i = 0; i < numToAdd; i++){
            int x = -1;
            int y = -1;
            while (x == -1){
                x = StaticVariables.rand.Next(size);
                y = StaticVariables.rand.Next(size);
                if (startingSolution[x,y] != 0){
                    x = -1;
                    y = -1;
                }
                else if (solution[x,y] == size){
                    x = -1;
                    y = -1;
                }
            }
            startingSolution[x,y] = solution[x,y];
        }

    }

    private void PrintAllSteps(Step[] steps){
        foreach (Step step in steps){
            string s = "step #" + step.index + " is to place " + step.numberToPlace + ". ";
            if (step.previousStep == null)
                s += " there is no previous step. ";
            else
                s += " the previous step index is " + step.previousStep.index;
            if (step.nextStep == null)
                s += " there is no next step. ";
            else
                s += " the next step index is " + step.nextStep.index;
            Debug.Log(s);
        }
    }

    private void PrintPuzzleState(){
        Debug.Log("puzzle state: ");
        for (int i = 0; i < size; i++) {
            string s = "";
            for (int j = 0; j < size; j++) {
                s += "[" + solution[i,j] + "] ";
            }
            Debug.Log(s);
        }
    }

    
    private void PrintStartingSolution(){
        Debug.Log("starting state: ");
        for (int i = 0; i < size; i++) {
            string s = "";
            for (int j = 0; j < size; j++) {
                s += "[" + startingSolution[i,j] + "] ";
            }
            Debug.Log(s);
        }
    }

    private bool CheckIfPuzzleIsValid(){
        //iterate through each possible building size
        //for each row and column
        
        for (int i = 0; i < size; i++) {
            
            //count up the number of times each size appears
            int[] xCounts = new int[size];
            int[] yCounts = new int[size];
            for (int j = 0; j < size; j++) {
                xCounts[solution[i,j]] ++;
                yCounts[solution[i,j]] ++;
            }
            
            //if any is more than one, we are not valid
            for (int z = 0 ; z < size; z++){
                if (xCounts[z] > 1)
                    return false;
                if (yCounts[z] > 1)
                    return false;
            }
        }
        return true;
    }

    private List<Space> CreateListOfAvailableSpaces(int numberToPlace){
        //create a list of possible spaces the building can go
        List<Space> spacesAvailable = new List<Space>();
        for (int j = 0; j < size; j++) {
            for (int k = 0; k < size; k++) {
                if (CanSpaceContainValue(j, k, numberToPlace)){
                    //Debug.Log("the number " + numberToPlace + " can go in the space [" + j + "," + k + "]");
                    spacesAvailable.Add(new Space(j, k));
                }
            }
        }
        return spacesAvailable;
    }

    private bool CanSpaceContainValue(int xcoord, int ycoord, int value){
        if (solution[xcoord, ycoord] != 0)
            return false;
        for (int i = 0; i < size; i++) {
            for (int j = 0; j < size; j++) {
                if (((i == xcoord) || (j == ycoord)) && (solution[i,j] == value))
                    return false;
            }
        }
        return true;
    }

    private int PickValue(int i, int j) {
        //returns the chosen value to be placed on tile i,j
        //the value returned is chosen from a list of random possible values
        ArrayList freeValues = new ArrayList();
        for (int k = 0; k < size; k++)
            freeValues.Add(k + 1);
        for (int k = 0; k < i; k++) {
            if (solution[k,j] != 0)
                freeValues.Remove(solution[k, j]);
        }
        for (int l = 0; l < j; l++) {
            if (solution[i, l] != 0)
                freeValues.Remove(solution[i, l]);
        }
        int index = StaticVariables.rand.Next(freeValues.Count);
        return((int)freeValues[index]);
    }

    private void GenerateSideNumbers() {
        //given a solution puzzle, creates the side hint numbers to match
        topNums = new int[size];
        bottomNums = new int[size];
        leftNums = new int[size];
        rightNums = new int[size];
        
        for (int index = 0; index < size; index++) {
            
            int[] row = new int[size];
            
            for (int j = 0; j < size; j++)
                row[j] = solution[index, j];
            leftNums[index] = GetSideNum(row);
            rightNums[index] = GetSideNum(ReverseList(row));
        }

        for (int index = 0; index < size; index++) {
            int[] column = new int[size];
            for (int i = 0; i < size; i++)
                column[i] = solution[i, index];
            topNums[index] = GetSideNum(column);
            bottomNums[index] = GetSideNum(ReverseList(column));
        }
    }

    private int GetSideNum(int[] nums) {
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

    private int[] ReverseList(int[] original) {
        //returns a list backwards
        int[] newList = new int[size];
        for (int i = 0; i < size; i++) {
            int oppositeIndex = size - i;
            newList[i] = original[oppositeIndex - 1];
        }
        return newList;
    }

    public bool IsPuzzleValid() {
        //returns true if the puzzle has less than 7 unique solutions
        PuzzleSolver p = new PuzzleSolver();
        return p.IsPuzzleValid(topNums, bottomNums, leftNums, rightNums, startingSolution);
    }

}

public class Space{
    public int x;
    public int y;
    public Space(int xcoord, int ycoord){
        x = xcoord;
        y = ycoord;
    }
}

public class Step{
    public int numberToPlace = 0;
    public List<Space> possibleSpaces = new List<Space>();
    public Step previousStep;
    public Step nextStep;
    public int index = -1;
    public Space selectedSpace;
    public bool hasSearchedForPossibleSpaces = false;
    public Step(int num){
        numberToPlace = num;
    }
    public Space PickRandomSpace(){
        if (possibleSpaces.Count == 0)
            return null;
        return possibleSpaces[StaticVariables.rand.Next(possibleSpaces.Count)];
    }

}
