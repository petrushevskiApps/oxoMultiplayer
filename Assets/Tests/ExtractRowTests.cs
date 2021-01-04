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
            }, 0).Returns(new int[] {2, 1, 1}).SetName("Table3x3_FirstRow");
            yield return new TestCaseData(new int[,]
            {
                {2, 1, 1},
                {1, 2, 2},
                {2, 2, 1}
            }, 3).Returns(new int[] {1, 2, 2}).SetName("Table3x3_MiddleRow");
            yield return new TestCaseData(new int[,]
            {
                {2, 1, 1},
                {1, 2, 2},
                {2, 2, 1}
            }, 6).Returns(new int[] {2, 2, 1}).SetName("Table3x3_LastRow");

            yield return new TestCaseData(new int[,]
            {
                {1, 1, 2, 2},
                {2, 1, 2, 1},
                {1, 2, 1, 2},
                {1, 1, 2, 2}
            }, 0).Returns(new int[] {1, 1, 2, 2}).SetName("Table4x4_FirstRow");
            yield return new TestCaseData(new int[,]
            {
                {1, 1, 2, 2},
                {2, 1, 2, 1},
                {1, 2, 1, 2},
                {1, 1, 2, 2}
            }, 8).Returns(new int[] {1, 2, 1, 2}).SetName("Table4x4_MiddleRow");
            yield return new TestCaseData(new int[,]
            {
                {1, 1, 2, 2},
                {2, 1, 2, 1},
                {1, 2, 1, 2},
                {1, 1, 2, 2}
            }, 13).Returns(new int[] {1, 1, 2, 2}).SetName("Table4x4_LastRow");


            yield return new TestCaseData(new int[,]
            {
                {1, 2, 1, 2, 1},
                {1, 1, 2, 1, 2},
                {2, 2, 1, 1, 2},
                {1, 2, 2, 2, 1},
                {2, 1, 1, 2, 1}
            }, 0).Returns(new int[] {1, 2, 1, 2, 1}).SetName("Table5x5_FirstRow");
            yield return new TestCaseData(new int[,]
            {
                {1, 2, 1, 2, 1},
                {1, 1, 2, 1, 2},
                {2, 2, 1, 1, 2},
                {1, 2, 2, 2, 1},
                {2, 1, 1, 2, 1}
            }, 10).Returns(new int[] {2, 2, 1, 1, 2}).SetName("Table5x5_MiddleRow");
            yield return new TestCaseData(new int[,]
            {
                {1, 2, 1, 2, 1},
                {1, 1, 2, 1, 2},
                {2, 2, 1, 1, 2},
                {1, 2, 2, 2, 1},
                {2, 1, 1, 2, 1}
            }, 21).Returns(new int[] {2, 1, 1, 2, 1}).SetName("Table5x5_LastRow");
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
            extractRow = new ExtractRow<int>();
        }

        [Test]
        [TestCaseSource(nameof(TableDataForRowMatch))]
        public int[] Extract_WhenCalled_ReturnsRowAtGivenRowIndex(int[,] table, int id)
        {
            return extractRow.Extract(table, id);
        }

        [Test]
        [TestCaseSource(nameof(TableDataForRowLength))]
        public void Extract_WhenCalled_ReturnsArrayWithLengthEqualToRowLength(int[,] table)
        {
            int[] row = extractRow.Extract(table, 0);
            Assert.That(row.Length, Is.EqualTo(table.GetUpperBound(0) + 1));
        }

        [Test]
        [TestCaseSource(nameof(TableDataForIdOutOfBounds))]
        public void Extract_IdOutOfBounds_ThrowException(int[,] table, int id)
        {
            Assert.That(() => extractRow.Extract(table, id), Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }
    }
}