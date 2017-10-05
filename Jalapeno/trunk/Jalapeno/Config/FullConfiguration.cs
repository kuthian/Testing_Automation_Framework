using System;
using System.Collections;
using System.Diagnostics;
using Jalapeno.Utils;

namespace Jalapeno.Config
{
    public class FullConfiguration
    {                
        private byte[] FullConfigArray;
        private static byte[] UpdatedFullConfigArray;
        public static NewConfigurationItems NewConfigItems;
        public Mode DefaultMode;
        public Mode[] AdvancedModes;
        public Mode AdvancedModes0;
        public Mode AdvancedModes1;
        public Mode AdvancedModes2;
        public Mode AdvancedModes3;
        public Mode AdvancedModes4;

        public Geofence[] ConfigGeofence;
        public Geofence ConfigGeofence0;
        public Geofence ConfigGeofence1;
        public Geofence ConfigGeofence2;
        public Geofence ConfigGeofence3;
        public Geofence ConfigGeofence4;
        public Geofence ConfigGeofence5;
        public Geofence ConfigGeofence6;
        public Geofence ConfigGeofence7;
        public Geofence ConfigGeofence8;
        public Geofence ConfigGeofence9;

        public Timefence[] ConfigTimefence;
        public Timefence ConfigTimefence0;
        public Timefence ConfigTimefence1;
        public Timefence ConfigTimefence2;
        public IOSettings ConfigIOSettings;
        public Notifications ConfigNotifications;
        public Iridium ConfigIridium;
        public Cell ConfigCell;

        public static UInt16 UpdatedConfigCRC;
        public static bool ConfigIsCurrent;
        int pos;



        public FullConfiguration()
        {
            FullConfigArray = new byte[828];
            UpdatedFullConfigArray = new byte[828];
            NewConfigItems = new NewConfigurationItems();
            DefaultMode = new Mode();
            AdvancedModes = new Mode[5];
            AdvancedModes0 = new Mode();
            AdvancedModes1 = new Mode();
            AdvancedModes2 = new Mode();
            AdvancedModes3 = new Mode();
            AdvancedModes4 = new Mode();

            ConfigGeofence= new Geofence[10];
            ConfigGeofence0 = new Geofence();
            ConfigGeofence1 = new Geofence();
            ConfigGeofence2 = new Geofence();
            ConfigGeofence3 = new Geofence();
            ConfigGeofence4 = new Geofence();
            ConfigGeofence5 = new Geofence();
            ConfigGeofence6 = new Geofence();
            ConfigGeofence7 = new Geofence();
            ConfigGeofence8 = new Geofence();
            ConfigGeofence9 = new Geofence();

            ConfigTimefence = new Timefence[3];
            ConfigTimefence0 = new Timefence();
            ConfigTimefence1 = new Timefence();
            ConfigTimefence2 = new Timefence();

            ConfigIOSettings = new IOSettings();
            ConfigNotifications = new Notifications();
            ConfigIridium = new Iridium();
            ConfigCell = new Cell();

            ConfigIsCurrent = false;

            pos = 0;

        }

        //
        public void UpdateFullConfiguration(byte[] ConfigurationByteArray)
        { 
            try
            {
                if (ConfigurationByteArray.Length == 828)
                {
                    if (!StructuralComparisons.StructuralEqualityComparer.Equals(FullConfigArray, ConfigurationByteArray))//!FullConfigArray.Equals(ConfigurationByteArray)
                    {
                        Debug.WriteLine("...Reading Configuration...\n");

                        //Copying the Configuration Byte Array
                        FullConfigArray = ConfigurationByteArray;
                        FullConfigArray.CopyTo(UpdatedFullConfigArray, 0);

                        //Default Mode and Advanced Modes                
                        DefaultMode.readMode(ConfigurationByteArray, pos);
                        pos += 46;

                        AdvancedModes0.readMode(ConfigurationByteArray, pos);
                        AdvancedModes[0] = AdvancedModes0;
                        pos += 46;

                        AdvancedModes1.readMode(ConfigurationByteArray, pos);
                        AdvancedModes[1] = AdvancedModes1;
                        pos += 46;

                        AdvancedModes2.readMode(ConfigurationByteArray, pos);
                        AdvancedModes[2] = AdvancedModes2;
                        pos += 46;

                        AdvancedModes3.readMode(ConfigurationByteArray, pos);
                        AdvancedModes[3] = AdvancedModes3;
                        pos += 46;

                        AdvancedModes4.readMode(ConfigurationByteArray, pos);
                        AdvancedModes[4] = AdvancedModes4;
                        pos += 46;

                        //Geofences
                        ConfigGeofence0.readGeofence(ConfigurationByteArray, pos);
                        ConfigGeofence[0] = ConfigGeofence0;
                        pos += 25;

                        ConfigGeofence1.readGeofence(ConfigurationByteArray, pos);
                        ConfigGeofence[1] = ConfigGeofence1;
                        pos += 25;

                        ConfigGeofence2.readGeofence(ConfigurationByteArray, pos);
                        ConfigGeofence[2] = ConfigGeofence2;
                        pos += 25;

                        ConfigGeofence3.readGeofence(ConfigurationByteArray, pos);
                        ConfigGeofence[3] = ConfigGeofence3;
                        pos += 25;

                        ConfigGeofence4.readGeofence(ConfigurationByteArray, pos);
                        ConfigGeofence[4] = ConfigGeofence4;
                        pos += 25;

                        ConfigGeofence5.readGeofence(ConfigurationByteArray, pos);
                        ConfigGeofence[5] = ConfigGeofence5;
                        pos += 25;

                        ConfigGeofence6.readGeofence(ConfigurationByteArray, pos);
                        ConfigGeofence[6] = ConfigGeofence6;
                        pos += 25;

                        ConfigGeofence7.readGeofence(ConfigurationByteArray, pos);
                        ConfigGeofence[7] = ConfigGeofence7;
                        pos += 25;

                        ConfigGeofence8.readGeofence(ConfigurationByteArray, pos);
                        ConfigGeofence[8] = ConfigGeofence8;
                        pos += 25;

                        ConfigGeofence9.readGeofence(ConfigurationByteArray, pos);
                        ConfigGeofence[9] = ConfigGeofence9;
                        pos += 25;

                        //Timefences
                        ConfigTimefence0.readTimefence(ConfigurationByteArray, pos);
                        ConfigTimefence[0] = ConfigTimefence0;
                        pos += 17;

                        ConfigTimefence1.readTimefence(ConfigurationByteArray, pos);
                        ConfigTimefence[1] = ConfigTimefence1;
                        pos += 17;

                        ConfigTimefence2.readTimefence(ConfigurationByteArray, pos);
                        ConfigTimefence[2] = ConfigTimefence2;
                        pos += 17;

                        //IO Settings
                        ConfigIOSettings.readIOSettings(ConfigurationByteArray, pos);
                        pos += 2;

                        //Notifications
                        ConfigNotifications.readNotifications(ConfigurationByteArray, pos);
                        pos += 11;

                        //Iridium
                        ConfigIridium.readIridium(ConfigurationByteArray, pos);
                        pos += 32;

                        //Cell
                        ConfigCell.readCell(ConfigurationByteArray, pos);
                        pos += 186; // just for reference

                        ConfigIsCurrent = true;

                        Debug.WriteLine("...Configuration Successfully Read...\n");
                        Debug.WriteLine("...Configuration Successfully Updated...\n");

                    }
                    else
                    {
                        Debug.WriteLine("...Configuration Unchanged...\n");
                        ConfigIsCurrent = true;
                    }
                }
                else throw new Exception("...Incorrect Configuration Length: " + ConfigurationByteArray.Length);                
            }
            catch(Exception e) 
            {
                ConfigIsCurrent = false;
                Debug.WriteLine(e.Message);
            }
            finally
            {
                pos = 0;
            }
        }

        public static void UpdateConfigurationArray(byte[] ConfigArray, int IndexStart)
        {
            ConfigIsCurrent = false;

            Debug.WriteLine("Original Config: " +BitConverter.ToString(UpdatedFullConfigArray));
            lock(UpdatedFullConfigArray)
            {
                Debug.WriteLine("Config Array Length: " + ConfigArray.Length);
                Debug.WriteLine("Current Config Length: " + UpdatedFullConfigArray.Length);
                for (int i = 0; i < ConfigArray.Length; i++)
                {
                    UpdatedFullConfigArray[IndexStart + i] = ConfigArray[i];
                }

                UpdatedConfigCRC = UtilCRC.calculateCRC(UpdatedFullConfigArray, 0, UpdatedFullConfigArray.Length);
                Debug.WriteLine("Updated CRC test: " + UpdatedConfigCRC);
            }
            Debug.WriteLine("New Config: " + BitConverter.ToString(UpdatedFullConfigArray));
        }
    }

    public class NewConfigurationItems
    {
        public Mode NewConfigMode0;
        public Mode NewConfigMode1;
        public Mode NewConfigMode2;
        public Mode NewConfigMode3;
        public Mode NewConfigMode4;
        public Mode NewConfigMode5;

        public Geofence NewConfigGeofence0;
        public Geofence NewConfigGeofence1;
        public Geofence NewConfigGeofence2;
        public Geofence NewConfigGeofence3;
        public Geofence NewConfigGeofence4;
        public Geofence NewConfigGeofence5;
        public Geofence NewConfigGeofence6;
        public Geofence NewConfigGeofence7;
        public Geofence NewConfigGeofence8;
        public Geofence NewConfigGeofence9;

        public Timefence NewConfigTimefence0;
        public Timefence NewConfigTimefence1;
        public Timefence NewConfigTimefence2;

        public IOSettings NewConfigIOSettings;
        public Notifications NewConfigNotifications;
        public Iridium NewConfigIridium;
        public Cell NewConfigCell;

        public NewConfigurationItems()
        {
            NewConfigMode0 = new Mode();
            NewConfigMode1 = new Mode();
            NewConfigMode2 = new Mode();
            NewConfigMode3 = new Mode();
            NewConfigMode4 = new Mode();
            NewConfigMode5 = new Mode();

            NewConfigGeofence0 = new Geofence();
            NewConfigGeofence1 = new Geofence();
            NewConfigGeofence2 = new Geofence();
            NewConfigGeofence3 = new Geofence();
            NewConfigGeofence4 = new Geofence();
            NewConfigGeofence5 = new Geofence();
            NewConfigGeofence6 = new Geofence();
            NewConfigGeofence7 = new Geofence();
            NewConfigGeofence8 = new Geofence();
            NewConfigGeofence9 = new Geofence();

            NewConfigTimefence0 = new Timefence();
            NewConfigTimefence1 = new Timefence();
            NewConfigTimefence2 = new Timefence();

            NewConfigIOSettings = new IOSettings();
            NewConfigNotifications = new Notifications();
            NewConfigIridium = new Iridium();
            NewConfigCell = new Cell();
        }


    }
}
