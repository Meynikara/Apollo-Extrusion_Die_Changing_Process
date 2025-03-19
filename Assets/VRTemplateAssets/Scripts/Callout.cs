using System.Collections;
using UnityEngine;

namespace Unity.VRTemplate
{
    public class Callout : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("The tooltip Transform associated with this Callout.")]
        Transform m_LazyTooltip;

        [SerializeField]
        [Tooltip("The line curve GameObject associated with this Callout.")]
        GameObject m_Curve;

        [SerializeField]
        [Tooltip("The required time to dwell on this callout before the tooltip and curve are enabled.")]
        float m_DwellTime = 1f;

        [SerializeField]
        [Tooltip("Whether the associated tooltip will be unparented on Start.")]
        bool m_Unparent = true;

        [SerializeField]
        [Tooltip("Whether the associated tooltip and curve will be disabled on Start.")]
        bool m_TurnOffAtStart = true;

        Coroutine m_StartCo;
        Coroutine m_EndCo;

        void Start()
        {
            if (m_Unparent && m_LazyTooltip != null)
                m_LazyTooltip.SetParent(null);

            if (m_TurnOffAtStart)
            {
                if (m_LazyTooltip != null)
                    m_LazyTooltip.gameObject.SetActive(false);
                if (m_Curve != null)
                    m_Curve.SetActive(false);
            }
        }

        public void ShowCallout()
        {
            if (m_StartCo != null)
                StopCoroutine(m_StartCo);
            if (m_EndCo != null)
                StopCoroutine(m_EndCo);
            m_StartCo = StartCoroutine(StartDelay());
        }

        public void HideCallout()
        {
            if (m_EndCo != null)
                StopCoroutine(m_EndCo);
            m_EndCo = StartCoroutine(EndDelay());
        }

        IEnumerator StartDelay()
        {
            yield return new WaitForSeconds(m_DwellTime);
            TurnOnStuff();
        }

        IEnumerator EndDelay()
        {
            yield return new WaitForSeconds(0.1f);
            TurnOffStuff();
        }

        void TurnOnStuff()
        {
            if (m_LazyTooltip != null)
                m_LazyTooltip.gameObject.SetActive(true);
            if (m_Curve != null)
                m_Curve.SetActive(true);
        }

        void TurnOffStuff()
        {
            if (m_LazyTooltip != null)
                m_LazyTooltip.gameObject.SetActive(false);
            if (m_Curve != null)
                m_Curve.SetActive(false);
        }
    }
}
