using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseAnimation : MonoBehaviour
{

    public Animation m_Animation; // Componente Animation del panel
    public AnimationClip m_OpeningPauseAnimationClip; // Animación del panel
    public AnimationClip m_ClosingPauseAnimationClip; // Animación del panel


    public void PlayOpeningPausePanelAnimation()
    {
        m_Animation.CrossFade(m_OpeningPauseAnimationClip.name);
    }

    public IEnumerator PlayOpeningPausePanelAnimationAndWait()
    {
        // Reproduce la animación y espera a que termine
        m_Animation.CrossFade(m_OpeningPauseAnimationClip.name);
        yield return new WaitForSeconds(m_OpeningPauseAnimationClip.length);
    }

    public void PlayClosingPausePanelAnimation()
    {
        m_Animation.CrossFade(m_OpeningPauseAnimationClip.name);
    }

    public IEnumerator PlayClosingPausePanelAnimationAndWait()
    {
        // Reproduce la animación y espera a que termine
        m_Animation.CrossFade(m_ClosingPauseAnimationClip.name);
        yield return new WaitForSeconds(m_ClosingPauseAnimationClip.length);
    }
}
