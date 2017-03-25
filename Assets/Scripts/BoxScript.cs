using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxScript : MonoBehaviour {
    private TicTacToe manager;
    private Sprite crossed;
    private Sprite circled;
    private SpriteRenderer myRenderer;
    private bool touched = false;
    [HideInInspector]
    public bool isPlayable = true;
    // Use this for initialization
    void Awake() {
        manager = FindObjectOfType<TicTacToe>();
        myRenderer = GetComponent<SpriteRenderer>();
        circled = Resources.Load("boxCircled", typeof(Sprite)) as Sprite;
        crossed= Resources.Load("boxCrossed", typeof(Sprite)) as Sprite;
    }

    void OnMouseDown() {
        if (!touched && isPlayable) {
            myRenderer.sprite = circled;
            touched = true;
            manager.PlayerTouch(this.gameObject);
        } else {
            print("this box has been touched already");
        }
    }

    public void ComputerTouch() {
        touched = true;
        myRenderer.sprite = crossed;
    }
}
