using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameData
{
    public static bool[] playersInGame = new bool[4] { false, false, false, false};
    public static GameObject[] playerObjects = new GameObject[4] { null, null, null, null };

    public static float musicVolume = 0.5f;
    public static float sfxVolume = 0.5f;
    public static float fliesVolume = 0.33f;
}
