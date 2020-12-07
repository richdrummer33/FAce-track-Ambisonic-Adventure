using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DrawGizmo : MonoBehaviour
{
    //public Mesh drawMesh;
    public enum GizmoIcon { wave, ambience, environment }
    public GizmoIcon icon;

    public enum GizmoShape { sphere, cube}
    public GizmoShape shape = GizmoShape.cube;

    public Color color;

    public float size = 0.2f;

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawIcon(transform.position, icon.ToString() + ".tiff");

        if (shape == GizmoShape.cube)
            Gizmos.DrawWireCube(transform.position, Vector3.one * size);
        else if (shape == GizmoShape.sphere)
            Gizmos.DrawWireSphere(transform.position, size);
    }
}
