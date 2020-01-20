using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SwipeDirection { Left, Right, Up, Down }

public class SwipeHandler : MonoBehaviour
{
    private Vector2 fingerDown;
    private Vector2 fingerUp;

    public float swipeMinDistThreshhold = 15f;

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.gameState == GameManager.GameState.Playing)
        {

            ProcessInput();
        }
    }

    private void ProcessInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            fingerDown = Input.mousePosition;
            fingerUp = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            fingerUp = Input.mousePosition;
            checkSwipe();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            OnSwipeUp();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            OnSwipeDown();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            OnSwipeRight();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            OnSwipeLeft();
        }
    }

    void checkSwipe()
    {
        //Check if Vertical swipe
        if (verticalMove() > swipeMinDistThreshhold && verticalMove() > horizontalValMove())
        {
            //Debug.Log("Vertical");
            if (fingerUp.y - fingerDown.y > 0)//up swipe
            {
                OnSwipeUp();
            }
            else if (fingerUp.y - fingerDown.y < 0)//Down swipe
            {
                OnSwipeDown();
            }
            fingerUp = fingerDown;
        }

        //Check if Horizontal swipe
        else if (horizontalValMove() > swipeMinDistThreshhold && horizontalValMove() > verticalMove())
        {
            //Debug.Log("Horizontal");
            if (fingerUp.x - fingerDown.x > 0)//Right swipe
            {
                OnSwipeRight();
            }
            else if (fingerUp.x - fingerDown.x < 0)//Left swipe
            {
                OnSwipeLeft();
            }
            fingerUp = fingerDown;
        }

        //No Movement at-all
        else
        {
            //Debug.Log("No Swipe!");
        }
    }

    float verticalMove()
    {
        return Mathf.Abs(fingerUp.y - fingerDown.y);
    }

    float horizontalValMove()
    {
        return Mathf.Abs(fingerUp.x - fingerDown.x);
    }

    //Callback Methods
    void OnSwipeUp()
    {
        GameManager.Instance.ProcessSwipe(SwipeDirection.Up);
    }

    void OnSwipeDown()
    {
        GameManager.Instance.ProcessSwipe(SwipeDirection.Down);
    }

    void OnSwipeLeft()
    {
        GameManager.Instance.ProcessSwipe(SwipeDirection.Left);
    }

    void OnSwipeRight()
    {
        GameManager.Instance.ProcessSwipe(SwipeDirection.Right);
    }

}
