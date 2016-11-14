using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace Assets._scripts
{
    public class CharacterSelect : MonoBehaviour
    {
        public Canvas menu;
        public Button fightButton;

        //lists are added for easy debugging
        public List<Monster> monList = DataClass.masterMonsterList;
        public List<Monster> Player1M = DataClass.player1.party.monsterList;
        public List<Monster> Player2M = DataClass.player2.party.monsterList;

        public void Start() {}
        // Use this for initialization
        public void pressFight()
        {
            if(DataClass.player1.party.monsterList.Count > 0)
            {
                /*for (int i = 0; i < DataClass.masterMonsterList.Count; i++)
                {
                    if (DataClass.player2.party.monsterList.Count < Party.getMaxPartySize() && !DataClass.monsterTaken[i])
                        DataClass.player2.party.addMonster(DataClass.masterMonsterList[i]);
                }*/
                SceneManager.LoadScene("Battle_with_background");
            }
            
        }
        public void selectMonster()
        {
            for (int i = 0; i < DataClass.masterMonsterList.Count; i++)
            {
                if (tag.Equals(DataClass.masterMonsterList[i].name))
                {
                    if (DataClass.monsterTaken[i])
                    {
                        DataClass.player1.party.removeMonster(DataClass.masterMonsterList[i]);
                        DataClass.monsterTaken[i] = false;
                    }
                    else
                    {
                        DataClass.player1.party.addMonster(DataClass.masterMonsterList[i]);
                        DataClass.monsterTaken[i] = true;
                    }   
                }                    
            }
        }
    }
}
