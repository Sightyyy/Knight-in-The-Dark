using UnityEngine;

public class stopper : StateMachineBehaviour
{
    public string deathParameter = "state";
    public int deathValue = 2;

    // Dipanggil saat animasi death dimulai
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Pastikan kita memang berada dalam animasi mati
        if (animator.GetInteger(deathParameter) == deathValue)
        {
            // Reset semua trigger dan bool
            animator.ResetTrigger("Jump");
            animator.SetBool("IsDead", true); // optional kalau pakai bool

            // Matikan semua integer parameter lain jika kamu punya sistem animasi kompleks
            animator.SetInteger("state", deathValue); // Paksa tetap di animasi mati
        }
    }

}
