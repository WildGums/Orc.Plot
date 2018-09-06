namespace Orc.Plot.Tests
{
    using System.Runtime.CompilerServices;
    using ApiApprover;
    using NUnit.Framework;

    [TestFixture]
    public class PublicApiFacts
    {
        [Test, MethodImpl(MethodImplOptions.NoInlining)]
        public void Orc_Plot_HasNoBreakingChanges()
        {
            var assembly = typeof(PlotView).Assembly;

            PublicApiApprover.ApprovePublicApi(assembly);
        }
    }
}
