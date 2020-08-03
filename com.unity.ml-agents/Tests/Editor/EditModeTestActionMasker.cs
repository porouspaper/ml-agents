using System;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;

namespace Unity.MLAgents.Tests
{
    public class EditModeTestActionMasker
    {
        [Test]
        public void Contruction()
        {
            var bam = new BufferedDiscreteActionMask(new List<IActuator>(), 0, 0);
            var masker = new DiscreteActionMasker(bam);
            Assert.IsNotNull(masker);
        }

        [Test]
        public void NullMask()
        {
            var bp = new BrainParameters();
            bp.VectorActionSpaceType = SpaceType.Discrete;
            var bam = new BufferedDiscreteActionMask(Array.Empty<int>());
            var masker = new DiscreteActionMasker(bam);
            var mask = masker.GetMask();
            Assert.IsNull(mask);
        }

        [Test]
        public void FirstBranchMask()
        {
            var bam = new BufferedDiscreteActionMask(new []{4, 5, 6});
            var masker = new DiscreteActionMasker(bam);
            var mask = masker.GetMask();
            Assert.IsNull(mask);
            masker.SetMask(0, new[] {1, 2, 3});
            mask = masker.GetMask();
            Assert.IsFalse(mask[0]);
            Assert.IsTrue(mask[1]);
            Assert.IsTrue(mask[2]);
            Assert.IsTrue(mask[3]);
            Assert.IsFalse(mask[4]);
            Assert.AreEqual(mask.Length, 15);
        }

        [Test]
        public void SecondBranchMask()
        {
            var bam = new BufferedDiscreteActionMask(new []{4, 5, 6});
            var masker = new DiscreteActionMasker(bam);
            masker.SetMask(1, new[] {1, 2, 3});
            var mask = masker.GetMask();
            Assert.IsFalse(mask[0]);
            Assert.IsFalse(mask[4]);
            Assert.IsTrue(mask[5]);
            Assert.IsTrue(mask[6]);
            Assert.IsTrue(mask[7]);
            Assert.IsFalse(mask[8]);
            Assert.IsFalse(mask[9]);
        }

        [Test]
        public void MaskReset()
        {
            var bam = new BufferedDiscreteActionMask(new []{4, 5, 6});
            var masker = new DiscreteActionMasker(bam);
            masker.SetMask(1, new[] {1, 2, 3});
            masker.ResetMask();
            var mask = masker.GetMask();
            for (var i = 0; i < 15; i++)
            {
                Assert.IsFalse(mask[i]);
            }
        }

        [Test]
        public void ThrowsError()
        {
            var bam = new BufferedDiscreteActionMask(new []{4, 5, 6});
            var masker = new DiscreteActionMasker(bam);

            Assert.Catch<UnityAgentsException>(
                () => masker.SetMask(0, new[] {5}));
            Assert.Catch<UnityAgentsException>(
                () => masker.SetMask(1, new[] {5}));
            masker.SetMask(2, new[] {5});
            Assert.Catch<UnityAgentsException>(
                () => masker.SetMask(3, new[] {1}));
            masker.GetMask();
            masker.ResetMask();
            masker.SetMask(0, new[] {0, 1, 2, 3});
            Assert.Catch<UnityAgentsException>(
                () => masker.GetMask());
        }

        [Test]
        public void MultipleMaskEdit()
        {
            var bam = new BufferedDiscreteActionMask(new []{4, 5, 6});
            var masker = new DiscreteActionMasker(bam);
            masker.SetMask(0, new[] {0, 1});
            masker.SetMask(0, new[] {3});
            masker.SetMask(2, new[] {1});
            var mask = masker.GetMask();
            for (var i = 0; i < 15; i++)
            {
                if ((i == 0) || (i == 1) || (i == 3) || (i == 10))
                {
                    Assert.IsTrue(mask[i]);
                }
                else
                {
                    Assert.IsFalse(mask[i]);
                }
            }
        }
    }
}
