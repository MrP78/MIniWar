using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    TurnOver turnOver;
    GameObject[] players1;
    GameObject[] players2;
    public Player currentPlayer;
    public List<Enemy> enemys;
    float[] Distance;
    Menu ActionMenu;
    GameSpace gameSpace;
    public bool play = true;
    public bool moving = false;
    Vector2 currentPos;
    public int j = 0;
    Vector3 currentPlayerPosition;
    MoveChecker moveChecker;
    Vector2 proposedPos;
   
    void Start()
    {
        turnOver = GetComponent<TurnOver>();
        gameSpace = GameObject.FindObjectOfType<GameSpace>();
        ActionMenu = GameObject.FindObjectOfType<Menu>();
        moveChecker = GameObject.FindObjectOfType<MoveChecker>();
    }
    
    void Update()
    {
       if(turnOver.activePlayer == 2 && play == true)
        {
            RunAi();            
        }
        if (turnOver.activePlayer == 1)
        {
            play = true;
        }
        if(currentPlayer != null)
       {
           if (currentPlayer.GetComponent<Player>().done)
           {
                currentPlayer = null;
                j++;
                RunAi();
          }
        }
    }

    void RunAi()
    {
        play = false;
        players1 = GameObject.FindGameObjectsWithTag("Player1");
        players2 = GameObject.FindGameObjectsWithTag("Player2");

        if (j >= players2.Length)
        {
            StartCoroutine("Turnit");
            return;
        }

        currentPlayer = players2[j].GetComponent<Player>();
        currentPlayerPosition = currentPlayer.transform.position;

        FindClosestEnemy();
        CalculatePositionToMoveTo();
        
        
    }

    private void AttackEnemy()
    {
        if (currentPlayer.IsMoving == false)
        {
            gameSpace.roundedPos = enemys[0].enemyObject.transform.position;

            ActionMenu.MenuOptions(2);
            enemys[0].enemyObject.GetComponent<Player>().InactiveTurnActions();
        }
    }

    private void CalculatePositionToMoveTo()
    {
        moveChecker.ClearFree();
        moveChecker.CheckAvaliable(currentPlayerPosition, 1);

        if (moveChecker.IsOccupied(enemys[0].enemyObject.transform.position.x, enemys[0].enemyObject.transform.position.y))
        {
            proposedPos = new Vector2(currentPlayer.transform.position.x,currentPlayer.transform.position.y);
        }
        else if (((int)currentPlayerPosition.x + currentPlayer.moves) + currentPlayer.GetComponent<CharacterAtributes>().range >= enemys[0].enemyObject.transform.position.x)
        {
            moveChecker.ClearFree();
            moveChecker.CheckAvaliable(currentPlayerPosition, currentPlayer.moves);
            
            int b = ((int)enemys[0].enemyObject.transform.position.x - (int)currentPlayerPosition.x) - currentPlayer.GetComponent<CharacterAtributes>().range;
            proposedPos = new Vector2(currentPlayerPosition.x + b, enemys[0].enemyObject.transform.position.y);
            //if((proposedPos.x == currentPlayerPosition.x)&& enemys[0].enemyObject.transform.position.y == currentPlayerPosition.y)
            //{
             //   proposedPos = new Vector2(currentPlayerPosition.x, currentPlayerPosition.y);
           // }
        }
        else
        {            
            moveChecker.ClearFree();
            moveChecker.CheckAvaliable(currentPlayerPosition, currentPlayer.moves);
            proposedPos = new Vector2(currentPlayerPosition.x + currentPlayer.moves, enemys[0].enemyObject.transform.position.y);
           // print(currentPlayer.name + " " + enemys[0].enemyName);
        }
        MoveToNewPosition();       
    }

    private void MoveToNewPosition()
    {
        currentPlayer.IsMoving = true;
        proposedPos = new Vector2(Mathf.Clamp(proposedPos.x, 1, 14), Mathf.Clamp(proposedPos.y, 1, 8));
        while (!CheckPropPos())
        {
           CheckPropPos();
        }
        int m;
        int n1 = Mathf.Abs((int)currentPlayer.transform.position.x - (int)proposedPos.x);
        int n2 = Mathf.Abs((int)currentPlayer.transform.position.y - (int)proposedPos.y);
        if (n1 > n2)
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
        currentPlayer.moves -= m;
        
        if(m == 0)
        {
            currentPlayer.moves = 0;
        }
        Vector2 currentPos3 = new Vector2(currentPlayer.transform.position.x, currentPlayer.transform.position.y);
        if (proposedPos != currentPos3)
        {
            currentPlayer.transform.position = proposedPos;
            float toller = 0.02f;
            float X = Mathf.Abs(currentPlayer.transform.position.x - proposedPos.x);
            float Y = Mathf.Abs(currentPlayer.transform.position.y - proposedPos.y);

            if (X < toller)
            {
                if (Y < toller)
                {
                    if (currentPlayer.transform.position.x == proposedPos.x)
                    {
                        currentPlayer.IsMoving = false;

                    }
                }
            }
        }
        currentPlayer.IsMoving = false;
        if (currentPlayer.moves > 0)
        {
            CalculatePositionToMoveTo();            
        }
        if (currentPlayer.moves == 0)
        {
            AttackEnemy();
            currentPlayer.done = true;
            //enemys.Clear();
        }
    }

    private void FindClosestEnemy()
    {
        enemys = new List<Enemy>();
        for (int i = 0; i < players1.Length; i++)
        {
            enemys.Add(new Enemy(players1[i], players1[i].name, Mathf.Abs(((int)players1[i].transform.position.x - (int)currentPlayerPosition.x)) + Mathf.Abs(((int)players1[i].transform.position.y - (int)currentPlayerPosition.y))));
        }
        enemys.Sort();
        currentPlayer.ActiveTurnActions();
    }

    bool CheckPropPos() {
        Vector2 currentPos2 = new Vector2(currentPlayerPosition.x, currentPlayerPosition.y);
        if (proposedPos != currentPos2)
        {
            moveChecker.ClearFree();
            moveChecker.CheckAvaliable(currentPlayer.transform.position, currentPlayer.moves);           

            if (moveChecker.IsOccupied(proposedPos.x, proposedPos.y))
            {               
                int r = Random.Range(-1, 2);
                proposedPos = new Vector2(proposedPos.x, enemys[0].enemyObject.transform.position.y + r);
                return false;
            }
            else
            {
                return true;
            }
        }
        return true;
    }

    IEnumerator Turnit() {
        yield return new WaitForSeconds(0.5f);
        enemys.Clear();
        turnOver.TurnOvers();
    }
}