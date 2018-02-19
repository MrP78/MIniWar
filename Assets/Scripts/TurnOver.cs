using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnOver : MonoBehaviour {
    InformationGatherer gatherer;

    public int turnNumber = 0;

    public int activePlayer;

    private GameObject coinToss;
    private GameObject messageBox;
    

    private void Start()
    {
        gatherer = GetComponent<InformationGatherer>();
        coinToss = GameObject.Find("CoinToss");
        messageBox = GameObject.Find("MessageBox");
        

        messageBox.SetActive(false);
    }

    public void TurnOvers()
    {
        turnNumber++;
        gatherer.ClearInfo();
        if (activePlayer == 1)
        

        {

            GameObject[] players1 = GameObject.FindGameObjectsWithTag("Player1");
            foreach (GameObject player in players1)
            {
                player.GetComponent<Player>().ResetMoves();
                player.GetComponent<Player>().turn = false;

            }
            GameObject[] players2 = GameObject.FindGameObjectsWithTag("Player2");
            foreach (GameObject player in players2)
            {
                player.GetComponent<Player>().turn = true;
            }
            activePlayer = 2;
            SetMessage("Player 2's turn", 0.75f);
            return;
        }
        if (activePlayer == 2)
        {

            GameObject[] players1 = GameObject.FindGameObjectsWithTag("Player1");
            foreach (GameObject player in players1)
            {
                
                player.GetComponent<Player>().turn = true;
            }
            
            GameObject[] players2 = GameObject.FindGameObjectsWithTag("Player2");
            foreach (GameObject player2 in players2)
            {
                player2.GetComponent<Player>().ResetMoves();
                player2.GetComponent<Player>().turn = false;
                activePlayer = 1;
                SetMessage("Player 1's turn", 0.75f);
                
            }
            return;
        }
    }
    public void CoinToss()
    {
        
        gatherer.ClearInfo();
        activePlayer = Random.Range(1, 3);
        TurnOvers();
        coinToss.SetActive(false);
        SetMessage(("Player " + activePlayer + " goes first"), 2);


    }
    public void SetMessage(string message, float N)
    {
        messageBox.SetActive(true);
        messageBox.GetComponent<Text>().text = (message);
        Invoke("ByeMessageBox", N);

    }

    public void ByeMessageBox()
    {
        messageBox.SetActive(false);
    }
}
