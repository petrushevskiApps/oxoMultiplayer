using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Tests
{
    public class ExtractDiagonalTests
    {
        private ExtractDiagonal<int> extractDiagonal;

        private static IEnumerable<TestCaseData> TableDataForDiagonalMatch()
        {
            int[,] table3x3 = new int[,]
            {
                {2, 1, 1},
                {1, 2, 2},
                {2, 2, 1}
            };
            
            yield return new TestCaseData(table3x3, 0).Returns(new int[] {2, 2, 1}).SetName("Table3x3_Index00");
            yield return new TestCaseData(table3x3, 1).Returns(new int[] {1, 2}).SetName("Table3x3_Index01");
            yield return new TestCaseData(table3x3, 2).Returns(new int[] {1}).SetName("Table3x3_Index02");
            
            yield return new TestCaseData(table3x3, 3).Returns(new int[] {1, 2}).SetName("Table3x3_Index10");
            yield return new TestCaseData(table3x3, 4).Returns(new int[] {2, 2, 1}).SetName("Table3x3_Index11");
            yield return new TestCaseData(table3x3, 5).Returns(new int[] {1, 2}).SetName("Table3x3_Index12");
            
            yield return new TestCaseData(table3x3, 6).Returns(new int[] {2}).SetName("Table3x3_Index20");
            yield return new TestCaseData(table3x3, 7).Returns(new int[] {1, 2}).SetName("Table3x3_Index21");
            yield return new TestCaseData(table3x3, 8).Returns(new int[] {2, 2, 1}).SetName("Table3x3_Index22");
          
            int[,] table4x4 = new int[,]
            {
                {1, 1, 2, 2},
                {2, 1, 2, 1},
                {1, 2, 1, 2},
                {1, 1, 2, 2}
            };
            
            yield return new TestCaseData(table4x4, 0).Returns(new int[] {1, 1, 1, 2}).SetName("Table4x4_Index00");
            yield return new TestCaseData(table4x4, 1).Returns(new int[] {1, 2, 2}).SetName("Table4x4_Index01");
            yield return new TestCaseData(table4x4, 2).Returns(new int[] {2, 1}).SetName("Table4x4_Index02");
            yield return new TestCaseData(table4x4, 3).Returns(new int[] {2}).SetName("Table4x4_Index03");

            yield return new TestCaseData(table4x4, 4).Returns(new int[] {2, 2, 2}).SetName("Table4x4_Index10");
            yield return new TestCaseData(table4x4, 5).Returns(new int[] {1, 1, 1, 2}).SetName("Table4x4_Index11");
            yield return new TestCaseData(table4x4, 6).Returns(new int[] {1, 2, 2}).SetName("Table4x4_Index12");
            yield return new TestCaseData(table4x4, 7).Returns(new int[] {2, 1}).SetName("Table4x4_Index13");

            yield return new TestCaseData(table4x4, 8).Returns(new int[] {1, 1}).SetName("Table4x4_Index20");
            yield return new TestCaseData(table4x4, 9).Returns(new int[] {2, 2, 2}).SetName("Table4x4_Index21");
            yield return new TestCaseData(table4x4, 10).Returns(new int[] {1, 1, 1, 2}).SetName("Table4x4_Index22");
            yield return new TestCaseData(table4x4, 11).Returns(new int[] {1, 2, 2}).SetName("Table4x4_Index23");
            
            yield return new TestCaseData(table4x4, 12).Returns(new int[] {1}).SetName("Table4x4_Index30");
            yield return new TestCaseData(table4x4, 13).Returns(new int[] {1, 1}).SetName("Table4x4_Index31");
            yield return new TestCaseData(table4x4, 14).Returns(new int[] {2, 2, 2}).SetName("Table4x4_Index32");
            yield return new TestCaseData(table4x4, 15).Returns(new int[] {1, 1, 1, 2}).SetName("Table4x4_Index33");
            
            int[,] table5x5 = new int[,]
            {
                {1, 2, 1, 2, 1},
                {1, 1, 2, 1, 2},
                {2, 2, 1, 1, 2},
                {1, 2, 2, 2, 1},
                {2, 1, 1, 2, 1}
            };
            
            yield return new TestCaseData(table5x5, 0).Returns(new int[] {1, 1, 1, 2, 1}).SetName("Table5x5_Index00");
            yield return new TestCaseData(table5x5, 1).Returns(new int[] {2, 2, 1, 1}).SetName("Table5x5_Index01");
            yield return new TestCaseData(table5x5, 2).Returns(new int[] {1, 1, 2}).SetName("Table5x5_Index02");
            yield return new TestCaseData(table5x5, 3).Returns(new int[] {2, 2}).SetName("Table5x5_Index03");
            yield return new TestCaseData(table5x5, 4).Returns(new int[] {1}).SetName("Table5x5_Index04");
            
            yield return new TestCaseData(table5x5, 5).Returns(new int[] {1, 2, 2, 2}).SetName("Table5x5_Index10");
            yield return new TestCaseData(table5x5, 6).Returns(new int[] {1, 1, 1, 2, 1}).SetName("Table5x5_Index11");
            yield return new TestCaseData(table5x5, 7).Returns(new int[] {2, 2, 1, 1}).SetName("Table5x5_Index12");
            yield return new TestCaseData(table5x5, 8).Returns(new int[] {1, 1, 2}).SetName("Table5x5_Index13");
            yield return new TestCaseData(table5x5, 9).Returns(new int[] {2, 2}).SetName("Table5x5_Index14");
            
            yield return new TestCaseData(table5x5, 10).Returns(new int[] {2, 2, 1}).SetName("Table5x5_Index20");
            yield return new TestCaseData(table5x5, 11).Returns(new int[] {1, 2, 2, 2}).SetName("Table5x5_Index21");
            yield return new TestCaseData(table5x5, 12).Returns(new int[] {1, 1, 1, 2, 1}).SetName("Table5x5_Index22");
            yield return new TestCaseData(table5x5, 13).Returns(new int[] {2, 2, 1, 1}).SetName("Table5x5_Index23");
            yield return new TestCaseData(table5x5, 14).Returns(new int[] {1, 1, 2}).SetName("Table5x5_Index24");
            
            yield return new TestCaseData(table5x5, 15).Returns(new int[] {1, 1}).SetName("Table5x5_Index30");
            yield return new TestCaseData(table5x5, 16).Returns(new int[] {2, 2, 1}).SetName("Table5x5_Index31");
            yield return new TestCaseData(table5x5, 17).Returns(new int[] {1, 2, 2, 2}).SetName("Table5x5_Index32");
            yield return new TestCaseData(table5x5, 18).Returns(new int[] {1, 1, 1, 2, 1}).SetName("Table5x5_Index33");
            yield return new TestCaseData(table5x5, 19).Returns(new int[] {2, 2, 1, 1}).SetName("Table5x5_Index34");
            
            yield return new TestCaseData(table5x5, 20).Returns(new int[] {2}).SetName("Table5x5_Index40");
            yield return new TestCaseData(table5x5, 21).Returns(new int[] {1, 1}).SetName("Table5x5_Index41");
            yield return new TestCaseData(table5x5, 22).Returns(new int[] {2, 2, 1}).SetName("Table5x5_Index42");
            yield return new TestCaseData(table5x5, 23).Returns(new int[] {1, 2, 2, 2}).SetName("Table5x5_Index43");
            yield return new TestCaseData(table5x5, 24).Returns(new int[] {1, 1, 1, 2, 1}).SetName("Table5x5_Index44");
        }

        private static IEnumerable<TestCaseData> TableDataForDiagonalLength()
        {
            int[,] table3x3 = new int[,]
            {
                {2, 1, 1},
                {1, 2, 2},
                {2, 2, 1}
            };
            yield return new TestCaseData(table3x3, 1).Returns(2).SetName("Table3x3_AboveMainDiagonal");
            yield return new TestCaseData(table3x3, 4).Returns(3).SetName("Table3x3_OnMainDiagonal");
            yield return new TestCaseData(table3x3, 7).Returns(2).SetName("Table3x3_BelowMainDiagonal");

            int[,] table4x4 = new int[,]
            {
                {1, 1, 2, 2},
                {2, 1, 2, 1},
                {1, 2, 1, 2},
                {1, 1, 2, 2}
            };
            
            yield return new TestCaseData(table4x4, 1).Returns(3).SetName("Table4x4_AboveMainDiagonal");
            yield return new TestCaseData(table4x4, 0).Returns(4).SetName("Table4x4_OnMainDiagonal");
            yield return new TestCaseData(table4x4, 4).Returns(3).SetName("Table4x4_BelowMainDiagonal");

            int[,] table5x5 = new int[,]
            {
                {1, 2, 1, 2, 1},
                {1, 1, 2, 1, 2},
                {2, 2, 1, 1, 2},
                {1, 2, 2, 2, 1},
                {2, 1, 1, 2, 1}
            };
            
            yield return new TestCaseData(table5x5, 1).Returns(4).SetName("Table5x5_AboveMainReversedDiagonal");
            yield return new TestCaseData(table5x5, 0).Returns(5).SetName("Table5x5_OnMainReversedDiagonal");
            yield return new TestCaseData(table5x5, 5).Returns(4).SetName("Table5x5_BelowMainReversedDiagonal");
        }

        private static IEnumerable<TestCaseData> TableDataForIdOutOfBounds()
        {
            yield return new TestCaseData(new int[,] { }, -1).SetName("TableEmpty_NegativeID");
            yield return new TestCaseData(new int[,] { }, 1).SetName("TableEmpty_PositiveID");
            
            yield return new TestCaseData(new int[,]
            {
                {2, 1, 1},
                {1, 2, 2},
                {2, 2, 1}
            }, -1).SetName("Table3x3_NegativeIdOutOfBounds");
            
            yield return new TestCaseData(new int[,]
            {
                {2, 1, 1},
                {1, 2, 2},
                {2, 2, 1}
            }, 9).SetName("Table3x3_PositiveIdOutOfBounds");

            yield return new TestCaseData(new int[,]
            {
                {1, 1, 2, 2},
                {2, 1, 2, 1},
                {1, 2, 1, 2},
                {1, 1, 2, 2}
            }, -1).SetName("Table4x4_NegativeIdOutOfBounds");

            yield return new TestCaseData(new int[,]
            {
                {1, 1, 2, 2},
                {2, 1, 2, 1},
                {1, 2, 1, 2},
                {1, 1, 2, 2}
            }, 16).SetName("Table4x4_PositiveIdOutOfBounds");

            yield return new TestCaseData(new int[,]
            {
                {1, 2, 1, 2, 1},
                {1, 1, 2, 1, 2},
                {2, 2, 1, 1, 2},
                {1, 2, 2, 2, 1},
                {2, 1, 1, 2, 1}
            }, -1).SetName("Table5x5_NegativeIdOutOfBounds");

            yield return new TestCaseData(new int[,]
            {
                {1, 2, 1, 2, 1},
                {1, 1, 2, 1, 2},
                {2, 2, 1, 1, 2},
                {1, 2, 2, 2, 1},
                {2, 1, 1, 2, 1}
            }, 25).SetName("Table5x5_PositiveIdOutOfBounds");

        }

        
        [SetUp]
        public void SetUp()
        {
            extractDiagonal = new ExtractDiagonal<int>();
        }

        [Test]
        [TestCaseSource(nameof(TableDataForDiagonalMatch))]
        public int[] Extract_WhenCalled_ReturnsDiagonalAtGivenIndex(int[,] table, int id)
        {
            return extractDiagonal.Extract(table, id);
        }

        [Test]
        [TestCaseSource(nameof(TableDataForDiagonalLength))]
        public int Extract_WhenCalled_ReturnsArrayWithLengthEqualToDiagonalLength(int[,] table, int id)
        {
            int[] diagonal = extractDiagonal.Extract(table, id);
            return diagonal.Length;
        }

        [Test]
        [TestCaseSource(nameof(TableDataForIdOutOfBounds))]
        public void Extract_IdOutOfBounds_ThrowArgumentOutOfRangeException(int[,] table, int id)
        {
            Assert.That(() => extractDiagonal.Extract(table, id), Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }

    }
}