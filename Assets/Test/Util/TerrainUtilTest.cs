using System;
using NUnit.Framework;
using ProjectRise.Utils;

namespace ProjectRise.Test.Util
{
    /// <summary>
    /// Test cases for the Terrain Util.
    /// </summary>
    public class TerrainUtilTest
    {
        [Test]
        public void IsValidIndex_ValidIndeces()
        {
            Assert.That(
                TerrainUtil.IsValidIndex( /*index=*/
                    0, /*mapLength=*/
                    15
                ),
                Is.True
            );
            Assert.That(
                TerrainUtil.IsValidIndex( /*index=*/
                    83, /*mapLength=*/
                    100
                ),
                Is.True
            );
            Assert.That(
                TerrainUtil.IsValidIndex( /*index=*/
                    199, /*mapLength=*/
                    1000
                ),
                Is.True
            );
        }

        [Test]
        public void IsValidIndex_InvalidIndeces()
        {
            Assert.That(
                TerrainUtil.IsValidIndex( /*index=*/
                    -24, /*mapLength=*/
                    10
                ),
                Is.False
            );
            Assert.That(
                TerrainUtil.IsValidIndex( /*index=*/
                    999, /*mapLength=*/
                    100
                ),
                Is.False
            );
            Assert.That(
                TerrainUtil.IsValidIndex( /*index=*/
                    1999, /*mapLength=*/
                    1000
                ),
                Is.False
            );
        }

        [Test]
        public void GetOneDimensionalIndex()
        {
            Assert.That(TerrainUtil.GetOneDimensionalIndex(0, 0, 10), Is.EqualTo(0));
            Assert.That(TerrainUtil.GetOneDimensionalIndex(2, 2, 3), Is.EqualTo(8));
            Assert.That(TerrainUtil.GetOneDimensionalIndex(1, 1, 3), Is.EqualTo(4));
        }

        [Test]
        public void GetNeighbors_ValidInputs()
        {
            Assert.That(
                TerrainUtil.GetNeighbors(0, unused => true, 2, 4),
                Is.EqualTo(new int[] { 2, 3, 1 })
            );
            Assert.That(
                TerrainUtil.GetNeighbors(1, unused => true, 2, 4),
                Is.EqualTo(new int[] { 2, 3, 0 })
            );
            Assert.That(
                TerrainUtil.GetNeighbors(2, unused => true, 2, 4),
                Is.EqualTo(new int[] { 3, 0, 1 })
            );
            Assert.That(
                TerrainUtil.GetNeighbors(3, unused => true, 2, 4),
                Is.EqualTo(new int[] { 2, 0, 1 })
            );
        }

        [Test]
        public void GetNeighbors_InvalidInputs()
        {
            Assert.Throws<ArgumentException>(
                () => TerrainUtil.GetNeighbors(-1, unused => true, 1, 1)
            );
            Assert.Throws<ArgumentException>(
                () => TerrainUtil.GetNeighbors(0, unused => true, -1, 1)
            );
            Assert.Throws<ArgumentException>(
                () => TerrainUtil.GetNeighbors(0, unused => true, 1, -1)
            );
            Assert.Throws<ArgumentException>(() => TerrainUtil.GetNeighbors(0, null, 1, 1));
            Assert.Throws<IndexOutOfRangeException>(
                () => TerrainUtil.GetNeighbors(6, unused => true, 1, 1)
            );
            Assert.Throws<ArgumentException>(
                () => TerrainUtil.GetNeighbors(0, unused => true, 9, 1)
            );
        }
    }
}
