using UnityEngine;

[System.Serializable]
public class SerializableObject
{
    protected string path;

    public virtual string Path { get { return path; } }

    public virtual void Save()
    {

    }

    public virtual T Load<T>()
    {
        return default(T);
    }
}
