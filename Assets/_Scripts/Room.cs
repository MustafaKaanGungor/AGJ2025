using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Direction nextDirection;
    public Room nextRoom;
    private bool doesHaveEnemy;
    private List<Enemy> enemiesInside = new();

    public void AddEnemyToArray(Enemy enemy) {
        enemiesInside.Add(enemy);
        doesHaveEnemy = true;
    }

    public void RemoveEnemyFromArray(Enemy enemy) {
        enemiesInside.Remove(enemy);
        if(enemiesInside.Count <= 0) {
            doesHaveEnemy = false;
        }
    }

    public bool CheckDoesHaveEnemy() {
        return doesHaveEnemy;
    }

    public Enemy GetEnemy() {
        return enemiesInside[0];
    }

    public int HowManyEnemiesRoomHas() {
        return enemiesInside.Count;
    }
}

public enum Direction {
    RIGHT,
    LEFT,
    STRAIGHT
}
