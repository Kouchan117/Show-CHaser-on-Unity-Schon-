using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private GameObject MainCamera;
    [SerializeField] private GameObject HotCamera;
    [SerializeField] private GameObject CoolCamera;
    [SerializeField] private GameObject CameraA;
    [SerializeField] private GameObject CameraB;
    [SerializeField] private GameObject CameraC;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 CamA = GameObject.Find("CameraA").transform.position;
        Vector3 CamB = GameObject.Find("CameraB").transform.position;
        Vector3 CamC = GameObject.Find("CameraC").transform.position;
        Vector3 CamHot = GameObject.Find("HotPlayer").transform.position;
        Vector3 CamCool = GameObject.Find("CoolPlayer").transform.position;

        Quaternion QuatA = GameObject.Find("CameraA").transform.rotation;
        Quaternion QuatB = GameObject.Find("CameraB").transform.rotation;
        Quaternion QuatC = GameObject.Find("CameraC").transform.rotation;
        Quaternion QuatHot = GameObject.Find("HotPlayer").transform.rotation;
        Quaternion QuatCool = GameObject.Find("CoolPlayer").transform.rotation;
        
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            MainCamera.transform.position = new Vector3(CamA.x, CamA.y, CamA.z);
            MainCamera.transform.rotation = Quaternion.Euler(QuatA.eulerAngles.x, QuatA.eulerAngles.y, QuatA.eulerAngles.z);
        }   

        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            MainCamera.transform.position = new Vector3(CamB.x, CamB.y, CamB.z);
            MainCamera.transform.rotation = Quaternion.Euler(QuatB.eulerAngles.x, QuatB.eulerAngles.y, QuatB.eulerAngles.z);
        }  

        else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            MainCamera.transform.position = new Vector3(CamC.x, CamC.y, CamC.z);
            MainCamera.transform.rotation = Quaternion.Euler(QuatC.eulerAngles.x, QuatC.eulerAngles.y, QuatC.eulerAngles.z);
        }

        else if(Input.GetKeyDown(KeyCode.H))
        {
            MainCamera.transform.position = new Vector3(CamHot.x, CamHot.y+1.7f, CamHot.z);
            MainCamera.transform.rotation = Quaternion.Euler(QuatHot.eulerAngles.x+75, QuatHot.eulerAngles.y, QuatHot.eulerAngles.z);
        }

        else if(Input.GetKeyDown(KeyCode.C))
        {
            MainCamera.transform.position = new Vector3(CamCool.x, CamCool.y+1.7f, CamCool.z);
            MainCamera.transform.rotation = Quaternion.Euler(QuatCool.eulerAngles.x+75, QuatCool.eulerAngles.y, QuatCool.eulerAngles.z);
        }
    }
}
