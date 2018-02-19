using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSpace : MonoBehaviour {

    public Camera myCamera;
    public Vector2 roundedPos;
    public bool moving = false;

    MoveChecker moveChecker;
    Player[] players;
    Player currentPlayer;
    Vector3 currentPlayerPosition;

    


    void Start()
    {
        moveChecker = GameObject.FindObjectOfType<MoveChecker>();
        players = GameObject.FindObjectsOfType<Player>();
       

    }    
    void Update()
    {
        if (moving)
        {
            Moving();
        }
    }
    void OnMouseDown()
    {
        Vector2 rawPos = CalculateWorldPointOfMouseClick();
        roundedPos = SnapToGrid(rawPos);
        
        foreach(Player player in players)
        {
            if (player.IsMoving)
            {
                currentPlayer = player;
                currentPlayerPosition = player.transform.position;
                if (moveChecker.IsFree(roundedPos.x, roundedPos.y))

                {
                    int m;
                    int n1 = Mathf.Abs((int)player.transform.position.x - (int)roundedPos.x);
                    int n2 = Mathf.Abs((int)player.transform.position.y - (int)roundedPos.y);
                    if(n1 > n2)
                    {
                        m = n1;
                    }
                    else if (n1 < n2)
                    {
                        m = n2;
                    }
                    else
                    {
                        m = n2;
                    }                    
                    moving = true;
                    player.SetAniBool("isWalking", true);             
                    player.moves-=m;                    
                    moveChecker.ClearFree();
                    GameObject[] boxes = GameObject.FindGameObjectsWithTag("box");
                    foreach (GameObject box in boxes)
                    {
                        Destroy(box);
                    }
                }
            }
        }
    }
    public void Moving()
    {        
        currentPlayer.transform.position = Vector3.MoveTowards(currentPlayerPosition, roundedPos, currentPlayer.speed * Time.deltaTime);
        currentPlayerPosition = currentPlayer.transform.position;       
        float toller = 0.02f;
        float X = Mathf.Abs(currentPlayer.transform.position.x - roundedPos.x);
        float Y = Mathf.Abs(currentPlayer.transform.position.y - roundedPos.y);
        if (X  < toller)
        {     
            if(Y < toller)
            {
                currentPlayer.transform.position = roundedPos;
                print("player in toller");
                moving = false;                
                currentPlayer.SetAniBool("isWalking", false);               
                if (currentPlayer.moves > 0)
                {
                    currentPlayer.Move();
                }
                if (currentPlayer.moves < 1)
                {
                    currentPlayer.IsMoving = false;
                    currentPlayer.DrawMenu();
                    currentPlayer.ByeMenu();
                }
            }        

        }
    }

   
    Vector2 SnapToGrid(Vector2 rawWorldPos)
    {

        float newX = Mathf.RoundToInt(rawWorldPos.x);
        float newY = Mathf.RoundToInt(rawWorldPos.y);
        return new Vector2(newX, newY);
    }

    Vector2 CalculateWorldPointOfMouseClick()
    {
        float mouseX = Input.mousePosition.x;
        float mouseY = Input.mousePosition.y;
        float distanceFromCamera = 10f;

        Vector3 weirdTriplet = new Vector3(mouseX, mouseY, distanceFromCamera);
        Vector2 worldPos = myCamera.ScreenToWorldPoint(weirdTriplet);

        return worldPos;
    }   
}
