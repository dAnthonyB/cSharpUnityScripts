//Dennis Bruce
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
namespace Assets
{
    public class TextBoxUI : MonoBehaviour
    {
        public float percentOfScreen = .5F;
        public Text[] textComponents;

        //added for debugging
        public List<Ability> masterAbilityList = DataClass.masterAbilityList;
        public List<Monster> masterMonsterList = DataClass.masterMonsterList;        
        public List<Item> masterItemList = DataClass.masterItemList;
        public List<Effect> masterEffectList = DataClass.masterEffectList;
        public PlayerCharacter Player1 = DataClass.player1;
        public PlayerCharacter Player2 = DataClass.player2;

        void Start()
        {
            transform.localPosition = new Vector2(0, 0);
            percentOfScreen = (1F - percentOfScreen) * 1920;
            textComponents = GetComponentsInChildren<Text>();
            GetComponent<RectTransform>().anchorMax = new Vector2(1,0);
            GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
            GetComponent<RectTransform>().pivot = new Vector2(0, 0);
            float width = (GetComponentInParent<RectTransform>().rect.width - 100) / 1920;
            
            GetComponent<RectTransform>().sizeDelta = new Vector2(width * -percentOfScreen, textComponents.Length * textComponents[0].rectTransform.rect.height/2);
            for(int i = 0; i < textComponents.Length; i++)
            {
                RectTransform temp = textComponents[i].GetComponent<RectTransform>();
                temp.anchorMax = new Vector2(0, 0);
                temp.anchorMin = new Vector2(0, 0);
                temp.pivot = new Vector2(0, 0);
                
                textComponents[i].horizontalOverflow = HorizontalWrapMode.Overflow;
                textComponents[i].text = "";
                textComponents[i].transform.localPosition = transform.position + new Vector3(-GetComponentInParent<Transform>().position.x, -GetComponentInParent<Transform>().position.y - 15 + i * 15);
            }            
        }
        void Update()
        {
            int j = 0;
            for (int i = DataClass.textString.Count-1; i >= 0 && j < textComponents.Length; i--)
            {
                textComponents[j].text = DataClass.textString[i];
                j++;
            }            
        }
    }
}
