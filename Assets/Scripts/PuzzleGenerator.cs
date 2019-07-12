using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleGenerator : MonoBehaviour{

    public int size;
    private Puzzle puzzle;
    public GameObject tilePrefab;
    private float defaultTileScale; // the size of the tile prefab before scaling. assumes the tile is a square!

    void Start(){
        setDefaultTileScale();
        if (size < 2) {
            print("the puzzle size is too small! It must be at least 2. It is currently " + size);
        }
        else {
            puzzle = new Puzzle(size);
            //printPuzzle();
            drawPuzzle(new Vector2(0, 0), 3);
        }
    }

    
    void Update(){
        
    }
    
    public void printPuzzle() {
        printCore();
        //print("top: " + formatListToString(puzzle.topNums) + " bottom: " + formatListToString(puzzle.bottomNums) + "left: " + formatListToString(puzzle.leftNums) + "right: " + formatListToString(puzzle.rightNums));
        
        print("top: " + formatListToString(puzzle.topNums));
        print("bottom: " + formatListToString(puzzle.bottomNums));
        print("left: " + formatListToString(puzzle.leftNums));
        print("right: " + formatListToString(puzzle.rightNums));
        /*
            ; printList(puzzle.topNums);
        print("bottom");
        printList(puzzle.bottomNums);
        print("left:");
        printList(puzzle.leftNums);
        print("right");
        printList(puzzle.rightNums);
        */
    }

    private string formatListToString(int[] l) {
        string output = "";
        foreach (int element in l) {
            output += element;
            output += ", ";
        }

        //cut off last 2 letters
        output = output.Substring(0, (output.Length - 2));
        //print(output);
        return output;
    }

    private void printCore() {
        string output = "";

        for (int i = 0; i < puzzle.puzzle.GetLength(0); i++) {
            for (int j = 0; j < puzzle.puzzle.GetLength(0); j++) {
                output += puzzle.puzzle[i, j];
            }
            output += "-";
        }
        //cut off last letter
        output = output.Substring(0, (output.Length - 1));
        print(output);
    }

    private void drawBlankPuzzle(Vector2 center, float tileSize) {
        float totalSize = puzzle.size * tileSize * defaultTileScale;
        
        for (int i = 0; i < puzzle.size; i++) {
            for (int j = 0; j < puzzle.size; j++) {
                //int value = puzzle.puzzle[i, j];
                GameObject tile = Instantiate(tilePrefab);
                tile.transform.position = new Vector2(center.x  - (totalSize / 2) + (tileSize * defaultTileScale * i), center.y - (totalSize / 2) + (tileSize * defaultTileScale * j)) ;
                tile.transform.localScale *= tileSize;
                tile.transform.parent = this.transform;
            }
        }
    }
    

    private void setDefaultTileScale() {
        defaultTileScale = tilePrefab.GetComponent<BoxCollider2D>().size.x;
    }
    

}
