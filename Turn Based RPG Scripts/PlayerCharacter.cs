using System;
namespace Assets._scripts
{
    [Serializable]
    public class PlayerCharacter
    {

        public Party party;
        public int number;
        public string name;
        public int money = 0;
        public int score = 0;
        
        public PlayerCharacter()
        {
            party = new Party();
        }
        public PlayerCharacter(Boolean random)
        {
            name = "Default";
            party = new Party(random);
        }
        public PlayerCharacter(string playerName, string[] monsterNames)
        {
            this.name = playerName;
            party = DataClass.createParty(monsterNames);
        }
        public string toString()
        {
            string player = "player = [name = "+name+"], " +party;

            return player;
        }
        public bool partyKOd()
        {
            return party.checkIfLoss();
        }
    }
}
