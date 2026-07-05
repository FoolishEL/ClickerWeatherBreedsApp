using UnityEngine;

namespace Game.Core
{
    public interface IContentManagementSystem
    {
        T LoadContent<T>(string path) where T : Object;
        T[] LoadAllContent<T>(string path) where T : Object;
        
        ResourceRequest LoadContentAsync<T>(string path) where T : Object;
    }
}