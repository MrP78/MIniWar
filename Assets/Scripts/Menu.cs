using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour {
    private Player SelectedPlayer;
    private Dropdown menu;
    // Use this for initialization
    void Start () {
        menu = GetComponentInChildren<Dropdown>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void SetPlayer(Player player)
    {
        SelectedPlayer = player;
        print("Selected Player" + player.name);
    }
    public Player GetPlayer()
    {
        return SelectedPlayer;
    }
    public void MenuOptions(int n)
    {
        if (n == 1)
        {
            SelectedPlayer.Move();
            SelectedPlayer.isAttacking = false;
            return;
        }
        if (n == 2)
        {
            
            SelectedPlayer.isAttacking = false;
            SelectedPlayer.DestroySquares();
            SelectedPlayer.IsMoving = false;
            if (!SelectedPlayer.AttackUsed)
            {
               
                SelectedPlayer.isAttacking = true;
                
                SelectedPlayer.DrawMenu();
                SelectedPlayer.ByeMenu();
            }
            
            //SelectedPlayer.AttackUsed = true;
             //menu.value = -1;
            
            

        }


    }
    public void Attack()
    {
        if (!SelectedPlayer.AttackUsed)
        {
           
            SelectedPlayer.Attack();
            //SelectedPlayer.AttackUsed = true;
            SelectedPlayer.DrawMenu();
            SelectedPlayer.ByeMenu();
        }
        
        
        
    }
}
