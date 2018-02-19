using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : IComparable<Enemy>
{
    public GameObject enemyObject;
    public string enemyName;
    public int enemyDistance;

	public Enemy(GameObject newEnemyObject, string newEnemyName, int newEnemyDistance)
    {
        enemyObject = newEnemyObject;
        enemyName = newEnemyName;
        enemyDistance = newEnemyDistance;
    }
    public int CompareTo(Enemy other)
    {
        if(other == null)
        {
            return 1;
        }
        return enemyDistance - other.enemyDistance;
    }
}
