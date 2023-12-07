using System;
using System.Collections.Generic;
using System.Linq;

namespace NextaCordingTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //参加者のリストを標準入力で,区切りで取得
                Console.WriteLine("参加者のリストを,区切りで入力してください");
                string playersString = Console.ReadLine();
                String[] players = playersString.Split(',');

                //どの参加者が何点をいつ取得したかのリストを標準入力で１行ずつ取得
                var pointList = new List<PointList>();
                pointList = ReadingInput(players);

                //ランキング集計のリストに、プレイヤーを格納
                var RankingList = new List<PointList>();
                RankingList = AddPlayers(RankingList, players);

                //ランキング集計のリストに、ポイントを加算
                RankingList = PointCalculation(RankingList, pointList);

                //トップ何人を表示するか
                int rankerCount = 10;

                //順位確定処理
                RankingList = determineRank(RankingList, rankerCount);

                //画面出力
                Console.WriteLine($"順位,プレイヤー名,点数,最終更新日");

                //トップrankerCount人を表示する
                for (int i = 0; i < rankerCount; i++)
                {
                    if (RankingList[i].Rank == rankerCount + 1) break;
                    if (RankingList[i].YMD == "未プレイ") continue;
                    Console.WriteLine($"{RankingList[i].Rank}位：{RankingList[i].PlayerName,8},{RankingList[i].Point,8},{RankingList[i].YMD,8}");
                }

                Console.WriteLine($"何かキーを押すと終了します");
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine($"予期せぬエラーが発生しました。エラーメッセージ：{e.Message}");
                Console.WriteLine($"何かキーを押すと終了します");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// どの参加者が何点をいつ取得したかのリストを標準入力で１行ずつ取得
        /// </summary>
        /// <param name="players"></param>
        /// <returns></returns>
        public static List<PointList> ReadingInput(string[] players)
        {
            var pointList = new List<PointList>();
            for (int i = 0; ; i++)
            {
                Console.WriteLine("どの参加者が何点をいつ取得したか、１行ずつ[参加者,点数,年月日８桁]の形で入力してください。　（例：tarou,100,20231122）");
                Console.WriteLine("１行ずつ入力してください。終了する場合はENDと入力してください");
                string inputString = Console.ReadLine();
                String[] points = inputString.Split(',');
                if (inputString == "END" || inputString == "end")
                {
                    break;
                }
                if (!players.Contains(points[0]))
                {
                    Console.WriteLine("参加者にいない名前が入力されました。");
                    continue;
                }
                if (points[2].Length != 8)
                {
                    Console.WriteLine("年月日は8桁で入力してください");
                    continue;
                }
                var temp2 = new PointList() { PlayerName = points[0], Point = int.Parse(points[1]), YMD = points[2] };
                pointList.Add(temp2);
            }
            return pointList;
        }

        /// <summary>
        /// ランキング集計のリストに、プレイヤーを格納
        /// </summary>
        /// <param name="RankingList"></param>
        /// <param name="players"></param>
        /// <returns></returns>
        public static List<PointList> AddPlayers(List<PointList> RankingList, string[] players)
        {
            for (int i = 0; i < players.Length; i++)
            {
                var playerItem = new PointList { PlayerName = players[i], Point = 0, YMD = "0000" };
                if (!RankingList.Contains(playerItem))
                {
                    RankingList.Add(playerItem);
                }
            }
            return RankingList;
        }

        /// <summary>
        /// ランキング集計のリストに、ポイントを加算
        /// </summary>
        /// <param name="RankingList"></param>
        /// <param name="pointList"></param>
        /// <returns></returns>
        public static List<PointList> PointCalculation(List<PointList> RankingList, List<PointList> pointList)
        {
            for (int i = 0; i < pointList.Count; i++)
            {
                foreach (PointList p in RankingList)
                {
                    if (p.PlayerName == pointList[i].PlayerName)
                    {
                        p.Point += pointList[i].Point;

                        if (int.Parse(p.YMD) < int.Parse(pointList[i].YMD))
                        {
                            p.YMD = pointList[i].YMD;
                        }
                    }
                }
            }
            return RankingList;
        }

        /// <summary>
        /// 順位確定処理
        /// </summary>
        /// <param name="RankingList"></param>
        /// <returns></returns>
        public static List<PointList> determineRank(List<PointList> RankingList, int rankerCount)
        {
            //年月をキーに昇順でソートした後、得点を降順でソート
            RankingList.Sort((a, b) => int.Parse(a.YMD) - int.Parse(b.YMD));
            RankingList.Sort((a, b) => b.Point - a.Point);
            //順位設定
            for (int i = 0, Rank = 1; i < RankingList.Count - 1; i++, Rank++)
            {
                if (RankingList[i].YMD == "0000")
                {
                    RankingList[i].YMD = "未プレイ";
                    continue;
                }
                RankingList[i].Rank = Rank;
            }
            return RankingList;
        }
    }
}

/// <summary>
/// プレイヤー名がいつ何点を取得したか記録する。
/// </summary>
public class PointList
{
    /// <summary>
    /// 順位
    /// </summary>
    public int Rank { get; set; }
    /// <summary>
    /// プレイヤー名
    /// </summary>
    public string PlayerName { get; set; }
    /// <summary>
    /// 取得した点数
    /// </summary>
    public int Point { get; set; }
    /// <summary>
    /// 最終更新日
    /// </summary>
    public string YMD { get; set; }
}

