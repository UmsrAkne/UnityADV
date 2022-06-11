namespace Animations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using Loaders;
    using SceneContents;

    public class AnimeElementConverter : IXMLElementConverter
    {
        public string TargetElementName => "anime";

        public List<string> Log { get; } = new List<string>();

        public void Convert(XElement xmlElement, Scenario scenario)
        {
            var tags = xmlElement.Elements(TargetElementName);

            if (tags.Count() != 0)
            {
                foreach (XElement animeTag in tags)
                {
                    IAnimation anime = GenerateAnimation(animeTag.Attribute("name").Value);

                    if (anime == null)
                    {
                        continue;
                    }

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

                            if (propInfo.PropertyType == typeof(string))
                            {
                                propInfo.SetValue(anime, attribute.Value);
                            }
                        }
                        else
                        {
                            Log.Add($"アニメーションのプロパティのセットに失敗しました。Index={scenario.Index}, name={attribute.Name}");
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
                case "alphaChanger": return new AlphaChanger(true);
                case "shake": return new Shake();
                case "slide": return new Slide();
                case "flash": return new Flash();
                case "bound": return new Bound();
            }

            Log.Add($"アニメーションの生成に失敗。 name={animationName}");

            return null;
        }
    }
}