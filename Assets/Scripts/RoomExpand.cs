using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomExpand : MonoBehaviour
{
    [SerializeField] private Vector2 _roomSize;
    [SerializeField] private float _valueRegeneration;
    private float _previousRoomSizeX;
    private float _previousRoomSizeY;

    [Space(10)] [Header("Walls Meshes")] [SerializeField]
    private GameObject _wallMesh;
    [SerializeField] private GameObject _doorMesh;

    [Space(10)]
    public List<GameObject> Walls = new List<GameObject>();
    
    //Hidden
    private int _wallCount;
    private Vector2 _wallSize;

    private GameObject _currentMesh;

    //End variables

    private void Start()
    {
        _wallSize.x = _wallMesh.GetComponent<MeshRenderer>().bounds.size.x;
        _wallSize.y = _wallMesh.GetComponent<MeshRenderer>().bounds.size.x;

        _roomSize.x = _wallSize.x;
        _roomSize.y = _wallSize.y;

        _previousRoomSizeX = _roomSize.x;
        
        _currentMesh = _wallMesh;

        CreateWalls();
    }

    private void Update()
    {
        //Limit to minimum
        if (_roomSize.x <= _wallSize.x)
            _roomSize.x = _wallSize.x;
        if (_roomSize.y <= _wallSize.y)
            _roomSize.y = _wallSize.y;
        
        //Room X
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
        
        //Room Y
        if (_roomSize.y > _previousRoomSizeY + _valueRegeneration)
        {
            _previousRoomSizeY = _roomSize.y;
            CreateWalls();
        }
        else if (_roomSize.y < _previousRoomSizeY - _valueRegeneration)
        {
            _previousRoomSizeY = _roomSize.y;
            CreateWalls();
        }
    }

    private void CreateWalls()
    {
        //Reset Wall List
        foreach (var wall in Walls)
        {
            Destroy(wall);
        }
        Walls.Clear();
        
        // Define size of walls
        int wallCountX = Mathf.Max(1, (int)(_roomSize.x / _wallSize.x));
        int wallCountY = Mathf.Max(1, (int)(_roomSize.y / _wallSize.y));
        float scaleX = (_roomSize.x / wallCountX) / _wallSize.x;
        float scaleY = (_roomSize.y / wallCountY) / _wallSize.y;

        //Define if the wall piece is Wall or Door
 
        
        // Create first row of walls
        for (int i = 0; i < wallCountX; i++)
        {
            Vector3 positionX = transform.position +
                                new Vector3(-_roomSize.x / 2 + _wallSize.x * scaleX / 2 + i * scaleX * _wallSize.x, 0,
                                    +_roomSize.y / 2);
            Quaternion rotationX = transform.rotation;
            Vector3 localScaleX = new Vector3(scaleX, 1, 1);

            GameObject currentWallX = null;
            
            var rand = Random.Range(0, 2);
            if (rand < 1)
            {
                currentWallX = Instantiate(_wallMesh, positionX, rotationX);
            }
            else if (rand < 2)
            {
                currentWallX = Instantiate(_doorMesh, positionX, rotationX);
            }
            
            currentWallX.transform.localScale = localScaleX;
            currentWallX.transform.parent = transform;
            Walls.Add(currentWallX);
        }
        
        // Create second row of walls parallel to the first row
        for (int j = 0; j < wallCountX; j++)
        {
            Vector3 positionY = transform.position +
                                new Vector3(-_roomSize.x / 2 + _wallSize.x * scaleX / 2 + j * scaleX * _wallSize.x, 0,
                                    +_roomSize.y / 2 - _roomSize.y);
            Quaternion rotationY = transform.rotation;
            Vector3 localScaleY = new Vector3(scaleX, 1, 1);

            GameObject currentWallY = null;
            
            var rand = Random.Range(0, 2);
            if (rand < 1)
            {
                currentWallY = Instantiate(_wallMesh, positionY, rotationY);
            }
            else if (rand < 2)
            {
                currentWallY = Instantiate(_doorMesh, positionY, rotationY);
            }
            
            currentWallY.transform.localScale = localScaleY;
            currentWallY.transform.parent = transform;
            Walls.Add(currentWallY);
        }
        
        // Create left wall
        for (int k = 0; k < wallCountY; k++)
        {
            Vector3 positionLeft = transform.position +
                                   new Vector3(-_roomSize.x / 2, 0,
                                       -_roomSize.y / 2 + _wallSize.y * scaleY / 2 + k * scaleY * _wallSize.y);
            Quaternion rotationLeft = Quaternion.Euler(0, 90, 0); // Rotate 90 degrees for left wall
            Vector3 localScaleLeft = new Vector3(scaleY, 1, 1);

            GameObject currentWallLeft = null;
            
            var rand = Random.Range(0, 2);
            if (rand < 1)
            {
                currentWallLeft = Instantiate(_wallMesh, positionLeft, rotationLeft);
            }
            else if (rand < 2)
            {
                currentWallLeft = Instantiate(_doorMesh, positionLeft, rotationLeft);
            }
            
            currentWallLeft.transform.localScale = localScaleLeft;
            currentWallLeft.transform.parent = transform;
            Walls.Add(currentWallLeft);
        }

        // Create right wall
        for (int l = 0; l < wallCountY; l++)
        {
            Vector3 positionRight = transform.position +
                                    new Vector3(+_roomSize.x / 2, 0,
                                        -_roomSize.y / 2 + _wallSize.y * scaleY / 2 + l * scaleY * _wallSize.y);
            Quaternion rotationRight = Quaternion.Euler(0, -90, 0); // Rotate -90 degrees for right wall
            Vector3 localScaleRight = new Vector3(scaleY, 1, 1);

            GameObject currentWallRight = null;
            
            var rand = Random.Range(0, 2);
            if (rand < 1)
            {
                currentWallRight = Instantiate(_wallMesh, positionRight, rotationRight);
            }
            else if (rand < 2)
            {
                currentWallRight = Instantiate(_doorMesh, positionRight, rotationRight);
            }
            
            currentWallRight.transform.localScale = localScaleRight;
            currentWallRight.transform.parent = transform;
            Walls.Add(currentWallRight);
        }
    }
}