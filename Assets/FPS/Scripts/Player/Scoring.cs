using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;


namespace Score
{
    public class Scoring : NetworkBehaviour
    {

       
        private int maxScore;
        
        [SyncVar]
        private List<int> teamScore = new List<int>();

        [SerializeField]
        private List<Text> scoreTexts = new List<Text>();
        

        // Start is called before the first frame update
        void Start()
        {
            for (int i = 0; i < scoreTexts.Count; i++)
            {
                teamScore.Add(0);
                UpdateScore(i);
            }
            
          
        }

        [Command]
        public void IncreaseScore(int team)
        {
            teamScore[team]++;
            UpdateScore(team);

            if(teamScore[team] >= maxScore)
            {
                TeamWon(team);
            }

        }
        
        private void TeamWon(int team)
        {

        }
        private void UpdateScore(int team)
        {
            scoreTexts[team].text = "" + teamScore[team];
        }
    }
}

