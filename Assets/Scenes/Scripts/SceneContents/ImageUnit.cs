namespace Scenes.Scripts.SceneContents
{
    using UnityEngine;
    using UnityEngine.Rendering;

    public class ImageUnit
    {
        private GameObject gameObject;
        private SpriteRenderer spriteRenderer;
        private SortingGroup sortingGroup;

        public GameObject GameObject
        {
            get
            {
                gameObject = gameObject ?? new GameObject();
                return gameObject;
            }
        }

        public SpriteRenderer SpriteRenderer
        {
            get
            {
                spriteRenderer = spriteRenderer ?? GameObject.AddComponent<SpriteRenderer>();
                return spriteRenderer;
            }
        }

        public SortingGroup SortingGroup
        {
            get
            {
                sortingGroup = sortingGroup ?? GameObject.AddComponent<SortingGroup>();
                return sortingGroup;
            }
        }

        public void SetMaskSprite(Sprite sprite)
        {
            var spriteMask = GameObject.GetComponent<SpriteMask>();
            if (spriteMask == null)
            {
                spriteMask = GameObject.AddComponent<SpriteMask>();
            }

            spriteMask.sprite = sprite;
        }

        public void SetParent(GameObject parent)
        {
            GameObject.transform.SetParent(parent.transform, false);
        }
    }
}
