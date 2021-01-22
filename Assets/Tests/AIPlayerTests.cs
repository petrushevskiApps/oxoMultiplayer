using System.Collections.Generic;
using NUnit.Framework;

namespace Tests
{
    public class AIPlayerTests
    {
        private WinCondition<ITile> win;
        private AiBrain aiBrain;

        private static IntegerTile id0 =  new IntegerTile(){ Id=0, Value = 2};
        private static IntegerTile id1 =  new IntegerTile(){ Id=1, Value = 0};
        private static IntegerTile id2 =  new IntegerTile(){ Id=2, Value = 1};
        private static IntegerTile id3 =  new IntegerTile(){ Id=3, Value = 1};
        private static IntegerTile id4 =  new IntegerTile(){ Id=4, Value = 2};
        private static IntegerTile id5 =  new IntegerTile(){ Id=5, Value = 2};
        private static IntegerTile id6 =  new IntegerTile(){ Id=6, Value = 0};
        private static IntegerTile id7 =  new IntegerTile(){ Id=7, Value = 1};
        private static IntegerTile id8 =  new IntegerTile(){ Id=8, Value = 0};
        // 2 0 1
        // 1 2 2
        // 0 1 0
        private static IEnumerable<TestCaseData> TableData()
        {
            yield return new TestCaseData(new [,]
            {
                {id0,id1,id2},
                {id3,id4,id5},
                {id6,id7,id8}
            }).Returns(8).SetName("AiWins");
        }
        
        [SetUp]
        public void Setup()
        {
            win = new WinCondition<ITile>(3);
            aiBrain = new AiBrain();
            aiBrain.Initialize(2, 1, win);
            
        }
        
//        [Test]
//        [TestCaseSource(nameof(TableData))]
//        public int FindBestMove(AiBrain.IntegerTile[,] table)
//        {
////            return aiBrain.FindBestMove(table);
//        }
        
    }
}