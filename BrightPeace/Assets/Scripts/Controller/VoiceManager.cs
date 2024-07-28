using Photon.Voice.Unity;
using UnityEngine;
public class VoiceManager : MonoBehaviour
{
    Recorder recorder;

    void Start()
    {
        recorder = GetComponent<Recorder>();
        recorder.TransmitEnabled = false;           // 처음엔 오디오 전송 비활성화
    }

    public void SetTransmitEnabled(bool enabled)
    {
        recorder.TransmitEnabled = enabled;
    }
}
