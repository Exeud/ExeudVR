﻿using UnityEngine;

public class CCRotateDirect : MonoBehaviour
{
    public enum ExMode
    {
        animation = 0,
        script = 1,
    }
    public ExMode ExModes = ExMode.animation;
    public bool IsInverse;
    public string Default = "";
    public string Inverse = "";
    public float Speed = 10;

    private FixedJoint joint;

	// Use this for initialization
	void Start ()
    {
        if (ExModes == ExMode.animation)
        {
            if (IsInverse) GetComponent<Animation>().Play(Inverse);
            else GetComponent<Animation>().Play(Default);
        }
    }

    void OnCollisionEnter(Collision c)
    {
        if (!gameObject.GetComponent<FixedJoint>())
        {
            joint = gameObject.AddComponent<FixedJoint>();
            joint.enablePreprocessing = false;
        }
        else
        {
            joint = gameObject.GetComponent<FixedJoint>();
        }
        joint.connectedBody = c.rigidbody;
    }

    private void OnCollisionExit(Collision collision)
    {
        Destroy(gameObject.GetComponent<FixedJoint>());
        joint = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (ExModes == ExMode.script)
        {
            transform.Rotate(0, Speed * Time.deltaTime, 0);
        }
    }
}
