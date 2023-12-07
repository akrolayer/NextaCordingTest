using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
                //string playersString = Console.ReadLine();
                var playersString = "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16";
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
                for (int i = 0; i < RankingList.Count; i++)
                {
                    if (RankingList[i].Rank == rankerCount + 1) break;
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
                Console.WriteLine("どの参加者が何点をいつ取得したか、１行ずつ[参加者,何点,年月日８桁]の形で入力してください。　（例：tarou,100,20231122）");
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

            //ランキング集計のリストに、ポイントを加算(計算量がえぐいのでアルゴリズムを考える)
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
            int Rank = 1;
            var duplicateList = new List<PointList>();
            for (int i = 0; i < RankingList.Count - 1; i++)
            {
                if (RankingList[i].YMD == "0000")
                {
                    p.YMD = "未プレイ";
                }
            }
            Console.WriteLine($"順位,プレイヤー名,点数,更新日");
            int Count = RankingList.Count;
            if (RankingList.Count > 10)
            {
                Count = 10;
            }
            if (Rank <= rankerCount + 1)
            {
                Console.WriteLine($"{i}位：{RankingList[i].PlayerName,8},{RankingList[i].Point,8},{RankingList[i].YMD,8}");
            }

           

            Console.ReadLine();
        }
    }
}

public class PointList
{
    public string PlayerName { get; set; }
    public int Point { get; set; }
    public string YMD { get; set; }
}