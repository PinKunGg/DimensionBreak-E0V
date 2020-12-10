using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillObj : MonoBehaviour
{
    [SerializeField] private GameObject Player;

    private void Update()
    {
        this.transform.position = Player.transform.position;
    }
}
