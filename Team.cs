using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UEFA_Simluator
{
    internal class Team
    {
        private string name;
        private (int home, int away) goals;
        private int points;


        public Team( string nam )
        {
            
            name = nam;
            goals.home = 0;
            goals.away = 0;
            points = 0;
        }

        public string Name
        {
            get { return name; }
            set {
                if (!value.Equals(null) && value.Length >= 2) {
                    name = value;
                } 
            }
        }

        public int Tgoals
        {
            get { return goals.home + goals.away; }
        }


        public int HomeGoals
        {
            get { return goals.home; }
            set {
                if (value >= 0) {
                    goals.home = value;
                     }
                }
        }


        public int AwayGoals
        {
            get { return goals.away;  }
            set
            {
                if (value >= 0) { 
                    goals.away = value;
                }
            }
        }


        public int Points
        {
            get { return points;  }
            set { if(points >= 0)
                    {
                    points = value;
                    }      
                 }
        }


        public void resetGoals()
        {
            this.HomeGoals = default;
            this.AwayGoals  = default;
            
        }



    }
}
