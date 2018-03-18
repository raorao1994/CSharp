using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessApp.Model
{
    public partial class CardSort
    {
        /// <summary>
        /// 对牌进行从小到大排序，4个一样的优先，3个一样的其次，2个一样的其次，1个的放在最后
        /// </summary>
        /// <param name="_card">要排序的牌</param>
        /// <returns>bool</returns>
        public static bool sort(int[] _card)
        {
            //<summary>
            //对牌进行从小到大排序，4个一样的优先，3个一样的其次，2个一样的其次，1个的放在最后
            //</summary>
            //<param name="_card"> 要排序的牌 </param>
            //<returns></returns>
            int temp = 0;
            for (int i = 0; i < _card.Length; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    if (_card[j] > _card[i])
                    {
                        temp = _card[i];
                        _card[i] = _card[j];
                        _card[j] = temp;
                    }
                }
            }
            int[] a = new int[4];
            int x;
            int n4 = 0;
            int n3 = 0;
            int n2 = 0;
            ///
            /// 把数组中的4个一样的数放在前面。按从小到大排列
            ///
            for (int j = 0; j < _card.Length;)
            {
                if (j + 3 < _card.Length && _card[j] == _card[j + 1] && _card[j + 1] == _card[j + 2] && _card[j + 2] == _card[j + 3])
                {
                    x = j;
                    a[0] = _card[j];
                    a[1] = _card[j + 1];
                    a[2] = _card[j + 2];
                    a[3] = _card[j + 3];
                    for (int k = 0; k < j - n4 * 4 && x > 0 && n4 * 4 != j; k++, x--)
                    {
                        _card[x + 3] = _card[x - 1];
                    }
                    _card[x] = a[0];
                    _card[x + 1] = a[0];
                    _card[x + 2] = a[0];
                    _card[x + 3] = a[0];
                    j += 4;
                    n4++;
                }
                else
                {
                    j++;
                }
            }
            ///
            /// 把数组中的3个一样的数放在前面。按从小到大排列,放在4个一样的后面
            ///
            for (int q = n4 * 4; q < _card.Length;)
            {

                if (q + 2 < _card.Length && _card[q] == _card[q + 1] && _card[q + 1] == _card[q + 2])
                {

                    x = q;
                    a[0] = _card[q];
                    a[1] = _card[q + 1];
                    a[2] = _card[q + 2];
                    for (int k = 0; k < q - n3 * 3 - n4 * 4 && x > 0 && n3 * 3 + n4 * 4 != q && x > n4 * 4 + n3 * 3; k++, x--)
                    {
                        _card[x + 2] = _card[x - 1];
                    }
                    _card[x] = a[0];
                    _card[x + 1] = a[1];
                    _card[x + 2] = a[2];
                    q += 3;
                    n3++;
                }
                else
                {
                    q++;
                }
            }

            for (int p = n4 * 4 + n3 * 3; p < _card.Length;)
            {

                if (p + 1 < _card.Length && _card[p] == _card[p + 1])
                {

                    x = p;
                    a[0] = _card[p];
                    a[1] = _card[p + 1];

                    for (int k = 0; k < p - n3 * 3 - n4 * 4 - n2 * 2 && x > 0 && n3 * 3 + n4 * 4 + n2 * 2 != p && x > n4 * 4 + n3 * 3 + n2 * 2; k++, x--)
                    {
                        _card[x + 1] = _card[x - 1];
                    }
                    _card[x] = a[0];
                    _card[x + 1] = a[1];

                    p += 2;
                    n2++;
                }
                else
                {
                    p++;
                }
            }
            return true;
        }
        /// <summary>
        /// 对牌进行从小到大排序
        /// </summary>
        /// <param name="_card">要排序的牌</param>
        /// <returns>bool</returns>
        public static bool sort1(byte[] _card)
        {
            //对牌进行从小到大排序
            byte temp = 0;
            for (int i = 0; i < _card.Length; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    if (_card[j] > _card[i])
                    {
                        temp = _card[i];
                        _card[i] = _card[j];
                        _card[j] = temp;
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// 对牌的类型进行判断
        /// </summary>
        /// <param name="_card">要判断的类型的牌</param>
        /// <returns>bool</returns>
        public static bool CardType(Card _card)
        {
            bool Is = false;
            switch (_card.number)
            {
                case 1:
                    Is = true; _card.type = Card.Type.单张.ToString();
                    break;
                case 2:
                    if (_card.card[0] == _card.card[1])
                    {
                        Is = true;
                        _card.type = Card.Type.一对子.ToString();
                    }
                    else if ((_card.card[0] == 16 && _card.card[1] == 17))
                    {
                        Is = true;
                        _card.type = Card.Type.双王.ToString();
                    }
                    break;
                case 3:
                    if (_card.card[0] == _card.card[1] && _card.card[1] == _card.card[2])
                    {
                        Is = true;
                        _card.type = Card.Type.三带.ToString();
                    }
                    break;
                case 4:
                    if (_card.card[0] == _card.card[1] && _card.card[1] == _card.card[2] && _card.card[2] != _card.card[3])
                    {
                        Is = true;
                        _card.type = Card.Type.三带一.ToString();
                    }
                    else if (_card.card[0] == _card.card[1] && _card.card[1] == _card.card[2] && _card.card[2] == _card.card[3])
                    {
                        Is = true;
                        _card.type = Card.Type.炸弹.ToString();
                    }
                    break;
                case 5:
                    if (_card.card[0] == _card.card[1] && _card.card[1] == _card.card[2] && _card.card[3] == _card.card[4])
                    {
                        Is = true;
                        _card.type = Card.Type.三带二.ToString();
                    }
                    else if (_card.card[4] < 15 && _card.card[0] == _card.card[1] - 1 && _card.card[0] == _card.card[2] - 2 && _card.card[0] == _card.card[3] - 3 && _card.card[0] == _card.card[4] - 4)
                    {
                        Is = true;
                        _card.type = Card.Type.五连.ToString();
                    }
                    break;
                case 6:
                    if (_card.card[0] == _card.card[1] && _card.card[2] == _card.card[3] && _card.card[4] == _card.card[5] && _card.card[1] == _card.card[2] - 1 && _card.card[3] == _card.card[4] - 1)
                    {
                        Is = true;
                        _card.type = Card.Type.三对子.ToString();
                    }
                    else if (_card.card[0] == _card.card[1] && _card.card[1] == _card.card[2] && _card.card[3] == _card.card[4] && _card.card[4] == _card.card[5] && _card.card[2] == _card.card[3] - 1)
                    {
                        Is = true;
                        _card.type = Card.Type.六带.ToString();
                    }
                    else if (_card.card[5] < 15 && _card.card[0] == _card.card[1] - 1 && _card.card[0] == _card.card[2] - 2 &&
                        _card.card[0] == _card.card[3] - 3 && _card.card[0] == _card.card[4] - 4 && _card.card[0] == _card.card[5] - 5)
                    {
                        Is = true;
                        _card.type = Card.Type.六连.ToString();
                    }
                    else if (_card.card[0] == _card.card[1] && _card.card[1] == _card.card[2] && _card.card[2] == _card.card[3] && _card.card[4] == _card.card[5])
                    {
                        Is = true;
                        _card.type = Card.Type.炸弹带二.ToString();
                    }
                    break;
                case 7:
                    if (_card.card[6] < 15 && _card.card[0] == _card.card[1] - 1 && _card.card[0] == _card.card[2] - 2 &&
                        _card.card[0] == _card.card[3] - 3 && _card.card[0] == _card.card[4] - 4 && _card.card[0] == _card.card[5] - 5 && _card.card[0] == _card.card[6] - 6)
                    {
                        Is = true;
                        _card.type = Card.Type.七连.ToString();
                    }
                    break;

                case 8:
                    if (_card.card[0] == _card.card[1] && _card.card[2] == _card.card[3] && _card.card[4] == _card.card[5] &&
                        _card.card[6] == _card.card[7] && _card.card[1] == _card.card[2] - 1 && _card.card[3] == _card.card[4] - 1 && _card.card[5] == _card.card[6] - 1)
                    {
                        Is = true;
                        _card.type = Card.Type.四对子.ToString();
                    }
                    else if (_card.card[0] == _card.card[1] && _card.card[1] == _card.card[2] && _card.card[3] == _card.card[4] &&
                        _card.card[4] == _card.card[5] && _card.card[2] == _card.card[3] - 1)
                    {
                        Is = true;
                        _card.type = Card.Type.六带二.ToString();
                    }
                    else if (_card.card[7] < 15 && _card.card[0] == _card.card[1] - 1 && _card.card[0] == _card.card[2] - 2 &&
                        _card.card[0] == _card.card[3] - 3 && _card.card[0] == _card.card[4] - 4 && _card.card[0] == _card.card[5] - 5 && _card.card[0] == _card.card[6] - 6 && _card.card[0] == _card.card[7] - 7)
                    {
                        Is = true;
                        _card.type = Card.Type.八连.ToString();
                    }
                    else if (_card.card[0] == _card.card[1] && _card.card[1] == _card.card[2] && _card.card[2] == _card.card[3] && _card.card[4] == _card.card[5] && _card.card[5] != _card.card[6] && _card.card[6] == _card.card[7])
                    {
                        Is = true;
                        _card.type = Card.Type.炸弹带四.ToString();
                    }
                    break;
                case 9:
                    if (_card.card[0] == _card.card[1] && _card.card[1] == _card.card[2] && _card.card[3] == _card.card[4] && _card.card[4] == _card.card[5] &&
                        _card.card[6] == _card.card[7] && _card.card[7] == _card.card[8] && _card.card[2] == _card.card[3] - 1 && _card.card[5] == _card.card[6] - 1)
                    {
                        Is = true;
                        _card.type = Card.Type.九带.ToString();
                    }
                    else if (_card.card[8] < 15 && _card.card[0] == _card.card[1] - 1 && _card.card[0] == _card.card[2] - 2 && _card.card[0] == _card.card[3] - 3 &&
                             _card.card[0] == _card.card[4] - 4 && _card.card[0] == _card.card[5] - 5 && _card.card[0] == _card.card[6] - 6 && _card.card[0] == _card.card[7] - 7 && _card.card[0] == _card.card[8] - 8)
                    {
                        Is = true;
                        _card.type = Card.Type.九连.ToString();
                    }

                    break;
                case 10:
                    if (_card.card[0] == _card.card[1] && _card.card[2] == _card.card[3] && _card.card[4] == _card.card[5] &&
                        _card.card[6] == _card.card[7] && _card.card[8] == _card.card[9] && _card.card[1] == _card.card[2] - 1
                        && _card.card[3] == _card.card[4] - 1 && _card.card[5] == _card.card[6] - 1 && _card.card[7] == _card.card[8] - 1)
                    {
                        Is = true;
                        _card.type = Card.Type.五对子.ToString();
                    }
                    else if (_card.card[0] == _card.card[1] && _card.card[1] == _card.card[2] && _card.card[3] == _card.card[4] &&
                      _card.card[4] == _card.card[5] && _card.card[2] == _card.card[3] - 1 && _card.card[6] == _card.card[7] && _card.card[7] != _card.card[8] && _card.card[8] == _card.card[9])
                    {
                        Is = true;
                        _card.type = Card.Type.六带四.ToString();
                    }
                    else if (_card.card[9] < 15 && _card.card[0] == _card.card[1] - 1 && _card.card[0] == _card.card[2] - 2 && _card.card[0] == _card.card[3] - 3 &&
                           _card.card[0] == _card.card[4] - 4 && _card.card[0] == _card.card[5] - 5 && _card.card[0] == _card.card[6] - 6 && _card.card[0] == _card.card[7] - 7 &&
                        _card.card[0] == _card.card[8] - 8 && _card.card[0] == _card.card[9] - 9)
                    {
                        Is = true;
                        _card.type = Card.Type.十连.ToString();
                    }

                    break;
                case 11:
                    if (_card.card[10] < 15 && _card.card[0] == _card.card[1] - 1 && _card.card[0] == _card.card[2] - 2 && _card.card[0] == _card.card[3] - 3 &&
                           _card.card[0] == _card.card[4] - 4 && _card.card[0] == _card.card[5] - 5 && _card.card[0] == _card.card[6] - 6 && _card.card[0] == _card.card[7] - 7 &&
                        _card.card[0] == _card.card[8] - 8 && _card.card[0] == _card.card[9] - 9 && _card.card[0] == _card.card[10] - 10)
                    {
                        Is = true;
                        _card.type = Card.Type.十一连.ToString();
                    }
                    break;
                case 12:
                    if (_card.card[0] == _card.card[1] && _card.card[1] == _card.card[2] && _card.card[3] == _card.card[4] && _card.card[4] == _card.card[5] &&
                       _card.card[6] == _card.card[7] && _card.card[7] == _card.card[8] && _card.card[2] == _card.card[3] - 1 && _card.card[5] == _card.card[6] - 1)
                    {
                        Is = true;
                        _card.type = Card.Type.九带三.ToString();
                    }
                    else if (_card.card[0] == _card.card[1] && _card.card[2] == _card.card[3] && _card.card[4] == _card.card[5] &&
                        _card.card[6] == _card.card[7] && _card.card[8] == _card.card[9] && _card.card[10] == _card.card[11] && _card.card[1] == _card.card[2] - 1
                        && _card.card[3] == _card.card[4] - 1 && _card.card[5] == _card.card[6] - 1 && _card.card[7] == _card.card[8] - 1 && _card.card[9] == _card.card[10] - 1)
                    {
                        Is = true;
                        _card.type = Card.Type.六对子.ToString();
                    }
                    else if (_card.card[0] == _card.card[1] && _card.card[1] == _card.card[2] && _card.card[3] == _card.card[4] && _card.card[4] == _card.card[5] &&
                           _card.card[6] == _card.card[7] && _card.card[7] == _card.card[8] && _card.card[9] == _card.card[10] && _card.card[10] == _card.card[11] &&
                       _card.card[2] == _card.card[3] - 1 && _card.card[5] == _card.card[6] - 1 && _card.card[8] == _card.card[9] - 1)
                    {
                        Is = true;
                        _card.type = Card.Type.十二带.ToString();
                    }
                    else if (_card.card[10] < 15 && _card.card[0] == _card.card[1] - 1 && _card.card[0] == _card.card[2] - 2 && _card.card[0] == _card.card[3] - 3 &&
                         _card.card[0] == _card.card[4] - 4 && _card.card[0] == _card.card[5] - 5 && _card.card[0] == _card.card[6] - 6 && _card.card[0] == _card.card[7] - 7 &&
                      _card.card[0] == _card.card[8] - 8 && _card.card[0] == _card.card[9] - 9 && _card.card[0] == _card.card[10] - 10 && _card.card[0] == _card.card[11] - 11)
                    {
                        Is = true;
                        _card.type = Card.Type.十二连.ToString();
                    }
                    break;
                case 14:
                    if (_card.card[0] == _card.card[1] && _card.card[2] == _card.card[3] && _card.card[4] == _card.card[5] &&
                       _card.card[6] == _card.card[7] && _card.card[8] == _card.card[9] && _card.card[10] == _card.card[11] && _card.card[12] == _card.card[13] && _card.card[1] == _card.card[2] - 1
                       && _card.card[3] == _card.card[4] - 1 && _card.card[5] == _card.card[6] - 1 && _card.card[7] == _card.card[8] - 1 && _card.card[9] == _card.card[10] - 1 && _card.card[11] == _card.card[12] - 1)
                    {
                        Is = true;
                        _card.type = Card.Type.七对子.ToString();
                    }
                    break;
                case 15:
                    if (_card.card[0] == _card.card[1] && _card.card[1] == _card.card[2] && _card.card[3] == _card.card[4] && _card.card[4] == _card.card[5]
                        && _card.card[6] == _card.card[7] && _card.card[7] == _card.card[8] && _card.card[2] == _card.card[3] - 1 &&
                        _card.card[5] == _card.card[6] - 1 && _card.card[9] == _card.card[10] && _card.card[10] != _card.card[11] && _card.card[11] == _card.card[12]
                        && _card.card[12] != _card.card[13] && _card.card[13] == _card.card[14])
                    {
                        Is = true;
                        _card.type = Card.Type.九带六.ToString();
                    }
                    else if (_card.card[0] == _card.card[1] && _card.card[1] == _card.card[2] && _card.card[3] == _card.card[4] && _card.card[4] == _card.card[5] &&
                           _card.card[6] == _card.card[7] && _card.card[7] == _card.card[8] && _card.card[9] == _card.card[10] && _card.card[10] == _card.card[11] &&
                         _card.card[12] == _card.card[13] && _card.card[13] == _card.card[14] &&
                       _card.card[2] == _card.card[3] - 1 && _card.card[5] == _card.card[6] - 1 && _card.card[8] == _card.card[9] - 1 && _card.card[11] == _card.card[12] - 1)
                    {
                        Is = true;
                        _card.type = Card.Type.十五带.ToString();
                    }
                    break;
                case 16:
                    if (_card.card[0] == _card.card[1] && _card.card[1] == _card.card[2] && _card.card[3] == _card.card[4] && _card.card[4] == _card.card[5]
                         && _card.card[6] == _card.card[7] && _card.card[7] == _card.card[8] && _card.card[9] == _card.card[10] && _card.card[10] == _card.card[11] &&
                         _card.card[2] == _card.card[3] - 1 && _card.card[5] == _card.card[6] - 1 && _card.card[8] == _card.card[9] - 1)
                    {
                        Is = true;
                        _card.type = Card.Type.十二带四.ToString();
                    }
                    else if (_card.card[0] == _card.card[1] && _card.card[2] == _card.card[3] && _card.card[4] == _card.card[5] &&
                     _card.card[6] == _card.card[7] && _card.card[8] == _card.card[9] && _card.card[10] == _card.card[11] && _card.card[12] == _card.card[13] && _card.card[14] == _card.card[15] && _card.card[1] == _card.card[2] - 1
                     && _card.card[3] == _card.card[4] - 1 && _card.card[5] == _card.card[6] - 1 && _card.card[7] == _card.card[8] - 1 && _card.card[9] == _card.card[10] - 1 && _card.card[11] == _card.card[12] - 1 && _card.card[13] == _card.card[14] - 1)
                    {
                        Is = true;
                        _card.type = Card.Type.八对子.ToString();
                    }
                    break;
                case 18:
                    if (_card.card[0] == _card.card[1] && _card.card[2] == _card.card[3] && _card.card[4] == _card.card[5] &&
                     _card.card[6] == _card.card[7] && _card.card[8] == _card.card[9] && _card.card[10] == _card.card[11] && _card.card[12] == _card.card[13] && _card.card[14] == _card.card[15] && _card.card[16] == _card.card[17] && _card.card[1] == _card.card[2] - 1
                     && _card.card[3] == _card.card[4] - 1 && _card.card[5] == _card.card[6] - 1 && _card.card[7] == _card.card[8] - 1 && _card.card[9] == _card.card[10] - 1 && _card.card[11] == _card.card[12] - 1 &&
                     _card.card[13] == _card.card[14] - 1 && _card.card[15] == _card.card[16] - 1)
                    {
                        Is = true;
                        _card.type = Card.Type.九对子.ToString();
                    }
                    break;
                case 20:
                    if (_card.card[0] == _card.card[1] && _card.card[2] == _card.card[3] && _card.card[4] == _card.card[5] &&
                    _card.card[6] == _card.card[7] && _card.card[8] == _card.card[9] && _card.card[10] == _card.card[11] && _card.card[12] == _card.card[13] && _card.card[14] == _card.card[15] && _card.card[16] == _card.card[17] && _card.card[18] == _card.card[19] &&
                    _card.card[1] == _card.card[2] - 1 && _card.card[3] == _card.card[4] - 1 && _card.card[5] == _card.card[6] - 1 && _card.card[7] == _card.card[8] - 1 && _card.card[9] == _card.card[10] - 1 && _card.card[11] == _card.card[12] - 1 &&
                    _card.card[13] == _card.card[14] - 1 && _card.card[15] == _card.card[16] - 1 && _card.card[17] == _card.card[18] - 1)
                    {
                        Is = true;
                        _card.type = Card.Type.十对子.ToString();
                    }
                    break;
                default:
                    break;
            }
            return Is;
        }
        /// <summary>
        /// 对你是否能出新牌进行判断
        /// </summary>
        /// <param name="old_card">要比较的牌</param>
        /// <param name="new_card">你出的牌</param>
        /// <returns>bool</returns>
        public static bool OutCard(Card old_card, Card new_card)
        {
            if (sort(new_card.card))
            {
                if (CardType(new_card))
                {
                    if (old_card.number == 0)
                    {
                        Console.WriteLine("首次出牌成功");
                        return true;
                    }
                    else if ((old_card.type != "双王" && old_card.type != "炸弹" && new_card.type == "炸弹") || new_card.type == "双王")
                    {
                        Console.WriteLine("出炸弹，成功");
                        return true;
                    }
                    else if (new_card.type == old_card.type && new_card.card[0] > old_card.card[0])
                    {
                        Console.WriteLine("出牌成功");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("您出的牌太小，管不上人家的牌");
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("不可以这样出牌");
                    return false;
                }
            }
            return true;
        }
        
        /// <summary>
        /// 牌的类型进行转换
        /// </summary>
        /// <param name="data"></param>
        /// <param name="_card"></param>
        public static void CardConversion(int[] _card, int[] Card)
        {
            CardTypeConversion CardTcn = new CardTypeConversion();
            for (int i = 0; i < _card.Length; i++)
            {
                Card[i] = CardTcn.CardType[_card[i]];
            }

        }

    }
}
