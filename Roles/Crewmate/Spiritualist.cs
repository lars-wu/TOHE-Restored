using System.Collections.Generic;
using System.Linq;
using MS.Internal.Xml.XPath;
using UnityEngine;
using static TOHE.Options;
using static UnityEngine.GraphicsBuffer;

namespace TOHE.Roles.Crewmate
{
    internal class Spiritualist
    {
        private static readonly int Id = 9123412;

        private static List<byte> playerIdList = new();

        public static OptionItem ShowGhostArrowEverySeconds;
        public static OptionItem ShowGhostArrowForSeconds;

        private static Dictionary<byte, long> ShowGhostArrowEverySecondsTime = new();
        private static Dictionary<byte, long> ShowGhostArrowForTime = new();

        public static Dictionary<byte, byte> SpiritualistTarget = new();

        public static void SetupCustomOption()
        {
            SetupRoleOptions(Id, TabGroup.CrewmateRoles, CustomRoles.Spiritualist);
            ShowGhostArrowEverySeconds = FloatOptionItem.Create(Id + 10, "SpiritualistShowGhostArrowEverySeconds", new(1f, 60f, 1f), 15f, TabGroup.CrewmateRoles, false).SetParent(CustomRoleSpawnChances[CustomRoles.Spiritualist])
                .SetValueFormat(OptionFormat.Seconds);
            ShowGhostArrowForSeconds = FloatOptionItem.Create(Id + 11, "SpiritualistShowGhostArrowForSeconds", new(1f, 60f, 1f), 2f, TabGroup.CrewmateRoles, false).SetParent(CustomRoleSpawnChances[CustomRoles.Spiritualist])
                .SetValueFormat(OptionFormat.Seconds);
        }
        public static void Init()
        {
            playerIdList = new();
            SpiritualistTarget = new();
        }
        public static void Add(byte playerId)
        {
            playerIdList.Add(playerId);
            SpiritualistTarget.Add(playerId, 0);
            ShowGhostArrowEverySecondsTime.Add(playerId, 0);
            ShowGhostArrowForTime.Add(playerId, 0);
        }
        public static bool IsEnable => playerIdList.Count > 0;

        private static bool ShowArrow(byte playerId)
        {
            long timestamp = Utils.GetTimeStamp();

            Logger.Info($"{timestamp}", "Spiritualist");
            Logger.Info($"{ShowGhostArrowEverySecondsTime[playerId]}", "Spiritualist");
            Logger.Info($"{ShowGhostArrowForTime[playerId]}", "Spiritualist");

            if (ShowGhostArrowEverySecondsTime[playerId] == 0 || ShowGhostArrowEverySecondsTime[playerId] >= timestamp)
            {
                Logger.Info($"1", "Spiritualist");

                ShowGhostArrowEverySecondsTime[playerId] = timestamp + (long)ShowGhostArrowEverySeconds.GetFloat();
                ShowGhostArrowForTime[playerId] = timestamp + (long)ShowGhostArrowForSeconds.GetFloat();
                return true;
            }
            else if (ShowGhostArrowForTime[playerId] > timestamp)
            {
                Logger.Info($"2", "Spiritualist");
                return true;
            }

            Logger.Info($"3", "Spiritualist");
            return false;
        }

        public static void OnReportDeadBody(GameData.PlayerInfo target)
        {
            foreach (var spiritualist in SpiritualistTarget)
            {
                ShowGhostArrowEverySecondsTime[spiritualist.Key] = 0;
                ShowGhostArrowForTime[spiritualist.Key] = 0;

                SpiritualistTarget[spiritualist.Key] = target.PlayerId;
                TargetArrow.Add(spiritualist.Key, target.PlayerId);
            }
        }

        public static string GetSpiritualistArrow(PlayerControl seer)
        {
            if (!seer.Is(CustomRoles.Spiritualist)) return "";
            if (GameStates.IsMeeting) return "";
            if (SpiritualistTarget.ContainsKey(seer.PlayerId) && SpiritualistTarget[seer.PlayerId] != 0 && ShowArrow(seer.PlayerId))
            {
                return Utils.ColorString(seer.GetRoleColor(), TargetArrow.GetArrows(seer, SpiritualistTarget[seer.PlayerId])); 
            }
            return "";
        }
    }
}
