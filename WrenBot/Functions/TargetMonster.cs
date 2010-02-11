using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WrenBot;
using WrenBot.Types;
using WrenBot.PathFinding;

namespace WrenBot.Functions
{
    public static partial class Functions
    {
        public static void TargetItem(this BotClient Client)
        {
            try
            {
                Item[] Items = Client.Items.ToArray();
                if (Items.Length > 0)
                    for (int i = 0; i < Items.Length; i++)
                        if (Items[i] != null)
                            if (Client.LootableItems.Contains(Items[i].Icon))
                            {
                                if (!Items[i].IsBanned)
                                {
                                    Client.ItemTargetSerial = Items[i].Serial;
                                    return;
                                }
                            }
            }
            catch { }
            Client.ItemTargetSerial = 0;
        }

        public static uint TargetItemSerial(this BotClient Client)
        {
            try
            {
                Item[] Items = Client.Items.ToArray();
                if (Items.Length > 0)
                    for (int i = 0; i < Items.Length; i++)
                        if (Items[i] != null)
                            if (Client.LootableItems.Contains(Items[i].Icon))
                                if (!Items[i].IsBanned)
                                    return Items[i].Serial;
            }
            catch { }
            return 0;
        }

        public static bool CanTargetPND(this BotClient Client, Monster Mon, bool Pathing)
        {
            try
            {
                if ((Mon.Targeted == true) && DateTime.Now - Mon.PossibleDeathTime > new TimeSpan(0, 0, 4)) return true;
                if (
                                    (
                                     (
                                      (
                                       (
                                        (
                                        !Mon.IsBanned
                                        ) && DateTime.Now - Mon.PossibleDeathTime > new TimeSpan(0, 0, 4)
                                      ) && Client.Aisling.Map.FindPath(Client.Aisling.Location, Mon.Location, true, Client.Proxy, Client.Form).Count < 10
                                     ) && !Mon.IsPet
                                    ) && !Client.BlackListMons.Contains(Mon.Icon)
                                   ))                
                {
                    uint Serial = Mon.Serial;
                    List<PathFinderNode> Path = Client.Aisling.Map.FindPath(Client.Aisling.Location, Client.Aisling.Map.Entities[Serial].Location, true, Client.Proxy, Client.Form);
                    if (!Client.Aisling.Map.Entities.ContainsKey(Serial)
                        || Client.Aisling.Map.Entities[Serial].IsBanned) return false;
                    foreach (AislingEntity Ent in Client.Aisling.Players)
                    {                        
                        if ((Client.IsClient(Ent.Name)) || Client.Aisling.PlayersAllowedNear.Contains(Ent.Name.ToLower()))
                            continue;
                        if (Ent.Location.DistanceFrom(Client.Aisling.Map.Entities[Serial].Location) <= 4)
                        {
                            Client.Aisling.Map.Entities[Serial].IsBanned = true;
                            return false;
                        }
                    }
                    return true;
                }
                return false;
            }
            catch { return false; }
        }

        public static bool CanTarget(this BotClient Client, Monster Mon)
        {

            if (Client.Aisling.Map.FindPath(Client.Aisling.Location, Mon.Location, true, Client.Proxy, Client.Form).Count < 10 && !Mon.IsPet && !!Mon.IsBanned
                && !!Client.BlackListMons.Contains(Mon.Icon) && Client.HasPathTo(Mon.Location))
            {
                return true;
            }

            try
            {
                if (
                                    (
                                     (
                                      (
                                       (
                                        (
                                        !Mon.IsBanned
                                        ) && DateTime.Now - Mon.PossibleDeathTime > new TimeSpan(0, 0, 4)
                                       ) && Client.HasPathTo(Mon.Location)
                                      ) && Client.Aisling.Map.FindPath(Client.Aisling.Location, Mon.Location, true, Client.Proxy, Client.Form).Count < 10
                                     ) && !Mon.IsPet
                                    ) && !Client.BlackListMons.Contains(Mon.Icon)
                                   )
                {
                    uint Serial = Mon.Serial;
                    List<PathFinderNode> Path = Client.Aisling.Map.FindPath(Client.Aisling.Location, Client.Aisling.Map.Entities[Serial].Location, true, Client.Proxy, Client.Form);
                    if (
                        (
                         (
                          (Path == null
                          ) || Path.Count >= 10
                         ) || !Client.Aisling.Map.Entities.ContainsKey(Serial)
                        ) || Client.Aisling.Map.Entities[Serial].IsBanned
                       )
                        return false;
                    foreach (AislingEntity Ent in Client.Aisling.Players)
                    {
                        if ((Client.IsClient(Ent.Name)) || Client.Aisling.PlayersAllowedNear.Contains(Ent.Name.ToLower()))
                            continue;
                        if (Ent.Location.DistanceFrom(Client.Aisling.Map.Entities[Serial].Location) <= 4)
                        {
                            Client.Aisling.Map.Entities[Serial].IsBanned = true;
                            return false;
                        }
                    }
                    return true;
                }
                return false;
            }
            catch { return false; }
        }

        public static List<uint> TargetMonsters(this BotClient Client)
        {
            List<uint> Serials = new List<uint>();
            try
            {
                Monster[] Mons = (from v in Client.Monsters
                                  where v.EntityType == MapEntity.Type.Monster
                                  orderby v.HPPercent ascending
                                  select v).ToArray();
                if (Mons.Length == 0) return Serials;
                for (int i = 0; i < Mons.Length; i++)
                    if ((Mons[i] != null) && Client.CanTarget(Mons[i]))
                        Serials.Add(Mons[i].Serial);
            }
            catch { }
            return Serials;
        }

        public static void TargetMonster(this BotClient Client)
        {
            try
            {
                Monster[] Mons = (from v in Client.Monsters where v.EntityType == MapEntity.Type.Monster orderby v.HPPercent ascending
                                  select v).ToArray();
                if (Mons.Length > 0)
                {
                    for (int i = 0; i < Mons.Length; i++)
                        try
                        {
                            if (Client.CanTarget(Mons[i]))
                            {
                                Client.AttackTargetSerial = Mons[i].Serial;
                                return;
                            }
                        }
                        catch { continue; }
                }
            }
            catch { }
            Client.AttackTargetSerial = 0;
        }
    }
}
