using UnityEngine;

public class AddCollidersAndTag : MonoBehaviour
{
    public string tagName = "PaintableWall"; // Tag to assign

    void Start()
    {
        // Loop through all child objects
        foreach (Transform child in transform)
        {
            GameObject obj = child.gameObject;

            // Add BoxCollider if it doesn't exist
            if (obj.GetComponent<BoxCollider>() == null)
            {
                obj.AddComponent<BoxCollider>();
                Debug.Log("Added BoxCollider to: " + obj.name);
            }

            // Assign the tag (make sure the tag exists in Unity first)
            obj.tag = tagName;
            Debug.Log("Assigned tag '" + tagName + "' to: " + obj.name);
        }
    }
}
