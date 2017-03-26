using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TicTacToe : MonoBehaviour {
    //UI Stuff
    private GameObject endMsg;
    private GameObject proxyCroc;
    private Text lossesText;
    private Text drawsText;

    [SerializeField]
    private int widthApart = 200;
    [SerializeField]
    private int heightApart = 200;
    [SerializeField]
    private GameObject boardTile;
    private int moveCount = 0;

    private Vector2 boardDimensions = new Vector2(3, 3);
    private GameObject[] actualBoard = new GameObject[9];
    private int[] board = { 0, 0, 0, 0, 0, 0, 0, 0, 0 };

    void Awake() {
        lossesText = GameObject.Find("Loss").GetComponent<Text>();
        drawsText = GameObject.Find("Draws").GetComponent<Text>();
        lossesText.text = "Losses: " + GetScores("Losses");
        drawsText.text = "Draws: " + GetScores("Draws");


        proxyCroc = GameObject.Find("Croc");
        endMsg = GameObject.Find("EndMsg");
        endMsg.SetActive(false);
    }
    int GetScores(string type) {
        if (PlayerPrefs.HasKey(type)) {
            return PlayerPrefs.GetInt(type);
        } else {
            PlayerPrefs.SetInt(type, 0);
            print(type + " not present");
        }
        return 0;
    }

    int IncrementScores(string type) {
        if (PlayerPrefs.HasKey(type)) {
            int val = PlayerPrefs.GetInt(type);
            val += 1;
            PlayerPrefs.SetInt(type, val);
            return val;
        }
        return 0;
    }

    void Start() {
        //make the visual board
        int total = 0;
        for (int y = 0; y < boardDimensions.x; y++) {
            for (int x = 0; x < boardDimensions.y; x++) {
                actualBoard[total] = Instantiate(boardTile, new Vector2(x * widthApart, -y * heightApart), Quaternion.identity);
                total++;
            }
        }
    }

    //assumes that you go first
    public void PlayerTouch(GameObject instance) {
        int winState = 0;
        moveCount++;
        if (moveCount <= Mathf.CeilToInt(board.Length / 2)) {
            for (int i = 0; i < actualBoard.Length; i++) {
                if (actualBoard[i] == instance) {
                    board[i] = -1;
                }
            }
            ComputerMove();

            winState = Win();
            if (winState == 1) {
                print("You lose.\n");
                lossesText.text = "Losses: " + IncrementScores("Losses");
                EndGame();
            } else if (winState == -1) {
                print("You win. Inconceivable!\n");
                //literally impossible...
            }
        } else {
            drawsText.text = "Draws: " + IncrementScores("Draws");
            print("A draw. How droll.\n");
            Text eText = endMsg.GetComponentInChildren<Text>();
            eText.text = "Draw!";
            EndGame();
        }
    }

    void EndGame() {
        endMsg.SetActive(true);
        proxyCroc.GetComponent<Animator>().SetBool("EndGame", true);
        foreach (GameObject box in actualBoard) {
            box.GetComponent<BoxScript>().isPlayable = false;
        }
    }

    int Win() {
        //determines if a player has won, returns 0 otherwise.
        int[,] wins = { { 0, 1, 2 }, { 3, 4, 5 }, { 6, 7, 8 }, { 0, 3, 6 }, { 1, 4, 7 }, { 2, 5, 8 }, { 0, 4, 8 }, { 2, 4, 6 } };
        int i;
        for (i = 0; i < 8; ++i) {
            if (board[wins[i, 0]] != 0 &&
                board[wins[i, 0]] == board[wins[i, 1]] &&
                board[wins[i, 0]] == board[wins[i, 2]]) {
                return board[wins[i, 2]];
            }
        }
        return 0;
    }

    int Minimax(int player) {
        //What is the position of the player (their turn) on board?
        int winner = Win();
        if (winner != 0) return winner * player;

        int move = -1;
        int score = -2;//Losing moves are preferred to no move
        int i;
        for (i = 0; i < board.Length; ++i) {//For all moves,
            if (board[i] == 0) {//If legal,
                board[i] = player;//Try the move
                int thisScore = -Minimax(player * -1);
                if (thisScore > score) {
                    score = thisScore;
                    move = i;
                }//Pick the one that's worst for the opponent
                board[i] = 0;//Reset board after try
            }
        }
        if (move == -1) return 0;
        return score;
    }

    void ComputerMove() {
        int move = -1;
        int score = -2;
        int i;
        for (i = 0; i < board.Length; ++i) {
            if (board[i] == 0) {
                board[i] = 1;
                int tempScore = -Minimax(-1);
                board[i] = 0;
                if (tempScore > score) {
                    score = tempScore;
                    move = i;
                }
            }
        }
        //returns a score based on minimax tree at a given node.
        board[move] = 1;
        actualBoard[move].GetComponent<BoxScript>().ComputerTouch();
    }
}
