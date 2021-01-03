using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    public class ElementIndexTests
    {
        [Test]
        [TestCase(-1, 0)]
        [TestCase(0, -1)]
        public void ElementIndex_LessThenZeroRowOrColumnIndexes_ThrowsException(int row, int column)
        {
            Assert.That(()=>new ElementIndex(row, column), Throws.Exception.TypeOf<ArgumentException>());
        }
    
        [Test]
        [TestCase(-1, 0)]
        [TestCase(0, -1)]
        public void ElementIndex_LessThenZeroRowOrColumnLengths_ThrowsException(int rLength, int cLength)
        {
            Assert.That(()=>new ElementIndex(1, rLength, cLength), Throws.Exception.TypeOf<ArgumentException>());
        }
    }
}


