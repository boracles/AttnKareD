using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

namespace BNG
{
    public class CollectDataAndPlot : MonoBehaviour
    {
        // Add Objects in Inspector
        [Header("Objects in Game")]
        [SerializeField] [Tooltip("XR Rig or XR Rig Advanced goes here")] Transform XRRig;
        [SerializeField] [Tooltip("CenterEyeAnchor goes here")] Transform Camera;
        [SerializeField] [Tooltip("LeftControllerAnchor goes here")] Transform LHand;
        [SerializeField] [Tooltip("RightControllerAnchor goes here")] Transform RHand;
        
        private Stats database; // All data is stored in this object
        private InputBridge _inputBridge; // XR Rig Input Bridge (C# Script)

        // Fields needed for Trigger Input (Do Not Remove)
        float RTriggerState = 0;
        float LTriggerState = 0;

        // For Checking in Inspector 
        [Header("Controller Button Debug Panel")]
        public int _LTriggerClicks;
        public float _LTrigger;
        public int _RTriggerClicks;
        public float _RTrigger;
        public int _LGripClicks;
        public float _LGrip;
        public int _RGripClicks;
        public float _RGrip;
        public int _AClicks;
        public int _BClicks;
        public int _XClicks;
        public int _YClicks;

        // File Writing
        FileStream InputDataInfo;
        StreamWriter InputDataWriter;
        string FilePath = Application.streamingAssetsPath + "/Hippo/";
        string SaveTime = DateTime.Now.ToString("yyyyMMddHHmmss");
        string InputSavePath;
        string DeviceSavePath;
        string TestSavePath;

        float Timer = 0;
        float plotTimer = 0;
        FileStream DeviceDataInfo;
        StreamWriter DeviceDataWriter;

        FileStream TestStream;
        StreamWriter TestWriter;

        // Object Class for storing data
        class Stats
        {
            // Head Transform
            public Vector3 HeadPosition;    // 3D Coordinates (Contains x, y, z)
            public Vector3 HeadAngle;   // Euler Angles (Contains x, y, z)
            // Controller Transform
            public Vector3 LHandPosition;
            public Vector3 RHandPosition;
            public string controllerTransform;

            // Controller Input
            public int LTriggerClicks = 0;
            public int RTriggerClicks = 0;
            public int LGripClicks = 0;
            public int RGripClicks = 0;
            public int AClicks = 0;
            public int BClicks = 0;
            public int XClicks = 0;
            public int YClicks = 0;
            public string controllerInput;

            // final output
            public string output;
        }

        // Start is called before the first frame update
        void Start()
        {
            database = new Stats();
            InputSavePath = FilePath + SceneManager.GetActiveScene().name + "_" + "INPUT" + "_" + SaveTime + "_DATA" + ".txt";
            DeviceSavePath = FilePath + SceneManager.GetActiveScene().name + "_" + "DEVICE" + "_" + SaveTime + "_DATA" + ".txt";
            TestSavePath = FilePath + SceneManager.GetActiveScene().name + "_" + "PLOT" + "_" + SaveTime + "_DATA" + ".txt";

            TestStream = new FileStream(TestSavePath, FileMode.Append, FileAccess.Write);
            TestWriter = new StreamWriter(TestStream, System.Text.Encoding.Unicode);
            TestWriter.WriteLine("Time, LTrigger Value, Time, RTrigger Value, Time, LGrip Value, Time, RGrip Value");
            TestWriter.Close();
        }

        // Update is called once per frame
        void Update()
        {
            Timer += Time.deltaTime;
            plotTimer += Time.deltaTime;

            if (Timer > .05f)
            {
                SaveDeviceData();
                TestPlot(plotTimer);
                Timer = 0;
            }
            ShowDataOnInspector();
        }

        private void OnApplicationQuit()
        {
            database.controllerInput = "Left Trigger Clicks: " + database.LTriggerClicks.ToString() + "\nRight Trigger Clicks: " + database.RTriggerClicks.ToString()
                + "\nLeft Grip Clicks: " + database.LGripClicks.ToString() + "\nRight Grip Clicks: " + database.RGripClicks.ToString()
                + "\nA Button Pressed: " + database.AClicks.ToString() + "\nB Button Pressed: " + database.BClicks.ToString()
                + "\nX Button Pressed: " + database.XClicks.ToString() + "\nY Button Pressed: " + database.YClicks.ToString();
            SaveInputData(database.controllerInput);
        }

        void ShowDataOnInspector()
        {
            _inputBridge = XRRig.GetComponent<InputBridge>();

            // Saves Camera Rig Position & EulerAngles, Left Controller Position and Right Controller Position
            database.HeadPosition = Camera.localPosition;
            database.HeadAngle = Camera.eulerAngles;
            database.LHandPosition = LHand.localPosition;
            database.RHandPosition = RHand.localPosition;

            // Trigger Button
            if (RTriggerState < _inputBridge.RightTrigger && _inputBridge.RightTrigger == 1)
            {
                database.RTriggerClicks++;
            }
            if (LTriggerState < _inputBridge.LeftTrigger && _inputBridge.LeftTrigger == 1)
            {
                database.LTriggerClicks++;
            }
            RTriggerState = _inputBridge.RightTrigger; // Save Right Trigger State in current frame
            LTriggerState = _inputBridge.LeftTrigger; // Save Left Trigger State in current frame
            
            // Grip Button
            if (_inputBridge.RightGripDown)
            {
                database.RGripClicks++;
            }
            if (_inputBridge.LeftGripDown)
            {
                database.LGripClicks++;
            }
            // Right Controller
            if (_inputBridge.AButtonDown)
            {
                database.AClicks++;
            }
            if (_inputBridge.BButtonDown)
            {
                database.BClicks++;
            }
            // Left Controller
            if (_inputBridge.XButtonDown)
            {
                database.XClicks++;
            }
            if (_inputBridge.YButtonDown)
            {
                database.YClicks++;
            }

            // Update Debug Panel per frame
            _LTriggerClicks = database.LTriggerClicks;
            _LTrigger = _inputBridge.LeftTrigger;
            _RTriggerClicks = database.RTriggerClicks;
            _RTrigger = _inputBridge.RightTrigger;
            _LGripClicks = database.LGripClicks;
            _LGrip = _inputBridge.LeftGrip;
            _RGripClicks = database.RGripClicks;
            _RGrip = _inputBridge.RightGrip;
            _AClicks = database.AClicks;
            _BClicks = database.BClicks;
            _XClicks = database.XClicks;
            _YClicks = database.YClicks;

            //////////////////////////////////////////
            //////////////////////////////////////////
            // Create Data String
            // CODE GOES HERE
            //////////////////////////////////////////
            //////////////////////////////////////////
        }

        public void SaveDeviceData()
        {
            DeviceDataInfo = new FileStream(DeviceSavePath, FileMode.Append, FileAccess.Write);
            DeviceDataWriter = new StreamWriter(DeviceDataInfo, System.Text.Encoding.Unicode);
            DeviceDataWriter.WriteLine(Buttons());
            DeviceDataWriter.WriteLine(Positions());
            DeviceDataWriter.Close();
        }

        public string Positions()
        {
            return "HEAD POSITION (" + database.HeadPosition.x.ToString() + ", " + database.HeadPosition.y.ToString() + ", " + database.HeadPosition.z.ToString() + 
                " )        HEAD ANGLE (" + database.HeadAngle.x.ToString() + ", " + database.HeadAngle.y.ToString() + ", " + database.HeadAngle.z.ToString() + 
                " )\nLEFT CONTROLLER POSITION (" + database.LHandPosition.x.ToString() + ", " + database.LHandPosition.y.ToString() + ", " + database.LHandPosition.z.ToString() + 
                " )        RIGHT CONTROLLER POSITION (" + database.RHandPosition.x.ToString() + ", " + database.RHandPosition.y.ToString() + ", " + database.RHandPosition.z.ToString() + " )"
                + "\n------------------------------------------------------------------------------------------------------------------------------------------------------------";
        }

        public string Buttons()
        {
            return "A Button: " + (_inputBridge.AButtonDown || _inputBridge.AButton || _inputBridge.AButtonUp ? 1 : 0).ToString() +
                "  B Button: " + (_inputBridge.BButtonDown || _inputBridge.BButton || _inputBridge.BButtonUp ? 1 : 0).ToString() +
                "  X Button: " + (_inputBridge.XButtonDown || _inputBridge.XButton || _inputBridge.XButtonUp ? 1 : 0).ToString() +
                "  Y Button: " + (_inputBridge.YButtonDown || _inputBridge.YButton || _inputBridge.YButtonUp ? 1 : 0).ToString() +
                "\nLeft Trigger: " + _LTrigger.ToString() + "  Right Trigger: " + _RTrigger.ToString() + "  Left Grip: " + _LGrip.ToString() + "  Right Grip: " + _RGrip.ToString() + "\n";
        }

        public void TestPlot(float time)
        {
            TestStream = new FileStream(TestSavePath, FileMode.Append, FileAccess.Write);
            TestWriter = new StreamWriter(TestStream, System.Text.Encoding.Unicode);
            TestWriter.WriteLine(Plot(time));
            TestWriter.Close();
        }

        public string Plot(float time)
        {
            string plot = time.ToString() + ", " + _LTrigger.ToString() + ", " + time.ToString() + ", " + _RTrigger.ToString() + ", " + time.ToString() + ", " + _LGrip.ToString() + ", " + time.ToString() + ", " + _RGrip.ToString();
            return plot;
        }

        public void SaveInputData(string myData)
        {
            InputDataInfo = new FileStream(InputSavePath, FileMode.Append, FileAccess.Write);
            InputDataWriter = new StreamWriter(InputDataInfo, System.Text.Encoding.Unicode);
            InputDataWriter.WriteLine(myData);
            InputDataWriter.Close();
        }
    }   
}


