using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace MiradorWorks.Pooling.Tests
{
    public sealed class PlayModeSmokeTests
    {
        [UnityTest]
        public IEnumerator PlayMode_AdvancesOneFrame()
        {
            yield return null;
            Assert.Pass();
        }
    }
}