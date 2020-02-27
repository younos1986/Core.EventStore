// using System;
// using System.Diagnostics.CodeAnalysis;
// using System.Threading.Tasks;
//
// namespace Core.EventStore.IntegrationTest.Infrastructures
// {
//     [TestFixture]
//     public abstract class GivenWhenThen<TSut> : GivenWhenThenBase
//         where TSut : class
//     {
//         /// <summary>
//         /// Initializes a new instance of the <see cref="GivenWhenThen{TSut}"/> class.
//         /// </summary>
//         protected GivenWhenThen()
//         {
//         }
//
//         /// <summary>
//         /// Initializes a new instance of the <see cref="GivenWhenThen{TSut}" /> class.
//         /// </summary>
//         /// <param name="fixtureIoC">The fixture IoC container.</param>
//         protected GivenWhenThen(IFixtureKernel fixtureIoC)
//             : base(fixtureIoC)
//         {
//         }
//
//         /// <summary>
//         /// Gets the Software under Test.
//         /// </summary>
//         protected TSut Sut { get; private set; }
//
//         /// <inheritdoc />
//         [SetUp]
//         public override void TestSetup()
//         {
//             base.TestSetup();
//             this.Sut = this.Given(this.Kernel);
//             this.When(this.Sut);
//         }
//
//         /// <summary>
//         /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
//         /// </summary>
//         public new void Dispose()
//         {
//             this.Cleanup(this.Sut);
//             base.Dispose();
//         }
//
//         /// <summary>
//         /// Cleans the test environment after the test methods have been executed
//         /// and also clears the IoC cache.
//         /// </summary>
//         /// <param name="sut">The SUT.</param>
//         /// <remarks>Will call the<see cref="IDisposable" /> interface if the SUT implements it.</remarks>
//         [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Test fixture code")]
//         protected virtual void Cleanup(TSut sut)
//         {
//             try
//             {
//                 var disposable = this.Sut as IDisposable;
//                 disposable?.Dispose();
//             }
//             catch (Exception ex)
//             {
//                 Console.WriteLine(ex.Message);
//             }
//
//             try
//             {
//                 this.Kernel?.Dispose();
//             }
//             catch (Exception ex)
//             {
//                 Console.WriteLine(ex.Message);
//             }
//         }
//
//         /// <summary>
//         /// Arrange all necessary preconditions and inputs.
//         /// </summary>
//         /// <param name="kernel">The <see cref="IFixtureKernel"/> Test Double IoC container.</param>
//         /// <returns>The System/Software Under Test.</returns>
//         protected virtual TSut Given(IFixtureKernel kernel)
//         {
//             return this.Get<TSut>();
//         }
//
//         /// <summary>
//         /// Act on the software/system under test.
//         /// </summary>
//         /// <param name="sut">The System/Software under Test.</param>
//         [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "When", Justification = "Given When Then")]
//         protected virtual void When(TSut sut)
//         {
//         }
//     }
// }
