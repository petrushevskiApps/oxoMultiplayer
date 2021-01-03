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
            
            yield return new TestCaseData(table3x3, new ElementIndex(0, 0)).Returns(new int[] {2, 2, 1}).SetName("Table3x3_Index00");
            yield return new TestCaseData(table3x3, new ElementIndex(0, 1)).Returns(new int[] {1, 2}).SetName("Table3x3_Index01");
            yield return new TestCaseData(table3x3, new ElementIndex(0, 2)).Returns(new int[] {1}).SetName("Table3x3_Index02");
            
            yield return new TestCaseData(table3x3, new ElementIndex(1, 0)).Returns(new int[] {1, 2}).SetName("Table3x3_Index10");
            yield return new TestCaseData(table3x3, new ElementIndex(1, 1)).Returns(new int[] {2, 2, 1}).SetName("Table3x3_Index11");
            yield return new TestCaseData(table3x3, new ElementIndex(1, 2)).Returns(new int[] {1, 2}).SetName("Table3x3_Index12");
            
            yield return new TestCaseData(table3x3, new ElementIndex(2, 0)).Returns(new int[] {2}).SetName("Table3x3_Index20");
            yield return new TestCaseData(table3x3, new ElementIndex(2, 1)).Returns(new int[] {1, 2}).SetName("Table3x3_Index21");
            yield return new TestCaseData(table3x3, new ElementIndex(2, 2)).Returns(new int[] {2, 2, 1}).SetName("Table3x3_Index22");
          
            int[,] table4x4 = new int[,]
            {
                {1, 1, 2, 2},
                {2, 1, 2, 1},
                {1, 2, 1, 2},
                {1, 1, 2, 2}
            };
            
            yield return new TestCaseData(table4x4, new ElementIndex(0, 0)).Returns(new int[] {1, 1, 1, 2}).SetName("Table4x4_Index00");
            yield return new TestCaseData(table4x4, new ElementIndex(0, 1)).Returns(new int[] {1, 2, 2}).SetName("Table4x4_Index01");
            yield return new TestCaseData(table4x4, new ElementIndex(0, 2)).Returns(new int[] {2, 1}).SetName("Table4x4_Index02");
            yield return new TestCaseData(table4x4, new ElementIndex(0, 3)).Returns(new int[] {2}).SetName("Table4x4_Index03");

            yield return new TestCaseData(table4x4, new ElementIndex(1, 0)).Returns(new int[] {2, 2, 2}).SetName("Table4x4_Index10");
            yield return new TestCaseData(table4x4, new ElementIndex(1, 1)).Returns(new int[] {1, 1, 1, 2}).SetName("Table4x4_Index11");
            yield return new TestCaseData(table4x4, new ElementIndex(1, 2)).Returns(new int[] {1, 2, 2}).SetName("Table4x4_Index12");
            yield return new TestCaseData(table4x4, new ElementIndex(1, 3)).Returns(new int[] {2, 1}).SetName("Table4x4_Index13");

            yield return new TestCaseData(table4x4, new ElementIndex(2, 0)).Returns(new int[] {1, 1}).SetName("Table4x4_Index20");
            yield return new TestCaseData(table4x4, new ElementIndex(2, 1)).Returns(new int[] {2, 2, 2}).SetName("Table4x4_Index21");
            yield return new TestCaseData(table4x4, new ElementIndex(2, 2)).Returns(new int[] {1, 1, 1, 2}).SetName("Table4x4_Index22");
            yield return new TestCaseData(table4x4, new ElementIndex(2, 3)).Returns(new int[] {1, 2, 2}).SetName("Table4x4_Index23");
            
            yield return new TestCaseData(table4x4, new ElementIndex(3, 0)).Returns(new int[] {1}).SetName("Table4x4_Index30");
            yield return new TestCaseData(table4x4, new ElementIndex(3, 1)).Returns(new int[] {1, 1}).SetName("Table4x4_Index31");
            yield return new TestCaseData(table4x4, new ElementIndex(3, 2)).Returns(new int[] {2, 2, 2}).SetName("Table4x4_Index32");
            yield return new TestCaseData(table4x4, new ElementIndex(3, 3)).Returns(new int[] {1, 1, 1, 2}).SetName("Table4x4_Index33");
            
            int[,] table5x5 = new int[,]
            {
                {1, 2, 1, 2, 1},
                {1, 1, 2, 1, 2},
                {2, 2, 1, 1, 2},
                {1, 2, 2, 2, 1},
                {2, 1, 1, 2, 1}
            };
            
            yield return new TestCaseData(table5x5, new ElementIndex(0, 0)).Returns(new int[] {1, 1, 1, 2, 1}).SetName("Table5x5_Index00");
            yield return new TestCaseData(table5x5, new ElementIndex(0, 1)).Returns(new int[] {2, 2, 1, 1}).SetName("Table5x5_Index01");
            yield return new TestCaseData(table5x5, new ElementIndex(0, 2)).Returns(new int[] {1, 1, 2}).SetName("Table5x5_Index02");
            yield return new TestCaseData(table5x5, new ElementIndex(0, 3)).Returns(new int[] {2, 2}).SetName("Table5x5_Index03");
            yield return new TestCaseData(table5x5, new ElementIndex(0, 4)).Returns(new int[] {1}).SetName("Table5x5_Index04");
            
            yield return new TestCaseData(table5x5, new ElementIndex(1, 0)).Returns(new int[] {1, 2, 2, 2}).SetName("Table5x5_Index10");
            yield return new TestCaseData(table5x5, new ElementIndex(1, 1)).Returns(new int[] {1, 1, 1, 2, 1}).SetName("Table5x5_Index11");
            yield return new TestCaseData(table5x5, new ElementIndex(1, 2)).Returns(new int[] {2, 2, 1, 1}).SetName("Table5x5_Index12");
            yield return new TestCaseData(table5x5, new ElementIndex(1, 3)).Returns(new int[] {1, 1, 2}).SetName("Table5x5_Index13");
            yield return new TestCaseData(table5x5, new ElementIndex(1, 4)).Returns(new int[] {2, 2}).SetName("Table5x5_Index14");
            
            yield return new TestCaseData(table5x5, new ElementIndex(2, 0)).Returns(new int[] {2, 2, 1}).SetName("Table5x5_Index20");
            yield return new TestCaseData(table5x5, new ElementIndex(2, 1)).Returns(new int[] {1, 2, 2, 2}).SetName("Table5x5_Index21");
            yield return new TestCaseData(table5x5, new ElementIndex(2, 2)).Returns(new int[] {1, 1, 1, 2, 1}).SetName("Table5x5_Index22");
            yield return new TestCaseData(table5x5, new ElementIndex(2, 3)).Returns(new int[] {2, 2, 1, 1}).SetName("Table5x5_Index23");
            yield return new TestCaseData(table5x5, new ElementIndex(2, 4)).Returns(new int[] {1, 1, 2}).SetName("Table5x5_Index24");
            
            yield return new TestCaseData(table5x5, new ElementIndex(3, 0)).Returns(new int[] {1, 1}).SetName("Table5x5_Index30");
            yield return new TestCaseData(table5x5, new ElementIndex(3, 1)).Returns(new int[] {2, 2, 1}).SetName("Table5x5_Index31");
            yield return new TestCaseData(table5x5, new ElementIndex(3, 2)).Returns(new int[] {1, 2, 2, 2}).SetName("Table5x5_Index32");
            yield return new TestCaseData(table5x5, new ElementIndex(3, 3)).Returns(new int[] {1, 1, 1, 2, 1}).SetName("Table5x5_Index33");
            yield return new TestCaseData(table5x5, new ElementIndex(3, 4)).Returns(new int[] {2, 2, 1, 1}).SetName("Table5x5_Index34");
            
            yield return new TestCaseData(table5x5, new ElementIndex(4, 0)).Returns(new int[] {2}).SetName("Table5x5_Index40");
            yield return new TestCaseData(table5x5, new ElementIndex(4, 1)).Returns(new int[] {1, 1}).SetName("Table5x5_Index41");
            yield return new TestCaseData(table5x5, new ElementIndex(4, 2)).Returns(new int[] {2, 2, 1}).SetName("Table5x5_Index42");
            yield return new TestCaseData(table5x5, new ElementIndex(4, 3)).Returns(new int[] {1, 2, 2, 2}).SetName("Table5x5_Index43");
            yield return new TestCaseData(table5x5, new ElementIndex(4, 4)).Returns(new int[] {1, 1, 1, 2, 1}).SetName("Table5x5_Index44");
        }

        private static IEnumerable<TestCaseData> TableDataForColumnLength()
        {
            yield return new TestCaseData(new int[,]
            {
                {2, 1, 1},
                {1, 2, 2},
                {2, 2, 1}
            }).SetName("Table3x3");

            yield return new TestCaseData(new int[,]
            {
                {1, 1, 2, 2},
                {2, 1, 2, 1},
                {1, 2, 1, 2},
                {1, 1, 2, 2}
            }).SetName("Table4x4");

            yield return new TestCaseData(new int[,]
            {
                {1, 2, 1, 2, 1},
                {1, 1, 2, 1, 2},
                {2, 2, 1, 1, 2},
                {1, 2, 2, 2, 1},
                {2, 1, 1, 2, 1}
            }).SetName("Table5x5");

        }

        private static IEnumerable<TestCaseData> TableDataForIndexOutOfBounds()
        {
            yield return new TestCaseData(new int[,] { }, new ElementIndex(1, 0)).SetName("TableEmpty_HigherRowIndex");
            yield return new TestCaseData(new int[,] { }, new ElementIndex(0, 1)).SetName("TableEmpty_HigherColumnIndex");

            yield return new TestCaseData(new int[,]
            {
                {2, 1, 1},
                {1, 2, 2},
                {2, 2, 1}
            }, new ElementIndex(0, 3)).SetName("Table3x3_HigherRowIndex");
            
            yield return new TestCaseData(new int[,]
            {
                {2, 1, 1},
                {1, 2, 2},
                {2, 2, 1}
            }, new ElementIndex(0, 3)).SetName("Table3x3_HigherColumnIndex");

            yield return new TestCaseData(new int[,]
            {
                {1, 1, 2, 2},
                {2, 1, 2, 1},
                {1, 2, 1, 2},
                {1, 1, 2, 2}
            }, new ElementIndex(4, 0)).SetName("Table4x4_HigherRowIndex");

            yield return new TestCaseData(new int[,]
            {
                {1, 1, 2, 2},
                {2, 1, 2, 1},
                {1, 2, 1, 2},
                {1, 1, 2, 2}
            }, new ElementIndex(0, 4)).SetName("Table4x4_HigherColumnIndex");

            yield return new TestCaseData(new int[,]
            {
                {1, 2, 1, 2, 1},
                {1, 1, 2, 1, 2},
                {2, 2, 1, 1, 2},
                {1, 2, 2, 2, 1},
                {2, 1, 1, 2, 1}
            }, new ElementIndex(5, 0)).SetName("Table5x5_HigherRowIndex");

            yield return new TestCaseData(new int[,]
            {
                {1, 2, 1, 2, 1},
                {1, 1, 2, 1, 2},
                {2, 2, 1, 1, 2},
                {1, 2, 2, 2, 1},
                {2, 1, 1, 2, 1}
            }, new ElementIndex(0, 5)).SetName("Table5x5_HigherColumnIndex");

        }

        private static IEnumerable<TestCaseData> TableDataForListSize()
        {
            yield return new TestCaseData(new int[,]
            {
                {2, 1, 1},
                {1, 2, 2},
                {2, 2, 1}
            }, new ElementIndex(0, 1)).SetName("Table3x3_Index03");

            yield return new TestCaseData(new int[,]
            {
                {1, 1, 2, 2},
                {2, 1, 2, 1},
                {1, 2, 1, 2},
                {1, 1, 2, 2}
            }, new ElementIndex(0, 2)).SetName("Table4x4_Index04");

            yield return new TestCaseData(new int[,]
            {
                {1, 2, 1, 2, 1},
                {1, 1, 2, 1, 2},
                {2, 2, 1, 1, 2},
                {1, 2, 2, 2, 1},
                {2, 1, 1, 2, 1}
            }, new ElementIndex(0, 4)).SetName("Table5x5_Index05");

        }

        [SetUp]
        public void SetUp()
        {
            extractDiagonal = new ExtractDiagonal<int>();
        }

        [Test]
        [TestCaseSource(nameof(TableDataForDiagonalMatch))]
        public int[] Extract_WhenCalled_ReturnsDiagonalAtGivenIndex(int[,] table, ElementIndex index)
        {
            return extractDiagonal.Extract(table, index);
        }

        [Test]
        [TestCaseSource(nameof(TableDataForColumnLength))]
        public void Extract_BellowMainDiagonal_ReturnsArrayWithLengthEqualToDiagonalLength(int[,] table)
        {
            ElementIndex index = new ElementIndex(1, 0);
            
            int[] diagonal = extractDiagonal.Extract(table, index);
            int expectedLength = (table.GetUpperBound(0) + 1) - (index.Row - index.Column);
            
            Assert.That(diagonal.Length, Is.EqualTo(expectedLength));
        }
        [Test]
        [TestCaseSource(nameof(TableDataForColumnLength))]
        public void Extract_AboveMainDiagonal_ReturnsArrayWithLengthEqualToDiagonalLength(int[,] table)
        {
            ElementIndex index = new ElementIndex(0, 1);
            
            int[] diagonal = extractDiagonal.Extract(table, index);
            int expectedLength = (table.GetUpperBound(0) + 1) - (index.Column - index.Row);
            
            Assert.That(diagonal.Length, Is.EqualTo(expectedLength));
        }
        [Test]
        [TestCaseSource(nameof(TableDataForColumnLength))]
        public void Extract_MainDiagonal_ReturnsArrayWithLengthEqualToDiagonalLength(int[,] table)
        {
            ElementIndex index = new ElementIndex(0, 0);
            
            int[] diagonal = extractDiagonal.Extract(table, index);
            int expectedLength = (table.GetUpperBound(0) + 1);
            
            Assert.That(diagonal.Length, Is.EqualTo(expectedLength));
        }
        
        [Test]
        public void Extract_ElementIndexIsNull_ThrowArgumentNullException()
        {
            Assert.That(() => extractDiagonal.Extract(new int[,] {{0, 0, 0}, {0, 0, 0}, {0, 0, 0}}, null),
                Throws.ArgumentNullException);
        }

        [Test]
        [TestCaseSource(nameof(TableDataForIndexOutOfBounds))]
        public void Extract_IndexOutOfTableBounds_ThrowArgumentOutOfRangeException(int[,] table, ElementIndex index)
        {
            Assert.That(() => extractDiagonal.Extract(table, index), Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }
        
        [Test]
        [TestCaseSource(nameof(TableDataForListSize))]
        public void GetIndexes_ListLengthEqualsArrayLength_ReturnsSizeEquality(int[,] table, ElementIndex index)
        {
            int[] column = extractDiagonal.Extract(table, index);

            Assert.That(extractDiagonal.GetIndexes().Count, Is.EqualTo(column.Length));
        }
    }
}