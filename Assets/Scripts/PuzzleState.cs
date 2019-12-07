using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PuzzleState {

    private int size;
    public int[,] buildings;
    public int[,][] notes1;
    public int[,][] notes2;


    public PuzzleState(PuzzleGenerator puzzle) {
        size = puzzle.puzzle.size;
        buildings = new int[size, size];
        for (int i =0; i<size; i++) {
            for (int j = 0; j<size; j++) {
                buildings[i, j] = puzzle.tilesArray[i, j].shownNumber;
            }
        }

        notes1 = new int[size, size][];

        for (int i = 0; i < size; i++) {
            for (int j = 0; j < size; j++) {
                notes1[i, j] = toList(puzzle.tilesArray[i, j].noteGroup1);
            }
        }
        notes2 = new int[size, size][];

        for (int i = 0; i < size; i++) {
            for (int j = 0; j < size; j++) {
                notes2[i, j] = toList(puzzle.tilesArray[i, j].noteGroup2);
            }
        }

    }
    public PuzzleState(string str, int s) {
        size = s;
        string[] split1 = str.Split(' ');
        string buildingString = split1[0];
        string notes1String = split1[1];
        string notes2String = split1[2];

        string[] buildingList = buildingString.Split('-');

        buildings = new int[size, size];
        for (int i = 0; i < size; i++) {
            for (int j = 0; j < size; j++) {
                buildings[i, j] = Int32.Parse(buildingList[(size * i) + j]);
            }
        }

        string[] notes1List = notes1String.Split('-');

        notes1 = new int[size, size][];
        for (int i = 0; i < size; i++) {
            for (int j = 0; j < size; j++) {
                string numbers = notes1List[(size * i) + j];
                List<int> numbersList = new List<int>();
                foreach (char c in numbers) {
                    int n = c - '0';
                    numbersList.Add(n);
                }

                notes1[i, j] = toList(numbersList);
            }
        }

        string[] notes2List = notes2String.Split('-');

        notes2 = new int[size, size][];
        for (int i = 0; i < size; i++) {
            for (int j = 0; j < size; j++) {
                string numbers = notes2List[(size * i) + j];
                List<int> numbersList = new List<int>();
                foreach (char c in numbers) {
                    int n = c - '0';
                    numbersList.Add(n);
                }

                notes2[i, j] = toList(numbersList);
            }
        }
    }

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
                for (int k = 0; k< notes1[i,j].Length; k++) {
                    puzzle.tilesArray[i, j].toggleNote1(notes1[i, j][k]);
                }
                for (int k = 0; k < notes2[i, j].Length; k++) {
                    puzzle.tilesArray[i, j].toggleNote2(notes2[i, j][k]);
                }
            }
        }
        

    }

    public string returnStateAsString() {
        //every tile is separated by a dash, building/notes1/notes2 is separated by a space
        string buildingsString = "";
        string notes1String = "";
        string notes2String = "";

        for (int i = 0; i < size; i++) {
            for (int j = 0; j < size; j++) {
                buildingsString += buildings[i, j];
                buildingsString += "-";
            }
        }
        buildingsString = buildingsString.Substring(0, buildingsString.Length - 1);

        for (int i = 0; i < size; i++) {
            for (int j = 0; j < size; j++) {
                for (int k = 0; k < notes1[i, j].Length; k++) {
                    notes1String += notes1[i, j][k];
                }
                notes1String += "-";
                for (int k = 0; k < notes2[i, j].Length; k++) {
                    notes2String += notes2[i, j][k];
                }
                notes2String += "-";
            }
        }
        notes1String = notes1String.Substring(0, notes1String.Length - 1);
        notes2String = notes2String.Substring(0, notes2String.Length - 1);

        string result = buildingsString + " " + notes1String + " " + notes2String;
        return result;
    }

}
