using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class WinConditionsTests
    {
        private WinCondition<Tile> win;
        
        private static IEnumerable<TestCaseData> TableEmptyTestData()
        {
            yield return new TestCaseData(new int[,]{{0,0,0},{0,0,0},{0,0,0}}).Returns(false).SetName("Table3x3_Empty");
            
            yield return new TestCaseData(new int[,]
            {
                {0,0,0,0},
                {0,0,0,0},
                {0,0,0,0},
                {0,0,0,0}
            }).Returns(false).SetName("Table4x4_Empty");
            
            yield return new TestCaseData(new int[,]
            {
                {0,0,0,0,0},
                {0,0,0,0,0},
                {0,0,0,0,0},
                {0,0,0,0,0},
                {0,0,0,0,0}
            }).Returns(false).SetName("Table5x5_Empty");
        }
        private static IEnumerable<TestCaseData> TableNotFullTestData()
        {
            yield return new TestCaseData(new int[,]{{0,1,0},{1,2,0},{2,2,1}}).Returns(false).SetName("Table3x3_NotFull");
            
            yield return new TestCaseData(new int[,]
            {
                {0,1,2,2},
                {2,1,2,0},
                {0,2,1,2},
                {1,1,2,0}
            }).Returns(false).SetName("Table4x4_NotFull");
            
            yield return new TestCaseData(new int[,]
            {
                {0,2,2,0,0},
                {1,0,1,0,0},
                {0,2,0,1,0},
                {0,2,2,0,1},
                {0,1,0,2,1}
            }).Returns(false).SetName("Table5x5_NotFull");
        }
        private static IEnumerable<TestCaseData> TableFullTestData()
        {
            yield return new TestCaseData(new int[,]{{2,1,1},{1,2,2},{2,2,1}}).Returns(true).SetName("Table3x3_Full");
            
            yield return new TestCaseData(new int[,]
            {
                {1,1,2,2},
                {2,1,2,1},
                {1,2,1,2},
                {1,1,2,2}
            }).Returns(true).SetName("Table4x4_Full");
            
            yield return new TestCaseData(new int[,]
            {
                {1,2,1,2,1},
                {1,1,2,1,2},
                {2,2,1,1,2},
                {1,2,2,2,1},
                {2,1,1,2,1}
            }).Returns(true).SetName("Table5x5_Full");
        }
        
        [SetUp]
        public void Setup()
        {
            win = new WinCondition<Tile>(3);
        }
 
        [Test]
        [Ignore("Should be fixed")]
        [TestCaseSource(nameof(TableEmptyTestData))]
        [TestCaseSource(nameof(TableNotFullTestData))]
        [TestCaseSource(nameof(TableFullTestData))]
        public bool IsTableFull(Tile[,] table)
        {
            return win.IsTableFull(table);
        }
    }
}
