using System.Collections.Generic;
using NUnit.Framework;

namespace Tests
{
    public class AIPlayerTests
    {
        private WinCondition<ITile> win;
        private AiBrain aiBrain;

        // 2 0 1
        // 1 2 2
        // 1 1 0
        private static IntegerTile id0 =  new IntegerTile(){ Id=0, Value = 2};
        private static IntegerTile id1 =  new IntegerTile(){ Id=1, Value = 0};
        private static IntegerTile id2 =  new IntegerTile(){ Id=2, Value = 1};
        private static IntegerTile id3 =  new IntegerTile(){ Id=3, Value = 1};
        private static IntegerTile id4 =  new IntegerTile(){ Id=4, Value = 2};
        private static IntegerTile id5 =  new IntegerTile(){ Id=5, Value = 2};
        private static IntegerTile id6 =  new IntegerTile(){ Id=6, Value = 1};
        private static IntegerTile id7 =  new IntegerTile(){ Id=7, Value = 1};
        private static IntegerTile id8 =  new IntegerTile(){ Id=8, Value = 0};
        
        // 0 2 0
        // 1 1 0
        // 0 0 0
        private static IntegerTile id00 =  new IntegerTile(){ Id=0, Value = 0};
        private static IntegerTile id10 =  new IntegerTile(){ Id=1, Value = 2};
        private static IntegerTile id20 =  new IntegerTile(){ Id=2, Value = 0};
        private static IntegerTile id30 =  new IntegerTile(){ Id=3, Value = 1};
        private static IntegerTile id40 =  new IntegerTile(){ Id=4, Value = 1};
        private static IntegerTile id50 =  new IntegerTile(){ Id=5, Value = 0};
        private static IntegerTile id60 =  new IntegerTile(){ Id=6, Value = 0};
        private static IntegerTile id70 =  new IntegerTile(){ Id=7, Value = 0};
        private static IntegerTile id80 =  new IntegerTile(){ Id=8, Value = 0};
        
        // 1 2 0
        // 1 0 0
        // 0 0 0
        private static IntegerTile id02 =  new IntegerTile(){ Id=0, Value = 1};
        private static IntegerTile id12 =  new IntegerTile(){ Id=1, Value = 2};
        private static IntegerTile id22 =  new IntegerTile(){ Id=2, Value = 0};
        private static IntegerTile id32 =  new IntegerTile(){ Id=3, Value = 1};
        private static IntegerTile id42 =  new IntegerTile(){ Id=4, Value = 0};
        private static IntegerTile id52 =  new IntegerTile(){ Id=5, Value = 0};
        private static IntegerTile id62 =  new IntegerTile(){ Id=6, Value = 0};
        private static IntegerTile id72 =  new IntegerTile(){ Id=7, Value = 0};
        private static IntegerTile id82 =  new IntegerTile(){ Id=8, Value = 0};
        
        // 0 1 1
        // 0 2 0
        // 1 2 2
        private static IntegerTile id04 =  new IntegerTile(){ Id=0, Value = 0};
        private static IntegerTile id14 =  new IntegerTile(){ Id=1, Value = 1};
        private static IntegerTile id24 =  new IntegerTile(){ Id=2, Value = 1};
        private static IntegerTile id34 =  new IntegerTile(){ Id=3, Value = 0};
        private static IntegerTile id44 =  new IntegerTile(){ Id=4, Value = 2};
        private static IntegerTile id54 =  new IntegerTile(){ Id=5, Value = 0};
        private static IntegerTile id64 =  new IntegerTile(){ Id=6, Value = 1};
        private static IntegerTile id74 =  new IntegerTile(){ Id=7, Value = 2};
        private static IntegerTile id84 =  new IntegerTile(){ Id=8, Value = 2};
        
        private static IntegerTile id01 =  new IntegerTile(){ Id=0, Value = 1};
        private static IntegerTile id11 =  new IntegerTile(){ Id=1, Value = 0};
        private static IntegerTile id21 =  new IntegerTile(){ Id=2, Value = 0};
        private static IntegerTile id31 =  new IntegerTile(){ Id=3, Value = 0};
        private static IntegerTile id41 =  new IntegerTile(){ Id=4, Value = 0};
        private static IntegerTile id51 =  new IntegerTile(){ Id=5, Value = 0};
        private static IntegerTile id61 =  new IntegerTile(){ Id=6, Value = 0};
        private static IntegerTile id71 =  new IntegerTile(){ Id=7, Value = 0};
        private static IntegerTile id81 =  new IntegerTile(){ Id=8, Value = 0};
        
        private static IEnumerable<TestCaseData> TableData()
        {
            yield return new TestCaseData(new [,]
            {
                {id0,id1,id2},
                {id3,id4,id5},
                {id6,id7,id8}
            }).Returns(8).SetName("AiWins");
        }
        private static IEnumerable<TestCaseData> TableData2()
        {
            yield return new TestCaseData(new [,]
            {
                {id01,id11,id21},
                {id31,id41,id51},
                {id61,id71,id81}
            }).Returns(4).SetName("FirstMove");
        }
        private static IEnumerable<TestCaseData> TableData1()
        {
            yield return new TestCaseData(new [,]
            {
                {id00,id10,id20},
                {id30,id40,id50},
                {id60,id70,id80}
            }).Returns(5).SetName("StopPlayerFromWinning");
        }
        private static IEnumerable<TestCaseData> TableData3()
        {
            yield return new TestCaseData(new [,]
            {
                {id02,id12,id22},
                {id32,id42,id52},
                {id62,id72,id82}
            }).Returns(6).SetName("StopPlayerFromWinning_2");
        }
        private static IEnumerable<TestCaseData> TableData4()
        {
            yield return new TestCaseData(new [,]
            {
                {id04,id14,id24},
                {id34,id44,id54},
                {id64,id74,id84}
            }).Returns(0).SetName("AiWin_ReverseDiagonal");
        }
        [SetUp]
        public void Setup()
        {
            win = new WinCondition<ITile>(3);
            aiBrain = new AiBrain();
            aiBrain.Initialize(2, 1, win);
            
        }
        
        [Test]
        [TestCaseSource(nameof(TableData))]
        [TestCaseSource(nameof(TableData1))]
        [TestCaseSource(nameof(TableData2))]
        [TestCaseSource(nameof(TableData3))]
        [TestCaseSource(nameof(TableData4))]
        public int FindBestMove(IntegerTile[,] table)
        {
            return aiBrain.FindBestMove(table);
        }


        private static IEnumerable<TestCaseData> Evaluate1()
        {
            yield return new TestCaseData(new [,]
            {
                {0,0,0},
                {0,0,0},
                {0,0,0}
            }).Returns(-1).SetName("EmptyTable");
        }
        
        private static IEnumerable<TestCaseData> Evaluate2()
        {
            yield return new TestCaseData(new [,]
            {
                {1,2,0},
                {1,1,2},
                {2,0,1}
            }).Returns(-9).SetName("FirstPlayerWins(-9)");
        }
        private static IEnumerable<TestCaseData> Evaluate3()
        {
            yield return new TestCaseData(new [,]
            {
                {2,1,0},
                {2,1,1},
                {2,0,0}
            }).Returns(12).SetName("SecondPlayerWins(9)");
        }
        private static IEnumerable<TestCaseData> Evaluate4()
        {
            yield return new TestCaseData(new [,]
            {
                {2,1,0},
                {2,1,1},
                {1,0,0}
            }).Returns(-1).SetName("NoWinnerYet");
        }
        [Test]
        [TestCaseSource(nameof(Evaluate1))]
        [TestCaseSource(nameof(Evaluate2))]
        [TestCaseSource(nameof(Evaluate3))]
        [TestCaseSource(nameof(Evaluate4))]
        public int Evaluate(int[,] table)
        {
            return aiBrain.Evaluate(table);
        }
    }
}