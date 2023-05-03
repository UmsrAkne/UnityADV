using System.Collections.Generic;
using System.Xml.Linq;
using Scenes.Scripts.Animations;
using Scenes.Scripts.Loaders;
using Scenes.Scripts.SceneContents;
using Tests.Animations;

namespace Tests.Loaders
{
    public class DummyAnimationGenerator : IAnimeElementConverter
    {
        public int GenerateCounter;

        public List<DummyAnimation> Animations { get; }= new List<DummyAnimation>();

        // ReSharper disable once UnassignedGetOnlyAutoProperty
        public string TargetElementName { get; }

        // ReSharper disable once UnassignedGetOnlyAutoProperty
        public List<string> Log { get; }

        public void Convert(XElement xmlElement, Scenario scenario)
        {
        }

        public IAnimation GenerateAnimation(XElement element)
        {
            return Animations[GenerateCounter++];
        }
    }
}