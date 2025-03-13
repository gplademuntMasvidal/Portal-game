using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseAnimation : MonoBehaviour
{

    public Animation m_Animation; // Componente Animation del panel
    public AnimationClip m_OpeningPauseAnimationClip; // Animaci�n del panel
    public AnimationClip m_ClosingPauseAnimationClip; // Animaci�n del panel


    public void PlayOpeningPausePanelAnimation()
    {
        m_Animation.CrossFade(m_OpeningPauseAnimationClip.name);
    }

    public IEnumerator PlayOpeningPausePanelAnimationAndWait()
    {
        // Reproduce la animaci�n y espera a que termine
        m_Animation.CrossFade(m_OpeningPauseAnimationClip.name);
        yield return new WaitForSeconds(m_OpeningPauseAnimationClip.length);
    }

    public void PlayClosingPausePanelAnimation()
    {
        m_Animation.CrossFade(m_OpeningPauseAnimationClip.name);
    }

    public IEnumerator PlayClosingPausePanelAnimationAndWait()
    {
        // Reproduce la animaci�n y espera a que termine
        m_Animation.CrossFade(m_ClosingPauseAnimationClip.name);
        yield return new WaitForSeconds(m_ClosingPauseAnimationClip.length);
    }
}
