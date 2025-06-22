using UnityEngine;

public class Initiate : MonoBehaviour
{
    public static void instantiatePrefabIfNonExisting(GameObject prefab)
    {
        if (GameObject.Find(prefab.name) == null)
        {
            Instantiate(prefab).name = prefab.name;
        }
    }

    public static void instantiatePrefab(GameObject prefab)
    {
        Instantiate(prefab).name = prefab.name;
    }
}
