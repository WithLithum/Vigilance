using Vigilance.Entities;
using Vigilance.User.UI;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Vigilance.Engine;
using Vigilance.Entities.Backup;
using Common = Vigilance.Engine.Common;

namespace Vigilance.Functional.Fibers
{
    public static class BackupManager
    {
        // static bool isMenuProcessing = false;
        // static bool isBackupRunning = false;
        static readonly MenuPool Pool = MenuShared.PublicPool;
        static UIMenu mainMenu;
        static UIMenuItem itemPolice;
        static UIMenuItem itemFiretruck;
        static UIMenuItem itemParamedic;

        private static List<BackupUnit> runningBackups = new List<BackupUnit>();

        internal static void Loop()
        {
            try
            {
                Game.LogTrivial("Thread Loop > Main Manager");
                // register menus before cycle
                Game.LogTrivial("Backup Manager > Registering Menus");
                mainMenu = new UIMenu("Backups", "Request Backup");
                itemPolice = new UIMenuItem("Local Patrol Unit", "Request a police patrol unit.");
                itemFiretruck = new UIMenuItem("Fire Department", "Request a fire truck. It will only respond if there's an actual fire.");
                itemParamedic = new UIMenuItem("Ambulance", "Request an ambulance. It will only respond if there's as least one dead ped around you.");
                
                mainMenu.AddItem(itemPolice);
                mainMenu.AddItem(itemFiretruck);
                mainMenu.AddItem(itemParamedic);
                mainMenu.RefreshIndex();
                Pool.Add(mainMenu);
                itemPolice.Activated += ItemPolice_Activated;
                itemFiretruck.Activated += ItemFiretruck_Activated;
                itemParamedic.Activated += ItemParamedic_Activated;
            }
            catch(Exception ex)
            {
                Game.LogTrivial("LANDTORY HAS ENCOUTERED ERROR: BACKUP MANAGER");
                Game.LogTrivial(ex.Message);
                Game.LogTrivial(ex.GetType().Name);
                Game.LogTrivial(ex.StackTrace);
            }
            while (Common.IsRunning)
            {
                if(Game.IsKeyDown(Keys.B) && !Pool.IsAnyMenuOpen())
                {
                    mainMenu.Visible = !mainMenu.Visible;
                }
                Pool.ProcessMenus();
                GameFiber.Yield();
            }
        }

        internal static void BackupTicker()
        {
            while (Common.IsRunning)
            {
                for (var i = 0; i < runningBackups.Count; i++)
                {
                    if (runningBackups[i].IsRunning)
                    {
                        runningBackups[i].Process();
                    }
                    else
                    {
                        runningBackups.RemoveAt(i);
                    }
                }
            }
        }
        
        private static void ItemParamedic_Activated(UIMenu sender, UIMenuItem selectedItem)
        {
            Game.DisplayNotification("Requesting <b>ambulance</b> from Dispatch");
            CallBackup(BackupType.Paramedics, Game.LocalPlayer.Character.Position);
            ReportCrime(44);
        }

        private static void ItemFiretruck_Activated(UIMenu sender, UIMenuItem selectedItem)
        {
            Game.DisplayNotification("Requesting <b>firetruck</b> from Dispatch");
            CallBackup(BackupType.FireDepartment, Game.LocalPlayer.Character.Position);
            ReportCrime(18);
        }

        private static void ItemPolice_Activated(UIMenu sender, UIMenuItem selectedItem)
        {
            Game.DisplayNotification("Requesting <b>backup</b> from Dispatch");
            CallBackup(BackupType.Police, Game.LocalPlayer.Character.Position);
            ReportCrime(28);
        }

        static void CallBackup(BackupType type, Vector3 position)
        {
            if (type == BackupType.Police)
            {
                if (Functions.IsPlayerInCountryside())
                {
                    var sheriff = new SheriffBackup();
                    sheriff.Start(World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position.Around(350f)), ResponseType.Emergency);
                    runningBackups.Add(sheriff);
                    
                }
                else
                {
                    var police = new PoliceBackup();
                    police.Start(World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position.Around(350f)),
                        ResponseType.Emergency);
                    runningBackups.Add(police);

                }
            }
            else
            {
                var _ = 0;
                NativeFunction.Natives.CREATE_INCIDENT<bool>((int)type, position.X, position.Y, position.Z, 2, 3.0f, ref _);
            }
            
           
        }

        static void ReportCrime(int id)
        {
            // use hash to hope triggers the audio
            NativeFunction.Natives.xE9B09589827545E7(Game.LocalPlayer, id, 2);
        }
    }
}
