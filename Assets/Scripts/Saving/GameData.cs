using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData : MonoBehaviour
{

    public Vector3 playerPosition;
    public GameData ()
    {
        playerPosition = Vector3.zero;
    }
}
