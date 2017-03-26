using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxScript : MonoBehaviour {
    public AudioClip touch;
    public AudioClip cantTouch;
    //public AudioClip end;
    private AudioSource audioSrc;

    private TicTacToe manager;
    private Sprite crossed;
    private Sprite circled;
    private Sprite empty;
    private SpriteRenderer myRenderer;
    private bool touched = false;
    [HideInInspector]
    public bool isPlayable = true;
    // Use this for initialization
    void Awake() {
        audioSrc = GetComponent<AudioSource>();
        manager = FindObjectOfType<TicTacToe>();
        myRenderer = GetComponent<SpriteRenderer>();
        empty = Resources.Load("box", typeof(Sprite)) as Sprite;
        circled = Resources.Load("boxCircled", typeof(Sprite)) as Sprite;
        crossed = Resources.Load("boxCrossed", typeof(Sprite)) as Sprite;
    }

    void OnMouseDown() {
        TouchBox();
    }

    public void Reset() {
        touched = false;
        myRenderer.sprite = empty;
    }

    public void TouchBox() {
        if (!touched && isPlayable) {
            myRenderer.sprite = circled;
            touched = true;
            manager.PlayerTouch(this.gameObject);
            audioSrc.PlayOneShot(touch, 0.4F);
        } else {
            print("this box has been touched already");
            audioSrc.PlayOneShot(cantTouch, 0.4F);
        }
    }

    public void ComputerTouch() {
        touched = true;
        myRenderer.sprite = crossed;
    }
}
