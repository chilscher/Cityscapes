using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleState {

    private int size;
    public int[,] buildings;
    public int[,][] redNotes;
    public int[,][] greenNotes;


    public PuzzleState(PuzzleGenerator puzzle) {
        size = puzzle.puzzle.size;
        buildings = new int[size, size];
        for (int i =0; i<size; i++) {
            for (int j = 0; j<size; j++) {
                buildings[i, j] = puzzle.tilesArray[i, j].shownNumber;
            }
        }

        redNotes = new int[size, size][];

        for (int i = 0; i < size; i++) {
            for (int j = 0; j < size; j++) {
                redNotes[i, j] = toList(puzzle.tilesArray[i, j].noteGroup1);
            }
        }
        greenNotes = new int[size, size][];

        for (int i = 0; i < size; i++) {
            for (int j = 0; j < size; j++) {
                greenNotes[i, j] = toList(puzzle.tilesArray[i, j].noteGroup2);
            }
        }

    }
    /*
    public void printPuzzleState() {
        string s = "";
        for (int i = 0; i < size; i++) {
            for (int j = 0; j < size; j++) {
                s += buildings[i, j];
            }
        }
        Debug.Log(s);

        string t = "";
        for (int i = 0; i < size; i++) {
            for (int j = 0; j < size; j++) {
                int[] n = notes[i, j];
                for (int k = 0; k<n.Length; k++) {
                    t += n[k];
                }
            }
        }
        Debug.Log(t);

    }

    */

    public int[] toList(List<int> array) {
        int[] result;
        result = new int[array.Count];
        for (int i = 0; i<array.Count; i++) {
            result[i] = array[i];
        }
        return result;
    }

    public void restorePuzzleState(PuzzleGenerator puzzle) {

        for (int i = 0; i < size; i++) {
            for (int j = 0; j < size; j++) {
                puzzle.tilesArray[i, j].shownNumber = buildings[i, j];
                puzzle.tilesArray[i, j].addNumberToTile(buildings[i, j]);
            }
        }
        


        for (int i = 0; i < size; i++) {
            for (int j = 0; j < size; j++) {
                puzzle.tilesArray[i, j].clearColoredNotes();
                for (int k = 0; k<redNotes[i,j].Length; k++) {
                    puzzle.tilesArray[i, j].toggleNote1(redNotes[i, j][k]);
                }
                for (int k = 0; k < greenNotes[i, j].Length; k++) {
                    puzzle.tilesArray[i, j].toggleNote2(greenNotes[i, j][k]);
                }
            }
        }
        

    }

}
