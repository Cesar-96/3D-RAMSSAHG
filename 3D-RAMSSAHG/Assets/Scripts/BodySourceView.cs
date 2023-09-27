using UnityEngine;

public class BodySourceView : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject BodySourceManager;
    private BodySourceManager _BodyManager;

    public Transform Spine_base;
    public Transform Spine_mid;
    public Transform Neck;
    public Transform Head;
    //public Transform Shoulder_left;
    //public Transform Elbow_left;
    //public Transform Wrist_left;
    public Transform Shoulder_right;
    public Transform Elbow_right;
    public Transform Wrist_right;
    public Transform Hand_right;
    public Transform Hip_left;
    public Transform Knee_left;
    public Transform Ankle_left;
    public Transform Foot_left;
    public Transform Hip_right;
    public Transform Knee_right;
    public Transform Ankle_right;
    public Transform Foot_right;
    public Transform Spines_shoulder;
    //public Transform Hand_tipleft;
    //public Transform Thumb_left;
    public Transform Hand_tipright;
    public Transform Thumb_right;
    void Start()
    {
        if (BodySourceManager == null)
        {
            Debug.Log("No hay script");
            return;
        }

        _BodyManager = BodySourceManager.GetComponent<BodySourceManager>();
        if (_BodyManager == null)
        {
            Debug.Log("No hay clase");
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Spine_base.localPosition = _BodyManager.GetBodyJointPosModded(Windows.Kinect.JointType.SpineBase);
        Spine_mid.localPosition = _BodyManager.GetBodyJointPosModded(Windows.Kinect.JointType.SpineMid);
        Neck.localPosition = _BodyManager.GetBodyJointPosModded(Windows.Kinect.JointType.Neck);
        Head.localPosition = _BodyManager.GetBodyJointPosModded(Windows.Kinect.JointType.Head);
        //Shoulder_left.localPosition = _BodyManager.GetBodyJointPosModded(Windows.Kinect.JointType.ShoulderLeft);
        //Elbow_left.localPosition = _BodyManager.GetBodyJointPosModded(Windows.Kinect.JointType.ElbowLeft);
        //Wrist_left.localPosition = _BodyManager.GetBodyJointPosModded(Windows.Kinect.JointType.WristLeft);
        //Hand_left.localPosition = _BodyManager.GetBodyJointPosModded(Windows.Kinect.JointType.HandLeft);
        Shoulder_right.localPosition = _BodyManager.GetBodyJointPosModded(Windows.Kinect.JointType.ShoulderRight);
        Elbow_right.localPosition = _BodyManager.GetBodyJointPosModded(Windows.Kinect.JointType.ElbowRight);
        Wrist_right.localPosition = _BodyManager.GetBodyJointPosModded(Windows.Kinect.JointType.WristRight);
        Hand_right.localPosition = _BodyManager.GetBodyJointPosModded(Windows.Kinect.JointType.HandRight);
        Hip_left.localPosition = _BodyManager.GetBodyJointPosModded(Windows.Kinect.JointType.HipLeft);
        Knee_left.localPosition = _BodyManager.GetBodyJointPosModded(Windows.Kinect.JointType.KneeLeft);
        Ankle_left.localPosition = _BodyManager.GetBodyJointPosModded(Windows.Kinect.JointType.AnkleLeft);
        Foot_left.localPosition = _BodyManager.GetBodyJointPosModded(Windows.Kinect.JointType.FootLeft);
        Hip_right.localPosition = _BodyManager.GetBodyJointPosModded(Windows.Kinect.JointType.HipRight);
        Knee_right.localPosition = _BodyManager.GetBodyJointPosModded(Windows.Kinect.JointType.KneeRight);
        Ankle_right.localPosition = _BodyManager.GetBodyJointPosModded(Windows.Kinect.JointType.AnkleRight);
        Foot_right.localPosition = _BodyManager.GetBodyJointPosModded(Windows.Kinect.JointType.FootRight);
        Spines_shoulder.localPosition = _BodyManager.GetBodyJointPosModded(Windows.Kinect.JointType.SpineShoulder);
        //Hand_tipleft.localPosition = _BodyManager.GetBodyJointPosModded(Windows.Kinect.JointType.HandTipLeft);
        //Thumb_left.localPosition = _BodyManager.GetBodyJointPosModded(Windows.Kinect.JointType.ThumbLeft);
        Hand_tipright.localPosition = _BodyManager.GetBodyJointPosModded(Windows.Kinect.JointType.HandTipRight);
        Thumb_right.localPosition = _BodyManager.GetBodyJointPosModded(Windows.Kinect.JointType.ThumbRight);
    }
}
