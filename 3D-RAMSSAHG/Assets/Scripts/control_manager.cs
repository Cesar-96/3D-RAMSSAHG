using UnityEngine;
using System.Collections;
using System.IO.Ports;


public class control_manager : MonoBehaviour
{
    // RAMSSAHG PIECES
    public GameObject Tower;
    public GameObject Arm;
    public GameObject ForeArm;
    public GameObject Gripper;

    // KINECT BODY
    public GameObject MyBodySourceManager;
    private BodySourceManager _BodyManager;

    // KINECT JOINTS
    public Transform Elbow_left;
    public Transform Shoulder_left;
    public Transform Wrist_left;
    public Transform Hand_left;
    public Transform Hand_tipleft;
    public Transform Thumb_left;

    // GUIDES
    public GameObject Cube;
    public GameObject Sphere;

    // Setters
    public float rotationSpeed = 10f;
    public float translationSpeed = 1.0f;

    // Memory Movements
    private (GameObject, float) TowerOldRotation;
    private (GameObject, float) ArmOldTranslation;
    private (GameObject, float) ForeArmOldRotation;
    private (GameObject, float) GripperOldRotation;

    // Canvas Input
    private string command;
    private float rotationTarget;

    // Animations Control
    private bool stopAnimation;
    private float noise = 0.01f;
    private float t_noise = 0.1f;

    private float towerTarget;
    private float foreArmTarget;
    private float towerAngleTarget;
    private float foreArmAngleTarget;

    private float L1 = 33.74001f;
    private float L2 = 17.0519f;

    private float timer = 0f;
    private float interval = 1f / 30f;
    private ValuePair<Vector3> pair;

    private float noise_joint = 1.7f;
    private float noise_joint_x = 1.9f;

    private string command_prev, commandd;

    //ARDUINO :V
    //SerialPort data_stream = new SerialPort("COM4", 115200);

    void Start()
    {
        TowerOldRotation = (Tower, 0.0f);
        ArmOldTranslation = (Arm, 29.71314f);
        ForeArmOldRotation = (ForeArm, 0.0f);
        GripperOldRotation = (Gripper, 0.0f);

        command_prev = "X";
        commandd = "Y";

        if (MyBodySourceManager == null) return;

        _BodyManager = MyBodySourceManager.GetComponent<BodySourceManager>();
        if (_BodyManager == null) return;

        pair.FirstValue = _BodyManager.GetBodyJointPosModded(Windows.Kinect.JointType.ElbowLeft);
        //data_stream.Open();
    }

    void Update()
    {
        timer += Time.deltaTime;
        Shoulder_left.localPosition = _BodyManager.GetBodyJointPosModded(Windows.Kinect.JointType.ShoulderLeft);
        Elbow_left.localPosition = _BodyManager.GetBodyJointPosModded(Windows.Kinect.JointType.ElbowLeft);
        Wrist_left.localPosition = _BodyManager.GetBodyJointPosModded(Windows.Kinect.JointType.WristLeft);
        Hand_left.localPosition = _BodyManager.GetBodyJointPosModded(Windows.Kinect.JointType.HandLeft);
        Hand_tipleft.localPosition = _BodyManager.GetBodyJointPosModded(Windows.Kinect.JointType.HandTipLeft);
        Thumb_left.localPosition = _BodyManager.GetBodyJointPosModded(Windows.Kinect.JointType.ThumbLeft);

        if (timer >= interval)
        {
            Vector3 directions = Elbow_left.localPosition - pair.FirstValue;
            pair.SecondValue = pair.FirstValue;
            pair.FirstValue = Elbow_left.localPosition;
            command_prev = commandd;

            if (directions.y >= 0.0f)
            {
                if (directions.y > noise_joint)
                {
                    commandd = "UPPER";
                    //data_stream.Write("G");
                    ReadStringInput("A15");
                }
            }
            else
            {
                if (Mathf.Abs(directions.y) > noise_joint)
                {
                    commandd = "DOWNER";
                    ReadStringInput("A-10");
                    //data_stream.Write("Y");
                }
            }

            if (directions.x >= 0.0f)
            {
                if (directions.x > noise_joint_x)
                {
                    commandd = "RIGHTER";
                    //data_stream.Write("B");
                    ReadStringInput("T-60");
                }
            }
            else
            {
                if (Mathf.Abs(directions.x) > noise_joint_x)
                {
                    commandd = "LEFTER";
                    //data_stream.Write("W");
                    ReadStringInput("T60");
                }

            }

            if (command_prev != commandd)
                Debug.Log(commandd + "    " + timer);
            //EditorApplication.Beep();


            timer = 0f;
        }

        if (Input.GetKey(KeyCode.W))
        {
            // Mover hacia adelante
            // Actualiza la posición/rotación del brazo hacia adelante
            // Por ejemplo, podrías modificar la posición de la parte del brazo correspondiente aquí
            // Ejemplo ficticio:
            Vector3 forwardMovement = Vector3.forward * translationSpeed * Time.deltaTime;
            Arm.transform.Translate(forwardMovement);
        }

        if (!stopAnimation)
        {
            switch (command)
            {
                case "T":
                    StartingRotation(ref TowerOldRotation, rotationTarget, 100);
                    break;
                case "A":
                    StartingTranslation(ref ArmOldTranslation, rotationTarget, 15);
                    break;
                case "F":
                    StartingRotation(ref ForeArmOldRotation, rotationTarget, 60);
                    break;
                case "G":
                    StartingRotation(ref GripperOldRotation, rotationTarget, 50);
                    break;
                case "K":
                    //StartingRotation(ref ForeArmOldRotation, foreArmTarget, 60);
                    stopAnimation = false;
                    stopAnimation = false;
                    forwardKinematics(towerTarget, foreArmTarget);
                    //StartingRotation(ref TowerOldRotation, towerTarget, 100);
                    break;
                case "I":
                    InverseKinematics(towerTarget, foreArmTarget);
                    //StartingRotation(ref ForeArmOldRotation, foreArmAngleTarget, 60);
                    stopAnimation = false;
                    stopAnimation = false;
                    //StartingRotation(ref TowerOldRotation, towerAngleTarget, 100);
                    break;
                default:
                    //Debug.Log("Invalid Input: Try Again pls!");
                    break;
            }
        }
    }

    private void StartingRotation(ref (GameObject gameObject, float currentRotationY) pairGameObject, float rotationTarget, float bound)
    {
        float yRotation = pairGameObject.gameObject.transform.eulerAngles.y;
        float rotation = pairGameObject.currentRotationY + rotationTarget;

        if (bound >= 0.0f)
        {
            if (rotation > bound) rotation = bound;
        }
        else
        {
            if (rotation < bound) rotation = -1 * bound;
        }

        float angleDifference = Quaternion.Angle(Quaternion.Euler(0f, yRotation, 0f), Quaternion.Euler(0f, rotation, 0f));

        if (angleDifference > noise)
        {
            yRotation += ((rotationSpeed * Time.deltaTime) * ((yRotation > rotation ? -1.0f : 1.0f)));
        }
        else
        {
            yRotation = (yRotation > rotation) ? Mathf.Min(yRotation, rotation) : Mathf.Max(yRotation, rotation);
            pairGameObject.currentRotationY = yRotation;
            stopAnimation = true;
        }
        pairGameObject.gameObject.transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
    }

    private void StartingTranslation(ref (GameObject gameObject, float currentPositionY) pairGameObject, float translationTarget, float bound)
    {
        float currentPosition = pairGameObject.gameObject.transform.position.y;
        float translation = pairGameObject.currentPositionY + translationTarget;
        float distance = Mathf.Abs(currentPosition - translation);
        Debug.Log("XXXXXXDD" + translation);

        if (bound >= 0.0f)
        {
            if (translation > 39.71f) { translation = 39.71f; }
        }
        else
        {
            if (translation < 39.71f) { translation = -1 * 39.71f; }
        }

        if (distance > t_noise)
        {
            currentPosition += (translationSpeed * Time.deltaTime) * ((currentPosition > translation ? -1.0f : 1.0f));
        }
        else
        {
            currentPosition = (currentPosition > translation) ? Mathf.Min(currentPosition, translation) : Mathf.Max(currentPosition, translation);
            pairGameObject.currentPositionY = currentPosition;
            stopAnimation = true;
        }
        pairGameObject.gameObject.transform.position = new Vector3(pairGameObject.gameObject.transform.position.x, currentPosition, pairGameObject.gameObject.transform.position.z);
    }

    public void ReadStringInput(string input)
    {
        command = input.Substring(0, 1);
        if (command == "K" || command == "I")
        {
            string[] partes = input.Split(',');
            string numero1String = partes[0].Substring(1);
            towerTarget = float.Parse(numero1String);
            foreArmTarget = float.Parse(partes[1]);
            Debug.Log("A command: " + command + " TowerTarget " + towerTarget + " ForeArmTarget " + foreArmTarget);
        }
        else
        {
            rotationTarget = float.Parse(input.Substring(1));
            Debug.Log("A command: " + command + " New Limit: " + rotationTarget);
        }
        stopAnimation = false;
    }

    private void forwardKinematics(float towerTarget, float foreArmTarget)
    {
        float theta1F = towerTarget * Mathf.PI / 180f;
        float theta2F = foreArmTarget * Mathf.PI / 180f;

        float X = Mathf.Round(L1 * Mathf.Sin(theta1F) + L2 * Mathf.Sin(theta1F + theta2F)) * -1f;
        float Z = Mathf.Round(L1 * Mathf.Cos(theta1F) + L2 * Mathf.Cos(theta1F + theta2F)) * -1f;

        Debug.Log("X: " + X + " Z: " + Z);

        Cube.transform.position = new Vector3(X, Cube.transform.position.y, Z);
    }

    private void InverseKinematics(float x, float y)
    {

        Sphere.transform.position = new Vector3(x, Cube.transform.position.y, y);

        float theta2 = Mathf.Acos((x * x + y * y - L1 * L1 - L2 * L2) / (2 * L1 * L2));

        float theta1 = Mathf.Atan(x / y) - Mathf.Atan((L2 * Mathf.Sin(theta2)) / (L1 + L2 * Mathf.Cos(theta2)));

        theta1 = Mathf.Rad2Deg * theta1;
        theta2 = Mathf.Rad2Deg * theta2;

        /* if (x < 0 && y > 0)
        {   // 2do cuadrante
            theta1 = 180 - (theta1 * -1f);
        }
                if (x >= 0 && y >= 0)
                {   // 1er cuadrante
                    theta1 = 90 - theta1;
                }
                else if (x < 0 && y > 0)
                {   // 2do cuadrante
                    theta1 = 90 - theta1;
                }
                else if (x < 0 && y < 0)
                {   // 3er cuadrante
                    theta1 = 270 - theta1;
                }
                else if (x > 0 && y < 0)
                {   // 4to cuadrante
                    theta1 = -90 - theta1;
                }
                else if (x < 0 && y == 0)
                {
                    theta1 = 270 + theta1;
                }
        */
        theta1 = Mathf.Round(theta1);
        theta2 = Mathf.Round(theta2);


        Debug.Log("Theta1: " + theta1 + " Theta2: " + theta2);
        towerAngleTarget = theta1;
        foreArmAngleTarget = theta2;
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

}
