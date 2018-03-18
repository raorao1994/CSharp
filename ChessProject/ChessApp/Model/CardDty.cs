using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessApp.Model
{
    public class CardDty
    {
        public Dictionary<int, string> CardType = new Dictionary<int, string>();
        public void CarDty()
        {
            CardType.Add(0, "红桃3");
            CardType.Add(1, "方片3");
            CardType.Add(2, "黑桃3");
            CardType.Add(3, "草花3");

            CardType.Add(4, "红桃4");
            CardType.Add(5, "方片4");
            CardType.Add(6, "黑桃4");
            CardType.Add(7, "草花4");

            CardType.Add(8, "红桃5");
            CardType.Add(9, "方片5");
            CardType.Add(10, "黑桃5");
            CardType.Add(11, "草花5");

            CardType.Add(12, "红桃6");
            CardType.Add(13, "方片6");
            CardType.Add(14, "黑桃6");
            CardType.Add(15, "草花6");

            CardType.Add(16, "红桃7");
            CardType.Add(17, "方片7");
            CardType.Add(18, "黑桃7");
            CardType.Add(19, "草花7");

            CardType.Add(20, "红桃8");
            CardType.Add(21, "方片8");
            CardType.Add(22, "黑桃8");
            CardType.Add(23, "草花8");

            CardType.Add(24, "红桃9");
            CardType.Add(25, "方片9");
            CardType.Add(26, "黑桃9");
            CardType.Add(27, "草花9");

            CardType.Add(28, "红桃10");
            CardType.Add(29, "方片10");
            CardType.Add(30, "黑桃10");
            CardType.Add(31, "草花10");

            CardType.Add(32, "红桃J");
            CardType.Add(33, "方片J");
            CardType.Add(34, "黑桃J");
            CardType.Add(35, "草花J");

            CardType.Add(36, "红桃Q");
            CardType.Add(37, "方片Q");
            CardType.Add(38, "黑桃Q");
            CardType.Add(39, "草花Q");

            CardType.Add(40, "红桃K");
            CardType.Add(41, "方片K");
            CardType.Add(42, "黑桃K");
            CardType.Add(43, "草花K");

            CardType.Add(44, "红桃A");
            CardType.Add(45, "方片A");
            CardType.Add(46, "黑桃A");

            CardType.Add(47, "红桃2");
        }
    }
}
