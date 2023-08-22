using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace SpaceShooter
{ 
public class ResultPanelController : MonoSingleton<ResultPanelController>
{
        [SerializeField] private Text m_Kills;
        [SerializeField] private Text m_Score;
        [SerializeField] private Text m_Time;
        [SerializeField] private Text m_Result;
        [SerializeField] private Text m_ButtonNextText;
        [SerializeField] private Text m_BonusScoreText;
        [SerializeField] private Text m_FinalScoreText;

        private bool m_Success;

        private void Start()
        {
            gameObject.SetActive(false);
        }

        public void ShowResults(PlayerStatistics levelResults, bool succes)
        {
            gameObject.SetActive(true);
            m_Success = succes;
            

            m_Result.text = succes ? "Win" : "Lose";
            m_ButtonNextText.text = succes ? "Next" : "Restart";

            m_Kills.text = "Kills :" + levelResults.numKills.ToString();
            m_Score.text = "Score :" + levelResults.score.ToString();
            m_Time.text = "Time :" + levelResults.time.ToString();
            m_BonusScoreText.text = "Time Score: " + levelResults.bonusScore.ToString();
            m_FinalScoreText.text = "Final Score: " + levelResults.finalScore.ToString();

            Time.timeScale = 0;
        }

        public void OnButtonNextAction()
        {
            gameObject.SetActive(false);
            Time.timeScale = 1;
            if(m_Success)
            {
                //LevelSequenceController.Instance.AdvanceLevel();
            }
            else
            {
                //LevelSequenceController.Instance.RestartLevel();
            }
        }    
    }
}
