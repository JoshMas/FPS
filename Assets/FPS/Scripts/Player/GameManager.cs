using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Scoring
{
    public class GameManager : MonoBehaviour
    {

       
        private int maxScore;
        private List<int> teamScore = new List<int>();

        [SerializeField]
        private List<Text> scoreTexts = new List<Text>();
        

        // Start is called before the first frame update
        void Start()
        {
            for (int i = 0; i < teamScore.Count; i++)
            {
                teamScore[i] = 0;
            }
        }

        public void IncreaseScore(int team)
        {
            teamScore[team]++; 
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

