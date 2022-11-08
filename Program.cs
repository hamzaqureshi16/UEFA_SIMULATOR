using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace UEFA_Simluator
{

    internal class Program
    {

        static Team[,] groups = new Team[8, 4];
        static Hashtable PlayedMatches = new Hashtable();

        public static void grouping()
        {
            string[] teams = {"Juventus","Bayern","Club Brugge","Rapid","Barcelona","Bremen","Udinese","Panathinaikos","Milan","PSV",
            "Schalke","Fenerbahce","Liverpool","Chelsea","Betis","Anderlecht","Arsenel","Ajax","Thun","Sparta TSC","Villareal"
            ,"Benfica","Lille","Man. United","Lyon","Real Madrid","Rosenborg","Olymiacos","Internazionale","Rangers","Artmedia","Porto"};
            int counter = 0;
            for (int i = 0; i < groups.GetLength(0); i++)
            {
                for (int j = 0; j < groups.GetLength(1); j++)
                {
                    groups[i, j] = new Team(teams[counter]);
                    counter++;
                }
            }
        }

        public int max2d(int[,] array)
        {
            int max = Int32.MinValue;
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    max = array[i, j] > max ? array[i, j] : max;
                }
            }
            return max;
        }

        public int[,] sort2d(int[,] array)
        {
            int[] temp = new int[array.GetLength(0) * array.GetLength(1)];
            int counter = 0;
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    temp[counter] = array[i, j];
                    counter++;
                }
            }

            counter = 0;
            Array.Sort(temp);
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    array[i, j] = temp[counter];
                    counter++;
                }
            }


            return array;
        }

        public static int instance(int[] array, int value)
        {
            int count = 0;
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].Equals(value))
                {
                    count++;
                }
            }
            return count;
        }

        //determining winners of the group stage
        public static Team[][] GroupWinnners()
        {
            Team[][] winners = new Team[8][];
            int[][] points = new int[8][];
            int[] maxPoints = new int[8]; 
            int[][] TotalGoals = new int[8][];
            int[] maxTgoals = new int[8];

            /* 
             int[][] homegoals = new int[8][];
             int[][] awaygoals = new int[8][];
             


             
             int[] maxHomeGoals = new int[8];
             int[] maxAwayGoals = new int[8];
             

            

            */

            for (int i = 0; i < points.Length; i++)
             {
                 points[i] = new int[4];
               //  homegoals[i] = new int[4];
                // awaygoals[i] = new int[4];
                 TotalGoals[i] = new int[4];
             }


             for (int i = 0; i < winners.Length; i++)
             {
                 winners[i] = new Team[2];
             }


             //populating these array

             for (int i = 0; i < groups.GetLength(0); i++)
             {
                 for (int j = 0; j < groups.GetLength(1); j++)
                 {
                     points[i][j] = groups[i, j].Points;
                    // homegoals[i][j] = groups[i, j].HomeGoals;
                    // awaygoals[i][j] = groups[i, j].AwayGoals;
                     TotalGoals[i][j] = groups[i, j].Tgoals;
                 }
             }


             for (int i = 0; i < maxPoints.Length; i++)
             {
                 maxTgoals[i] = TotalGoals[i].Max();
                // maxHomeGoals[i] = homegoals[i].Max();
                // maxAwayGoals[i] = awaygoals[i].Max();
                 maxPoints[i] = points[i].Max();

             }

            (int g1, int g2, int p1, int p2) m1Deets, m2Deets;

            for (int i = 0; i < points.Length; i++)
            {
                int[] temp = new int[points[i].Length];
                points[i].CopyTo(temp, 0);
                Array.Sort(temp);
                
                if (temp[^1] == temp[^2])
                {
                   
                    points[i][Array.IndexOf(points[i], temp[^1])]++;
                   
                    temp[^1]++;
                    groups[i, Array.IndexOf(points[i], temp[^1])].Points++;

                    string match1 = groups[i, Array.IndexOf(points[i], temp[^1])].Name + groups[i, Array.IndexOf(points[i], temp[^2])].Name;
                    
                    if (PlayedMatches.ContainsKey(match1))
                    {
                        
                       
                        string match2 = groups[i, Array.IndexOf(points[i], temp[^2])].Name + groups[i, Array.IndexOf(points[i], temp[^1])].Name;// problem here as both values of temp are the same so it returns the same index
                        m1Deets = ((int g1, int g2, int p1, int p2))PlayedMatches[match1];//home
                        m2Deets = ((int g2, int g3, int p1, int p2))PlayedMatches[match2];//away
                        

                        if((m1Deets.p1 + m2Deets.p2) > (m1Deets.p2 + m2Deets.p1))
                        {
                            winners[i][0] = groups[i, Array.IndexOf(points[i], temp[^1])];
                            winners[i][1] = groups[i, Array.IndexOf(points[i], temp[^2])];
                        }
                        else if((m1Deets.p1 + m2Deets.p2) < (m1Deets.p2 + m2Deets.p1))
                        {
                            winners[i][1] = groups[i, Array.IndexOf(points[i], temp[^1])];
                            winners[i][0] = groups[i, Array.IndexOf(points[i], temp[^2])];
                        }
                        else
                        {
                            //commence here
                            if ((m1Deets.g1 + m2Deets.g2) > (m1Deets.g2 + m2Deets.g1))
                            {
                                winners[i][0] = groups[i, Array.IndexOf(points[i], temp[^1])];
                                winners[i][1] = groups[i, Array.IndexOf(points[i], temp[^2])];
                            }
                            else if ((m1Deets.g1 + m2Deets.g2) < (m1Deets.g2 + m2Deets.g1))
                            {
                                winners[i][1] = groups[i, Array.IndexOf(points[i], temp[^1])];
                                winners[i][0] = groups[i, Array.IndexOf(points[i], temp[^2])];
                            }
                            else
                            {
                                //now here
                                if (m2Deets.g2 > m1Deets.g2)
                                {
                                    winners[i][0] = groups[i, Array.IndexOf(points[i], temp[^1])];
                                    winners[i][1] = groups[i, Array.IndexOf(points[i], temp[^2])];
                                }
                                else if(m2Deets.g2 < m1Deets.g2)
                                {
                                    winners[i][1] = groups[i, Array.IndexOf(points[i], temp[^1])];
                                    winners[i][0] = groups[i, Array.IndexOf(points[i], temp[^2])];
                                }
                                else
                                {
                                    int[] temp1 = new int[points[i].Length];
                                    TotalGoals[i].CopyTo(temp1, 0);
                                    Array.Sort(temp1);
                                    temp[^1]++;
                                    groups[i, Array.IndexOf(TotalGoals[i], temp[^1])].Points++;
                                    int rand = Random(2);
                                    if(rand == 0)
                                    {
                                        winners[i][0] = groups[i, Array.IndexOf(TotalGoals[i],  temp[^1])];
                                        winners[i][1] = groups[i, Array.IndexOf(TotalGoals[i], temp1[^2])];
                                    }
                                    else
                                    {
                                        winners[i][1] = groups[i, Array.IndexOf(TotalGoals[i], temp[^1])];
                                        winners[i][0] = groups[i, Array.IndexOf(TotalGoals[i], temp1[^2])];
                                    }
                                }
                            }
                        }
                        
                    }
                    
                    

                }
                else
                {

                   
                    //winnner of the group
                    winners[i][0] = groups[i, Array.IndexOf(points[i], temp[^1])];
                    //runner up of the group
                    winners[i][1] = groups[i, Array.IndexOf(points[i], temp[^2])];

                }
            }


            
           
            return winners;
        }


        public static Team Play(ref Team[][] grp, int group, int team, ref bool[][] stats)
        {
            int winnerOrRunner = team == 0 ? 1 : 0;
            int oppGroup = group;
            //playing home
            while (true)
            {
                oppGroup = Random(grp.Length);
                if (oppGroup != group && !(stats[oppGroup][winnerOrRunner]) && !(stats[group][team]))
                {
                    break;
                }
            }

            stats[oppGroup][winnerOrRunner] = true;
            stats[group][team] = true;

            grp[group][team].resetGoals();
            grp[oppGroup][winnerOrRunner].resetGoals();

            int goals1 = Random(15), goals2 = Random(15);

            //playing away
            int g1 = Random(15);
            int g2 = Random(15);
            grp[oppGroup][winnerOrRunner].AwayGoals += (goals2 + g1);
            grp[group][team].HomeGoals += (goals1 + g2);

            System.Threading.Thread.Sleep(200);
            Console.Beep();
            Console.WriteLine($"\t\t\t\t\t{grp[group][team].Name} vs {grp[oppGroup][winnerOrRunner].Name} {goals1}:{goals2}  {g1}:{g2}");

            if (grp[group][team].Tgoals > grp[oppGroup][winnerOrRunner].Tgoals)
            {
                return grp[group][team];
            }
            else if (grp[group][team].Tgoals < grp[oppGroup][winnerOrRunner].Tgoals)
            {
                return grp[oppGroup][winnerOrRunner];
            }
            else
            {
                if (grp[group][team].AwayGoals > grp[oppGroup][winnerOrRunner].AwayGoals)
                {
                    return grp[group][team];
                }
                else if (grp[group][team].AwayGoals < grp[oppGroup][winnerOrRunner].AwayGoals)
                {
                    return grp[oppGroup][winnerOrRunner];
                }
                else
                {
                    int rand = Random(2);
                    return rand == 0 ? grp[group][team] : grp[oppGroup][winnerOrRunner];
                }
            }

        }



        public static Team[] Knockout(Team[][] groupwinners)
        {
            int grpno = 1;
            Console.WriteLine("\nGroup Table at the end of the Group Stage");
            foreach (Team[] gr in groupwinners)
            {
                Console.WriteLine($"Group {grpno}");
                grpno++;
                foreach (Team grp in gr)
                {
                    Console.WriteLine($"{grp.Name}\n\t\tPoints:{grp.Points}\t\tTotal Goals:{grp.Tgoals}\t\tHome goals:{grp.HomeGoals}\t\tAway goals:{grp.AwayGoals}");
                    
                }
                Console.WriteLine("---------------------------------------------------------------------------------------------------------------------");
            }

            Team[] KnockWinners = new Team[groupwinners.Length];
            bool[][] PlayStatus = new bool[groupwinners.Length][];
            for (int i = 0; i < PlayStatus.Length; i++)
            {
                PlayStatus[i] = new bool[2];
            }
            Console.Write("\n\t\t\t\t\tComencing Knockouts\n\n");
            for (int i = 0; i < groupwinners.Length; i++)
            {
                KnockWinners[i] = Play(ref groupwinners, i, 0, ref PlayStatus);
            }


            Console.WriteLine("Knockout Stage winners are: ");
             
            foreach (Team grp in KnockWinners)
            {
 
                    Console.WriteLine($"{grp.Name}\n\t\tPoints:{grp.Points}\t\tTotal Goals:{grp.Tgoals}\t\tHome goals:{grp.HomeGoals}\t\tAway goals:{grp.AwayGoals}");

                Console.WriteLine("---------------------------------------------------------------------------------------------------------------------");
            }
            return KnockWinners;
        }

        public static Team PlayQuarterFinals(ref Team[] grp, int team, ref bool[] status)
        {

            status[team] = true;

            int Opponent = Array.IndexOf(status, false);

            //Console.Write("cla");
            status[Opponent] = true;

            grp[team].resetGoals();
            grp[Opponent].resetGoals();

            int g1 = Random(15), g2 = Random(15), g3 = Random(15), g4 = Random(15);

            grp[team].HomeGoals = g1;
            grp[Opponent].AwayGoals = g2;

            grp[Opponent].HomeGoals += g3;
            grp[team].AwayGoals += g4;

            System.Threading.Thread.Sleep(200);
            Console.Beep();
            Console.WriteLine($"\t\t\t\t\t{grp[team].Name} vs {grp[Opponent].Name}  {g1}:{g2}  {g3}:{g4}");

            if (grp[team].Tgoals > grp[Opponent].Tgoals)
            {
                return grp[team];
            }
            else if (grp[team].Tgoals < grp[Opponent].Tgoals)
            {
                return grp[Opponent];
            }
            else
            {
                if (grp[team].AwayGoals > grp[Opponent].AwayGoals)
                {
                    return grp[team];
                }
                else if (grp[team].AwayGoals < grp[Opponent].AwayGoals)
                {
                    return grp[Opponent];
                }
                else
                {
                    int rand = Random(2);
                    return rand == 0 ? grp[team] : grp[Opponent];
                }
            }





        }

        public static Team[] QuarterFinals(Team[] Kwinners)
        {
            Console.WriteLine($"\n\n\t\t\t\t\tPlaying quarter finals\n\n");
            Team[] Qwinners = new Team[4];
            bool[] status = new bool[Kwinners.Length];
            for (int i = 0; i < Qwinners.Length; i++)
            {
                Qwinners[i] = PlayQuarterFinals(ref Kwinners, i, ref status);
            }

           
            Console.WriteLine("Quarter Final Stage winners are: ");

            foreach (Team grp in Qwinners)
            {

                Console.WriteLine($"{grp.Name}\n\t\tPoints:{grp.Points}\t\tTotal Goals:{grp.Tgoals}\t\tHome goals:{grp.HomeGoals}\t\tAway goals:{grp.AwayGoals}");

                Console.WriteLine("---------------------------------------------------------------------------------------------------------------------");
            }
            return Qwinners;
        }


        public static Team[] SemiFinals(Team[] teams)
        {
            Team[] Fwinners = new Team[2];
            bool[] status = new bool[teams.Length];
           
            Console.WriteLine("\n\n\t\t\t\t\tplaying semis\n\n");

            for (int i = 0; i < Fwinners.Length; i++)
            {
                Fwinners[i] = PlayQuarterFinals(ref teams, i, ref status);
            }
            Console.WriteLine("Semi-Final Stage winners are: ");

            foreach (Team grp in Fwinners)
            {

                Console.WriteLine($"{grp.Name}\n\t\tPoints:{grp.Points}\t\tTotal Goals:{grp.Tgoals}\t\tHome goals:{grp.HomeGoals}\t\tAway goals:{grp.AwayGoals}");

                Console.WriteLine("---------------------------------------------------------------------------------------------------------------------");
            }
            return Fwinners;
        }

        

        public static Team Final(Team[] finalist)
        {
            Console.WriteLine("\n\n\t\t\t\t\t Get ready we're now in the FINALS!!!\n\n");
            int g1 = Random(15), g2 = Random(15);

            for (int i = 0; i < finalist.Length; i++){
                finalist[i].resetGoals();
                finalist[i].HomeGoals = g1;
            }
            Console.WriteLine($"\t\t\t\t\tWait the final is being played");
            System.Threading.Thread.Sleep(3000);
            Console.Beep();
            Console.WriteLine($"\n\n\t\t\t\t\tThe result of the final is {finalist[0].Name} vs {finalist[1].Name} {g1}:{g2}");
            if(g1 > g2)
            {
                return finalist[0];
            }
            else if(g1 < g2)
            {
                return finalist[1];
            }
            else
            {
                int rand = Random(2);
                return rand == 0? finalist[0] : finalist[1];
            } 
        }

        static int mday = 1;// for match day
        
        public static int GroupStage(int i = 0)
        {
            if(i == 0)
            {
                grouping();
            }

            int[] MatchSeq = { 23,41,12,34,31,24,13,42,32,14,21,43};

          if(i >= MatchSeq.Length)
            {
               
                    Console.WriteLine("\t\t\t\t\tWinner is " + Final(SemiFinals(QuarterFinals(Knockout(GroupWinnners())))).Name);
                //GroupWinnners();
                return 0;
            }
                
        
                
                    if( i ==0 || i ==2 || i ==4 || i == 6 || i == 8 || i == 10)
            {
                Console.WriteLine($"\n\t\t\t\t\tMatchday:{mday}");
                mday++;
            }

                for (int j = 0; j < groups.GetLength(0); j++)
                {
                    
                    int goal1 = Random(15), goal2 = Random(15);

                    groups[j, (MatchSeq[i] / 10) - 1].HomeGoals += goal1;
                    groups[j, (MatchSeq[i] % 10) - 1].AwayGoals += goal2;
                    string k = groups[j, (MatchSeq[i] / 10) - 1].Name + (groups[j, (MatchSeq[i] % 10) - 1].Name) ;
                   
                    if (goal1 > goal2)
                    {
                    
                        groups[j, (MatchSeq[i] / 10) - 1].Points += 3;
                    //editing
                    PlayedMatches.Add(k, (goal1,goal2,3,0));
                    }
                    else if (goal1 == goal2)
                    {
                        groups[j, (MatchSeq[i] / 10) - 1].Points += 1;
                        groups[j, (MatchSeq[i] % 10) - 1].Points += 1;
                    PlayedMatches.Add(k, (goal1, goal2, 1, 1));
                }
                    else
                    {
                        groups[j, (MatchSeq[i] % 10) - 1].Points += 3;
                    PlayedMatches.Add(k, (goal1, goal2, 0, 3));
                }
                System.Threading.Thread.Sleep(200);
                Console.Beep();
                Console.WriteLine($"\t\t\t\t\t{groups[j, (MatchSeq[i] / 10) - 1].Name} vs {groups[j, (MatchSeq[i] % 10) - 1].Name} {goal1}:{goal2}");

                }





            return GroupStage(i + 1);
        }

        public static int Random(int seed)
        {

            return new Random().Next(seed);
        }

        static void Main(string[] args)
        {
             Console.ForegroundColor = ConsoleColor.Black;
             Console.BackgroundColor = ConsoleColor.DarkBlue;
             Console.Clear();
             
            GroupStage();
            
            



        }
    }
}