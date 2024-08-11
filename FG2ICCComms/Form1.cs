using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows.Forms;
using J2534;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;


namespace FG2ICCComms
{
	// Token: 0x02000002 RID: 2
	public partial class Form1 : Form
	{
		  // ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //       ____.________   .________________     _____  
        //      |    |\_____  \  |   ____/\_____  \   /  |  | 
        //      |    | /  ____/  |____  \   _(__  <  /   |  |_
        //  /\__|    |/       \  /       \ /       \/    ^   /
        //  \________|\_______ \/______  //______  /\____   |  Interface Connect
        //                    \/       \/        \/      |__| 
        // ///////////////////////////////////////////////////
       
        ////////////////////////////////////////////////////////////////////////////////	
        public void connectJ2534()
        {
            // ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //
            try
            {
                if (j2534Flag == true)
                {
                    //addTxt1("FoA Orion Comms \r\n ");
                    // Disable ELM327 Interface Connect Button // Make it Different Colour
                    // Make Connect J2534 Button Go Red and Say Disconnect
                    //buttonConnectJ2534Device.Text = "Reconnect to HighSpeed";  //change the Connect Button text to "Disconnect"
              
                    button1.Text = "Disconnect";                                                        //
                    addTxt1("Initialising Comms...\r\n");
                    // ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //
                    List<J2534.J2534Device> MyListOfJ2534Devices = J2534DeviceFinder.FindInstalledJ2534DLLs();
                    for (int i = 0; i < MyListOfJ2534Devices.Count; i++)
                    {
                        string J2534ToolsName = MyListOfJ2534Devices[i].Name;
                        //comboBoxJ2534Devices.Items.Add(J2534ToolsName);
                        //addTxtLog("Found Installed Device: " + J2534ToolsName.ToString() + "\r\n");
                    }
                    // ///////////////////////////////////////////////////
                    //2) Select our device and load the DLL into memory
                    //Below is my fancy way of specificaly targeting a tool in the list based on its name.
                    //J2534Port.LoadedDevice = MyListOfJ2534Devices.Where(x => x.Name == "CDR900").First(); //MyListOfJ2534Devices[0];
                    J2534Port.LoadedDevice = MyListOfJ2534Devices[comboBoxJ2534Devices.SelectedIndex];
                    if (J2534Port.Functions.LoadLibrary(J2534Port.LoadedDevice) == false)
                    {
                        addTxt1("Failed to load library DLL ERROR \r\n");
                        //Error here, DLL is fucked.
                    }
                    else
                    {
                        addTxt1("Library Loaded Succesfully \r\n");

                    }
                    addTxt1("Selected Device: " + comboBoxJ2534Devices.SelectedItem.ToString() + "\r\n");  //get ti this point successfully
                                                                                                           // ///////////////////////////////////////////////////
                                                                                                           // ///////////////////////////////																					   //3) Open the connection to the J2534 Tool
                                                                                                           //IntPtr newPtr = Marshal.AllocHGlobal(100);
                                                                                                           //3) Open the connection to the J2534 Tool
                    var ErrorResult = J2534Port.Functions.PassThruOpen(IntPtr.Zero, ref DeviceID); //then error out here with invalid device number
                    if (ErrorResult != J2534Err.STATUS_NOERROR)
                    {
                        addTxt1(ErrorResult.ToString() + "\r\n");
                        addTxt1("PassThru Open ERROR) \r\n");

                    }
                    else
                    {
                        addTxt1("PassThru Open Success \r\n");
                    }
                    ushort psValue = 0x30B;
                    //var ErrorResult = J2534Err.STATUS_NOERROR;
                    ErrorResult = J2534Port.Functions.PassThruConnect(DeviceID, ProtocolID.ISO15765_PS, ConnectFlag.NONE, BaudRate.CAN_125000, ref ChannelID);
                    //ErrorResult = J2534Port.Functions.PassThruConnect(DeviceID, ProtocolID.ISO15765_PS, ConnectFlag.NONE, BaudRate.CAN_500000, ref ChannelID);
                    if (ErrorResult != J2534Err.STATUS_NOERROR)
                    {
                        addTxt1(ErrorResult.ToString() + "\r\n");
                        addTxt1("PassThru Connect ERROR \r\n");
                    }
                    else
                    {
                        addTxt1("PassThru Connect Success \r\n");
                    }
                    List<SConfig> configuration = new List<SConfig>();
                    // TO BE SET TO 0x30B for real world J2534, at the moment on bench rig we are on pins 6 & 14
                    configuration.Add(new SConfig() { Parameter = ConfigParameter.J1962_PINS, Value = psValue });
                    ErrorResult = J2534Port.Functions.SetConfig((int)ChannelID, ref configuration);
                    if (ErrorResult != J2534Err.STATUS_NOERROR)
                    {
                        addTxt1(ErrorResult.ToString() + "\r\n");
                        addTxt1("SConfig Pin Select ERROR \r\n");
                    }
                    else
                    {
                        addTxt1("SConfig Pin Select Success\r\n");
                    }

                    // //////////////////////////////////////////////////////////////
                    //6) Set Filters
                    //A flow filter is whats required for ending multi line messages. Pass is for sending just individual messages.
                    PassThruMsg maskMsg = new PassThruMsg(ProtocolID.ISO15765_PS, TxFlag.NONE, new byte[] { 0, 0, 07, 0xFF }); //Set mask of 7FF (Only accept the exact PATTERN
                    PassThruMsg patternMsg = new PassThruMsg(ProtocolID.ISO15765_PS, TxFlag.NONE, new byte[] { 0, 0, 07, 0xAE }); //Search for 7E8
                    PassThruMsg flowMsg = new PassThruMsg(ProtocolID.ISO15765_PS, TxFlag.NONE, new byte[] { 0, 0, 07, 0xA6 }); //Use the flow message of 7E0
                    IntPtr maskPtr = maskMsg.ToIntPtr();
                    IntPtr PatternPtr = patternMsg.ToIntPtr();
                    IntPtr FlowPtr = flowMsg.ToIntPtr();
                    ErrorResult = J2534Port.Functions.PassThruStartMsgFilter((int)ChannelID, FilterType.FLOW_CONTROL_FILTER, maskPtr, PatternPtr, FlowPtr, ref FilterID);
                    if (ErrorResult != J2534Err.STATUS_NOERROR)
                    {
                        addTxt1(ErrorResult.ToString() + "\r\n");
                        addTxt1("Couldn't Set maskPtr, PatternPtr & Flow Ptr Filters");
                    }
                    else
                    {
                        addTxt1("PCM Start Message Filter Success \r\n");
                    }
                    //ALWAYS do this after all commands that use a INTPTR so that it releases any used memory/ram by that variable.
                    Marshal.FreeHGlobal(maskPtr);
                    Marshal.FreeHGlobal(PatternPtr);
                    Marshal.FreeHGlobal(FlowPtr);
                    // Check if "Tactrix" is in the selected item

                    // ///////////////////////////////////////////////////////////////////
                    //4) Start our OBD2 connection 
                    //Use ProtocolID.ISO15765_PS if needing to connect to MS CAN. Have to pass the pins 3/11 to tell it to go to those.
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    // ///////////////////////////////////////////////////////////////////
                    //4) Start our OBD2 connection 
                    //Use ProtocolID.ISO15765_PS if needing to connect to MS CAN. Have to pass the pins 3/11 to tell it to go to those.
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //         .__                   __       __  .__    .__                        __    __  .__                      ____.  _____  _________  ____  __.
                    //    ____ |  |__   ____   ____ |  | __ _/  |_|  |__ |__| ______   ______ _____/  |__/  |_|__| ____    ____       |    | /  _  \ \_   ___ \|    |/ _|
                    //  _/ ___\|  |  \_/ __ \_/ ___\|  |/ / \   __\  |  \|  |/  ___/  /  ___// __ \   __\   __\  |/    \  / ___\      |    |/  /_\  \/    \  \/|      <  
                    //  \  \___|   Y  \  ___/\  \___|    <   |  | |   Y  \  |\___ \   \___ \\  ___/|  |  |  | |  |   |  \/ /_/  > /\__|    /    |    \     \___|    |  \ 
                    //   \___  >___|  /\___  >\___  >__|_ \  |__| |___|  /__/____  > /____  >\___  >__|  |__| |__|___|  /\___  /  \________\____|__  /\______  /____|__ \
                    //       \/     \/     \/     \/     \/            \/        \/       \/     \/                   \//_____/                    \/        \/        \/
                    // TO BE SET TO ProtocolID.ISO15765_PS for real world J2534, at the moment on bench rig we are on pins 6 & 14
                    //setPinSelect();
                    // //////////////////////////////////////////////////////////////
                    //5) Set the OBD2 pins we are connecting to on the canbus line
                    //Set pins if doing MSCAN or selecting the _PS channel (PS stands for Pin Select)
 
                    return;
                }
            }
            catch (Exception ex)
            {
                // Catching any exception of type Exception
                // Handle the exception here, such as logging or displaying an error message
                addTxt1("J2534 Connection Error: " + ex.Message);
                addTxt1("Please try to connect again \r\n");
                //j2534Flag = false;
            }
            // //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            if (j2534Flag = false) ;
            {
                // Make Connect J2534 Button Go Red and Say Disconnect
                button1.Text = "Connect";  //change the Connect Button text to "Disconnect"
                //metroButton1.BackColor = System.Drawing.Color.RoyalBlue; // Change Connect BTN colour to RED															   //
                                                                         //addTxt1("Initialising Comms...\r\n");
                var ErrorResult = J2534Port.Functions.PassThruDisconnect((int)ChannelID);
                if (ErrorResult != J2534Err.STATUS_NOERROR)
                {
                    addTxt1(ErrorResult.ToString() + "\r\n");
                    addTxt1("PassThru Disconnect ERROR \r\n");
                }
                else
                {
                    addTxt1("PassThru Disconnected.\r\n");
                }
                ErrorResult = J2534Port.Functions.PassThruClose(DeviceID); ;
                if (ErrorResult != J2534Err.STATUS_NOERROR)
                {
                    addTxt1(ErrorResult.ToString() + "\r\n");
                    addTxt1("PassThru Disconnect ERROR \r\n");
                }
                else
                {
                    addTxt1("PassThru Library Closed. \r\n");
                }
                return;
            }
        }
        ////////////////////////////////////////////////////////////////////////////////	
        class J2534_Struct
        {
            public J2534_Struct()
            {
                Functions = new J2534FunctionsExtended();
                LoadedDevice = new J2534Device();
            }
            public J2534.J2534FunctionsExtended Functions;
            public J2534.J2534Device LoadedDevice;
        }
        // ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private J2534_Struct J2534Port = new J2534_Struct();
        private uint DeviceID;
        //private uint DeviceID;//This is the unique ID of the J2534 that is used in all functions.
        public uint ChannelID;//This is the unqiue ID of the CHannel (Protocol) that we connect to
        public int FilterID; //This is the unique ID of the Filter we set (Every filter gets a ID, so you should make this a list to save them all if doing more then 1.
        // ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        bool j2534Flag = false;
        byte ecuRxIdentifier1 = 0x07;
        byte ecuRxIdentifier2 = 0xA6;
       
	    // ////////////////////////////////////////////////////////////////
        // Detect J2534 Devices Installed on the system and display them
        // in the combo box as selectedable items
        private void buttonJ2534Detect_Click(object sender, EventArgs e)
        {
            // ///////////////////////////////////////////////////
            //1) Search for all of our J2534 Devices
            List<J2534.J2534Device> MyListOfJ2534Devices = J2534DeviceFinder.FindInstalledJ2534DLLs();
            //List of devices installed on the PC
            for (int i = 0; i < MyListOfJ2534Devices.Count; i++)
            {
                string J2534ToolsName = MyListOfJ2534Devices[i].Name;
                comboBoxJ2534Devices.Items.Add(J2534ToolsName);
                addTxt1("Found Installed Device: " + J2534ToolsName.ToString() + "\r\n");
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            comboBoxJ2534Devices.Items.Clear();
            // ///////////////////////////////////////////////////
            //1) Search for all of our J2534 Devices
            List<J2534.J2534Device> MyListOfJ2534Devices = J2534DeviceFinder.FindInstalledJ2534DLLs();
            //List of devices installed on the PC
            for (int i = 0; i < MyListOfJ2534Devices.Count; i++)
            {
                string J2534ToolsName = MyListOfJ2534Devices[i].Name;
                comboBoxJ2534Devices.Items.Add(J2534ToolsName);
                addTxt1("Found Installed Device: " + J2534ToolsName.ToString() + "\r\n");
            }
        }
		// Token: 0x06000003 RID: 3 RVA: 0x0000280C File Offset: 0x00000A0C
		

		// Token: 0x06000004 RID: 4 RVA: 0x000028F0 File Offset: 0x00000AF0
		private bool ishexpair(string s)
		{
			return s.Length >= 2 && ((s[0] >= '0' && s[1] <= '9') || s[0] >= 'A' || s[1] <= 'F' || s[0] >= 'a' || s[1] <= 'f');
		}

		// Token: 0x06000005 RID: 5 RVA: 0x0000294C File Offset: 0x00000B4C
		private int key_from_seed(int s)
		{
			byte b = (byte)(s >> 16 & 255);
			byte b2 = (byte)(s >> 8 & 255);
			byte b3 = (byte)(s & 255);
			int num = 112;
			int num2 = 76;
			int num3 = 97;
			int num4 = 82;
			int num5 = 77;
			int num6 = ((int)b << 16) + ((int)b2 << 8) + (int)b3;
			int num7 = (num6 & 16711680) >> 16 | (num6 & 65280) | num << 24 | (num6 & 255) << 16;
			int num8 = 12927401;
			for (int i = 0; i < 32; i++)
			{
				int num9 = ((num7 >> i & 1) ^ (num8 & 1)) << 23;
				int num11;
				int num10;
				num8 = (((num10 = (num11 = (num9 | num8 >> 1))) & 15691735) | ((num10 & 1048576) >> 20 ^ (num11 & 8388608) >> 23) << 20 | ((num8 >> 1 & 32768) >> 15 ^ (num11 & 8388608) >> 23) << 15 | ((num8 >> 1 & 4096) >> 12 ^ (num11 & 8388608) >> 23) << 12 | 32 * ((num8 >> 1 & 32) >> 5 ^ (num11 & 8388608) >> 23) | 8 * ((num8 >> 1 & 8) >> 3 ^ (num11 & 8388608) >> 23));
			}
			for (int i = 0; i < 32; i++)
			{
				int num9 = (((num5 << 24 | num4 << 16 | num2 | num3 << 8) >> i & 1) ^ (num8 & 1)) << 23;
				int num14;
				int num13;
				int num12 = num13 = (num14 = (num9 | num8 >> 1));
				num8 = ((num13 & 15691735) | ((num12 & 1048576) >> 20 ^ (num14 & 8388608) >> 23) << 20 | ((num8 >> 1 & 32768) >> 15 ^ (num14 & 8388608) >> 23) << 15 | ((num8 >> 1 & 4096) >> 12 ^ (num14 & 8388608) >> 23) << 12 | 32 * ((num8 >> 1 & 32) >> 5 ^ (num14 & 8388608) >> 23) | 8 * ((num8 >> 1 & 8) >> 3 ^ (num14 & 8388608) >> 23));
			}
			return (num8 & 983040) >> 16 | 16 * (num8 & 15) | ((num8 & 15728640) >> 20 | (num8 & 61440) >> 8) << 8 | (num8 & 4080) >> 4 << 16;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002BA0 File Offset: 0x00000DA0
		private int h2d(char c)
		{
			if (c <= '0')
			{
				return 0;
			}
			if (c > '9' && c < 'A')
			{
				return 0;
			}
			if (c > 'F')
			{
				return 0;
			}
			if (c <= '9')
			{
				return (int)(c - '0');
			}
			return (int)('\n' + (c - 'A'));
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002BD0 File Offset: 0x00000DD0
		private void addTxt1(string m)
		{
			TextBox textBox = this.textBox1;
			textBox.Text += m;
			this.textBox1.SelectionStart = this.textBox1.Text.Length;
			this.textBox1.SelectionLength = 0;
			this.textBox1.ScrollToCaret();
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002C28 File Offset: 0x00000E28
		private string printerr(int error)
		{
			if (error <= 38)
			{
				switch (error)
				{
				case 16:
					return "General reject\r\n";
				case 17:
					return "Service not supported\r\n";
				case 18:
					return "Subfunction not supported\r\n";
				case 19:
					return "Incorrect message length or invalid format\r\n";
				case 20:
					return "Response too long\r\n";
				default:
					switch (error)
					{
					case 33:
						return "Busy repeat request\r\n";
					case 34:
						return "Condition not correct\r\n";
					case 36:
						return "Request sequence error\r\n";
					case 37:
						return "No response from subnet component\r\n";
					case 38:
						return "Failure prevents execution of requested action\r\n";
					}
					break;
				}
			}
			else
			{
				switch (error)
				{
				case 49:
					return "Request out of range\r\n";
				case 50:
				case 52:
					break;
				case 51:
					return "Security access denied\r\n";
				case 53:
					return "Invalid key\r\n";
				case 54:
					return "Exceeded number of attempts\r\n";
				case 55:
					return "Required time delay not expired\r\n";
				default:
					switch (error)
					{
					case 112:
						return "Upload/download not accepted\r\n";
					case 113:
						return "Transfer data suspended\r\n";
					case 114:
						return "General programming failure\r\n";
					case 115:
						return "Wrong block sequence counter\r\n";
					case 116:
					case 117:
					case 118:
					case 119:
						break;
					case 120:
						return "";
					default:
						switch (error)
						{
						case 126:
							return "Subfunction not supported in active session\r\n";
						case 127:
							return "Service not supported in active session\r\n";
						}
						break;
					}
					break;
				}
			}
			return "";
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002D70 File Offset: 0x00000F70
	

	
		// Token: 0x06000013 RID: 19 RVA: 0x00003978 File Offset: 0x00001B78
		private void button1_Click(object sender, EventArgs e)
		{
			if (button1.Text == "Connect")
            {
                j2534Flag = true;
            }
            else
            {
                j2534Flag = false;
            }
            connectJ2534();
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00003C74 File Offset: 0x00001E74
		private void button2_Click(object sender, EventArgs e)
		{
			base.Enabled = false;
			requestSecurityAccess0x27();
			this.addTxt1("Running Recore from USB\r\n");
            //this.Write("31AB01\r");
            byte[] recore = new byte[] { 0, 0, ecuRxIdentifier1, ecuRxIdentifier2, 0x31, 0xAB, 0x01 };
            string recoreSend = sendPassThruMsg(recore);
            this.addTxt1("OK\r\n");
			base.Enabled = true;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00003CCC File Offset: 0x00001ECC
		private void button3_Click(object sender, EventArgs e)
		{
            byte[] ecuReset = new byte[] { 0, 0, ecuRxIdentifier1, ecuRxIdentifier2, 0x11, 0x01 };
            string ecuResetSend = sendPassThruMsg(ecuReset);
            this.addTxt1("OK\r\n");
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00003CE4 File Offset: 0x00001EE4
		private void button4_Click(object sender, EventArgs e)
		{

			requestSecurityAccess0x27();
			this.addTxt1("Updating Data ID E610 - High Series\r\n");
			byte[] didE610 = new byte[] { 0, 0, ecuRxIdentifier1, ecuRxIdentifier2, 0x2E, 0xE6, 0x10, 0x41, 0x52, 0x37, 0x39, 0x2D, 0x31, 0x34, 0x44, 0x30, 0x31, 0x35, 0x2D, 0x46, 0x43, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            string didE610Send = sendPassThruMsg(didE610);
			//this.Write("2EE610415237392D3134443031352D464300000000000000000000\r");
			this.addTxt1("OK\r\n");
			base.Enabled = true;
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00003F20 File Offset: 0x00002120
		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			
			this.button1.Text = "Connect";
			this.button2.Enabled = false;
			this.button3.Enabled = false;
			this.button4.Enabled = false;
			
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00003FDC File Offset: 0x000021DC
		private void Form1_LocationChanged(object sender, EventArgs e)
		{
			if (this.updating || base.WindowState != FormWindowState.Normal)
			{
				return;
			}
			Form1.Settings.SetObject("Form X", base.Location.X.ToString());
			Form1.Settings.SetObject("Form Y", base.Location.Y.ToString());
			Form1.Settings.Save();
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00004040 File Offset: 0x00002240
		private void Form1_Resize(object sender, EventArgs e)
		{
			if (this.updating || base.WindowState != FormWindowState.Normal)
			{
				return;
			}
			Form1.Settings.SetObject("Form Width", base.Width.ToString());
			Form1.Settings.SetObject("Form Height", base.Height.ToString());
			Form1.Settings.Save();
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00004094 File Offset: 0x00002294
		public Form1()
		{
			this.InitializeComponent();
			this.updating = true;
			base.Width = Convert.ToInt32(Form1.Settings.GetObject("Form Width", base.Width.ToString()));
			base.Height = Convert.ToInt32(Form1.Settings.GetObject("Form Height", base.Height.ToString()));
			Rectangle bounds = Screen.GetBounds(new Point(base.Location.X, base.Location.Y));
			string defval = Convert.ToString(bounds.Width / 2 - base.Width / 2);
			string defval2 = Convert.ToString(bounds.Height / 2 - base.Height / 2);
			base.Left = Convert.ToInt32(Form1.Settings.GetObject("Form X", defval));
			base.Top = Convert.ToInt32(Form1.Settings.GetObject("Form Y", defval2));
			this.updating = false;
		}

		// Token: 0x04000010 RID: 16
		public static bool reset_using_usb;

		// Token: 0x04000012 RID: 18
		private bool updating;

		// Token: 0x04000013 RID: 19
		private bool checking;

		// Token: 0x04000014 RID: 20
		private bool NVMcheck1;

		// Token: 0x04000015 RID: 21
		private bool NVMcheck2;

		// Token: 0x04000016 RID: 22
		private int timeout;

		// Token: 0x04000017 RID: 23
		private int cmdready;

		// Token: 0x04000018 RID: 24
		private int waitfor2;

		// Token: 0x04000019 RID: 25
		private int waitfor;

		// Token: 0x0400001A RID: 26
		private int inerror;

		// Token: 0x0400001B RID: 27
		private int enabled = 1;

		// Token: 0x0400001C RID: 28
		private int hidesend;

		// Token: 0x0400001D RID: 29
		private string msg = "";

		// Token: 0x0400001E RID: 30
		private bool newpacket;

		// Token: 0x0400001F RID: 31
		private byte[] rxBuf = new byte[8];

		// Token: 0x04000020 RID: 32
		private int lastchecksum;

		// Token: 0x02000003 RID: 3
		public static class Settings
		{
			// Token: 0x17000001 RID: 1
			// (get) Token: 0x0600001F RID: 31 RVA: 0x0000420C File Offset: 0x0000240C
			public static string DataPath
			{
				get
				{
					if (Form1.Settings.mDataPath == null)
					{
						Form1.Settings.mDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FG2ICCComms");
						if (!Directory.Exists(Form1.Settings.mDataPath))
						{
							Directory.CreateDirectory(Form1.Settings.mDataPath);
						}
					}
					return Form1.Settings.mDataPath;
				}
			}

			// Token: 0x17000002 RID: 2
			// (get) Token: 0x06000020 RID: 32 RVA: 0x00004248 File Offset: 0x00002448
			private static string filename
			{
				get
				{
					string text = "FG2ICCComms.Settings.bin";
					string text2 = Path.Combine(Form1.Settings.DataPath, text);
					if (!File.Exists(text2) && File.Exists(text))
					{
						File.Copy(text, text2);
					}
					return text2;
				}
			}

			// Token: 0x06000021 RID: 33 RVA: 0x00004280 File Offset: 0x00002480
			static Settings()
			{
				try
				{
					if (File.Exists(Form1.Settings.filename))
					{
						BinaryFormatter binaryFormatter = new BinaryFormatter();
						binaryFormatter.AssemblyFormat = FormatterAssemblyStyle.Simple;
						using (FileStream fileStream = new FileStream(Form1.Settings.filename, FileMode.Open, FileAccess.Read, FileShare.None))
						{
							Form1.Settings.dic = (Dictionary<string, object>)binaryFormatter.Deserialize(fileStream);
							fileStream.Close();
						}
					}
				}
				catch
				{
				}
				if (Form1.Settings.dic == null)
				{
					Form1.Settings.dic = new Dictionary<string, object>();
				}
			}

			// Token: 0x06000022 RID: 34 RVA: 0x0000430C File Offset: 0x0000250C
			public static object GetObject(string key, object defval)
			{
				if (!Form1.Settings.dic.ContainsKey(key) || Form1.Settings.dic[key] == null)
				{
					return defval;
				}
				return Form1.Settings.dic[key];
			}

			// Token: 0x06000023 RID: 35 RVA: 0x00004338 File Offset: 0x00002538
			public static object GetAndDeleteObject(string key, object defval)
			{
				object result = (Form1.Settings.dic.ContainsKey(key) && Form1.Settings.dic[key] != null) ? Form1.Settings.dic[key] : defval;
				Form1.Settings.DeleteObject(key);
				return result;
			}

			// Token: 0x06000024 RID: 36 RVA: 0x00004375 File Offset: 0x00002575
			public static void SetObject(string key, object value)
			{
				if (Form1.Settings.dic.ContainsKey(key))
				{
					Form1.Settings.dic[key] = value;
					return;
				}
				Form1.Settings.dic.Add(key, value);
			}

			// Token: 0x06000025 RID: 37 RVA: 0x000043A0 File Offset: 0x000025A0
			public static void Save()
			{
				try
				{
					BinaryFormatter binaryFormatter = new BinaryFormatter();
					binaryFormatter.AssemblyFormat = FormatterAssemblyStyle.Simple;
					using (FileStream fileStream = new FileStream(Form1.Settings.filename, FileMode.Create, FileAccess.Write, FileShare.None))
					{
						binaryFormatter.Serialize(fileStream, Form1.Settings.dic);
						fileStream.Close();
					}
				}
				catch
				{
				}
			}

			// Token: 0x06000026 RID: 38 RVA: 0x00004408 File Offset: 0x00002608
			internal static void DeleteObject(string key)
			{
				if (Form1.Settings.dic.ContainsKey(key))
				{
					Form1.Settings.dic.Remove(key);
				}
			}

			// Token: 0x06000027 RID: 39 RVA: 0x00004423 File Offset: 0x00002623
			internal static bool ExistObject(string key)
			{
				return Form1.Settings.dic.ContainsKey(key);
			}

			// Token: 0x04000021 RID: 33
			private static Dictionary<string, object> dic;

			// Token: 0x04000022 RID: 34
			private static string mDataPath;
		}

		// Token: 0x02000004 RID: 4
		public class UsbSerial
		{
			// Token: 0x06000028 RID: 40 RVA: 0x00004430 File Offset: 0x00002630
			public void Configure(string port, int baud)
			{
				this.mPortName = port;
				this.mBaudRate = baud;
			}

			// Token: 0x06000029 RID: 41 RVA: 0x00004440 File Offset: 0x00002640
			public void Open()
			{
				if (!this.com.IsOpen)
				{
					try
					{
						this.com.DataBits = 8;
						this.com.Parity = Parity.None;
						this.com.StopBits = StopBits.One;
						this.com.Handshake = Handshake.None;
						this.com.PortName = this.mPortName;
						this.com.BaudRate = this.mBaudRate;
						this.com.NewLine = "\n";
						this.com.WriteTimeout = 1000;
						this.com.DtrEnable = Form1.reset_using_usb;
						this.com.Open();
						this.com.DiscardOutBuffer();
						this.com.DiscardInBuffer();
					}
					catch (IOException ex)
					{
						if (!char.IsDigit(this.mPortName[this.mPortName.Length - 1]) || !char.IsDigit(this.mPortName[this.mPortName.Length - 2]))
						{
							throw ex;
						}
						this.com.PortName = this.mPortName.Substring(0, this.mPortName.Length - 1);
						this.com.Open();
						this.com.DiscardOutBuffer();
						this.com.DiscardInBuffer();
					}
					catch
					{
						MessageBox.Show("COM Port might be busy, please check it and retry", "Warning");
					}
				}
			}

			// Token: 0x0600002A RID: 42 RVA: 0x000045C0 File Offset: 0x000027C0
			public void Close()
			{
				if (this.com.IsOpen)
				{
					this.com.DiscardOutBuffer();
					this.com.DiscardInBuffer();
					try
					{
						this.com.Close();
					}
					catch
					{
					}
				}
			}

			// Token: 0x17000003 RID: 3
			// (get) Token: 0x0600002B RID: 43 RVA: 0x00004610 File Offset: 0x00002810
			public bool IsOpen
			{
				get
				{
					return this.com.IsOpen;
				}
			}

			// Token: 0x0600002C RID: 44 RVA: 0x00004620 File Offset: 0x00002820
			public void Write(char b)
			{
				this.com.Write(new byte[]
				{
					(byte)b
				}, 0, 1);
			}

			// Token: 0x0600002D RID: 45 RVA: 0x00004648 File Offset: 0x00002848
			public void Write(string s)
			{
				int i = 0;
				char[] array = s.ToCharArray();
				while (i < s.Length)
				{
					this.com.Write(new byte[]
					{
						(byte)array[i]
					}, 0, 1);
					i++;
				}
			}

			// Token: 0x0600002E RID: 46 RVA: 0x0000468C File Offset: 0x0000288C
			public char ReadByte()
			{
				int num = this.com.ReadByte();
				if (num == -1)
				{
					throw new EndOfStreamException("No COM data was available.");
				}
				return (char)num;
			}

			// Token: 0x0600002F RID: 47 RVA: 0x000046B6 File Offset: 0x000028B6
			public int HasData()
			{
				return this.com.BytesToRead;
			}

			// Token: 0x04000023 RID: 35
			private SerialPort com = new SerialPort();

			// Token: 0x04000024 RID: 36
			private string mPortName;

			// Token: 0x04000025 RID: 37
			private int mBaudRate;
		}

		// Token: 0x02000005 RID: 5
		public struct Comms
		{
			// Token: 0x04000026 RID: 38
			public Form1.UsbSerial com;
		}

      
    }
}
