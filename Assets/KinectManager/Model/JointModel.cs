using UnityEngine;
using Windows.Kinect;

public class JointModel
{
    public Vector3 Position { get; private set; }
    public JointType Type { get; private set; }
    public bool IsTracked { get; private set; }

    public JointModel(Windows.Kinect.Joint joint)
    {
        Type = joint.JointType;
        Position = new Vector3(joint.Position.X, joint.Position.Y, joint.Position.Z);
        IsTracked = (joint.TrackingState == TrackingState.Tracked);
    }
}
