using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UniRx;

using Player;

public class Fieldfactory : MonoBehaviour
{
    public GameObject Cube;
    public GameObject Cone;

    [SerializeField] private PlayerController hotPlayer;
    [SerializeField] private PlayerController coolPlayer;

    public Text Turn_obj;
    public Text Hot_obj;
    public Text Cool_obj;
    GameObject[,] Map_obj = new GameObject[19,17];
    
    MapData map = new MapData();
    MapData before_map = null;

    public void GenerateField(string clientmsg)
    {
        clientmsg = clientmsg.Replace("[[", "[{\"array\":[");
        clientmsg = clientmsg.Replace("],[", "]},{\"array\":[");
        clientmsg = clientmsg.Replace("]]", "]}]");
        string[] s = clientmsg.Split('\n');
        int x, y;
        try
        {
            map = (MapData)JsonUtility.FromJson(s[0], typeof(MapData));
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
            return;
        }

        IntArray[] m;

        if (before_map == null) m = map.map;
        else m = MapData.Difference(before_map.map, map.map);
        Turn_obj.text = "Turn : " + map.turn;
        Hot_obj.text = "Hot : " + map.score.hot;
        Cool_obj.text = "Cool : " + map.score.cool;


        for (x = 0; x < 19; x++)
        {
            for (y = 0; y < 17; y++)
            {
                if (m[x].array[y] != -1 && Map_obj[x, y] != null)
                {
                    GameObject.Destroy(Map_obj[x, y]);
                }

                if (m[x].array[y] == 2)
                {
                    Map_obj[x, y] = Instantiate(Cube, new Vector3(x, 0.5f, y), Quaternion.identity);
                }
                else if (m[x].array[y] == 3)
                {
                    Map_obj[x, y] = Instantiate(Cone, new Vector3(x, 0.5f, y), Quaternion.identity);
                }
                else if (m[x].array[y] == 4)
                {
                    // 4 is Move CoolPlayer
                    coolPlayer.Move(new Vector3(x, 1, y));
                }
                else if (m[x].array[y] == 5)
                {
                    // 5 is Move CoolPlayer
                    hotPlayer.Move(new Vector3(x, 1, y));
                }
            }
        }
        before_map = map;
    }
    
}
