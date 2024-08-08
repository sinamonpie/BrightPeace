using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}

public class SoundManager : MonoBehaviour
{
    static public SoundManager instance;

    #region singleton
    void Awake()    
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    public AudioSource[] audioSoundEffects;
    public AudioSource audioSourceBGM;

    public Sound[] soundEffects;
    public Sound[] bgmSounds;

    public void PlaySoundEffect(string _name)
    {
        for (int i = 0; i < soundEffects.Length; i++)
        {
            if (_name == soundEffects[i].name)
            {
                for (int j = 0; j < audioSoundEffects.Length; j++)
                {
                    if (!audioSoundEffects[j].isPlaying)
                    {
                        audioSoundEffects[j].clip = soundEffects[i].clip;
                        audioSoundEffects[j].Play();
                        return;
                    }
                }
                Debug.Log("모든 AudioSource 재생 중");
                return;
            }
        }
        Debug.Log(_name + "사운드 효과를 찾을 수 없음");
    }

    public void PlayBGM(string _name)
    {
        for (int i = 0; i < bgmSounds.Length; i++)
        {
            if (_name == bgmSounds[i].name)
            {
                audioSourceBGM.clip = bgmSounds[i].clip;
                audioSourceBGM.Play();
                return;
            }
        }
        Debug.Log(_name + "BGM을 찾을 수 없음");
    }

    public void StopBGM()
    {
        audioSourceBGM.Stop();
    }

    public void StopEverySoundEffects()
    {
        for (int i = 0; i < audioSoundEffects.Length; i++)
        {
            audioSoundEffects[i].Stop();
        }
    }

    public void StopAllSounds()
    {
        StopBGM();
        StopEverySoundEffects();
    }
}



/*

Ambience는 무한 반복되는 특정 방이나 사물 주변 배경음입니다

SF는 단일 효과음입니다

BGM은 배경음악입니다 한 번에 하나씩만 재생됩니다


각 층의 방들의 배경음
1FRoomAmbienceSF.wav
2FRoomAmbienceSF.wav

2층 수술실의 배경음
2FSurgeryRoomAmbienceSF.wav

지하실의 배경음
BasementRoomsAmbienceSF.wav

유일하게 1, 2층을 차지하는 방, 이하 강당의 배경음
BigRoomAmbienceSF.wav

1, 2층의 대형 문 열고 닫는 소리
BigRoomDoubleDoorCloseSF.wav
BigRoomDoubleDoorOpenSF.wav

UI 버튼 클릭 소리
ButtonClickSF.wav

캐비넷 열고 닫는 소리
CabinetOpenCloseSF.wav

강당 테이프처럼 생긴 프롭에 넣을 배경음
DayRoomAmbienceSF.wav

두 개 이상의 탈출구가 열리면 3 분간 나오는 배경음
EmergencyEscapeBGM.wav

탈출구가 열리면 탈출구 주변에서 나오는 소리, 10M 이내에서 들림
EscapeRouteOpenedAmbienceSF.wav

탈출 성공 시 나오는 배경음
EscapeSuccessBGM.wav

시간 여유가 있다면 간수 발소리 이걸로 변경해주세용 가능하다면 랜덤으로
SecurityFootstepSF01.wav
SecurityFootstepSF02.wav
SecurityFootstepSF03.wav
SecurityFootstepSF04.wav

나머지 플레이어들 발소리
FootstepSF01.wav
FootstepSF02.wav
FootstepSF03.wav
FootstepSF04.wav

게임씬 전체에 깔린 배경음, 앰비언스들과 중첩될 수 있습니다.
GameMainBGM.wav

아이템 상자 열고 닫는 소리
ItemChestCloseSF.wav
ItemChestOpenSF.wav

죽음 애니메이션 때 나오는 소리들
KillSoundSF01.wav
KillSoundSF02.wav
KillSoundSF03.wav

칼로 때릴 때 나오는 소리들
KnifeHitSF01.wav
KnifeHitSF02.wav
KnifeMissSF01.wav
KnifeMissSF02.wav

게임 시작부터 로비 내내 배경음
LobbyMainBGM.wav

미치광이 플레이어에게 랜덤하게 들리는 소리
ManiacRandomWhisperSF.wav

철제 문 열고 닫는 소리
MetalDoorCloseSF.wav
MetalDoorOpenSF.wav

투시경 사용 시 나오는 소리
NightVisionSF.wav

하나의 퀘스트를 완료했을 때 나오는 소리
OneQuestDoneSF.wav

퀘스트 아이템 상자 열 때 나오는 소리
QuestItemBoxOpenSF.wav

간수 플레이어가 플레이어를 때리거나 헛칠 때 나오는 소리들
SecurityHit02.wav
SecurityHitSF01.wav
SecurityMissSF01.wav
SecurityMissSF02.wav

간수 플레이어 주변 타 플레이어들에게 들리는 소리
SecuritySiren15MSF.wav
SecuritySiren7MSF.wav

누군가의 피격, 사망 소리, 간수를 제외한 모든 플레이어에게 들림
SomeoneHitSF.wav
SomeoneKilledSF.wav

밸브 열고 닫는 소리
ValveClosedSF.wav
ValveClosingSF.wav
ValveKeepLosingAirSF.wav

나무 문 열고 닫는 소리
WoodDoorCloseSF.wav
WoodDoorOpenSF.wav
*/