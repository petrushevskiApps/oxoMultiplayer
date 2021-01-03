using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Tests
{
    public class ExtractColumnTests
    {
        private ExtractColumn<int> extractColumn;

        private static IEnumerable<TestCaseData> TableDataForColumnMatch()
        {
            yield return new TestCaseData(new int[,]
            {
                {2, 1, 1},
                {1, 2, 2},
                {2, 2, 1}
            }, new ElementIndex(0, 0)).Returns(new int[] {2, 1, 2}).SetName("Table3x3_FirstColumn");
            yield return new TestCaseData(new int[,]
            {
                {2, 1, 1},
                {1, 2, 2},
                {2, 2, 1}
            }, new ElementIndex(0, 1)).Returns(new int[] {1, 2, 2}).SetName("Table3x3_MiddleColumn");
            yield return new TestCaseData(new int[,]
            {
                {2, 1, 1},
                {1, 2, 2},
                {2, 2, 1}
            }, new ElementIndex(0, 2)).Returns(new int[] {1, 2, 1}).SetName("Table3x3_LastColumn");

            yield return new TestCaseData(new int[,]
            {
                {1, 1, 2, 2},
                {2, 1, 2, 1},
                {1, 2, 1, 2},
                {1, 1, 2, 2}
            }, new ElementIndex(0, 0)).Returns(new int[] {1, 2, 1, 1}).SetName("Table4x4_FirstColumn");
            yield return new TestCaseData(new int[,]
            {
                {1, 1, 2, 2},
                {2, 1, 2, 1},
                {1, 2, 1, 2},
                {1, 1, 2, 2}
            }, new ElementIndex(0, 2)).Returns(new int[] {2, 2, 1, 2}).SetName("Table4x4_MiddleColumn");
            yield return new TestCaseData(new int[,]
            {
                {1, 1, 2, 2},
                {2, 1, 2, 1},
                {1, 2, 1, 2},
                {1, 1, 2, 2}
            }, new ElementIndex(0, 3)).Returns(new int[] {2, 1, 2, 2}).SetName("Table4x4_LastColumn");


            yield return new TestCaseData(new int[,]
            {
                {1, 2, 1, 2, 1},
                {1, 1, 2, 1, 2},
                {2, 2, 1, 1, 2},
                {1, 2, 2, 2, 1},
                {2, 1, 1, 2, 1}
            }, new ElementIndex(0, 0)).Returns(new int[] {1, 1, 2, 1, 2}).SetName("Table5x5_FirstColumn");
            yield return new TestCaseData(new int[,]
            {
                {1, 2, 1, 2, 1},
                {1, 1, 2, 1, 2},
                {2, 2, 1, 1, 2},
                {1, 2, 2, 2, 1},
                {2, 1, 1, 2, 1}
            }, new ElementIndex(0, 2)).Returns(new int[] {1, 2, 1, 2, 1}).SetName("Table5x5_MiddleColumn");
            yield return new TestCaseData(new int[,]
            {
                {1, 2, 1, 2, 1},
                {1, 1, 2, 1, 2},
                {2, 2, 1, 1, 2},
                {1, 2, 2, 2, 1},
                {2, 1, 1, 2, 1}
            }, new ElementIndex(0, 4)).Returns(new int[] {1, 2, 2, 1, 1}).SetName("Table5x5_LastColumn");
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

        private static IEnumerable<TestCaseData> TableDataForColumnIndex()
        {
            yield return new TestCaseData(new int[,] { }, new ElementIndex(0, 1)).SetName("TableEmpty_Index01");

            yield return new TestCaseData(new int[,]
            {
                {2, 1, 1},
                {1, 2, 2},
                {2, 2, 1}
            }, new ElementIndex(0, 3)).SetName("Table3x3_Index03");

            yield return new TestCaseData(new int[,]
            {
                {1, 1, 2, 2},
                {2, 1, 2, 1},
                {1, 2, 1, 2},
                {1, 1, 2, 2}
            }, new ElementIndex(0, 4)).SetName("Table4x4_Index04");

            yield return new TestCaseData(new int[,]
            {
                {1, 2, 1, 2, 1},
                {1, 1, 2, 1, 2},
                {2, 2, 1, 1, 2},
                {1, 2, 2, 2, 1},
                {2, 1, 1, 2, 1}
            }, new ElementIndex(0, 5)).SetName("Table5x5_Index05");

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
            extractColumn = new ExtractColumn<int>();
        }

        [Test]
        [TestCaseSource(nameof(TableDataForColumnMatch))]
        public int[] Extract_WhenCalled_ReturnsColumnAtGivenIndex(int[,] table, ElementIndex index)
        {
            return extractColumn.Extract(table, index);
        }

        [Test]
        [TestCaseSource(nameof(TableDataForColumnLength))]
        public void Extract_WhenCalled_ReturnsArrayWithLengthEqualToColumnLength(int[,] table)
        {
            int[] column = extractColumn.Extract(table, new ElementIndex(0, 0));
            Assert.That(column.Length, Is.EqualTo(table.GetUpperBound(1) + 1));
        }

        [Test]
        public void Extract_ElementIndexIsNull_ThrowArgumentNullException()
        {
            Assert.That(() => extractColumn.Extract(new int[,] {{0, 0, 0}, {0, 0, 0}}, null),
                Throws.ArgumentNullException);
        }

        [Test]
        [TestCaseSource(nameof(TableDataForColumnIndex))]
        public void Extract_ColumnIndexIsHigherThanColumnLength_ThrowException(int[,] table, ElementIndex index)
        {
            Assert.That(() => extractColumn.Extract(table, index), Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        [TestCaseSource(nameof(TableDataForListSize))]
        public void GetIndexes_ListLengthEqualsArrayLength_ReturnsSizeEquality(int[,] table, ElementIndex index)
        {
            int[] column = extractColumn.Extract(table, index);

            Assert.That(extractColumn.GetIndexes().Count, Is.EqualTo(column.Length));
        }
       
        
    }
}