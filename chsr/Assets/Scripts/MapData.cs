using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MapData
{
    [SerializeField] public int turn;
    [SerializeField] public String player  = default;
    [SerializeField] public String code = default;
    [SerializeField] public Score score = default;
    [SerializeField] public IntArray[] diff = default;
    [SerializeField] public IntArray[] map = default;

    public MapData()
    {
        turn = 0;
    }

    
    public static IntArray[] Difference(IntArray[] before, IntArray[] after)
    {
    IntArray[] diff = new IntArray[before.Length];

    for(int x = 0; x < 19; x++)
    {
        diff[x] = new IntArray(17);
        for(int y = 0; y < 17; y++)
        {
            if(before[x].array[y] == after[x].array[y]) diff[x].array[y] = -1;
            else diff[x].array[y] = after[x].array[y];
        }
    }

    return(diff);
    }
}

[Serializable]
public class Score
{
    [SerializeField] public int hot;
    [SerializeField] public int cool; 
}

[Serializable]
public class IntArray
{
    [SerializeField] public int[] array;

    public IntArray(int n)
    {
        this.array = new int[n];
    }
}
