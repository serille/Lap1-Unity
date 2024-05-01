using System;
using System.Collections;
using System.Collections.Generic;

public sealed class ScoreTracker
{
    // Using C#'s Lazy should make the singleton threadsafe (probably not needed here but w/e)
    private static readonly Lazy<ScoreTracker> lazy = new Lazy<ScoreTracker>(() => new ScoreTracker());

    public static ScoreTracker Instance {  get {  return lazy.Value; } }

    public const int playerCount = 4;

    private int[,] _scores;

    private ScoreTracker()
    {
        _scores = new int[playerCount,playerCount];
        ResetScores();
    }

    public void ResetScores()
    {
        for (int i = 0; i < playerCount; ++i)
        {
            for (int j = 0; j < playerCount; ++j)
            {
                if (i == j)
                {
                    _scores[i, j] = -1;
                    continue;
                }
                _scores[i, j] = 0;
            }
        }
    }

    public int GetScore(int playerNum)
    {
        if (playerNum >= playerCount)
        {
            return -1;
        }
        // Start from 1 to compensate Score for that same player being -1
        int totalScore = 1;
        for (int i = 0; i < playerCount; ++i)
        {
            totalScore += _scores[playerNum, i];
        }

        return totalScore;
    }

    public int GetScore(int playerNum, int targetNum)
    {
        if (playerNum >= playerCount || targetNum >= playerCount)
        {
            return -1;
        }
        return _scores[playerNum, targetNum];
    }

    public void AddScore(int playerNum, int targetNum)
    {
        if (playerNum >= playerCount || targetNum >= playerCount)
        {
            return;
        }
        _scores[playerNum, targetNum] += 1;
    }
}
