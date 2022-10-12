using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CharacterMovement))]
public class FieldOfViewEditor : Editor
{
    private void OnSceneGUI()
    {
        CharacterMovement fov = (CharacterMovement)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.playerObj.transform.position, Vector3.up, Vector3.forward, 360, fov.spotRadius);

        Vector3 viewAngleOne = DirectionFromAngle(fov.playerObj.transform.eulerAngles.y, -fov.spotAngle / 2);
        Vector3 viewAngleTwo = DirectionFromAngle(fov.playerObj.transform.eulerAngles.y, fov.spotAngle / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(fov.playerObj.transform.position, fov.playerObj.transform.position + viewAngleOne * fov.spotRadius);
        Handles.DrawLine(fov.playerObj.transform.position, fov.playerObj.transform.position + viewAngleTwo * fov.spotRadius);
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
