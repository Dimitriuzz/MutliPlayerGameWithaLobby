using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SpaceShooter
{ 
public class PlayerStatistics
{
        public int numKills;
        public int score;
        public int time;
        public int bonusScore;
        public int finalScore;

        public void Reset()
        {
            numKills = 0;
            score = 0;
            time = 0;
            bonusScore = 0;
            finalScore = 0;
        }
    }
}
