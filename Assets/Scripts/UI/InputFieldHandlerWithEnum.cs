using System;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;

namespace UI
{
    public class InputFieldHandlerWithEnum : MonoBehaviour
    {
        public TMP_InputField inputField;
        public MonoBehaviour scriptCible;
        [HideInInspector] public string selectedVariable;

        private string[] publicVariables;

        private void OnValidate()
        {
            publicVariables = GetPublicVariables(scriptCible);
        }

        private void Start()
        {
            inputField.onValueChanged.AddListener(OnValueChanged);
            inputField.onEndEdit.AddListener(OnEndEdit);
        }

        private void OnValueChanged(string value)
        {
            string filteredValue = new string(value.Where(char.IsDigit).ToArray());
            inputField.text = filteredValue;
        }

        private void OnEndEdit(string value)
        {
            if (!string.IsNullOrEmpty(value) && int.TryParse(value, out int result))
            {
                inputField.GetComponentInChildren<TMP_Text>().text = value;
                AssignValueToSelectedVariable(result);
            }
        }

        private void AssignValueToSelectedVariable(int value)
        {
            if (scriptCible != null && !string.IsNullOrEmpty(selectedVariable))
            {
                var field = scriptCible.GetType().GetField(selectedVariable);
                if (field != null && field.FieldType == typeof(int))
                {
                    field.SetValue(scriptCible, value);
                    Debug.Log($"Valeur {value} assignée à {selectedVariable} dans {scriptCible.name}");
                }
                else
                {
                    Debug.LogError($"La variable {selectedVariable} est introuvable ou n'est pas un int.");
                }
            }
        }

        private string[] GetPublicVariables(MonoBehaviour targetScript)
        {
            if (targetScript == null) 
                return Array.Empty<string>();

            return targetScript.GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.Public)
                .Where(field => field.FieldType == typeof(int))
                .Select(field => field.Name)
                .ToArray();
        }

        private void OnDestroy()
        {
            inputField.onValueChanged.RemoveListener(OnValueChanged);
            inputField.onEndEdit.RemoveListener(OnEndEdit);
        }

        public string[] GetVariableNames() => publicVariables;
    }
}