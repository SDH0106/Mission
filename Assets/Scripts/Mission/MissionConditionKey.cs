using UnityEngine.Rendering;

public static class MissionConditionKeys
{
    public const string Null = "null";

    public const string BodyId = "body.id"; // 사용하는 캐릭터 Id
    public const string Chapter = "chapter"; // 지역 id
    public const string CostumeAcquired = "costume.acquired"; // 의상 수집
    public const string CostumeRarity = "costume.rarity"; // 의상 등급
    public const string CostumeId = "costume.id"; // 의상 id
    public const string GameClear = "game.clear"; // 게임 모드 클리어
    public const string GamePlay = "game.play"; // 게임 모드 플레이
    public const string GameStage = "game.stage"; // stage의 타입(MainGameStage)
    public const string ItemAcquired = "item.acquired"; // 아이템 획득
    public const string MissionCompleteAll = "mission.complete.all"; // 모든 미션 클리어
    public const string MissionCompleteCount = "mission.complete.count"; // 미션 클리어 일정 횟수 달성
    public const string MissionId = "mission.id"; // 미션 id
    public const string MissionIdSubCategory = "mission.id.subcategory"; // 미션 id 소분류(id 5~6번째 자리, xxxx00xxxx)
    public const string MissionLifeCycle = "mission.life_cycle"; // 미션 초기화 주기
    public const string MissionType = "mission.type"; // 미션 타입 종류
    public const string MonsterDamageCauser = "monser.damage_causer"; // 몬스터 처치자
    public const string MonsterId = "monster.id"; // 몬스터 id
    public const string MonsterKill = "monster.kill"; // 몬스터 킬
    public const string MonsterTag = "monster.tag"; // 몬스터 태그
    public const string MonsterType = "monster.type"; // 몬스터 타입
    public const string PetId = "pet.id"; // 펫 id
    public const string PetType = "pet.type"; // 펫 타입
    public const string PlayTime = "play.time"; // 게임 플레이 시간(게임에 접속한 시간)
    public const string QuestComplete = "quest.complete"; // 퀘스트 완료
    public const string QuestUpgrade = "quest.upgrade"; // 퀘스트 업그레이드
    public const string QuestId = "quest.id"; // 퀘스트 Id
    public const string RankingReached = "ranking.reached"; // 랭킹 순위 도달
    public const string SideEventId = "side_event.id"; // 사이드 이벤트 id
    public const string SideEventSuccess = "side_event.success"; //  사이드 이벤트 성공
    public const string SideEventTag = "side_event.tag"; // 사이드 태그
    public const string StatId = "stat.id"; // 스탯 id
    public const string StatUpgrade = "stat.upgrade"; // 스탯 업그레이드
    public const string TimeSlip = "time_slip"; //  타임 슬립 수행
    public const string TutorialClear = "tutorial.clear"; // 튜토리얼 클리어
    public const string Warp = "warp"; // 워프 이벤트 완료
    public const string WeaponUpgrade = "weapon.upgrade"; // 무기 업그레이드
}