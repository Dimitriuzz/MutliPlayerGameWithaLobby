using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace SpaceShooter
{
    public class GlobalResultPanelController : MonoSingleton<ResultPanelController>
    {
        [SerializeField] private Text m_GKills;
        [SerializeField] private Text m_GScore;
        [SerializeField] private Text m_GTime;
        [SerializeField] private Text m_GButtonNextText;
        [SerializeField] private GameObject m_MainMenu;

        private int m_PlayerKills;
        private int m_PlayerScore;
        private int m_PlayerTime;



        private void Start()
        {
            gameObject.SetActive(false);

        }

        public void ShowResults()
        {
            gameObject.SetActive(true);
            m_PlayerKills = PlayerPrefs.GetInt("PlayerKills", 0);
            m_PlayerScore = PlayerPrefs.GetInt("PlayerScore", 0);
            m_PlayerTime = PlayerPrefs.GetInt("PlayerTime", 0);
            Debug.Log("kills" + m_PlayerKills + " score" + m_PlayerScore + " time " + m_PlayerTime);
            m_GKills.text = "Kills :" + m_PlayerKills.ToString();
            m_GScore.text = "Score :" + m_PlayerScore.ToString();
            m_GTime.text = "Time :" + m_PlayerTime.ToString();


        }

        public void OnButtonNextAction()
        {
            gameObject.SetActive(false);
            m_MainMenu.SetActive(true);

        }

        public void ResetResults()
        {
            PlayerPrefs.SetInt("PlayerTime",0);
            PlayerPrefs.SetInt("PlayerKills", 0);
            PlayerPrefs.SetInt("PlayerScore", 0);
            ShowResults();

        }
    }
}
