using TMPro;
using UnityEngine;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _doorsGenerateInputField;

        public void OnClickDoorGeneration()
        {
            RoomEditorManager.Instance.RoomExpand.GenerateDoorsRandomPosition();
        }
    }
}
