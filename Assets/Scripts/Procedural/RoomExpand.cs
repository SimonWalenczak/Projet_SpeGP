using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Procedural
{
    public class RoomExpand : MonoBehaviour
    {
        #region Fields and Properties

        [Header("Room Meshes")] [SerializeField] private RoomMeshesData _roomMeshes;

        [Space(10)] [Header("Room Properties")] [SerializeField] private Vector2 _roomSize;

        [SerializeField] private Transform ExpandHandleLength;
        [SerializeField] private Transform ExpandHandleWidth;

        private float _initialHandleLengthPosition;
        private float _initialHandleWidthPosition;

        [Space(10)] public bool UsingGenerationStep;

        [ConditionalHide("UsingGenerationStep", true)] [SerializeField]
        private float _regenerationStep;

        private float _regenerationStepSavedValue;

        [Space(10)] [field: Header("Door Generation")]
        public bool RamdomDoorPosition;

        [Space(10)] [ConditionalHide("RamdomDoorPosition", true)] [SerializeField] private int _numberOfDoors;
        private List<GameObject> Walls = new List<GameObject>();
        private List<GameObject> Pillars = new List<GameObject>();

        //Hidden
        private float _previousRoomSizeX;
        private float _previousRoomSizeY;

        private int _wallCount;
        private Vector2 _wallSize;

        private Vector2 _handleOffset = new Vector2(3, 3);

        private Transform _selectedHandle;
        private Vector3 _handleStartPosition;
        private Vector3 _dragStartPosition;

        private Camera _mainCamera;

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
            foreach (var wall in Walls)
            {
                Destroy(wall);
            }

            Walls.Clear();

            foreach (var pillar in Pillars)
            {
                Destroy(pillar);
            }

            Pillars.Clear();

            int wallCountX = Mathf.Max(1, (int)(_roomSize.x / _wallSize.x));
            int wallCountY = Mathf.Max(1, (int)(_roomSize.y / _wallSize.y));
            float scaleX = (_roomSize.x / wallCountX) / _wallSize.x;
            float scaleY = (_roomSize.y / wallCountY) / _wallSize.y;

            for (int i = 0; i < wallCountX; i++)
            {
                Vector3 positionX = transform.position + new Vector3(-_roomSize.x / 2 + _wallSize.x * scaleX / 2 + i * scaleX * _wallSize.x, 0, +_roomSize.y / 2);
                Quaternion rotationX = transform.rotation;
                Vector3 localScaleX = new Vector3(scaleX, 1, 1);

                GameObject currentWallX = null;

                currentWallX = Instantiate(_roomMeshes.WallMesh, positionX, rotationX);

                currentWallX.transform.localScale = localScaleX;
                currentWallX.transform.parent = transform;
                Walls.Add(currentWallX);

                if (i < wallCountX - 1)
                {
                    Vector3 pillarPosition = positionX + new Vector3(scaleX * _wallSize.x / 2, 0, 0);
                    GameObject pillar = Instantiate(_roomMeshes.PillarMesh, pillarPosition, Quaternion.identity);
                    pillar.transform.localScale = new Vector3(1, 1, scaleY);
                    pillar.transform.parent = transform;
                    Pillars.Add(pillar);
                }
            }

            for (int j = 0; j < wallCountX; j++)
            {
                Vector3 positionY = transform.position + new Vector3(-_roomSize.x / 2 + _wallSize.x * scaleX / 2 + j * scaleX * _wallSize.x, 0, +_roomSize.y / 2 - _roomSize.y);
                Quaternion rotationY = transform.rotation;
                Vector3 localScaleY = new Vector3(scaleX, 1, 1);

                GameObject currentWallY = null;
                currentWallY = Instantiate(_roomMeshes.WallMesh, positionY, rotationY);

                currentWallY.transform.localScale = localScaleY;
                currentWallY.transform.parent = transform;
                Walls.Add(currentWallY);

                if (j < wallCountX - 1)
                {
                    Vector3 pillarPosition = positionY + new Vector3(scaleX * _wallSize.x / 2, 0, 0);
                    GameObject pillar = Instantiate(_roomMeshes.PillarMesh, pillarPosition, Quaternion.identity);
                    pillar.transform.localScale = new Vector3(1, 1, scaleY);
                    pillar.transform.parent = transform;
                    Pillars.Add(pillar);
                }
            }

            // Create left wall
            for (int k = 0; k < wallCountY; k++)
            {
                Vector3 positionLeft = transform.position + new Vector3(-_roomSize.x / 2, 0, -_roomSize.y / 2 + _wallSize.y * scaleY / 2 + k * scaleY * _wallSize.y);
                Quaternion rotationLeft = Quaternion.Euler(0, 90, 0); // Rotate 90 degrees for left wall
                Vector3 localScaleLeft = new Vector3(scaleY, 1, 1);

                GameObject currentWallLeft = null;

                currentWallLeft = Instantiate(_roomMeshes.WallMesh, positionLeft, rotationLeft);

                currentWallLeft.transform.localScale = localScaleLeft;
                currentWallLeft.transform.parent = transform;
                Walls.Add(currentWallLeft);

                if (k < wallCountY - 1)
                {
                    Vector3 pillarPosition = positionLeft + new Vector3(0, 0, scaleY * _wallSize.y / 2);
                    GameObject pillar = Instantiate(_roomMeshes.PillarMesh, pillarPosition, Quaternion.identity);
                    pillar.transform.localScale = new Vector3(scaleX, 1, 1);
                    pillar.transform.parent = transform;
                    Pillars.Add(pillar);
                }
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


                currentWallRight = Instantiate(_roomMeshes.WallMesh, positionRight, rotationRight);

                currentWallRight.transform.localScale = localScaleRight;
                currentWallRight.transform.parent = transform;
                Walls.Add(currentWallRight);

                if (l < wallCountY - 1)
                {
                    Vector3 pillarPosition = positionRight + new Vector3(0, 0, scaleY * _wallSize.y / 2);
                    GameObject pillar = Instantiate(_roomMeshes.PillarMesh, pillarPosition, Quaternion.identity);
                    pillar.transform.localScale = new Vector3(scaleX, 1, 1);
                    pillar.transform.parent = transform;
                    Pillars.Add(pillar);
                }
            }

            // Create corner pillars
            Vector3 cornerPosition1 = transform.position + new Vector3(-_roomSize.x / 2, 0, +_roomSize.y / 2);
            Vector3 cornerPosition2 = transform.position + new Vector3(-_roomSize.x / 2, 0, -_roomSize.y / 2);
            Vector3 cornerPosition3 = transform.position + new Vector3(+_roomSize.x / 2, 0, +_roomSize.y / 2);
            Vector3 cornerPosition4 = transform.position + new Vector3(+_roomSize.x / 2, 0, -_roomSize.y / 2);

            GameObject cornerPillar1 = Instantiate(_roomMeshes.PillarMesh, cornerPosition1, Quaternion.identity);
            GameObject cornerPillar2 = Instantiate(_roomMeshes.PillarMesh, cornerPosition2, Quaternion.identity);
            GameObject cornerPillar3 = Instantiate(_roomMeshes.PillarMesh, cornerPosition3, Quaternion.identity);
            GameObject cornerPillar4 = Instantiate(_roomMeshes.PillarMesh, cornerPosition4, Quaternion.identity);

            cornerPillar1.transform.localScale = new Vector3(scaleX, 1, scaleY);
            cornerPillar2.transform.localScale = new Vector3(scaleX, 1, scaleY);
            cornerPillar3.transform.localScale = new Vector3(scaleX, 1, scaleY);
            cornerPillar4.transform.localScale = new Vector3(scaleX, 1, scaleY);

            cornerPillar1.transform.parent = transform;
            cornerPillar2.transform.parent = transform;
            cornerPillar3.transform.parent = transform;
            cornerPillar4.transform.parent = transform;

            Pillars.Add(cornerPillar1);
            Pillars.Add(cornerPillar2);
            Pillars.Add(cornerPillar3);
            Pillars.Add(cornerPillar4);
        }

        public void GenerateDoorsRandomPosition()
        {
            ResetWalls();

            List<GameObject> wallPieces = Walls.FindAll(wall => wall.CompareTag("Wall"));

            int numberOfDoorsToGenerate = Mathf.Min(_numberOfDoors, wallPieces.Count);

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
                // Détection du handler via un raycast
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
                // Obtenir la position actuelle de la souris sur le même plan
                Vector3 currentMousePosition = GetMousePositionOnPlane(_dragStartPosition, Vector3.up);
                Vector3 delta = currentMousePosition - _dragStartPosition;

                if (_selectedHandle == ExpandHandleLength)
                {
                    // Déplacer le handle en X
                    _selectedHandle.position = _handleStartPosition + new Vector3(delta.x, 0, 0);
                }
                else if (_selectedHandle == ExpandHandleWidth)
                {
                    // Déplacer le handle en Z
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