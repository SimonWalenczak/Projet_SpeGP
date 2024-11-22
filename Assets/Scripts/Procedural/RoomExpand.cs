using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Procedural
{
    public class RoomExpand : MonoBehaviour
    {
        #region Fields and Properties

        [Header("Room Meshes")] [SerializeField] private RoomMeshesData _roomMeshes;

        [Space(10)] [Header("Room Properties")] private Vector2 _roomSize;

        [Header("Expand Handles")] [SerializeField] private Transform ExpandHandleLength;
        [SerializeField] private Transform ExpandHandleWidth;

        [Space(10)] public bool UsingGenerationStep;

        #region Hidden Properties

        private Camera _mainCamera;

        private float _initialHandleLengthPosition;
        private float _initialHandleWidthPosition;

        private float _previousRoomSizeX;
        private float _previousRoomSizeY;

        private int _wallCount;
        private Vector2 _wallSize;

        private Transform _selectedHandle;
        private Vector3 _handleStartPosition;
        private Vector3 _dragStartPosition;
        private Vector2 _handleOffset = new Vector2(3, 3);

        private float _regenerationStep;
        private float _regenerationStepSavedValue;
        
        int requiredWallCountX;
        int requiredWallCountY;
        float scaleX;
        float scaleY;
        int currentWallIndex;


        private List<GameObject> Walls = new List<GameObject>();
        private List<GameObject> Pillars = new List<GameObject>();

        [HideInInspector] public int NumberOfDoors;

        #endregion

        #endregion

        #region Methods

        private void OnValidate()
        {
            _regenerationStepSavedValue = UsingGenerationStep ? _regenerationStep : 0;
        }

        private void Start()
        {
            _mainCamera = Camera.main;

            _wallSize.x = _roomMeshes.WallMesh.GetComponent<MeshRenderer>().bounds.size.x;
            _wallSize.y = _roomMeshes.WallMesh.GetComponent<MeshRenderer>().bounds.size.x;

            _roomSize = new Vector2(_wallSize.x, _wallSize.y);
            _previousRoomSizeX = _roomSize.x;
            _previousRoomSizeY = _roomSize.y;

            ExpandHandleLength.transform.position = new Vector3(transform.position.x + _roomSize.x / 2 + _handleOffset.x, transform.position.y + 2, transform.position.z);
            ExpandHandleWidth.transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z + _roomSize.y / 2 + _handleOffset.y);

            CreateWalls();
        }

        private void Update()
        {
            float diffLength = ExpandHandleLength.transform.position.x - (transform.position.x + _roomSize.x / 2 + _handleOffset.x);
            float diffWidth = ExpandHandleWidth.transform.position.z - (transform.position.z + _roomSize.y / 2 + _handleOffset.y);

            _roomSize.x = Mathf.Max(_wallSize.x, _roomSize.x + diffLength);
            _roomSize.y = Mathf.Max(_wallSize.y, _roomSize.y + diffWidth);

            ExpandHandleLength.transform.position = new Vector3(transform.position.x + _roomSize.x / 2 + _handleOffset.x, ExpandHandleLength.transform.position.y,
                ExpandHandleLength.transform.position.z);
            ExpandHandleWidth.transform.position = new Vector3(ExpandHandleWidth.transform.position.x, ExpandHandleWidth.transform.position.y,
                transform.position.z + _roomSize.y / 2 + _handleOffset.y);


            HandleInput();

            if (_roomSize.x > _previousRoomSizeX + _regenerationStepSavedValue || _roomSize.x < _previousRoomSizeX - _regenerationStepSavedValue)
            {
                _previousRoomSizeX = _roomSize.x;
                CreateWalls();
            }

            if (_roomSize.y > _previousRoomSizeY + _regenerationStepSavedValue || _roomSize.y < _previousRoomSizeY - _regenerationStepSavedValue)
            {
                _previousRoomSizeY = _roomSize.y;
                CreateWalls();
            }
        }

        private void CreateWalls()
        {
            requiredWallCountX = Mathf.Max(1, (int)(_roomSize.x / _wallSize.x));
            requiredWallCountY = Mathf.Max(1, (int)(_roomSize.y / _wallSize.y));
            scaleX = (_roomSize.x / requiredWallCountX) / _wallSize.x;
            scaleY = (_roomSize.y / requiredWallCountY) / _wallSize.y;

            currentWallIndex = 0;

            GameObject GetOrCreateWall(Vector3 position, Quaternion rotation, Vector3 scale)
            {
                GameObject wall;
                if (currentWallIndex < Walls.Count)
                {
                    wall = Walls[currentWallIndex];
                    wall.transform.position = position;
                    wall.transform.rotation = rotation;
                    wall.transform.localScale = scale;
                }
                else
                {
                    wall = Instantiate(_roomMeshes.WallMesh, position, rotation);
                    wall.transform.localScale = scale;
                    wall.transform.parent = transform;
                    Walls.Add(wall);
                }

                currentWallIndex++;
                return wall;
            }

            for (int i = 0; i < requiredWallCountX; i++)
            {
                Vector3 position = transform.position + new Vector3(-_roomSize.x / 2 + _wallSize.x * scaleX / 2 + i * scaleX * _wallSize.x, 0, +_roomSize.y / 2);
                Quaternion rotation = Quaternion.identity;
                Vector3 scale = new Vector3(scaleX, 1, 1);
                GetOrCreateWall(position, rotation, scale);
            }

            for (int i = 0; i < requiredWallCountX; i++)
            {
                Vector3 position = transform.position + new Vector3(-_roomSize.x / 2 + _wallSize.x * scaleX / 2 + i * scaleX * _wallSize.x, 0, -_roomSize.y / 2);
                Quaternion rotation = Quaternion.identity;
                Vector3 scale = new Vector3(scaleX, 1, 1);
                GetOrCreateWall(position, rotation, scale);
            }

            for (int i = 0; i < requiredWallCountY; i++)
            {
                Vector3 position = transform.position + new Vector3(-_roomSize.x / 2, 0, -_roomSize.y / 2 + _wallSize.y * scaleY / 2 + i * scaleY * _wallSize.y);
                Quaternion rotation = Quaternion.Euler(0, 90, 0);
                Vector3 scale = new Vector3(scaleY, 1, 1);
                GetOrCreateWall(position, rotation, scale);
            }

            for (int i = 0; i < requiredWallCountY; i++)
            {
                Vector3 position = transform.position + new Vector3(+_roomSize.x / 2, 0, -_roomSize.y / 2 + _wallSize.y * scaleY / 2 + i * scaleY * _wallSize.y);
                Quaternion rotation = Quaternion.Euler(0, -90, 0);
                Vector3 scale = new Vector3(scaleY, 1, 1);
                GetOrCreateWall(position, rotation, scale);
            }

            for (int i = currentWallIndex; i < Walls.Count; i++)
            {
                Destroy(Walls[i]);
            }

            Walls.RemoveRange(currentWallIndex, Walls.Count - currentWallIndex);

            CreateCornerAndIntermediatePillars(scaleX, scaleY, requiredWallCountX, requiredWallCountY);
        }

        private void CreateCornerAndIntermediatePillars(float scaleX, float scaleY, int countX, int countY)
        {
            int currentPillarIndex = 0;

            GameObject GetOrCreatePillar(Vector3 position, Vector3 scale)
            {
                GameObject pillar;
                if (currentPillarIndex < Pillars.Count)
                {
                    pillar = Pillars[currentPillarIndex];
                    pillar.transform.position = position;
                    pillar.transform.localScale = scale;
                }
                else
                {
                    pillar = Instantiate(_roomMeshes.PillarMesh, position, Quaternion.identity);
                    pillar.transform.localScale = scale;
                    pillar.transform.parent = transform;
                    Pillars.Add(pillar);
                }

                currentPillarIndex++;
                return pillar;
            }

            Vector3[] cornerPositions = new Vector3[]
            {
                transform.position + new Vector3(-_roomSize.x / 2, 0, +_roomSize.y / 2),
                transform.position + new Vector3(-_roomSize.x / 2, 0, -_roomSize.y / 2),
                transform.position + new Vector3(+_roomSize.x / 2, 0, +_roomSize.y / 2),
                transform.position + new Vector3(+_roomSize.x / 2, 0, -_roomSize.y / 2)
            };

            foreach (var position in cornerPositions)
            {
                GetOrCreatePillar(position, new Vector3(scaleX, 1, scaleY));
            }

            for (int i = 1; i < countX; i++)
            {
                float offsetX = -_roomSize.x / 2 + i * (_roomSize.x / countX);
                GetOrCreatePillar(transform.position + new Vector3(offsetX, 0, +_roomSize.y / 2), new Vector3(scaleX, 1, scaleY));
                GetOrCreatePillar(transform.position + new Vector3(offsetX, 0, -_roomSize.y / 2), new Vector3(scaleX, 1, scaleY));
            }

            for (int i = 1; i < countY; i++)
            {
                float offsetY = -_roomSize.y / 2 + i * (_roomSize.y / countY);
                GetOrCreatePillar(transform.position + new Vector3(-_roomSize.x / 2, 0, offsetY), new Vector3(scaleX, 1, scaleY));
                GetOrCreatePillar(transform.position + new Vector3(+_roomSize.x / 2, 0, offsetY), new Vector3(scaleX, 1, scaleY));
            }

            for (int i = currentPillarIndex; i < Pillars.Count; i++)
            {
                Destroy(Pillars[i]);
            }

            Pillars.RemoveRange(currentPillarIndex, Pillars.Count - currentPillarIndex);
        }

        public void GenerateDoorsRandomPosition()
        {
            ResetWalls();

            List<GameObject> wallPieces = Walls.FindAll(wall => wall.CompareTag("Wall"));

            int numberOfDoorsToGenerate = Mathf.Min(NumberOfDoors, wallPieces.Count);

            for (int i = 0; i < numberOfDoorsToGenerate; i++)
            {
                int randomIndex = Random.Range(0, wallPieces.Count);
                GameObject wallToReplace = wallPieces[randomIndex];
                Vector3 position = wallToReplace.transform.position;
                Quaternion rotation = wallToReplace.transform.rotation;
                Vector3 scale = wallToReplace.transform.localScale;

                GameObject door = Instantiate(_roomMeshes.DoorMesh, position, rotation);
                door.transform.localScale = scale;
                door.transform.parent = transform;

                Walls.Remove(wallToReplace);
                Destroy(wallToReplace);

                Walls.Add(door);

                wallPieces.RemoveAt(randomIndex);
            }
        }

        private void ResetWalls()
        {
            List<GameObject> wallsToReset = new List<GameObject>();

            foreach (var wall in Walls)
            {
                if (wall.CompareTag("Door"))
                {
                    wallsToReset.Add(wall);
                }
            }

            foreach (var wall in wallsToReset)
            {
                GameObject newWall = Instantiate(_roomMeshes.WallMesh, wall.transform.position, wall.transform.rotation);
                newWall.transform.localScale = wall.transform.localScale;
                newWall.transform.parent = transform;

                Walls.Remove(wall);
                Destroy(wall);

                Walls.Add(newWall);
            }
        }

        private void HandleInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.transform.CompareTag("Handle"))
                    {
                        _selectedHandle = hit.transform;
                        _dragStartPosition = GetMousePositionOnPlane(hit.point, Vector3.up);
                        _handleStartPosition = _selectedHandle.position;
                    }
                }
            }

            if (Input.GetMouseButton(0) && _selectedHandle != null)
            {
                Vector3 currentMousePosition = GetMousePositionOnPlane(_dragStartPosition, Vector3.up);
                Vector3 delta = currentMousePosition - _dragStartPosition;

                if (_selectedHandle == ExpandHandleLength)
                {
                    _selectedHandle.position = _handleStartPosition + new Vector3(delta.x, 0, 0);
                }
                else if (_selectedHandle == ExpandHandleWidth)
                {
                    _selectedHandle.position = _handleStartPosition + new Vector3(0, 0, delta.z);
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                _selectedHandle = null;
            }
        }

        private Vector3 GetMousePositionOnPlane(Vector3 planePoint, Vector3 planeNormal)
        {
            Plane plane = new Plane(planeNormal, planePoint);
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            if (plane.Raycast(ray, out float enter))
            {
                return ray.GetPoint(enter);
            }

            return planePoint;
        }

        #endregion
    }
}