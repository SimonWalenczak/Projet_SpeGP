using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomMatrix : MonoBehaviour
{
    [SerializeField] private Vector2 _roomSize;
    [SerializeField] private float _valueRegeneration;
    private float _previousRoomSizeX;

    [Space(10)] [Header("Walls Meshes")] [SerializeField]
    private Mesh _wallMesh;

    [SerializeField] private Mesh _doorMesh;

    [Space(10)] [Header("Pillars Meshes")] [SerializeField]
    private Mesh _pillarMesh;

    [Space(10)] [Header("Materials")] [SerializeField]
    private Material _dungeonMaterial;

    //Hidden
    private int _wallCount;
    private Vector2 _wallSize;
    private List<Matrix4x4> _wallMatrices;
    private List<Matrix4x4> _doorMatrices;
    private Mesh _currentMesh;

    //End variables

    private void Start()
    {
        _wallSize.x = _wallMesh.bounds.size.x;
        _wallSize.y = _wallMesh.bounds.size.y;

        _roomSize.x = _wallSize.x;
        _roomSize.y = _wallSize.y;

        _previousRoomSizeX = _roomSize.x;
        
        _wallMatrices = new List<Matrix4x4>();
        _doorMatrices = new List<Matrix4x4>();
        
        _currentMesh = _wallMesh;

        CreateWalls();
    }

    private void Update()
    {
        if (_roomSize.x > _previousRoomSizeX + _valueRegeneration)
        {
            _previousRoomSizeX = _roomSize.x;
            CreateWalls();
        }
        else if (_roomSize.x < _previousRoomSizeX - _valueRegeneration)
        {
            _previousRoomSizeX = _roomSize.x;
            CreateWalls();
        }
        
        RenderWalls();
    }

    private void CreateWalls()
    {
        _wallMatrices.Clear();
        _doorMatrices.Clear();

        int wallCount = Mathf.Max(1, (int)(_roomSize.x / _wallSize.x));
        float scale = (_roomSize.x / wallCount) / _wallSize.x;

        for (int i = 0; i < wallCount; i++)
        {
            var t = transform.position +
                    new Vector3(-_roomSize.x / 2 + _wallSize.x * scale / 2 + i * scale * _wallSize.x, 0,
                        +_roomSize.y / 2);
            var r = transform.rotation;
            var s = new Vector3(scale, 1, 1);

            var mat = Matrix4x4.TRS(t, r, s);

            var rand = Random.Range(0, 2);
            if (rand < 1)
            {
                _wallMatrices.Add(mat);
            }
            else if (rand < 2)
            {
                _doorMatrices.Add(mat);
            }
        }
    }

    private void RenderWalls()
    {
        if (_wallMatrices != null)
        {
            Graphics.DrawMeshInstanced(_wallMesh, 0, _dungeonMaterial, _wallMatrices.ToArray(),
                _wallMatrices.Count);
        }

        if (_doorMatrices != null)
        {
            Graphics.DrawMeshInstanced(_doorMesh, 0, _dungeonMaterial, _doorMatrices.ToArray(),
                _doorMatrices.Count);
        }
    }
}