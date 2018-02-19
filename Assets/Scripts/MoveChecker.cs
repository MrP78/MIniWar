using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveChecker : MonoBehaviour {
    public GameObject box;
    public Vector3[] PosArray;
    public List<Vector3> Free;
    public List<Vector3> Occupied;
    
    void Start () {
        Free = new List<Vector3>();
        Occupied = new List<Vector3>();
    }	
    public void CheckAvaliable(Vector3 CharPos, int numSquares) ///checks all the squares around character = to the number passed through
    {
        ClearFree();
        //print("Checking Moves " + CharPos);
        int N = ((numSquares * 2) + 1);
        int size = N*N; // works out the size of the array
        
        PosArray = new Vector3 [size];
        
        //Free = new List<Vector3>(); // makes a list of all these spaces that will evntually be the list of unocuppied 
        //Occupied = new List<Vector3>();
        for (int i = 0; i < PosArray.Length;) { 
            for (int x = -numSquares; x <= numSquares; x++)
            {
                for (int y = -numSquares; y <= numSquares; y++)
                {                   
                    PosArray[i] = new Vector3(CharPos.x + x, CharPos.y + y,0);/// adds all positions to the pos array
                    if ((PosArray[i].x > 0 && PosArray[i].x < 15) && PosArray[i].y > 0 && PosArray[i].y < 9) /// making sure its inside the game area
                    {
                        Free.Add(PosArray[i]); // adds all the positions to the free list
                        
                    }
                    Free.Remove(CharPos);/// removes our own position
                    i++;
                }             
            }
        }       
        foreach (Vector3 posArray in PosArray)
        {
            Player[] charArray = GameObject.FindObjectsOfType<Player>();/// finds all the characters on the map            
            foreach (Player Char in charArray)// checks through all the charachters
            {
                if (Char.transform.position != CharPos) //that isnt me 
                {
                    if (posArray != CharPos) //Makes sure that we aren't checking our self
                    {
                        if (posArray == Char.transform.position)// if a position in the array = a charachter position 
                        {                            
                            Free.Remove(posArray);
                            Occupied.Add(posArray); /// removes the position from the freelist adds it to the occ 
                            
                               
                        }
                        if ((Char.transform.position.y < CharPos.y)&& posArray.y < Char.transform.position.y)
                        {
                            Free.Remove(posArray);
                        }
                        if ((Char.transform.position.y > CharPos.y) && posArray.y > Char.transform.position.y)
                        {
                            Free.Remove(posArray);
                        }
                        if ((Char.transform.position.x < CharPos.x) && posArray.x < Char.transform.position.x)
                        {
                            Free.Remove(posArray);
                        }
                        if ((Char.transform.position.x > CharPos.x) && posArray.x > Char.transform.position.x)
                        {
                            Free.Remove(posArray);
                        }
                        
                    }
                }
            }
        }
    }
    public void ShowSquares() {
        DestroySquares();
        foreach (Vector3 free in Free)
        {
           if((free.x < 1 || free.x >14)||(free.y >1 || free.y < 8)) { }
            Instantiate(box, free, Quaternion.identity);            
        }
    }
    public void DestroySquares()
    {
        GameObject[] boxes = GameObject.FindGameObjectsWithTag("box");
        foreach (GameObject box in boxes)
        {
            Destroy(box);
        }
    }
    public bool IsFree(float X, float Y)/// checks if a given position is in freelist
    {
        foreach(Vector3 free in Free)
        {
            if((X  == free.x) && (Y == free.y))
            {
                return true;
            }
           
        }
        return false;
    }

    public bool IsOccupied(float X, float Y)
    {
        foreach (Vector3 Occ in Occupied)
        {
            if ((X == Occ.x && Y == Occ.y))
            {
               
                return true;
            }
            if(Occupied.Count == 0)
            {
                return false;
            }

        }
        return false;
    }

    public void ClearFree()
    {
        Free.Clear();
        Occupied.Clear();
    }
}
