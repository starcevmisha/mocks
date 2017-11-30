using System.Collections.Generic;
using NUnit.Framework;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using FluentAssertions;

namespace MockFramework
{
	public class ThingCache
	{
		private readonly IDictionary<string, Thing> dictionary
			= new Dictionary<string, Thing>();
		private readonly IThingService thingService;

		public ThingCache(IThingService thingService)
		{
			this.thingService = thingService;
		}

		public Thing Get(string thingId)
		{
			Thing thing;
			if (dictionary.TryGetValue(thingId, out thing))
				return thing;
			if (thingService.TryRead(thingId, out thing))
			{
				dictionary[thingId] = thing;
				return thing;
			}
			return null;
		}
	}

	[TestFixture]
	public class ThingCache_Should
	{
		private IThingService thingService;
		private ThingCache thingCache;

		private const string thingId1 = "TheDress";

		private const string thingId2 = "CoolBoots";
		private Thing thing1;
		private Thing thing2;

		[SetUp]
		public void SetUp()
		{
			thingService = A.Fake<IThingService>();
			thingCache = new ThingCache(thingService);
			
			thing1 = new Thing(thingId1);
			thing2 = new Thing(thingId2);
			
		}

		//TODO: �������� ���������� ����, � ����� ��� ���������
		//Live Template tt ��������!
		[Test]
		public void TryGetExistedElement()
		{
			var value = new Thing(thingId1);

			A.CallTo(() => thingService.TryRead(thingId1, out value))
				.Returns(true);
			
			var actual = thingCache.Get(thingId1);
			actual.Should().Be(value);
		}

		[Test]
		public void TryGetNonExistedElement_ShouldBeNull()
		{
			Thing value;

			A.CallTo(() => thingService.TryRead(thingId1, out value))
				.Returns(false);
			
			var actual = thingCache.Get(thingId1);
			actual.Should().Be(null);			
		}

		[Test]
		public void ElementIsCachedTest()
		{
			var value = new Thing(thingId1);

			A.CallTo(() => thingService.TryRead(thingId1, out value))
				.Returns(true);
			
			thingCache.Get(thingId1);
			thingCache.Get(thingId1);

			A.CallTo(() => thingService.TryRead(thingId1, out value))
				.MustHaveHappened(Repeated.Exactly.Once);

		}

		[Test]
		public void DoSomething_WhenSomething()
		{
			A.CallTo(() => thingService.TryRead(thingId1, out thing1))
				.Returns(true);
			A.CallTo(() => thingService.TryRead(thingId2, out thing2))
				.Returns(true);
			
			var actual1 = thingCache.Get(thingId1);
			var actual2 = thingCache.Get(thingId2);
			
			actual1.Should().Be(thing1);
			actual2.Should().Be(thing2);
		}


		[Test]
		public void DoSomething_WhenSomething2()
		{
			A.CallTo(() => thingService.TryRead(thingId1, out thing1))
				.Returns(true);
			A.CallTo(() => thingService.TryRead(thingId2, out thing2))
				.Returns(true);
			
			thingCache.Get(thingId1);
			thingCache.Get(thingId2);
			var actual1 = thingCache.Get(thingId1);
			var actual2 = thingCache.Get(thingId2);
			
			actual1.Should().Be(thing1);
			actual2.Should().Be(thing2);
		}
	}
}