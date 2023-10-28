using UnityEngine;

public class UnitSkin: MonoBehaviour
{
    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate() 
    { 
        gameObject.SetActive(false); 
    }
}