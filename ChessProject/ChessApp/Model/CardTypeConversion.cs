using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessApp.Model
{
    /// <summary>
    /// 扑克牌类型
    /// </summary>
    public class CardTypeConversion
    {
        //扑克牌类型字典
        public Dictionary<int, int> CardType = new Dictionary<int, int>();

        #region 标准扑克牌
        ///// <summary>
        ///// 标准扑克牌
        ///// </summary>
        //public CardTypeConversion()
        //{ 
        //    // 3 4 5 6 7 8 9 10 11--J 12--Q 13--K  14--A 15--2 16--小王  17--大王
        //    CardType.Add(0, 3);
        //    CardType.Add(1, 3);
        //    CardType.Add(2, 3);
        //    CardType.Add(3, 3);

        //    CardType.Add(4, 4);
        //    CardType.Add(5, 4);
        //    CardType.Add(6, 4);
        //    CardType.Add(7, 4);

        //    CardType.Add(8, 5);
        //    CardType.Add(9, 5);
        //    CardType.Add(10, 5);
        //    CardType.Add(11, 5);

        //    CardType.Add(12, 6);
        //    CardType.Add(13, 6);
        //    CardType.Add(14, 6);
        //    CardType.Add(15, 6);

        //    CardType.Add(16, 7);
        //    CardType.Add(17, 7);
        //    CardType.Add(18, 7);
        //    CardType.Add(19, 7);

        //    CardType.Add(20, 8);
        //    CardType.Add(21, 8);
        //    CardType.Add(22, 8);
        //    CardType.Add(23, 8);

        //    CardType.Add(24, 9);
        //    CardType.Add(25, 9);
        //    CardType.Add(26, 9);
        //    CardType.Add(27, 9);

        //    CardType.Add(28, 10);
        //    CardType.Add(29, 10);
        //    CardType.Add(30, 10);
        //    CardType.Add(31, 10);

        //    CardType.Add(32, 11);
        //    CardType.Add(33, 11);
        //    CardType.Add(34, 11);
        //    CardType.Add(35, 11);

        //    CardType.Add(36, 12);
        //    CardType.Add(37, 12);
        //    CardType.Add(38, 12);
        //    CardType.Add(39, 12);

        //    CardType.Add(40, 13);
        //    CardType.Add(41, 13);
        //    CardType.Add(42, 13);
        //    CardType.Add(43, 13);

        //    CardType.Add(44, 14);
        //    CardType.Add(45, 14);
        //    CardType.Add(46, 14);
        //    CardType.Add(47, 14);

        //    CardType.Add(48, 15);
        //    CardType.Add(49, 15);
        //    CardType.Add(50, 15);
        //    CardType.Add(51, 15);

        //    CardType.Add(52, 16);
        //    CardType.Add(53, 17);
        //} 
        #endregion

        /// <summary>
        /// 跑得快扑克牌
        /// </summary>
        public CardTypeConversion()
        {
            // 3 4 5 6 7 8 9 10 11--J 12--Q 13--K  14--A 15--2

            CardType.Add(0, 3);//红桃3
            CardType.Add(1, 3);//梅花3
            CardType.Add(2, 3);//方块3
            CardType.Add(3, 3);//黑桃3

            CardType.Add(4, 4);//红桃4
            CardType.Add(5, 4);//梅花4
            CardType.Add(6, 4);//方块4
            CardType.Add(7, 4);//黑桃4

            CardType.Add(8, 5); //红桃5
            CardType.Add(9, 5); //梅花5
            CardType.Add(10, 5);//方块5
            CardType.Add(11, 5);//黑桃5

            CardType.Add(12, 6);//红桃6
            CardType.Add(13, 6);//梅花6
            CardType.Add(14, 6);//方块6
            CardType.Add(15, 6);//黑桃6

            CardType.Add(16, 7);//红桃7
            CardType.Add(17, 7);//梅花7
            CardType.Add(18, 7);//方块7
            CardType.Add(19, 7);//黑桃7

            CardType.Add(20, 8);//红桃8
            CardType.Add(21, 8);//梅花8
            CardType.Add(22, 8);//方块8
            CardType.Add(23, 8);//黑桃8

            CardType.Add(24, 9); //红桃9
            CardType.Add(25, 9); //梅花9
            CardType.Add(26, 9); //方块9
            CardType.Add(27, 9); //黑桃9

            CardType.Add(28, 10); //红桃10
            CardType.Add(29, 10); //梅花10
            CardType.Add(30, 10); //方块10
            CardType.Add(31, 10); //黑桃10

            CardType.Add(32, 11); //红桃J
            CardType.Add(33, 11); //梅花J
            CardType.Add(34, 11); //方块J
            CardType.Add(35, 11); //黑桃J

            CardType.Add(36, 12); //红桃Q
            CardType.Add(37, 12); //梅花Q
            CardType.Add(38, 12); //方块Q
            CardType.Add(39, 12); //黑桃Q

            CardType.Add(40, 13); //红桃K
            CardType.Add(41, 13); //梅花K
            CardType.Add(42, 13); //方块K
            CardType.Add(43, 13); //黑桃K

            CardType.Add(44, 14); //红桃A
            CardType.Add(45, 14); //梅花A
            CardType.Add(46, 14); //方块A

            CardType.Add(47, 15); //红桃2
        }
    }
}
