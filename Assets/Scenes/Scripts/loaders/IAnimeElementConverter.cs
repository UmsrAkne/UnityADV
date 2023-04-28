using System.Xml.Linq;
using Scenes.Scripts.Animations;

namespace Scenes.Scripts.Loaders
{
    public interface IAnimeElementConverter : IXMLElementConverter
    {
        IAnimation GenerateAnimation(XElement element);
    }
}