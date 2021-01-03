using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Tests
{
    public class ExtractRowTests
    {
        private ExtractRow<int> extractRow;

        private static IEnumerable<TestCaseData> TableDataForRowMatch()
        {
            yield return new TestCaseData(new int[,]
            {
                {2, 1, 1},
                {1, 2, 2},
                {2, 2, 1}
            }, new ElementIndex(0, 0)).Returns(new int[] {2, 1, 1}).SetName("Table3x3_FirstRow");
            yield return new TestCaseData(new int[,]
            {
                {2, 1, 1},
                {1, 2, 2},
                {2, 2, 1}
            }, new ElementIndex(1, 0)).Returns(new int[] {1, 2, 2}).SetName("Table3x3_MiddleRow");
            yield return new TestCaseData(new int[,]
            {
                {2, 1, 1},
                {1, 2, 2},
                {2, 2, 1}
            }, new ElementIndex(2, 0)).Returns(new int[] {2, 2, 1}).SetName("Table3x3_LastRow");

            yield return new TestCaseData(new int[,]
            {
                {1, 1, 2, 2},
                {2, 1, 2, 1},
                {1, 2, 1, 2},
                {1, 1, 2, 2}
            }, new ElementIndex(0, 0)).Returns(new int[] {1, 1, 2, 2}).SetName("Table4x4_FirstRow");
            yield return new TestCaseData(new int[,]
            {
                {1, 1, 2, 2},
                {2, 1, 2, 1},
                {1, 2, 1, 2},
                {1, 1, 2, 2}
            }, new ElementIndex(2, 0)).Returns(new int[] {1, 2, 1, 2}).SetName("Table4x4_MiddleRow");
            yield return new TestCaseData(new int[,]
            {
                {1, 1, 2, 2},
                {2, 1, 2, 1},
                {1, 2, 1, 2},
                {1, 1, 2, 2}
            }, new ElementIndex(3, 0)).Returns(new int[] {1, 1, 2, 2}).SetName("Table4x4_LastRow");


            yield return new TestCaseData(new int[,]
            {
                {1, 2, 1, 2, 1},
                {1, 1, 2, 1, 2},
                {2, 2, 1, 1, 2},
                {1, 2, 2, 2, 1},
                {2, 1, 1, 2, 1}
            }, new ElementIndex(0, 0)).Returns(new int[] {1, 2, 1, 2, 1}).SetName("Table5x5_FirstRow");
            yield return new TestCaseData(new int[,]
            {
                {1, 2, 1, 2, 1},
                {1, 1, 2, 1, 2},
                {2, 2, 1, 1, 2},
                {1, 2, 2, 2, 1},
                {2, 1, 1, 2, 1}
            }, new ElementIndex(2, 0)).Returns(new int[] {2, 2, 1, 1, 2}).SetName("Table5x5_MiddleRow");
            yield return new TestCaseData(new int[,]
            {
                {1, 2, 1, 2, 1},
                {1, 1, 2, 1, 2},
                {2, 2, 1, 1, 2},
                {1, 2, 2, 2, 1},
                {2, 1, 1, 2, 1}
            }, new ElementIndex(4, 0)).Returns(new int[] {2, 1, 1, 2, 1}).SetName("Table5x5_LastRow");
        }

        private static IEnumerable<TestCaseData> TableDataForRowLength()
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

        private static IEnumerable<TestCaseData> TableDataForRowIndex()
        {
            yield return new TestCaseData(new int[,] { }, new ElementIndex(1, 0)).SetName("TableEmpty_Index10");

            yield return new TestCaseData(new int[,]
            {
                {2, 1, 1},
                {1, 2, 2},
                {2, 2, 1}
            }, new ElementIndex(3, 0)).SetName("Table3x3_Index30");

            yield return new TestCaseData(new int[,]
            {
                {1, 1, 2, 2},
                {2, 1, 2, 1},
                {1, 2, 1, 2},
                {1, 1, 2, 2}
            }, new ElementIndex(4, 0)).SetName("Table4x4_Index40");

            yield return new TestCaseData(new int[,]
            {
                {1, 2, 1, 2, 1},
                {1, 1, 2, 1, 2},
                {2, 2, 1, 1, 2},
                {1, 2, 2, 2, 1},
                {2, 1, 1, 2, 1}
            }, new ElementIndex(5, 0)).SetName("Table5x5_Index50");

        }

        private static IEnumerable<TestCaseData> TableDataForListSize()
        {
            yield return new TestCaseData(new int[,]
            {
                {2, 1, 1},
                {1, 2, 2},
                {2, 2, 1}
            }, new ElementIndex(1, 0)).SetName("Table3x3_Index03");

            yield return new TestCaseData(new int[,]
            {
                {1, 1, 2, 2},
                {2, 1, 2, 1},
                {1, 2, 1, 2},
                {1, 1, 2, 2}
            }, new ElementIndex(2, 2)).SetName("Table4x4_Index04");

            yield return new TestCaseData(new int[,]
            {
                {1, 2, 1, 2, 1},
                {1, 1, 2, 1, 2},
                {2, 2, 1, 1, 2},
                {1, 2, 2, 2, 1},
                {2, 1, 1, 2, 1}
            }, new ElementIndex(4, 4)).SetName("Table5x5_Index05");

        }

        
        [SetUp]
        public void SetUp()
        {
            extractRow = new ExtractRow<int>();
        }

        [Test]
        [TestCaseSource(nameof(TableDataForRowMatch))]
        public int[] Extract_WhenCalled_ReturnsRowAtGivenRowIndex(int[,] table, ElementIndex index)
        {
            return extractRow.Extract(table, index);
        }

        [Test]
        [TestCaseSource(nameof(TableDataForRowLength))]
        public void Extract_WhenCalled_ReturnsArrayWithLengthEqualToRowLength(int[,] table)
        {
            int[] row = extractRow.Extract(table, new ElementIndex(0, 0));
            Assert.That(row.Length, Is.EqualTo(table.GetUpperBound(0) + 1));
        }

        [Test]
        public void Extract_ElementIndexIsNull_ThrowArgumentNullException()
        {
            Assert.That(() => extractRow.Extract(new int[,] {{0, 0, 0}, {0, 0, 0}}, null),
                Throws.ArgumentNullException);
        }

        [Test]
        [TestCaseSource(nameof(TableDataForRowIndex))]
        public void Extract_RowIndexIsHigherThenRowCount_ThrowException(int[,] table, ElementIndex index)
        {
            Assert.That(() => extractRow.Extract(table, index), Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }
        
        [Test]
        [TestCaseSource(nameof(TableDataForListSize))]
        public void GetIndexes_ListLengthEqualsArrayLength_ReturnsSizeEquality(int[,] table, ElementIndex index)
        {
            int[] column = extractRow.Extract(table, index);

            Assert.That(extractRow.GetIndexes().Count, Is.EqualTo(column.Length));
        }
    }
}