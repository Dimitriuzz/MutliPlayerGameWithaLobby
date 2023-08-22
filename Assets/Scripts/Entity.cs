
using UnityEngine;
/// <summary>
/// Base class for all interactible objects on scene
/// </summary>
public abstract class Entity : MonoBehaviour
{
    [SerializeField] private string m_Nickname;
    public string Nickname => m_Nickname;
  
}
