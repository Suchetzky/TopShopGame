using System.Collections.Generic;
using UnityEngine;

public class TracksVisualizer : MonoBehaviour
{
    public List<Transform> track;
    [SerializeField] private int vertexCount;
    [SerializeField] private Color color;
    
    private void OnDrawGizmos()
    {
        for (var i = 0; i < track.Count; i++)
        {
            Gizmos.color = color;
            for (float ratio = 0.5f / vertexCount; ratio < 1; ratio += 1.0f/ vertexCount)
            {
                var ab = Vector3.Lerp(track[0].position, track[1].position, ratio );
                var bc = Vector3.Lerp(track[1].position, track[2].position, ratio );
                var cd = Vector3.Lerp(track[2].position, track[3].position, ratio);

                var ab_bc= Vector3.Lerp(ab, bc, ratio);
                var bc_cd = Vector3.Lerp(bc, cd, ratio);
        
                Gizmos.DrawLine(ab_bc, bc_cd);
            }
        }
        
        
        
    }
}
