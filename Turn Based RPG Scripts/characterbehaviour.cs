using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Assets._scripts
{
    public class characterbehaviour : MonoBehaviour
    {

        Animator animator;
        
        public Monster mon;
        public PlayerCharacter owner;
        public string action;

        public void Start()
        {
            action = "idle";
            string temp = tag;
            if (tag.Contains("1"))
            {
                owner = DataClass.player1;
                temp = temp.Remove(temp.Length - 1);
            }
                
            else if (tag.Contains("2"))
            {
                owner = DataClass.player2;
                temp = temp.Remove(temp.Length - 1);
            }

            for (int i = 0; i < owner.party.monsterList.Count; i++)
            {
                if (temp.Equals(owner.party.monsterList[i].name))
                {
                    mon = owner.party.monsterList[i];
                    break;
                }
            }
            animator = GetComponent<Animator>();
        }

        void Update(){
            if (action.Equals("tackle"))
                action = "attack";
            if (!action.Equals("idle"))
            {
                animator.SetTrigger(action);
                action = "idle";
            }
                
        }
        public void setAction(string a)
        {
            action = a;
        }
    }
}

