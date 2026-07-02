using UnityEngine;

namespace Game.DogBreads
{
    [CreateAssetMenu(menuName = "Game/Configs/DogBreeds Config", fileName = "DogBreedsConfig")]
    public class DogBreedsConfig : ScriptableObject
    {
        [field: SerializeField]
        public string DogBreedRequestURL { get; set; }
        
        [field: SerializeField]
        public int DisplayBreedCount { get; set; }
    }
}