using System.Collections;
using UnityEngine;

public class DeadPanelAnimation : MonoBehaviour
{
    public Animation m_Animation; // Componente Animation del panel
    public AnimationClip m_PanelAnimationClip; // Animaci�n del panel

    public void PlayPanelAnimation()
    {
        m_Animation.CrossFade(m_PanelAnimationClip.name);
    }

    public IEnumerator PlayPanelAnimationAndWait()
    {
        // Reproduce la animaci�n y espera a que termine
        m_Animation.CrossFade(m_PanelAnimationClip.name);
        yield return new WaitForSeconds(m_PanelAnimationClip.length);
    }
}