using RBot;
using RBot.Items;
using RBot.Skills;
using System.ComponentModel;
using System.Data;

namespace CoreBotsOptions
{
    public partial class CoreBotsOptions : Form
    {
        public static CoreBotsOptions Instance { get; } = new CoreBotsOptions();
        public static ScriptInterface Bot => ScriptInterface.Instance;

        public CoreBotsOptions()
        {
            InitializeComponent();
            TabsStorage.TabPages.Remove(Class_Equipment);
            if (File.Exists(AppPath + @"\Skills\AdvancedSkills.txt"))
                AdvancedSkills = File.ReadAllLines(AppPath + @"\Skills\AdvancedSkills.txt");
            if (File.Exists(AppPath + @"\Scripts\CoreBots.cs"))
            {
                string CoreBotsVersion = File.ReadAllLines(AppPath + @"\Scripts\CoreBots.cs").First();
                if (CoreBotsVersion.StartsWith("//"))
                    CurrentScriptsVersion.Text = CoreBotsVersion.Replace("//", "");
            }
        }
        private string[] AdvancedSkills = null;
        private string AppPath = Path.GetDirectoryName(Application.ExecutablePath);

        private void CoreBotsOptions_Load(object sender, EventArgs e)
        {
            if (!InventoryGrabber.IsBusy)
                InventoryGrabber.RunWorkerAsync();
            if (!UseModeWorker.IsBusy)
                UseModeWorker.RunWorkerAsync();
        }

        private void CoreBotsOptions_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void CoreBotsOptions_FocusEnter(object sender, EventArgs e)
        {
            ReadSave();
            if (!InventoryGrabber.IsBusy)
                InventoryGrabber.RunWorkerAsync();
        } 

        private void ClassSelect_TextUpdate(object sender, EventArgs e)
        {
            if (!UseModeWorker.IsBusy)
                UseModeWorker.RunWorkerAsync();
        }

        private void SoloEquipCheck_CheckedChanged(object sender, EventArgs e)
        {
            SoloEquipmentBox.Visible = SoloEquipCheck.Checked;

            if (SoloEquipCheck.Checked || FarmEquipCheck.Checked)
            {
                if (!TabsStorage.TabPages.Contains(Class_Equipment))
                    TabsStorage.TabPages.Add(Class_Equipment);
            }
            else TabsStorage.TabPages.Remove(Class_Equipment);
        }

        private void FarmEquipCheck_CheckedChanged(object sender, EventArgs e)
        {
            FarmEquipmentBox.Visible = FarmEquipCheck.Checked;

            if (SoloEquipCheck.Checked || FarmEquipCheck.Checked)
            {
                if (!TabsStorage.TabPages.Contains(Class_Equipment))
                    TabsStorage.TabPages.Add(Class_Equipment);
            }
            else TabsStorage.TabPages.Remove(Class_Equipment);
        }

        private string[] LastClasses;
        private string[] LastHelms;
        private string[] LastArmors;
        private string[] LastCapes;
        private string[] LastWeapons;
        private string[] LastPets;
        private string[] LastGroundItems;
        private void InventoryGrabber_DoWork(object sender, DoWorkEventArgs e)
        {
            if (!Bot.Player.LoggedIn || !Bot.Player.Loaded)
                return;
            
            List<InventoryItem> InventoryData = Bot.Inventory.Items.OrderBy(x => x.Name).ToList();
            string[] Classes = InventoryData.FindAll(i => i.Category == ItemCategory.Class && i.EnhancementLevel > 0).Select(j => j.Name).ToArray();
            string[] Helms = InventoryData.FindAll(i => i.Category == ItemCategory.Helm && i.EnhancementLevel > 0).Select(j => j.Name).ToArray();
            string[] Armors = InventoryData.FindAll(i => i.Category == ItemCategory.Armor).Select(j => j.Name).ToArray();
            string[] Capes = InventoryData.FindAll(i => i.Category == ItemCategory.Cape && i.EnhancementLevel > 0).Select(j => j.Name).ToArray();
            string[] Weapons = InventoryData.FindAll(i => i.ItemGroup == "Weapon" && i.EnhancementLevel > 0).Select(j => j.Name).ToArray();
            string[] Pets = InventoryData.FindAll(i => i.Category == ItemCategory.Pet).Select(j => j.Name).ToArray();
            string[] GroundItems = InventoryData.FindAll(i => i.Category == ItemCategory.Misc).Select(j => j.Name).ToArray();

            if (InventoryData == null)
                return;

            if (!DesignMode)
            {
                if (LastClasses == null || !CompareArrays(Classes, LastClasses))
                {
                    SoloClassSelect.BeginUpdate();
                    FarmClassSelect.BeginUpdate();

                    //try { SoloClassSelect.AutoCompleteMode = AutoCompleteMode.None; }
                    //catch { }

                    SoloClassSelect.Items.Clear();
                    FarmClassSelect.Items.Clear();

                    SoloClassSelect.Items.AddRange(Classes);
                    FarmClassSelect.Items.AddRange(Classes);

                    //try { SoloClassSelect.AutoCompleteMode = AutoCompleteMode.SuggestAppend; }
                    //catch { }

                    SoloClassSelect.EndUpdate();
                    FarmClassSelect.EndUpdate();

                    LastClasses = Classes;
                }
                if (LastHelms == null || !CompareArrays(Helms, LastHelms))
                {
                    Helm1Select.BeginUpdate();
                    Helm2Select.BeginUpdate();

                    Helm1Select.Items.Clear();
                    Helm2Select.Items.Clear();

                    Helm1Select.Items.AddRange(Helms);
                    Helm2Select.Items.AddRange(Helms);

                    Helm1Select.EndUpdate();
                    Helm2Select.EndUpdate();

                    LastHelms = Helms;
                }
                if (LastArmors == null || !CompareArrays(Armors, LastArmors))
                {
                    Armor1Select.BeginUpdate();
                    Armor2Select.BeginUpdate();

                    Armor1Select.Items.Clear();
                    Armor2Select.Items.Clear();

                    Armor1Select.Items.AddRange(Armors);
                    Armor2Select.Items.AddRange(Armors);

                    Armor1Select.EndUpdate();
                    Armor2Select.EndUpdate();

                    LastArmors = Armors;
                }
                if (LastCapes == null || !CompareArrays(Capes, LastCapes))
                {
                    Cape1Select.BeginUpdate();
                    Cape2Select.BeginUpdate();

                    Cape1Select.Items.Clear();
                    Cape2Select.Items.Clear();

                    Cape1Select.Items.AddRange(Capes);
                    Cape2Select.Items.AddRange(Capes);

                    Cape1Select.EndUpdate();
                    Cape2Select.EndUpdate();

                    LastCapes = Capes;
                }
                if (LastWeapons == null || !CompareArrays(Weapons, LastWeapons))
                {
                    Weapon1Select.BeginUpdate();
                    Weapon2Select.BeginUpdate();

                    Weapon1Select.Items.Clear();
                    Weapon2Select.Items.Clear();

                    Weapon1Select.Items.AddRange(Weapons);
                    Weapon2Select.Items.AddRange(Weapons);

                    Weapon1Select.EndUpdate();
                    Weapon2Select.EndUpdate();

                    LastWeapons = Weapons;
                }
                if (LastPets == null || !CompareArrays(Pets, LastPets))
                {
                    Pet1Select.BeginUpdate();
                    Pet2Select.BeginUpdate();

                    Pet1Select.Items.Clear();
                    Pet2Select.Items.Clear();

                    Pet1Select.Items.AddRange(Pets);
                    Pet2Select.Items.AddRange(Pets);

                    Pet1Select.EndUpdate();
                    Pet2Select.EndUpdate();

                    LastPets = Pets;
                }
                if (LastGroundItems == null || !CompareArrays(GroundItems, LastGroundItems))
                {
                    GroundItem1Select.BeginUpdate();
                    GroundItem2Select.BeginUpdate();

                    GroundItem1Select.Items.Clear();
                    GroundItem2Select.Items.Clear();

                    GroundItem1Select.Items.AddRange(GroundItems);
                    GroundItem2Select.Items.AddRange(GroundItems);

                    GroundItem1Select.EndUpdate();
                    GroundItem2Select.EndUpdate();

                    LastGroundItems = GroundItems;
                }

                bool CompareArrays(string[] CurrentArray, string[] LastArray)
                {
                    List<string> CurrentList = CurrentArray.ToList();
                    List<string> LastList = LastArray.ToList();

                    if (CurrentList.Count != LastList.Count)
                        return false;

                    int CLC = 0;
                    int LLC = 0;

                    foreach (string Item in CurrentList)
                    {
                        if (LastList.Contains(Item))
                            CLC++;
                    }
                    foreach (string Item in LastList)
                    {
                        if (CurrentList.Contains(Item))
                            LLC++;
                    }

                    if (CLC != CurrentList.Count || CLC != LastList.Count || LLC != CurrentList.Count || LLC != LastList.Count)
                        return false;
                    return true;
                }
            }
        }

        private List<string> LastSoloModes = new();
        private List<string> LastFarmModes = new();
        private string LastSoloClass;
        private string LastFarmClass;
        private void UseModeWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            SoloModeSelect.Items.Remove("M.A.S. not found");
            FarmModeSelect.Items.Remove("M.A.S. not found");

            if (AdvancedSkills == null)
            {
                SoloModeSelect.Items.Add("M.A.S. not found");
                FarmModeSelect.Items.Add("M.A.S. not found");
                return;
            }

            List<string> SoloModes = new();
            List<string> FarmModes = new();

            if (SoloClassSelect.Text == LastSoloClass)
                SoloModes = LastSoloModes;
            else if (SoloClassSelect.Text != null)
            {
                foreach (string Line in AdvancedSkills)
                {
                    string Lowered = Line.ToLower();
                    if (Lowered.Contains($"= {SoloClassSelect.Text.ToLower()} ="))
                        SoloModes.Add(Line.Split(' ')[0]);
                    if (FarmModes.Contains("Base"))
                        break;
                }
            }

            if (FarmClassSelect.Text == LastFarmClass)
                FarmModes = LastFarmModes;
            else if (FarmClassSelect.Text != null)
            {
                foreach (string Line in AdvancedSkills)
                {
                    string Lowered = Line.ToLower();
                    if (Lowered.Contains($"= {FarmClassSelect.Text.ToLower()} ="))
                        FarmModes.Add(Line.Split(' ')[0]);
                    if (FarmModes.Contains("Base"))
                        break;
                }
            }

            SoloModeSelect.BeginUpdate();
            FarmModeSelect.BeginUpdate();

            foreach (string Mode in Enum.GetNames(typeof(ClassUseMode)))
            {
                if (!SoloModes.Contains(Mode))
                    SoloModeSelect.Items.Remove(Mode);
                else if (!SoloModeSelect.Items.Contains(Mode))
                    SoloModeSelect.Items.Add(Mode);

                if (!FarmModes.Contains(Mode))
                    FarmModeSelect.Items.Remove(Mode);
                else if (!FarmModeSelect.Items.Contains(Mode))
                    FarmModeSelect.Items.Add(Mode);
            }

            SoloModeSelect.EndUpdate();
            FarmModeSelect.EndUpdate();

            if (SoloModeSelect.Text == "" || !SoloModeSelect.Items.Contains(SoloModeSelect.Text))
                SoloModeSelect.Text = SoloModeSelect.Items[0].ToString();
            if (FarmModeSelect.Text == "" || !FarmModeSelect.Items.Contains(FarmModeSelect.Text))
                FarmModeSelect.Text = FarmModeSelect.Items[0].ToString();

            LastSoloModes = SoloModes;
            LastFarmModes = FarmModes;
            LastSoloClass = SoloClassSelect.Text;
            LastFarmClass = FarmClassSelect.Text;
        }

        private void OtherTreeView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            TreeNode Node = OtherTreeView.SelectedNode;
            if (!Node.Text.StartsWith('['))
                return;

            if (Node.ForeColor == Color.Red || Node.ForeColor == Color.Green)
            {
                Node.ForeColor = Node.ForeColor == Color.Green ? Color.Red : Color.Green;
                Node.Text = (Node.ForeColor == Color.Green ? "[On] " : "[Off] ") + Node.Text.Split(']')[1].Trim();
            }
            else if (Node.ForeColor == Color.DarkGoldenrod || Node.ForeColor == Color.Blue)
            {
                Node.ForeColor = Node.ForeColor == Color.DarkGoldenrod ? Color.Blue : Color.DarkGoldenrod;
                Node.Text = (Node.ForeColor == Color.DarkGoldenrod ? "[A] " : "[B] ") + Node.Text.Split(']')[1].Trim();
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Bot.Player.Username))
            {
                MessageBox.Show("Login first so that we can fetch your username for the save file");
                return;
            }
            string[] ToWrite = 
            {
                //Generic
                $"PrivateRooms: {PrivateRooms.Checked}",
                $"PrivateRoomNr: {PrivateRoomNr.Value}",
                $"PublicDifficult: {PublicDifficult.Checked}",
                $"AntiLag: {AntiLag.Checked}",
                $"BankMiscAC: {BankMiscAC.Checked}",
                $"LoggerInChat: {BankMiscAC.Checked}",

                $@"SoloClassSelect: {SoloClassSelect.Text}",
                $"SoloEquipCheck: {SoloEquipCheck.Checked}",
                $@"SoloModeSelect: {SoloModeSelect.Text}",

                $@"FarmClassSelect: {FarmClassSelect.Text}",
                $"FarmEquipCheck: {FarmEquipCheck.Checked}",
                $@"FarmModeSelect: {FarmModeSelect.Text}",

                //Advanced
                $"MessageBoxCheck: {MessageBoxCheck.Checked}",
                $"RestCheck: {RestCheck.Checked}",

                $"ActionDelayNr: {ActionDelayNr.Value}",
                $"ExitCombatNr: {ExitCombatNr.Value}",
                $"HuntDelayNr: {HuntDelayNr.Value}",
                $"QuestTriesNr: {QuestTriesNr.Value}",

                //Other
                $@"HBDK_PreFarm: {GetNode("HBDK_PreFarm").ForeColor == Color.Green}",

                $"NSOD_MaxStack: {GetNode("NSOD_MaxStack").ForeColor == Color.Green}",
                $"NSOD_PreFarm: {GetNode("NSOD_PreFarm").ForeColor == Color.Green}",

                $"VHL_Sparrow: {GetNode("VHL_Sparrow").ForeColor == Color.Green}",

                $"SDKA_Quest: {GetNode("SDKA_Quest").ForeColor == Color.DarkGoldenrod}",

                $"DBON_SoloPvPBoss: {GetNode("DBON_SoloPvPBoss").ForeColor == Color.Green}",

                $"WOO_NonAC: {GetNode("WOO_NonAC").ForeColor == Color.Green}",

                $"BCO_Story_TestBot: {GetNode("BCO_Story_TestBot").ForeColor == Color.Green}",

                //Class Equipment
                $"Helm1Select: {Helm1Select.Text}",
                $"Armor1Select: {Armor1Select.Text}",
                $"Cape1Select: {Cape1Select.Text}",
                $"Weapon1Select: {Weapon1Select.Text}",
                $"Pet1Select: {Pet1Select.Text}",
                $"GroundItem1Select: {GroundItem1Select.Text}",

                $"Helm2Select: {Helm2Select.Text}",
                $"Armor2Select: {Armor2Select.Text}",
                $"Cape2Select: {Cape2Select.Text}",
                $"Weapon2Select: {Weapon2Select.Text}",
                $"Pet2Select: {Pet2Select.Text}",
                $@"GroundItem2Select: {GroundItem2Select.Text}",
            };

            File.WriteAllLines(AppPath + $@"\plugins\options\CBO_Storage({Bot.Player.Username}).txt", ToWrite);
            MessageBox.Show($@"Saved to \plugins\options\CBO_Storage({Bot.Player.Username}).txt", "Save Successful!");
        }

        private void ReadSave()
        {
            if (AlreadyRead)
                return;
            if (string.IsNullOrEmpty(Bot.Player.Username))
            {
                MessageBox.Show("Login first so that we can fetch your username for the save file");
                FetchUsername.Visible = true;
                return;
            }
            if (!File.Exists(AppPath + $@"\plugins\options\CBO_Storage({Bot.Player.Username}).txt"))
                return;

            List<string> OptionsList = File.ReadAllLines(AppPath + $@"\plugins\options\CBO_Storage({Bot.Player.Username}).txt").ToList();

            //Generic
            PrivateRooms.Checked = GetBool("PrivateRooms");
            PrivateRoomNr.Value = GetInt("PrivateRoomNr");
            PublicDifficult.Checked = GetBool("PublicDifficult");
            AntiLag.Checked = GetBool("AntiLag");
            BankMiscAC.Checked = GetBool("BankMiscAC");
            LoggerInChat.Checked = GetBool("LoggerInChat");

            SoloClassSelect.Text = GetString("SoloClassSelect");
            SoloEquipCheck.Checked = GetBool("SoloEquipCheck");
            SoloModeSelect.Text = GetString("SoloModeSelect");

            FarmClassSelect.Text = GetString("FarmClassSelect");
            FarmEquipCheck.Checked = GetBool("FarmEquipCheck");
            FarmModeSelect.Text = GetString("FarmModeSelect");

            //Advanced
            MessageBoxCheck.Checked = GetBool("MessageBoxCheck");
            RestCheck.Checked = GetBool("RestCheck");

            ActionDelayNr.Value = GetInt("ActionDelayNr");
            ExitCombatNr.Value = GetInt("ExitCombatNr");
            HuntDelayNr.Value = GetInt("HuntDelayNr");
            QuestTriesNr.Value = GetInt("QuestTriesNr");

            //Other
            SetNodeData("HBDK_PreFarm");

            SetNodeData("NSOD_MaxStack");
            SetNodeData("NSOD_PreFarm");

            SetNodeData("VHL_Sparrow");

            SetNodeData("SDKA_Quest", 1);

            SetNodeData("DBON_SoloPvPBoss");

            SetNodeData("WOO_NonAC");

            SetNodeData("BCO_Story_TestBot");

            //Class Equipment
            Helm1Select.Text = GetString("Helm1Select");
            Armor1Select.Text = GetString("Armor1Select");
            Cape1Select.Text = GetString("Cape1Select");
            Weapon1Select.Text = GetString("Weapon1Select");
            Pet1Select.Text = GetString("Pet1Select");
            GroundItem1Select.Text = GetString("GroundItem1Select");

            Helm2Select.Text = GetString("Helm2Select");
            Armor2Select.Text = GetString("Armor2Select");
            Cape2Select.Text = GetString("Cape2Select");
            Weapon2Select.Text = GetString("Weapon2Select");
            Pet2Select.Text = GetString("Pet2Select");
            GroundItem2Select.Text = GetString("GroundItem2Select");

            FetchUsername.Visible = false;
            AlreadyRead = true;

            string GetString(string Name)
            {
                return OptionsList.Find(x => x.StartsWith(Name)).Split(": ")[1];
            }
            bool GetBool(string Name)
            {
                return GetString(Name) == "True";
            }
            int GetInt(string Name)
            {
                return int.Parse(GetString(Name));
            }

            void SetNodeData(string Name, int ColorMode = 0)
            {
                TreeNode Node = GetNode(Name);
                switch (ColorMode)
                {
                    case 0:
                        Node.ForeColor = GetBool(Name) ? Color.Green : Color.Red;
                        Node.Text = (Node.ForeColor == Color.Green ? "[On] " : "[Off] ") + Node.Text.Split(']')[1].Trim();
                        break;
                    case 1:
                        Node.ForeColor = GetBool(Name) ? Color.DarkGoldenrod : Color.Blue;
                        Node.Text = (Node.ForeColor == Color.DarkGoldenrod ? "[A] " : "[B] ") + Node.Text.Split(']')[1].Trim();
                        break;
                    default:
                        MessageBox.Show($"Unexpected input in SetNodeData. Received {ColorMode}");
                        break;
                }
            }
        }
        private bool AlreadyRead = false;

        private TreeNode GetNode(string Name)
        {
            return OtherTreeView.Nodes.Find(Name, true).First();
        }

        private void CurrentScriptsVersion_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("explorer", "https://github.com/BrenoHenrike/Scripts/releases/tag/Sv2.0");
        }

        private void FetchUsername_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ReadSave();
        }
    }
}

