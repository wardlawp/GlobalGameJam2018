using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class HoseMeshGen : MonoBehaviour
{
    private List<Collider> _hoseSegments;
    private Mesh _mesh;
    private MeshFilter _filter;
    private MeshRenderer _renderer;
    private Vector3[] _verts;
    private int[] _tris;

    public int numPointsPerEdge = 12;
    public float radius = 1;
    public float lengthPerSeg = 0.5f;
    public Material material;

    void Start()
    {
        _hoseSegments = GetComponentsInChildren<Collider>().ToList();
        _mesh = new Mesh();
        _filter = gameObject.AddComponent<MeshFilter>();
        _renderer = gameObject.AddComponent<MeshRenderer>();
        _filter.mesh = _mesh;
        _renderer.material = material;

        var numVerts = numPointsPerEdge * 2 * _hoseSegments.Count;
        var numTris = numPointsPerEdge * 12 * _hoseSegments.Count;
        _verts = new Vector3[numVerts];
        _tris = new int[numTris];

        for (int seg = 0; seg < _hoseSegments.Count; seg++)
        {
            int triOffset = seg * numPointsPerEdge * 12;
            int vertOffset = seg * numPointsPerEdge * 2;

            for (int i = 0; i < numPointsPerEdge; i++)
            {
                _tris[triOffset + i * 12 + 0] = (vertOffset + i);
                _tris[triOffset + i * 12 + 1] = (vertOffset + i + 1);
                _tris[triOffset + i * 12 + 2] = (vertOffset + numPointsPerEdge + i);

                if (seg < _hoseSegments.Count - 1 || i < numPointsPerEdge-1)
                {
                    _tris[triOffset + i * 12 + 3] = (vertOffset + numPointsPerEdge + i);
                    _tris[triOffset + i * 12 + 4] = (vertOffset + i + 1);
                    _tris[triOffset + i * 12 + 5] = (vertOffset + numPointsPerEdge + i + 1);
                }
            }

            if (seg < _hoseSegments.Count-1)
            for (int i = 0; i < numPointsPerEdge; i++)
            {
                _tris[triOffset + i * 12 + 0 + 6] = (vertOffset + i) + numPointsPerEdge;
                _tris[triOffset + i * 12 + 1 + 6] = (vertOffset + i + 1) + numPointsPerEdge;
                _tris[triOffset + i * 12 + 2 + 6] = (vertOffset + numPointsPerEdge + i) + numPointsPerEdge;

                _tris[triOffset + i * 12 + 3 + 6] = (vertOffset + numPointsPerEdge + i) + numPointsPerEdge;
                _tris[triOffset + i * 12 + 4 + 6] = (vertOffset + i + 1) + numPointsPerEdge;
                _tris[triOffset + i * 12 + 5 + 6] = (vertOffset + numPointsPerEdge + i + 1) + numPointsPerEdge;
            }
        }

    }

    void Update()
    {
        for (int seg = 0; seg < _hoseSegments.Count; seg++)
        {
            var segment = _hoseSegments[seg];
            for (int end = 0; end < 2; end++)
            {
                var right = segment.transform.right;
                var forward = segment.transform.forward;

                if (seg > 0 && end == 0)
                {
                    right = (right + _hoseSegments[seg - 1].transform.right*0.5f).normalized;
                    forward = (forward + _hoseSegments[seg - 1].transform.forward * 0.5f).normalized;
                }

                if (seg < _hoseSegments.Count - 1 && end == 1)
                {
                    right = (right + _hoseSegments[seg + 1].transform.right * 0.5f).normalized;
                    forward = (forward + _hoseSegments[seg + 1].transform.forward * 0.5f).normalized;
                }

                var endCenter = segment.transform.position + lengthPerSeg * segment.transform.up * (end * 2 - 1);
                for (int i = 0; i < numPointsPerEdge; i++)
                {
                    float r = (Mathf.Deg2Rad * i * 360 / (float)numPointsPerEdge);
                    int offset = (seg * 2 + end) * numPointsPerEdge + i;
                    _verts[offset] = transform.InverseTransformPoint(endCenter
                        + forward * Mathf.Cos(r) * radius
                        + right * Mathf.Sin(r) * radius);

                    //Debug.DrawLine(endCenter, transform.TransformPoint(_verts[offset]));
                }
            }
        }

        _mesh.vertices = _verts;
        _mesh.triangles = _tris;

        _mesh.RecalculateBounds();
        _mesh.RecalculateTangents();
        _mesh.RecalculateNormals();
        _mesh.UploadMeshData(false);
    }
}
