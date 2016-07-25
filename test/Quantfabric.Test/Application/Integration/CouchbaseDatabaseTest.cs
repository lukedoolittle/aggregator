using System;
using System.Collections.Generic;
using System.Linq;
using Aggregator.Domain.Events;
using Aggregator.Framework.Exceptions;
using Aggregator.Test.Helpers;
using Aggregator.Test.Helpers.Mocks;
using SimpleCQRS.Domain;
using SimpleCQRS.Framework.Contracts;
using SimpleCQRS.Infrastructure;
using Xunit;

namespace Aggregator.Test.Integration
{
	[CollectionDefinition("Database collection")]
	public class CouchbaseDatabaseTest : IClassFixture<DatabaseFixture>
	{
		private readonly DatabaseFixture _fixture;

		public CouchbaseDatabaseTest(DatabaseFixture fixture)
		{
			_fixture = fixture;
		}
		
		[Fact]
		public void PutEntityWithDuplicateStorageKeyExpectException()
		{
			var storage = _fixture.GetDatabase<UniqueMock>();
			var id = Guid.NewGuid();
			var entity = new UniqueMock(id);
			storage.Put(entity);

			var anotherEntity = new UniqueMock(id);
			Exception expectedException = null;

			try
			{
				storage.Put(anotherEntity);
			}
			catch (CouchbaseDuplicateStorageKeyException e)
			{
				expectedException = e;
			}

			Assert.NotNull(expectedException);
			Assert.True(expectedException is CouchbaseDuplicateStorageKeyException);
		}

		[Fact]
		public void DatabasePutShouldSucceed()
		{
			var storage = _fixture.GetDatabase<UniqueMock>();
			var id = Guid.NewGuid();
			var entity = new UniqueMock(id);

			storage.Put(entity);

			storage.Get(id.ToString());
		}

		[Fact]
		public void DatabasePutThenUpdateEntity()
		{
			var storage = _fixture.GetDatabase<UniqueMock>();
			var id = Guid.NewGuid();
			var entity = new UniqueMock(id);

			storage.Put(entity);

			storage.Update(entity);

			var actual = storage.Get(id.ToString());

			Assert.NotNull(actual);
		}

		[Fact]
		public void DatabasePutThenGetAnEntity()
		{
			var storage = _fixture.GetDatabase<UniqueMock>();

			var id = Guid.NewGuid();
			var entity = new UniqueMock(id);

			storage.Put(entity);

			var actual = storage.Get(id.ToString());

			Assert.NotNull(actual);
			Assert.Equal(entity.Id, actual.Id);
		}

		[Fact]
		public void DatabasePutAndThenGetEventDescriptorsIsSuccessful()
		{
			var storage = _fixture.GetDatabase<EventDescriptors>();
			var aggregateId = Guid.NewGuid();
			var entity = new EventDescriptors {Id = aggregateId};

			var expectedId = Guid.NewGuid();
			const int expectedVersion = -1;
			var expectedEvent = new PersonCreated(aggregateId);
			var expected = new EventDescriptor {Id = expectedId, EventData = expectedEvent, Version = expectedVersion};
			entity.Add(expected);

			storage.Put(entity);

			var actual = storage.Get(aggregateId.ToString());

			Assert.NotNull(actual);
			Assert.Equal(aggregateId, actual.Id);
			Assert.Equal(1, actual.Count());

			var actualId = actual[0].Id;
			var actualVersion = actual[0].Version;
			Assert.Equal(expectedId, actualId);
			Assert.Equal(expectedVersion, actualVersion);

			var actualEvent = actual[0].EventData;
			Assert.True(actualEvent is PersonCreated);
			Assert.Equal(expectedEvent.PersonId.ToString(), (actualEvent as PersonCreated).PersonId.ToString());
		}

		[Fact]
		public void PutManyItemsAndGetAllForOnlyASubsetOfAggregatesAndTypes()
		{
			var expected = 3;

			var storage = _fixture.GetDatabase<TargetType>();
			var anotherStorage = _fixture.GetDatabase<AnotherType>();

			var targetAggregateId = Guid.NewGuid();
			var anotherAggregateId = Guid.NewGuid();

			var itemsToPut = new List<TargetType>
			{
				new TargetType(targetAggregateId),
				new TargetType(targetAggregateId),
				new TargetType(targetAggregateId),
				new TargetType(anotherAggregateId),
				new TargetType(anotherAggregateId),
				new TargetType(anotherAggregateId),
			};

			var otherItemsToPut = new List<AnotherType>
			{
				new AnotherType(anotherAggregateId),
				new AnotherType(anotherAggregateId),
				new AnotherType(anotherAggregateId),
				new AnotherType(targetAggregateId),
				new AnotherType(targetAggregateId),
				new AnotherType(targetAggregateId)
			};

			foreach (var item in itemsToPut)
			{
				storage.Put(item);
			}

			foreach (var item in otherItemsToPut)
			{
				anotherStorage.Put(item);
			}

			var actual = storage.GetAll(targetAggregateId.ToString());

			Assert.Equal(expected, actual.Count());
		}
	}

	public class TargetType : Entity
	{
		public Guid AggregateId { get; set; }

		public TargetType(Guid aggregateId)
		{
			AggregateId = aggregateId;
			Id = Guid.NewGuid();
		}
	}

	public class AnotherType : Entity
	{
		public Guid AggregateId { get; set; }

		public AnotherType(Guid aggregateId)
		{
			AggregateId = aggregateId;
			Id = Guid.NewGuid();
		}
	}
}
