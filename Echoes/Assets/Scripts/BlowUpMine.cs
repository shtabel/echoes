using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlowUpMine : MonoBehaviour
{
    [SerializeField]
    EnemyController mine;

    public void BlowMine()
    {
        mine.BlowUpEnemy(false);
    }
}
