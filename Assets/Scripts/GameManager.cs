using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    const int rowSize = 4;
    const int columnSize = 4;

    Tile[,] allTiles = new Tile[rowSize, columnSize];

    public enum GameState { Playing,GameOver}
    public GameState gameState;

    public List<Tile[]> columns = new List<Tile[]>();
    public List<Tile[]> rows = new List<Tile[]>();

    List<Tile> emptyTiles = new List<Tile>();

    public Text finalScoreText;
    public GameObject winScreen;
    public GameObject gameOverScreen;
    public GameObject gameOverPanel;

    // Start is called before the first frame update
    void Start()
    {
        ResetTilesWhenGameStarts();
        InitializeAllTiles();
        GeneratingFirstTwoStartingTiles();
        gameOverPanel.SetActive(false);
        gameState = GameState.Playing;
    }

    private void GeneratingFirstTwoStartingTiles()
    {
        for (int i = 0; i < 2; i++)
        {
            GenerateRandomTile();
        }
    }

    private void InitializeAllTiles()
    {
        for (int i = 0; i < allTiles.GetLength(0); i++)
        {
            Tile[] tempRowArr = new Tile[rowSize];
            Tile[] tempColArr = new Tile[columnSize];
            for (int j = 0; j < allTiles.GetLength(1); j++)
            {
                tempRowArr[j] = allTiles[i, j];
                tempColArr[j] = allTiles[j, i];
            }
            rows.Add(tempRowArr);
            columns.Add(tempColArr);
        }
        /* Debugging block
        Debug.Log(rows.Count);
        Debug.Log(columns.Count);
        foreach(Tile[] t in rows)
        {
            foreach(Tile tile in t)
            {
                Debug.Log(tile.rowIndex + "/" + tile.colIndex);
            }
        }
        foreach (Tile[] t in columns)
        {
            foreach (Tile tile in t)
            {
                Debug.Log(tile.rowIndex + "/" + tile.colIndex);
            }
        }*/

    }

    private void ResetTilesWhenGameStarts()
    {
        Tile[] allTilesInScene = FindObjectsOfType<Tile>();
        foreach (Tile tile in allTilesInScene)
        {
            tile.Number = 0;
            allTiles[tile.rowIndex, tile.colIndex] = tile;
            emptyTiles.Add(tile);
        }
    }

    private bool ShiftByOneIndexDown(Tile[] tiles)
    {
        for (int i = 0; i < tiles.Length - 1; i++)
        {
            //Beginning of Move Block
            if (tiles[i].Number == 0 && tiles[i + 1].Number != 0)
            {
                tiles[i].Number = tiles[i + 1].Number;
                tiles[i + 1].Number = 0;
                return true;
            }
            //End of Move Block
            //Beginning of Merge Block
            if (tiles[i].Number != 0 && tiles[i].Number == tiles[i + 1].Number && !tiles[i].mergedThisTurn && !tiles[i + 1].mergedThisTurn)
            {
                tiles[i].Number *= 2;
                tiles[i + 1].Number = 0;
                tiles[i].mergedThisTurn = true;
                ScoreManager.Instance.Score += tiles[i].Number;
                if(tiles[i].Number == 2048)
                {
                    DisplayWinScreen();
                }
                return true;
            }
            //End of Merge Block
        }

        return false;
    }

    private bool ShiftByOneIndexUp(Tile[] tiles)
    {
        for (int i = tiles.Length - 1; i > 0; i--)
        {
            //Beginning of Move Block
            if (tiles[i].Number == 0 && tiles[i - 1].Number != 0)
            {
                tiles[i].Number = tiles[i - 1].Number;
                tiles[i - 1].Number = 0;
                return true;
            }
            //End of Move Block
            //Beginning of Merge Block
            if (tiles[i].Number != 0 && tiles[i].Number == tiles[i - 1].Number && !tiles[i].mergedThisTurn && !tiles[i - 1].mergedThisTurn)
            {
                tiles[i].Number *= 2;
                tiles[i - 1].Number = 0;
                tiles[i].mergedThisTurn = true;
                ScoreManager.Instance.Score += tiles[i].Number;
                if (tiles[i].Number == 2048)
                {
                    DisplayWinScreen();
                }
                return true;
            }
            //End of Merge Block
        }
        return false;
    }

    private void ResetTileMergedFlags()
    {
        foreach (Tile t in allTiles)
        {
            t.mergedThisTurn = false;
        }
    }


    private void GenerateRandomTile()
    {
        if (emptyTiles.Count > 0)
        {
            int indexForNewNumber = Random.Range(0, emptyTiles.Count);
            int randomNumber = Random.Range(0, 10);

            if (randomNumber == 9) //10% probability of spawning a 4 Tile
            {
                emptyTiles[indexForNewNumber].Number = 4;
            }
            else
            {
                emptyTiles[indexForNewNumber].Number = 2;
            }
            emptyTiles.RemoveAt(indexForNewNumber);

        }
    }

    // Update is called once per frame
    /*void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            GameOver();
        }
    }*/


    private void UpdateEmptyTiles()
    {
        emptyTiles.Clear();
        foreach (Tile tile in allTiles)
        {
            if (tile.Number == 0)
            {
                emptyTiles.Add(tile);
            }
        }
    }

    public void ProcessSwipe(SwipeDirection swipe)
    {
        Debug.Log(swipe.ToString());
        bool moveMadeThisTurn = false;
        ResetTileMergedFlags();

        for (int i = 0; i < rows.Count; i++)
        {
            switch (swipe)
            {
                case SwipeDirection.Down:
                    while (ShiftByOneIndexUp(columns[i]))
                    {
                        moveMadeThisTurn = true;
                    }
                    break;
                case SwipeDirection.Up:
                    while (ShiftByOneIndexDown(columns[i]))
                    {
                        moveMadeThisTurn = true;
                    }
                    break;
                case SwipeDirection.Left:
                    while (ShiftByOneIndexDown(rows[i]))
                    {
                        moveMadeThisTurn = true;
                    }
                    break;
                case SwipeDirection.Right:
                    while (ShiftByOneIndexUp(rows[i]))
                    {
                        moveMadeThisTurn = true;
                    }
                    break;
            }
        }

        if (moveMadeThisTurn)//Generate a new tile only after a move is made successfully
        {
            UpdateEmptyTiles();
            GenerateRandomTile();
            if (!IsMovePossible())//Trigger Game over if no moves are possible
            {
                
                GameOver();
            }
        }

    }

    private void GameOver()
    {
        gameState = GameState.GameOver;
        finalScoreText.text = "Final Score:" + ScoreManager.Instance.Score;
        winScreen.SetActive(false);
        gameOverScreen.SetActive(true);
        gameOverPanel.SetActive(true);
    }

    private void DisplayWinScreen()
    {
        gameState = GameState.GameOver;
        finalScoreText.gameObject.SetActive(false);
        winScreen.SetActive(true);
        gameOverScreen.SetActive(false);
        gameOverPanel.SetActive(true);
    }

    //Check Possible Moves
    private bool IsMovePossible()
    {
        if (emptyTiles.Count > 0)
        {
            return true;
        }
        else
        {
            //Check for available merges in columns
            for(int i = 0; i < columns.Count; i++)
            {
                for(int j = 0; j < rows.Count-1; j++)
                {
                    if(allTiles[j,i].Number == allTiles[j + 1, i].Number)
                    {
                        return true;
                    }
                }
            }

            //Check for available merges in rows 
            for (int i = 0; i < rows.Count; i++)
            {
                for (int j = 0; j < columns.Count - 1; j++)
                {
                    if (allTiles[i, j].Number == allTiles[i, j+1].Number)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public void InitializeNewGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
