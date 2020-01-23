using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UniRx;

using Player;

namespace Field
{
    public class Fieldfactory : MonoBehaviour
    {
        [SerializeField] private GameObject wallPrefab = default;
        [SerializeField] private GameObject blockPrefab = default;
        [SerializeField] private GameObject itemPrefab = default;

        [SerializeField] private PlayerController hotPlayer = default;
        [SerializeField] private PlayerController coolPlayer = default;

        public Text Turn_obj;
        public Text Hot_obj;
        public Text Cool_obj;
        GameObject[,] Map_obj = new GameObject[19, 17];

        MapData before_map = null;

        private MapData TextToMapData(string text)
        {
            try
            {
                text = text.Replace("[[", "[{\"array\":[");
                text = text.Replace("],[", "]},{\"array\":[");
                text = text.Replace("]]", "]}]");
                string[] s = text.Split('\n');

                return (MapData)JsonUtility.FromJson(s[0], typeof(MapData));
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
                return null;
            }
        }

        public void GenerateField(string text)
        {
            MapData map = TextToMapData(text);
            if (map == null) return;

            IntArray[] m;

            if (before_map == null)
                m = map.map;
            else
                m = MapData.Difference(before_map.map, map.map);

            Turn_obj.text = map.turn.ToString();
            Hot_obj.text = "Hot : " + map.score.hot;
            Cool_obj.text = "Cool : " + map.score.cool;
            
            for (int x = 0; x < 19; x++)
            {
                for (int y = 0; y < 17; y++)
                {
                    if (m[x].array[y] != -1 && Map_obj[x, y] != null)
                    {
                        Destroy(Map_obj[x, y]);
                    }

                    if (m[x].array[y] == 2)
                    {
                        if (map.turn == -1)
                            Map_obj[x, y] = Instantiate(wallPrefab, new Vector3(x, 0, y), Quaternion.identity, transform);
                        else
                            Map_obj[x, y] = Instantiate(blockPrefab, new Vector3(x, 0, y), Quaternion.identity, transform);
                    }
                    else if (m[x].array[y] == 3)
                    {
                        Map_obj[x, y] = Instantiate(itemPrefab, new Vector3(x, 0, y), Quaternion.identity, transform);
                    }
                    else if (m[x].array[y] == 4)
                    {
                        // 4 is Move CoolPlayer
                        coolPlayer.Move(new Vector3(x, 0, y));
                    }
                    else if (m[x].array[y] == 5)
                    {
                        // 5 is Move CoolPlayer
                        hotPlayer.Move(new Vector3(x, 0, y));
                    }
                }
            }
            before_map = map;
        }

    }
}