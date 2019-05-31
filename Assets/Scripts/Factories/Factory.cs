using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour
{
    [SerializeField]
    GameObject prototype;
    [SerializeField]
    int capacity = 5;

    protected Stack<FactoryObject> passiveObjects;
    protected List<FactoryObject> activeObjects;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        passiveObjects = new Stack<FactoryObject>(capacity);
        activeObjects = new List<FactoryObject>(capacity);

        FactoryObject temp;
        GameObject tempGameObject;
        for(int i = 0; i < capacity; i++)
        {
            tempGameObject = Instantiate(prototype, transform.position, Quaternion.identity) as GameObject;
            temp = tempGameObject.GetComponent<FactoryObject>();
            tempGameObject.SetActive(false);
            temp.transform.SetParent(this.transform);
            passiveObjects.Push(temp);
        }
    }

    public virtual FactoryObject CreateInstance(float height)
    {
        if (passiveObjects.Count == 0)
            return null;

        FactoryObject temp = passiveObjects.Pop();
        temp.gameObject.SetActive(true);
        temp.rigidbody.position = new Vector2(transform.position.x, height);
        activeObjects.Add(temp);
        return temp;
    }

    public virtual void DestroyInstance(FactoryObject toRemove)
    {
        if(activeObjects.Contains(toRemove))
        {
            activeObjects.Remove(toRemove);
            passiveObjects.Push(toRemove);
            toRemove.gameObject.SetActive(false);
        }
    }
}
