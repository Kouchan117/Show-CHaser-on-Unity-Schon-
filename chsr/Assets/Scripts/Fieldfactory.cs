using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UniRx;



public class Fieldfactory : MonoBehaviour
{
    public GameObject Cube;
    public GameObject Cone;
    public GameObject Hot;
    public GameObject Cool;
    public Text Turn_obj;
    public Text Hot_obj;
    public Text Cool_obj;
    GameObject[,] Map_obj = new GameObject[19,17];
    

    //private string str = File.ReadAllText("C:\\Users\\koushi\\Documents\\lab\\CHaserLog-master\\CH-20181108214143.json");
    MapData map = new MapData();
    MapData before_map = null;
    // Start is called before the first frame update
    
    public void Start()
    {
        /* 中間発表時での進捗 */ 
        /* Start()で1回読んで１文読むのが限界でした */
        /*
        //GenerateField(str);
        str = str.Replace("[[", "[{\"array\":[");
        str = str.Replace("],[", "]},{\"array\":[");
        str = str.Replace("]]", "]}]");
        string[] s = str.Split('\n');

            int x, y;
            map = (MapData)JsonUtility.FromJson(s[0], typeof(MapData));

            for(x = 0; x < 19; x++)
            {
                for(y = 0; y < 17; y++)
                {
                    if(map.map[x].array[y] == 2)
                    {
                        Instantiate(Cube, new Vector3(x, 0.5f, y), Quaternion.identity);
                    }
                    else if(map.map[x].array[y] == 3)
                    {
                        GameObject obj = Instantiate(Cone, new Vector3(x, 0.5f, y), Quaternion.identity);
                    }
                }
            }
            */
            //GenerateField(test);
    }

    public void GenerateField(string clientmsg)
    {
        Debug.Log(System.Threading.Thread.CurrentThread.ManagedThreadId);
        clientmsg = clientmsg.Replace("[[", "[{\"array\":[");
        clientmsg = clientmsg.Replace("],[", "]},{\"array\":[");
        clientmsg = clientmsg.Replace("]]", "]}]");
        string[] s = clientmsg.Split('\n');
        int x, y;
        try
        {
            map = (MapData)JsonUtility.FromJson(s[0], typeof(MapData));
            Debug.Log(map.map);
        }
        catch(Exception ex)
        {
            Debug.Log(ex);
             return;
        }

        IntArray[] m;

        if(before_map == null) m = map.map;
        else m = MapData.Difference(before_map.map, map.map);
        Turn_obj.text = "Turn : " + map.turn;
        Hot_obj.text = "Hot : " + map.score.hot;
        Cool_obj.text = "Cool : " + map.score.cool;

            
        for(x = 0; x < 19; x++)
        {
            for(y = 0; y < 17; y++)
            {
                if (m[x].array[y] != -1 && Map_obj[x,y] != null)
                {
                    GameObject.Destroy(Map_obj[x,y]);
                }

                   if(m[x].array[y] == 2)
                   {
                     //Debug.Log("2");
                     Map_obj[x,y] = Instantiate(Cube, new Vector3(x, 0.5f, y), Quaternion.identity);
                 }
                   else if(m[x].array[y] == 3)
                {
                    //Debug.Log("3");
                    Map_obj[x,y] = Instantiate(Cone, new Vector3(x, 0.5f, y), Quaternion.identity);
                }
                   else if(m[x].array[y] == 4)
                   {
                    Cool.transform.position = new Vector3(x, 1, y);
                }
                else if(m[x].array[y] == 5)
                 {
                    Hot.transform.position = new Vector3(x, 1, y);
                }
               }
        }
           before_map = map;    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
