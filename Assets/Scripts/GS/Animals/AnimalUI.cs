using System.Globalization;
using GS.Animals.States;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GS.Animals
{
    public class AnimalUI : MonoBehaviour
    {
        [FormerlySerializedAs("_rect")] [SerializeField]
        private RectTransform rect;

        private Canvas canvas;
        private Text _age;

        private IAnimal _animal;
        // private Text _danger;
        private Text _hunger;
        private Text _stamina;
        private Text _state;
        private Text _thirst;
        private Text _sex;

        private void Awake()
        {
            _animal = GetComponent<Animal>();
            
            _sex = GenerateItem("Sex");
            _hunger = GenerateItem("Hunger");
            _thirst = GenerateItem("Thirst");
            _stamina = GenerateItem("Stamina");
            _age = GenerateItem("Age");
            // _danger = GenerateItem("In Danger");
            _state = GenerateItem("State");

            canvas = GetComponentInChildren<Canvas>();
            canvas.enabled = false;
        }

        private void Update()
        {
            if (!_animal.IsSelected)
            {
                return;
            }
            
            canvas.enabled = true;
            _hunger.text = _animal.Hunger.Current.ToString();
            _thirst.text = _animal.Thirst.Current.ToString();
            _stamina.text = _animal.Stamina.Current.ToString();
            _age.text = _animal.Age.Current.ToString();
            _sex.text = _animal.Sex.ToString();
        
            if (_animal.StateMachine?.CurrentState == null)
            {
                return;
            }
        
            var state = _animal.StateMachine.CurrentState;
        
            _state.text = state switch {
                BeingAttacked _ => "BeingAttacked",
                Breeding _ => "Breeding",
                Drinking _ => "Drinking",
                Dying _=> "Dying",
                Eating _ => "Eating",
                Hunting _ => "Hunting",
                LookingForFood _ => "LookingForFood",
                LookingForSex _ => "LookingForSex",
                LookingForWater _ => "LookingForWater",
                Resting _ => "Resting",
                Fleeing _ => "Fleeing",
                _ => _state.text
            };
        }

        private Text GenerateItem(string textName)
        {
            var groupObj = new GameObject();
            var groupComp = groupObj.AddComponent<HorizontalLayoutGroup>();
            groupComp.childControlWidth = true;
            groupComp.childControlHeight = true;
            groupObj.name = textName;
            groupObj.transform.SetParent(rect.transform, false);

            var arial = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
            var labelObj = new GameObject();
            var labelComp = labelObj.AddComponent<Text>();
            labelObj.name = "Label";
            labelComp.font = arial;
            labelObj.transform.SetParent(groupObj.transform, false);
            labelComp.text = textName;
            labelComp.color = Color.black;
            labelComp.fontSize = 10;
            labelComp.alignment = TextAnchor.MiddleLeft;

            var textObj = new GameObject();
            var textComp = textObj.AddComponent<Text>();
            textObj.name = "Value";
            textObj.transform.SetParent(groupObj.transform, false);
            textComp.font = arial;
            textComp.color = Color.black;
            textComp.alignment = TextAnchor.MiddleRight;
            textComp.fontSize = 10;

            return textComp;
        }
    }
}
