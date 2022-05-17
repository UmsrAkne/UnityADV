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
                        // xml の属性名は先頭小文字。プロパティは先頭大文字となるため
                        // プロパティの存在を確認する前に先頭を大文字に変換した文字列を準備する。
                        var originalName = attribute.Name.ToString();
                        var attributeNameUpperCamel = $"{Char.ToUpper(originalName[0])}{originalName.Substring(1)}";

                        // リフレクションを使用して、セットしたいプロパティがクラスに存在するかを確認してからセットする。
                        var propInfo = type.GetProperty(attributeNameUpperCamel);

                        if (propInfo != null)
                        {
                            if (propInfo.PropertyType == typeof(int))
                            {
                                propInfo.SetValue(anime, int.Parse(attribute.Value));
                            }

                            if (propInfo.PropertyType == typeof(double))
                            {
                                propInfo.SetValue(anime, double.Parse(attribute.Value));
                            }
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
                case "shake": return new Shake();
                case "slide": return new Slide();
                case "flash": return new Flash();
            }

            throw new ArgumentException($"指定されたアニメーションの名前が不正です。[{animationName}]");
        }
    }
}