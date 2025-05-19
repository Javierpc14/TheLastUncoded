using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    public EnemyAttack enemyAttack;

    void ActivarGolpe(){
        enemyAttack.ActivarGolpe();
    }

    void DesactivarGolpe(){
        enemyAttack.DesactivarGolpe();
    }
}
