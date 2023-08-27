using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RocketPiglet
{ 
public class MainMenuController : MonoSingleton<MainMenuController>
{
        [SerializeField] private GameObject m_EpisodeSelection;
        [SerializeField] private GameObject m_ShipSelection;
        [SerializeField] private GameObject m_Results;
        [SerializeField] private Piglet m_DefaultSpaceShip;

        private void Start()
        {
            //LevelSequenceController.PlayerShip = m_DefaultSpaceShip;
        }
        public void OnButtonStartNew()
        {
            m_EpisodeSelection.gameObject.SetActive(true);

            gameObject.SetActive(false);
        }

        public void OnSelectShip()
        {
            m_ShipSelection.SetActive(true);
            gameObject.SetActive(false);
        }

        public void OnRsults()
        {
            m_Results.SetActive(true);
            gameObject.SetActive(false);
        }



        public void OnButtonExit()
        {
            Application.Quit();
        }
}
}
