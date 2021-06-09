using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;
using UnityEngine.UI;


namespace Shooter.Score
{
    public class Scoring : NetworkBehaviour
    {

       
        private int maxScore;
        
        [SyncVar]
        private List<int> teamScore = new List<int>();


        private List<GameObject> scoreHUD = new List<GameObject>();
        private List<Text> scoreTexts = new List<Text>();
        

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(GetHUD());

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
        
        [Command]
        private void TeamWon(int team)
        {

        }
        private void UpdateScore(int team)
        {
            scoreTexts[team].text = "" + teamScore[team];
        }
        IEnumerator GetHUD()
        {

            while (scoreHUD.Count < 2)
            {
                scoreHUD.AddRange(GameObject.FindGameObjectsWithTag("Score Text"));
                Debug.Log(scoreHUD.Count);
                
               
                yield return null;
                
            }
            for (int i = 0; i < scoreHUD.Count; i++)
            {
                scoreTexts.Add(scoreHUD[i].GetComponent<Text>());
                Debug.Log(scoreTexts[i]);
                teamScore.Add(0);
                UpdateScore(i);
                
            }

           
            

           
            

        }
    }
}

