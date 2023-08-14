using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TOHE.Options;
using static UnityEngine.GraphicsBuffer;
using static TOHE.Translator;
using Hazel;
using MS.Internal.Xml.XPath;

namespace TOHE.Roles.Crewmate
{
    public class Enigma
    {
        private static readonly int Id = 8100;
        private static List<byte> playerIdList = new();

        private static string HatClue1 = "The Killer wears a Hat!";
        private static string HatClue2 = "The Killer does not wear a Hat!";
        private static string HatClue3 = "The Killer wears {0} as a Hat!";
        private static string SkinClue1 = "The Killer wears a Skin!";
        private static string SkinClue2 = "The Killer does not wear a Skin!";
        private static string SkinClue3 = "The Killer wears {0} as a Skin!";
        private static string VisorClue1 = "The Killer wears a Visor!";
        private static string VisorClue2 = "The Killer does not wear a Visor!";
        private static string VisorClue3 = "The Killer wears {0} as a Visor!";
        private static string PetClue1 = "The Killer does have a Pet!";
        private static string PetClue2 = "The Killer does not have as a Pet!";
        private static string PetClue3 = "The Killer has {0} as a Pet!";
        private static string NameClue1 = "The Name of the Killer contains a {0}!";
        private static string NameClue2 = "The Name of the Killer contains a {0} and a {1}!";
        private static string NameClue3 = "The Name of the Killer contains a {0} and a {1} and a {2}!";
        private static string ColorClue1 = "The Killer has a light color!";
        private static string ColorClue2 = "The Killer has a dark color!";
        private static string LocationClue = "The Last Room the Killer was in is {0}!";
        private static string KillerStatusClue1 = "The Killer is still Alive!";
        private static string KillerStatusClue2 = "The Killer is already Dead!";
        private static string KillerStatusClue3 = "The Killer is currently inside a Vent!";
        private static string KillerRoleClue1 = "The Killer is an Impostor!";
        private static string KillerRoleClue2 = "The Killer is Neutral!";
        private static string KillerRoleClue3 = "The Killer is a Crewmate!";
        private static string KillerRoleClue4 = "The Killer's Role is {0}!";
        private static string KillerRoleClue5 = "The Killer's killing method resulted in {0}!"; // deathreason
        private static string KillerRoleClue6 = "The Killer can change his appearance!";
        //private static string KillerKillAmountClue = "The Killer has already killed {0} people!";
        //private static string RandomClue1 = "The Killer killed with a Knife!";
        //private static string RandomClue2 = "The Killer killed with a Pistol!";
        //private static string RandomClue3 = "The Killer killed with a Laserbeam!";
        //private static string RandomClue4 = "The Killer killed with a Neck Twist!";

        private static List<string> EnigmaCluesStage1 = new List<string>
        {
            HatClue1,
            HatClue2,
            SkinClue1,
            SkinClue2,
            VisorClue1,
            VisorClue2,
            PetClue1,
            PetClue2,
            NameClue1,
            KillerStatusClue1,
            KillerStatusClue2,
            KillerStatusClue3,
            KillerRoleClue1,
            KillerRoleClue2,
            KillerRoleClue3,
            KillerRoleClue6,
        };

        private static List<string> EnigmaCluesStage2 = new List<string>
        {
            ColorClue1,
            ColorClue2,
            LocationClue,
            KillerRoleClue4,
            KillerRoleClue5,
        };

        private static List<string> EnigmaCluesStage3 = new List<string>
        {
            HatClue3,
            SkinClue3,
            VisorClue3,
            PetClue3,
            NameClue2,
            NameClue3,
        };

        public static void SetupCustomOption()
        {
            SetupRoleOptions(Id, TabGroup.CrewmateRoles, CustomRoles.Enigma);
        }
        public static void Init()
        {
            playerIdList = new();
        }
        public static void Add(byte playerId)
        {
            playerIdList.Add(playerId);
        }
        public static bool IsEnable => playerIdList.Count > 0;

        public static void OnReportDeadBody(PlayerControl pc, GameData.PlayerInfo target)
        {
            if (target == null || pc.PlayerId == target.PlayerId) return;

            string title = "Enigma Clue!";
            string msg = "";

            var rd = new System.Random();
            int tasksDone = 0;
            if (tasksDone > 6)
            {
                
            }
            else if (tasksDone > 2)
            {

            }
            else
            {
                msg = EnigmaCluesStage1[rd.Next(0, EnigmaCluesStage1.Count)];
            }

            foreach (var enigma in playerIdList)
            {
                var enigmaPlayer = Utils.GetPlayerById(enigma);

                var writer = CustomRpcSender.Create("EngimaClueMessage", SendOption.None);
                writer.StartMessage(enigmaPlayer.GetClientId());
                writer.StartRpc(enigmaPlayer.NetId, (byte)RpcCalls.SetName)
                    .Write(title)
                    .EndRpc();
                writer.StartRpc(enigmaPlayer.NetId, (byte)RpcCalls.SendChat)
                    .Write(msg)
                    .EndRpc();
                writer.StartRpc(enigmaPlayer.NetId, (byte)RpcCalls.SetName)
                    .Write(enigmaPlayer.Data.PlayerName)
                    .EndRpc();
                writer.EndMessage();
                writer.SendMessage();
            }
        }
    }
}
