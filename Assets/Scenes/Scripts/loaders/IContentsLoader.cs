using System;
using System.Collections.Generic;
using Scenes.Scripts.SceneContents;

namespace Scenes.Scripts.Loaders
{
    public interface IContentsLoader
    {
         event EventHandler LoadCompleted;

         List<string> Log { get; }

         Resource Resource { get; set; }

         void Load(string sceneDirectoryPath);
    }
}