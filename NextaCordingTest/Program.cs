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
            //参加者のリストを標準入力で,区切りで取得
            Console.WriteLine("参加者のリストを,区切りで入力してください");
            string playersString = Console.ReadLine();
            String[] players = playersString.Split(',');

            //どの参加者が何点をいつ取得したかのリストを標準入力で１行ずつ取得
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

            //ランキング集計のリストに、プレイヤーを格納
            var RankingList = new List<PointList>();
            for(int i = 0; i < players.Length; i++)
            {
                var temp = new PointList { PlayerName = players[i], Point = 0, YMD = "00000000" };
                if (!RankingList.Contains(temp)){
                    RankingList.Add(temp);
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

            //画面出力
            RankingList.Sort((a,b) => b.Point- a.Point);

            int Rank = 1;
            for(int i= 0;i < RankingList.Count - 1; i++) { 
                if (RankingList[1].YMD == "未プレイ")
                {
                    continue;
                }
                if (RankingList[i].Point == RankingList[i+1].Point)
                {
                    RankingList[i].Rank = Rank;
                }
                else
                {
                    RankingList[i].Rank = Rank;
                    Rank++;
                }
            }
            if(Rank <= 11)
                RankingList[RankingList.Count-1].Rank = Rank;

            
            Console.WriteLine($"順位,プレイヤー名,点数,最終更新日");


            for (int i = 0; i < RankingList.Count; i++)
            {
                if (RankingList[i].Rank == 11) break;
                Console.WriteLine($"{RankingList[i].Rank}位：{RankingList[i].PlayerName,8},{RankingList[i].Point,8},{RankingList[i].YMD,8}");
            }


            Console.WriteLine($"Enterキーを押すと終了します");
            Console.ReadLine();
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

