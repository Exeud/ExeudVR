using ExeudVR;
using UnityEngine;

[System.Serializable]
public class VRMap
{
    public Transform rigTarget;
    public Vector3 trackingPositionOffset;
    public Vector3 trackingRotationOffset;

    public void Map(Transform vrTarget)
    {
        rigTarget.position = vrTarget.TransformPoint(trackingPositionOffset);
        rigTarget.rotation = vrTarget.rotation * Quaternion.Euler(trackingRotationOffset);
    }
}

public class ExeudVRRig : MonoBehaviour
{
    [Tooltip("Connection to ICVR \nHint: ICVRCameraSet ➥ CharacterRoot ➥ Body")]
    [SerializeField] private BodyController bodyController;

    [SerializeField] private float yFac;

    public VRMap Body;
    public VRMap Head;
    public VRMap LeftHand;
    public VRMap RightHand;

    private Transform headRef;
    private Transform bodyRef;
    private Transform leftHandRef;
    private Transform rightHandRef;

    private void Start()
    {
        bodyController.avatar = GetComponent<ExeudVRAvatarController>();

        headRef = bodyController.GetBodyReference("head");
        bodyRef = bodyController.GetBodyReference("body");
        leftHandRef = bodyController.GetBodyReference("leftHand");
        rightHandRef = bodyController.GetBodyReference("rightHand");
    }

    void Update()
    {
        Head.Map(headRef);
        Body.Map(bodyRef);

        // adjustment to prevent seeing the inside of your face
        //yFac = (0.1f * Mathf.Sin(headRef.localRotation.eulerAngles.x * Mathf.Deg2Rad)) + 0.1f;
        //transform.position = bodyRef.TransformPoint(new Vector3(0f, yFac, -0.2f));
        //transform.rotation = bodyRef.rotation;

        RightHand.Map(rightHandRef);
        LeftHand.Map(leftHandRef);

        if (Input.GetKeyDown(KeyCode.J)) 
        {
            GetComponent<ExeudVRAvatarController>().PrepareArmRig();
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            GetComponent<ExeudVRAvatarController>().RelaxArmRig();
        }
    }
}
