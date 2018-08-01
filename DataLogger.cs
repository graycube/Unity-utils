using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;


public class DataLogger : MonoBehaviour {

    public bool doLog = true;
    public string filterString = "";
    private string logSavePath;
    //private List<SensorData> logList;
    private List<string> logList;


    void Start()
    {
        logList = new List<string>();
        logSavePath = Application.persistentDataPath + "/log";
        Application.logMessageReceived += HandleLog;
    }

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }


    [Serializable]
    public class SensorData
    {
        public readonly float ktimestamp;
        public readonly Quaternion kquaternion;
        public readonly Vector3 kposition;
        public readonly float pos_x;
        public readonly float pos_y;
        public readonly float pos_z;
        public readonly float qx;
        public readonly float qy;
        public readonly float qz;
        public readonly float qw;


        public SensorData(float ktimestamp, Quaternion kquaternion, Vector3 kposition)
        {
            this.ktimestamp = Time.realtimeSinceStartup;
            this.kquaternion = kquaternion;
            this.kposition = kposition;
            this.pos_x = kposition.x;
            this.pos_y = kposition.y;
            this.pos_z = kposition.z;
            this.qw = kquaternion.w;
            this.qx = kquaternion.x;
            this.qy = kquaternion.y;
            this.qz = kquaternion.z;

        }

         // hide default ToString() from base object class (extend with override)
        public new string ToString()
        {
            string sep = ",";
            //string line = "\n";
            string str = this.ktimestamp.ToString() + sep + this.pos_x.ToString()
                + sep + this.pos_y.ToString() + sep + this.pos_z.ToString() + sep
                + this.qw.ToString() + sep + this.qx.ToString() + sep + this.qy.ToString()
                + sep + this.qz.ToString(); // + line;
            return str;
        }

    }


    void HandleLog(string logString, string stackTrace, LogType type)
    {
        //using (StreamWriter writer = new StreamWriter(logSavePath, true, Encoding.UTF8))
        //{
        //    writer.WriteLine(logString);
        //}

        logList.Add(logString);
    }

	void Update()
    {
        if (Input.GetKeyDown("space")) {
            doLog = !doLog;
            Application.Quit();
        }

        float timeNow = Time.realtimeSinceStartup;
        SensorData updateData = new SensorData(timeNow, transform.rotation, transform.position);

        if (doLog)
        {
            string OutputString = updateData.ToString();
            //using(StreamWriter writer = new StreamWriter(logSavePath, true, Encoding.UTF8))
            //{
            //    writer.WriteLine(OutputString);
            HandleLog(OutputString,StackTraceUtility.ExtractStackTrace(), LogType.Log);
                
                //Application.logMessageReceived += HandleLog;
            //}
            //Debug.Log(timeNow.ToString() + " , " + transform.position.x.ToString() + " , " + transform.position.y.ToString() + " , " + transform.position.z.ToString() + " , " + transform.rotation.w.ToString() + " , " + transform.rotation.x.ToString() + " , " + transform.rotation.y.ToString() + " , " + transform.rotation.z.ToString());
            
            //doLog = !doLog;
        }
	}

    void OnApplicationQuit()
    {
        Application.logMessageReceived -= HandleLog;
        Application.OpenURL(Application.persistentDataPath);

        using (StreamWriter writer = new StreamWriter(logSavePath, true, Encoding.UTF8))
        {
            writer.WriteLine("-------------" +
               System.DateTime.Now + "------------");
            foreach (var item in logList)
            {
                writer.WriteLine(item);
            }
        }
        
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }
}
