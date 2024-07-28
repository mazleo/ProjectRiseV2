using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using ProjectRise.Terrain;

namespace ProjectRise.Test.Terrain
{
    /// <summary>
    /// Test cases for the TerrainTransformerCollection.
    /// </summary>
    public class TerrainTransformerCollectionTest
    {
        private TerrainTransformerCollection _collection;

        [SetUp]
        public void Setup()
        {
            _collection = new TerrainTransformerCollection();
        }

        [Test]
        public void TerrainTransformerCollection_CorrectInitialization()
        {
            Assert.That(_collection.TransformerCollection, !Is.Null);
            Assert.That(_collection.TransformerMap, !Is.Null);

            Assert.That(_collection.TransformerCollection.Count, Is.EqualTo(0));
            Assert.That(_collection.Count(), Is.EqualTo(0));
            Assert.That(_collection.IsEmpty(), Is.True);
            Assert.That(_collection.TransformerMap.Count, Is.EqualTo(0));
        }

        [Test]
        public void Register_ValidTransformers()
        {
            FakeNoopTerrainTransformer transformer0 = new FakeNoopTerrainTransformer();
            FakeNoopTerrainTransformer transformer1 = new FakeNoopTerrainTransformer();
            FakeNoopTerrainTransformer transformer2 = new FakeNoopTerrainTransformer();

            _collection.Register(0, "id0", transformer0);
            _collection.Register(1, "id1", transformer1);
            _collection.Register(2, "id2", transformer2);

            Assert.That(_collection.TransformerCollection[0], Is.EqualTo("id0"));
            Assert.That(_collection.TransformerCollection[1], Is.EqualTo("id1"));
            Assert.That(_collection.TransformerCollection[2], Is.EqualTo("id2"));

            Assert.That(_collection.TransformerMap["id0"], Is.EqualTo(transformer0));
            Assert.That(_collection.TransformerMap["id1"], Is.EqualTo(transformer1));
            Assert.That(_collection.TransformerMap["id2"], Is.EqualTo(transformer2));

            Assert.That(_collection.Count(), Is.EqualTo(3));
        }

        [Test]
        public void Register_InvalidValues()
        {
            FakeNoopTerrainTransformer transformer = new FakeNoopTerrainTransformer();

            // Registering invalid indeces
            Assert.Throws<ArgumentException>(() => _collection.Register(-1, "id", transformer));
            Assert.Throws<ArgumentException>(() => _collection.Register(1, "id", transformer));
            _collection.Register(0, "id", transformer);
            Assert.Throws<ArgumentException>(() => _collection.Register(2, "id", transformer));

            // Registering invalid IDs
            Assert.Throws<ArgumentException>(() => _collection.Register(1, "", transformer));
            Assert.Throws<ArgumentNullException>(() => _collection.Register(1, null, transformer));
            Assert.Throws<ArgumentException>(() => _collection.Register(1, "id", transformer));

            // Registering invalid transformers
            Assert.Throws<ArgumentNullException>(() => _collection.Register(1, "id2", null));
        }

        [Test]
        public void Get_ValidId()
        {
            FakeNoopTerrainTransformer transformer = new FakeNoopTerrainTransformer();
            _collection.Register(0, "id", transformer);

            Assert.That(_collection.Get("id"), Is.EqualTo(transformer));
        }

        [Test]
        public void Get_InvalidId()
        {
            Assert.Throws<ArgumentNullException>(() => _collection.Get(null));
            Assert.Throws<ArgumentException>(() => _collection.Get("nonexistent-id"));
        }

        [Test]
        public void GetAll_Empty()
        {
            List<ITerrainModelTransformer> emptyList = _collection.GetAll();
            Assert.That(emptyList, !Is.Null);
            Assert.That(emptyList.Count(), Is.EqualTo(0));
        }

        [Test]
        public void GetAll_NonEmpty()
        {
            FakeNoopTerrainTransformer transformer0 = new FakeNoopTerrainTransformer();
            FakeNoopTerrainTransformer transformer1 = new FakeNoopTerrainTransformer();
            FakeNoopTerrainTransformer transformer2 = new FakeNoopTerrainTransformer();

            _collection.Register(0, "id0", transformer0);
            _collection.Register(1, "id1", transformer1);
            _collection.Register(2, "id2", transformer2);

            List<ITerrainModelTransformer> transformers = _collection.GetAll();
            Assert.That(transformers.Count(), Is.EqualTo(3));
            Assert.That(transformers[0], Is.EqualTo(transformer0));
            Assert.That(transformers[1], Is.EqualTo(transformer1));
            Assert.That(transformers[2], Is.EqualTo(transformer2));
        }
    }
}
