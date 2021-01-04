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
            }, 0).Returns(new int[] {2, 1, 2}).SetName("Table3x3_FirstColumn");
            yield return new TestCaseData(new int[,]
            {
                {2, 1, 1},
                {1, 2, 2},
                {2, 2, 1}
            }, 1).Returns(new int[] {1, 2, 2}).SetName("Table3x3_MiddleColumn");
            yield return new TestCaseData(new int[,]
            {
                {2, 1, 1},
                {1, 2, 2},
                {2, 2, 1}
            }, 2).Returns(new int[] {1, 2, 1}).SetName("Table3x3_LastColumn");

            yield return new TestCaseData(new int[,]
            {
                {1, 1, 2, 2},
                {2, 1, 2, 1},
                {1, 2, 1, 2},
                {1, 1, 2, 2}
            }, 0).Returns(new int[] {1, 2, 1, 1}).SetName("Table4x4_FirstColumn");
            yield return new TestCaseData(new int[,]
            {
                {1, 1, 2, 2},
                {2, 1, 2, 1},
                {1, 2, 1, 2},
                {1, 1, 2, 2}
            }, 2).Returns(new int[] {2, 2, 1, 2}).SetName("Table4x4_MiddleColumn");
            yield return new TestCaseData(new int[,]
            {
                {1, 1, 2, 2},
                {2, 1, 2, 1},
                {1, 2, 1, 2},
                {1, 1, 2, 2}
            }, 3).Returns(new int[] {2, 1, 2, 2}).SetName("Table4x4_LastColumn");


            yield return new TestCaseData(new int[,]
            {
                {1, 2, 1, 2, 1},
                {1, 1, 2, 1, 2},
                {2, 2, 1, 1, 2},
                {1, 2, 2, 2, 1},
                {2, 1, 1, 2, 1}
            }, 0).Returns(new int[] {1, 1, 2, 1, 2}).SetName("Table5x5_FirstColumn");
            yield return new TestCaseData(new int[,]
            {
                {1, 2, 1, 2, 1},
                {1, 1, 2, 1, 2},
                {2, 2, 1, 1, 2},
                {1, 2, 2, 2, 1},
                {2, 1, 1, 2, 1}
            }, 2).Returns(new int[] {1, 2, 1, 2, 1}).SetName("Table5x5_MiddleColumn");
            yield return new TestCaseData(new int[,]
            {
                {1, 2, 1, 2, 1},
                {1, 1, 2, 1, 2},
                {2, 2, 1, 1, 2},
                {1, 2, 2, 2, 1},
                {2, 1, 1, 2, 1}
            }, 4).Returns(new int[] {1, 2, 2, 1, 1}).SetName("Table5x5_LastColumn");
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
            extractColumn = new ExtractColumn<int>();
        }

        [Test]
        [TestCaseSource(nameof(TableDataForColumnMatch))]
        public int[] Extract_WhenCalled_ReturnsColumnAtGivenIndex(int[,] table, int id)
        {
            return extractColumn.Extract(table, id);
        }

        [Test]
        [TestCaseSource(nameof(TableDataForColumnLength))]
        public void Extract_WhenCalled_ReturnsArrayWithLengthEqualToColumnLength(int[,] table)
        {
            int[] column = extractColumn.Extract(table, 0);
            Assert.That(column.Length, Is.EqualTo(table.GetUpperBound(1) + 1));
        }


        [Test]
        [TestCaseSource(nameof(TableDataForIdOutOfBounds))]
        public void Extract_IdOutOfBounds_ThrowArgumentOutOfRangeException(int[,] table, int id)
        {
            Assert.That(() => extractColumn.Extract(table, id), Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }

       
        
    }
}