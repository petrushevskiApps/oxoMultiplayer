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
        private WinCondition win;
        
        private static IEnumerable<TestCaseData> HigherThenMinStrikeTestData()
        {
            yield return new TestCaseData(1,3, new int[]{1,1,1,0}).Returns(true).SetName("Length_4_StartingStrike_3_Win");
            yield return new TestCaseData(1,3, new int[]{0,1,1,1}).Returns(true).SetName("Length_4_EndingStrike_3_Win");
            yield return new TestCaseData(1,3, new int[]{1,1,1,1}).Returns(true).SetName("Length_4_FullStrike_4_Win");
            
            yield return new TestCaseData(1,3, new int[]{0,1,0,0}).Returns(false).SetName("Length_4_OneElement_NoWin");
            yield return new TestCaseData(1,3, new int[]{0,1,1,0}).Returns(false).SetName("Length_4_TwoElements_NoWin");
            yield return new TestCaseData(1,3, new int[]{1,1,0,1}).Returns(false).SetName("Length_4_ThreeElements_NoWin");
            
            yield return new TestCaseData(1,3, new int[]{1,1,1,0,0}).Returns(true).SetName("Length_5_StartingStrike_3_Win");
            yield return new TestCaseData(1,3, new int[]{0,1,1,1,0}).Returns(true).SetName("Length_5_MiddleStrike_3_Win");
            yield return new TestCaseData(1,3, new int[]{0,0,1,1,1}).Returns(true).SetName("Length_5_EndingStrike_3_Win");
            yield return new TestCaseData(1,3, new int[]{1,1,1,1,0}).Returns(true).SetName("Length_5_StartingStrike_4_Win");
            yield return new TestCaseData(1,3, new int[]{0,1,1,1,1}).Returns(true).SetName("Length_5_EndingStrike_4_Win");
            yield return new TestCaseData(1,3, new int[]{1,1,1,1,1}).Returns(true).SetName("Length_5_FullStrike_5_Win");
            
            yield return new TestCaseData(1,3, new int[]{0,1,0,0,0}).Returns(false).SetName("Length_3_OneElement_NoWin");
            yield return new TestCaseData(1,3, new int[]{0,1,1,0,0}).Returns(false).SetName("Length_3_TwoElement_NoWin");
            yield return new TestCaseData(1,3, new int[]{0,1,1,0,1}).Returns(false).SetName("Length_3_ThreeElements_NoWin");
            yield return new TestCaseData(1,3, new int[]{1,1,0,1,1}).Returns(false).SetName("Length_3_FourElements_NoWin");
        }

        private static IEnumerable<TestCaseData> LessThenMinStrikeTestData()
        {
            yield return new TestCaseData(1,3, new int[0]{}).Returns(false).SetName("LessThenMinStrike_Length_0_NoWin");
            yield return new TestCaseData(1,3, new int[]{1}).Returns(false).SetName("LessThenMinStrike_Length_1_NoWin");
            yield return new TestCaseData(1,3, new int[]{1,0}).Returns(false).SetName("LessThenMinStrike_Length_2_NoWin");
        }
        
        private static IEnumerable<TestCaseData> MinStrikeTestData()
        {
            yield return new TestCaseData(1,3, new int[]{1,1,1}).Returns(true).SetName("MinStrike_Length_3_FullMatch_Win");
            
            yield return new TestCaseData(1,3, new int[]{0,1,0}).Returns(false).SetName("MinStrike_Length_3_OneMatch_NoWin");
            yield return new TestCaseData(1,3, new int[]{1,1,0}).Returns(false).SetName("MinStrike_Length_3_TwoMatch_NoWin");
        }
        
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
            win = new WinCondition(3);
        }
 
        [Test]
        [TestCaseSource(nameof(TableEmptyTestData))]
        [TestCaseSource(nameof(TableNotFullTestData))]
        [TestCaseSource(nameof(TableFullTestData))]
        public bool IsTableFull(int[,] table)
        {
            return win.IsTableFull(table);
        }
    }
}
