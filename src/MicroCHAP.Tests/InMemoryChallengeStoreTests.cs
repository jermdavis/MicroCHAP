using System.Threading;
using FluentAssertions;
using MicroCHAP.Server;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MicroCHAP.Tests
{

	[TestClass]
	public class InMemoryChallengeStoreTests
	{
		[TestMethod]
		public void ConsumeChallenge_ShouldReturnFalseIfChallengeDoesNotExist()
		{
			var store = CreateTestStore();

			store.ConsumeChallenge("FAKE").Should().BeFalse();
		}

		[TestMethod]
		public void ConsumeChallenge_ShouldReturnFalseIfChallengeIsTooOld()
		{
			var store = CreateTestStore();

			store.AddChallenge("FAKE", 100);

			Thread.Sleep(150);

			store.ConsumeChallenge("FAKE").Should().BeFalse();
		}

		[TestMethod]
		public void ConsumeChallenge_ShouldReturnTrue_IfTokenIsValid()
		{
			var store = CreateTestStore();

			store.AddChallenge("FAKE", 100);

			store.ConsumeChallenge("FAKE").Should().BeTrue();
		}

		[TestMethod]
		public void ConsumeChallenge_ShouldNotAllowReusingTokens()
		{
			var store = CreateTestStore();

			store.AddChallenge("FAKE", 100);

			store.ConsumeChallenge("FAKE").Should().BeTrue();
			store.ConsumeChallenge("FAKE").Should().BeFalse();
		}

		private IChallengeStore CreateTestStore()
		{
			return new InMemoryChallengeStore(null);
		}
	}
}
