using UnityEngine;

namespace Procedural
{
    public enum PieceType
    {
        Wall = 0,
        Door = 1,
        Pillar = 2
    }
    
    public class WallPiece : MonoBehaviour
    {
        public PieceType PieceType;
    }
}