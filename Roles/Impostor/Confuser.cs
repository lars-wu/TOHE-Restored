using System.Collections.Generic;
using System.Linq;
using AmongUs.GameOptions;
using TOHE.Roles.Neutral;
using UnityEngine;
using static TOHE.Options;
using static TOHE.Translator;
using static TOHE.Utils;

namespace TOHE.Roles.Impostor
{
    public static class Confuser
    {
        private static readonly int Id = 987456;
        public static List<byte> playerIdList = new();

        public static bool ConfuseActive = false;

        private static OptionItem KillCooldown;
        private static OptionItem ShapeshiftCooldown;
        private static OptionItem ShapeshiftDuration;
        
        public static void SetupCustomOption()
        {
            SetupRoleOptions(Id, TabGroup.ImpostorRoles, CustomRoles.Confuser);
            KillCooldown = FloatOptionItem.Create(Id + 10, "KillCooldown", new(0f, 180f, 2.5f), 22.5f, TabGroup.ImpostorRoles, false).SetParent(CustomRoleSpawnChances[CustomRoles.Confuser])
                .SetValueFormat(OptionFormat.Seconds);
            ShapeshiftCooldown = FloatOptionItem.Create(Id + 11, "ShapeshiftCooldown", new(0f, 180f, 1f), 25f, TabGroup.ImpostorRoles, false).SetParent(CustomRoleSpawnChances[CustomRoles.Confuser])
                .SetValueFormat(OptionFormat.Seconds);
            ShapeshiftDuration = FloatOptionItem.Create(Id + 12, "ShapeshiftDuration", new(0f, 180f, 1f), 20f, TabGroup.ImpostorRoles, false).SetParent(CustomRoleSpawnChances[CustomRoles.Confuser])
                .SetValueFormat(OptionFormat.Seconds);
        }

        public static void Init()
        {
            playerIdList = new();
        }

        public static void Add(byte playerId)
        {
            playerIdList.Add(playerId);
        }

        public static void ApplyGameOptions()
        {
            AURoleOptions.ShapeshifterCooldown = ShapeshiftCooldown.GetFloat();
            AURoleOptions.ShapeshifterDuration = ShapeshiftDuration.GetFloat();
        }
        public static bool IsEnable => playerIdList.Count > 0;

        public static void SetKillCooldown(byte id) => Main.AllPlayerKillCooldown[id] = KillCooldown.GetFloat();

        public static void OnShapeshift(PlayerControl pc, bool shapeshifting)
        {
            if (!pc.IsAlive() || Pelican.IsEaten(pc.PlayerId)) return;
            ConfuseActive = shapeshifting;
        }

        public static void OnReportDeadBody()
        {
            ConfuseActive = false;
        }

        public static void isDead(PlayerControl target)
        {
            if (!target.Data.IsDead || GameStates.IsMeeting) return;
            if (target.Is(CustomRoles.Confuser) && target.Data.IsDead) ConfuseActive = false;
        }
    }
}
