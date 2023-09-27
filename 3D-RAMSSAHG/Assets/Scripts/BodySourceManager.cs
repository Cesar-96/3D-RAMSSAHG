using UnityEngine;
using System;
using System.Collections.Generic;

using Windows.Kinect;

public class BodySourceManager : MonoBehaviour
{
    public Material BoneMaterial;

    private KinectSensor _Sensor;
    private Body firstBody = null;
    private BodyFrameReader _Reader;
    private Body[] _Data = null;
    private Dictionary<ulong, GameObject> _Bodies = new Dictionary<ulong, GameObject>();
    private Dictionary<string, Windows.Kinect.JointType> jointTypeByName = new Dictionary<string, Windows.Kinect.JointType>();

    public Body[] GetData()
    {
        return _Data;
    }

    void Start()
    {
        _Sensor = KinectSensor.GetDefault();

        if (_Sensor != null)
        {
            _Reader = _Sensor.BodyFrameSource.OpenReader();

            if (!_Sensor.IsOpen)
            {
                _Sensor.Open();
            }
        }

        foreach (Windows.Kinect.JointType jointType in Enum.GetValues(typeof(Windows.Kinect.JointType)))
        {
            jointTypeByName[jointType.ToString()] = jointType;
        }
    }

    void Update()
    {
        if (_Reader != null)
        {
            var frame = _Reader.AcquireLatestFrame();
            if (frame != null)
            {
                if (_Data == null)
                {
                    _Data = new Body[_Sensor.BodyFrameSource.BodyCount];
                }

                frame.GetAndRefreshBodyData(_Data);

                foreach (Body body in _Data)
                {
                    if (body != null && body.IsTracked)
                    {
                        firstBody = body;
                        break;
                    }
                }

                frame.Dispose();
                frame = null;
            }
        }
    }

    public bool isThereBody() { return firstBody != null; }

    public Vector3 GetBodyJointPos(JointType jt)
    {
        if (firstBody == null) return Vector3.zero;

        Windows.Kinect.Joint myJoint = firstBody.Joints[jt];
        return new Vector3(myJoint.Position.X, myJoint.Position.Y, myJoint.Position.Z);
    }

    public Vector3 GetBodyJointPosModded(JointType jt)
    {
        if (firstBody == null) return Vector3.zero;

        Windows.Kinect.Joint myJoint = firstBody.Joints[jt];
        return new Vector3(myJoint.Position.X * 60, myJoint.Position.Y * 60, myJoint.Position.Z * 60);
    }

    void OnApplicationQuit()
    {
        if (_Reader != null)
        {
            _Reader.Dispose();
            _Reader = null;
        }

        if (_Sensor != null)
        {
            if (_Sensor.IsOpen)
            {
                _Sensor.Close();
            }

            _Sensor = null;
        }
    }
}

public struct ValuePair<T>
{
    public T FirstValue { get; set; }
    public T SecondValue { get; set; }

    public ValuePair(T firstValue, T secondValue)
    {
        FirstValue = firstValue;
        SecondValue = secondValue;
    }
}
