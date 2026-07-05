using System.Collections.Generic;

namespace Game.DogBreads
{

    #region Breeds

    [System.Serializable]
    public class DogBreedsResponse
    {
        public List<DogBreedData> data;
        public DogMeta meta;
    }

    [System.Serializable]
    public class DogBreedData
    {
        public string id;
        public string type;
        public DogBreedAttributes attributes;
    }

    [System.Serializable]
    public class DogBreedAttributes
    {
        public string name;
        public string description;
        public DogLife life;
        public bool hypoallergenic;
    }

    [System.Serializable]
    public class DogLife
    {
        public int max;
        public int min;
    }

    [System.Serializable]
    public class DogMeta
    {
        public DogPagination pagination;
    }

    [System.Serializable]
    public class DogPagination
    {
        public int current;
        public int last;
    }

    [System.Serializable]
    public class DogBreedDetailResponse
    {
        public DogBreedData data;
    }

    #endregion
}