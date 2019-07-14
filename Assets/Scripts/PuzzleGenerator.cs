using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleGenerator : MonoBehaviour{

    public int size;
    private Puzzle puzzle;
    public GameObject puzzleTilePrefab;
    public GameObject sideHintTilePrefab;

    void Start(){
        if (size < 2 || size > 7) {
            print("the puzzle size has to be between 2 and 7. It is currently " + size);
        }
        else {
            puzzle = new Puzzle(size);
            drawPuzzle(Vector2.zero, 3);
            drawSideHints(Vector2.zero, 3);
            //printPuzzle();
        }
    }

    private void drawPuzzle(Vector2 center, float tileSize) {
        float defaultTileScale = puzzleTilePrefab.GetComponent<BoxCollider2D>().size.x;
        float totalSize = puzzle.size * tileSize * defaultTileScale;

        for (int i = 0; i < puzzle.size; i++) {
            for (int j = 0; j < puzzle.size; j++) {
                //Vector2 pos = new Vector2(center.x - (totalSize / 2) + (tileSize * defaultTileScale * i), center.y - (totalSize / 2) + (tileSize * defaultTileScale * j));
                Vector2 pos = new Vector2(center.x - (totalSize / 2) + (tileSize * defaultTileScale * (j + 0.5f)), center.y + (totalSize / 2) - (tileSize * defaultTileScale * (i + 0.5f)));
                GameObject tile = Instantiate(puzzleTilePrefab);
                tile.GetComponent<PuzzleTile>().initialize(pos, tileSize, puzzle.solution[i, j], this.transform);
                //print(puzzle.solution[i, j]);
                //print(pos.x + " " + pos.y);
                
            }
        }
    }

    private void drawSideHints(Vector2 center, float tileSize) {
        float defaultTileScale = sideHintTilePrefab.GetComponent<BoxCollider2D>().size.x;
        float puzzleTotalSize = puzzle.size * tileSize * puzzleTilePrefab.GetComponent<BoxCollider2D>().size.x;
        float topy = center.y + ((puzzleTotalSize) / 2) + (tileSize * defaultTileScale * 0.5f);
        float bottomy = center.y - ((puzzleTotalSize) / 2) - (tileSize * defaultTileScale * 0.5f);
        float leftx = center.x - (puzzleTotalSize / 2) - (tileSize * defaultTileScale * (0.5f));
        float rightx = center.x + (puzzleTotalSize / 2) + (tileSize * defaultTileScale * (0.5f));
        for (int i = 0; i < puzzle.size; i++) {
            float tbx = center.x - (puzzleTotalSize / 2) + (tileSize * defaultTileScale * (i + 0.5f));
            float lry = center.y + ((puzzleTotalSize) / 2) - (tileSize * defaultTileScale * (i + 0.5f));
            GameObject topTile = Instantiate(sideHintTilePrefab);
            GameObject bottomTile = Instantiate(sideHintTilePrefab);
            GameObject rightTile = Instantiate(sideHintTilePrefab);
            GameObject leftTile = Instantiate(sideHintTilePrefab);
            topTile.GetComponent<SideHintTile>().initialize(new Vector2(tbx, topy), tileSize, puzzle.topNums[i], this.transform);
            bottomTile.GetComponent<SideHintTile>().initialize(new Vector2(tbx, bottomy), tileSize, puzzle.bottomNums[i], this.transform);
            leftTile.GetComponent<SideHintTile>().initialize(new Vector2(leftx, lry), tileSize, puzzle.leftNums[i], this.transform);
            rightTile.GetComponent<SideHintTile>().initialize(new Vector2(rightx, lry), tileSize, puzzle.rightNums[i], this.transform);
        }
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

        for (int i = 0; i < puzzle.size; i++) {
            for (int j = 0; j < puzzle.size; j++) {
                output += puzzle.solution[i, j];
            }
            output += "-";
        }
        //cut off last letter
        output = output.Substring(0, (output.Length - 1));
        print(output);
    }





}
