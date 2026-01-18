using Cinemachine;
using UnityEngine;

public class WolfDeath : MonoBehaviour
{

    public CinemachineVirtualCamera camera;
    public GameObject house;
    public void Dead()
    {
        camera.Follow =house.transform;
    }
}
