namespace Animations
{
    using System;
    using System.Linq;
    using System.Xml.Linq;
    using Loaders;
    using SceneContents;

    public class AnimeElementConverter : IXMLElementConverter
    {
        public string TargetElementName => "anime";

        public void Convert(XElement xmlElement, Scenario scenario)
        {
            var tags = xmlElement.Elements(TargetElementName);

            if (tags.Count() != 0)
            {
                foreach (XElement animeTag in tags)
                {
                    IAnimation anime = GenerateAnimation(animeTag.Attribute("name").Value);

                    Type type = anime.GetType();
                    var attributes = animeTag.Attributes().Where(a => a.Name != "name");

                    foreach (var attribute in attributes)
                    {
                        // リフレクションを使用して、セットしたいプロパティがクラスに存在するかを確認してからセットする。
                        // 20220513 現在の実装では、存在を確認してセットできるのは整数型のみ。
                        var propInfo = type.GetProperty(attribute.Name.ToString());

                        if (propInfo != null)
                        {
                            propInfo.SetValue(anime, int.Parse(attribute.Value));
                        }
                        else
                        {
                            UnityEngine.Debug.Log($"アニメーションのプロパティをセットできませんでした {attribute.Name}");
                        }
                    }

                    scenario.Animations.Add(anime);
                }
            }
        }

        private IAnimation GenerateAnimation(string animationName)
        {
            switch (animationName)
            {
                case "alphaChanger": return new AlphaChanger();
            }

            throw new ArgumentException($"指定されたアニメーションの名前が不正です。[{animationName}]");
        }
    }
}