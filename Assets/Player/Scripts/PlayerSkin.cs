using UnityEngine;

public class PlayerSkin: MonoBehaviour
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