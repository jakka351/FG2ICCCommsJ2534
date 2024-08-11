#region Copyright (c) 2024, Jack Leighton
// /////     __________________________________________________________________________________________________________________
// /////
// /////                  __                   __              __________                                      __   
// /////                _/  |_  ____   _______/  |_  __________\______   \_______   ____   ______ ____   _____/  |_ 
// /////                \   __\/ __ \ /  ___/\   __\/ __ \_  __ \     ___/\_  __ \_/ __ \ /  ___// __ \ /    \   __\
// /////                 |  | \  ___/ \___ \  |  | \  ___/|  | \/    |     |  | \/\  ___/ \___ \\  ___/|   |  \  |  
// /////                 |__|  \___  >____  > |__|  \___  >__|  |____|     |__|    \___  >____  >\___  >___|  /__|  
// /////                           \/     \/            \/                             \/     \/     \/     \/      
// /////                                                          .__       .__  .__          __                    
// /////                               ____________   ____   ____ |__|____  |  | |__| _______/  |_                  
// /////                              /  ___/\____ \_/ __ \_/ ___\|  \__  \ |  | |  |/  ___/\   __\                 
// /////                              \___ \ |  |_> >  ___/\  \___|  |/ __ \|  |_|  |\___ \  |  |                   
// /////                             /____  >|   __/ \___  >\___  >__(____  /____/__/____  > |__|                   
// /////                                  \/ |__|        \/     \/        \/             \/                         
// /////                                  __                         __  .__                                        
// /////                   _____   __ ___/  |_  ____   _____   _____/  |_|__|__  __ ____                            
// /////                   \__  \ |  |  \   __\/  _ \ /     \ /  _ \   __\  \  \/ // __ \                           
// /////                    / __ \|  |  /|  | (  <_> )  Y Y  (  <_> )  | |  |\   /\  ___/                           
// /////                   (____  /____/ |__|  \____/|__|_|  /\____/|__| |__| \_/  \___  >                          
// /////                        \/                         \/                          \/                           
// /////                                                  .__          __  .__                                      
// /////                                       __________ |  |  __ ___/  |_|__| ____   ____   ______                
// /////                                      /  ___/  _ \|  | |  |  \   __\  |/  _ \ /    \ /  ___/                
// /////                                      \___ (  <_> )  |_|  |  /|  | |  (  <_> )   |  \\___ \                 
// /////                                     /____  >____/|____/____/ |__| |__|\____/|___|  /____  >                
// /////                                          \/                                      \/     \/                 
// /////                                   Tester Present Specialist Automotive Solutions
// /////     __________________________________________________________________________________________________________________
// /////      |--------------------------------------------------------------------------------------------------------------|
// /////      |       https://github.com/jakka351/| https://testerPresent.com.au | https://facebook.com/testerPresent        |
// /////      |--------------------------------------------------------------------------------------------------------------|
// /////      | Copyright (c) 2022/2023/2024 Benjamin Jack Leighton                                                          |          
// /////      | All rights reserved.                                                                                         |
// /////      |--------------------------------------------------------------------------------------------------------------|
// /////        Redistribution and use in source and binary forms, with or without modification, are permitted provided that
// /////        the following conditions are met:
// /////        1.    With the express written consent of the copyright holder.
// /////        2.    Redistributions of source code must retain the above copyright notice, this
// /////              list of conditions and the following disclaimer.
// /////        3.    Redistributions in binary form must reproduce the above copyright notice, this
// /////              list of conditions and the following disclaimer in the documentation and/or other
// /////              materials provided with the distribution.
// /////        4.    Neither the name of the organization nor the names of its contributors may be used to
// /////              endorse or promote products derived from this software without specific prior written permission.
// /////      _________________________________________________________________________________________________________________
// /////      THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES,
// /////      INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// /////      DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
// /////      SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
// /////      SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,
// /////      WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE
// /////      USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// /////      _________________________________________________________________________________________________________________
// /////
// /////       This software can only be distributed with my written permission. It is for my own educational purposes and  
// /////       is potentially dangerous to ECU health and safety. Gracias a Gato Blancoford desde las alturas del mar de chelle.                                                        
// /////      _________________________________________________________________________________________________________________
// /////
// /////
// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
#endregion License
// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Threading;
using static System.Net.Mime.MediaTypeNames;
using System;
using System.Collections.Generic;

using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Threading;
using J2534;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Runtime.InteropServices;
using System.ComponentModel;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection.Emit;
using System.Collections;
using System.Globalization;
using System.Runtime.ConstrainedExecution;
using System.Text;
// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
namespace FG2ICCComms
{
	public partial class Form1
	{
		// /////
		// //////////////////////////
		// ////////////////////////////////////////
		// //////////////////////////////////////////////////////
		// ////////////////////////////////////////////////////////////////////
		// ///////////////////////////////////////////////////////////////////////////////////
		// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		////////////////////////////////////////////////////////////////////////////\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//  _______         _________________                                              __   _________                          .__  __            _____                                    
		//  \   _  \ ___  __\_____  \______  \_______   ____  ________ __   ____   _______/  |_/   _____/ ____   ____  __ _________|__|/  |_ ___.__. /  _  \   ____  ____  ____   ______ ______
		//  /  /_\  \\  \/  //  ____/   /    /\_  __ \_/ __ \/ ____/  |  \_/ __ \ /  ___/\   __\_____  \_/ __ \_/ ___\|  |  \_  __ \  \   __<   |  |/  /_\  \_/ ___\/ ___\/ __ \ /  ___//  ___/
		//  \  \_/   \>    </       \  /    /  |  | \/\  ___< <_|  |  |  /\  ___/ \___ \  |  | /        \  ___/\  \___|  |  /|  | \/  ||  |  \___  /    |    \  \__\  \__\  ___/ \___ \ \___ \ 
		//   \_____  /__/\_ \_______ \/____/   |__|    \___  >__   |____/  \___  >____  > |__|/_______  /\___  >\___  >____/ |__|  |__||__|  / ____\____|__  /\___  >___  >___  >____  >____  >
		//         \/      \/       \/                     \/   |__|           \/     \/              \/     \/     \/                       \/            \/     \/    \/    \/     \/     \/ 
		//sEcReT KeYs fRoM fOrScaN
		//'Carol', 'JAMES', 'Bosch', 'Flash', 'Bosch', 'FAITH', 'TAMER', 'REMAT', 'DIODE', 'Rowan', 'LAURA', 'JaMes', 'SAMMY', 'DIODE', 'conti', 'conti', 'Lupin', 'BOSEX', 'DIODE', 
		//'nowaR', 'PANDA', 'Jesus', 'Rowan', 'Flash', 'JAMES', 'GANES', 'SAMMY', 'Janis', 'COLIN', 'BOSCH', 'DIODE', 'Rowan', 'Rowan', 'ARIAN', 'ARIAN', 'DRIFT', 'BroWn', 'JaMes', 
		//'kbobA', '.Ted\', 'WALy\', 'euHUN', 'DRIFT', 'DRIFT', 'Flash', 'Bosch', 'Rowan', 'nowaR', 'DIODE', 'DIODE', 'DIODE', 'JaMes', 'conti', 'Rowan', 'MACOM', 'JAMES', 'MACOM', 
		//'MACOM', 'conti', 'Rowan', 'DIODE', 'BOSCH', 'JAMES', 'GANES', 'SKAND', 'FAITH', 'DIODE', 'OuTuY', 'slIor', '-MErM', 'pEde '
		/// <summary> SERVICE: 0X27 REQUEST SECURITY ACCESS seed keys 
		/// </summary>
		///         0x720: {0x01: 0x434f4c494e, 0x03: 0x40E234995F, 0x11: 0x0926F26388},
		///         0x720: {0x01: 0xfa5fc0, 0x03: 0x92c13b},
		///         0x7A6: {0x01: 0x4272616457, 0x11: 0x128665},
		///         0x727: {0x01: 0x42, 0x72, 0x61, 0x64, 0x57},
		///         0x767: {0x01: 0x4272616457}, 
		///         0x781: {0x01: 0x4272616457},
		///         0x731: {0x01: 0x672a70, 0x11: 0x462a71},0-
		///         0x760: {0x01: 0x5B4174657D, 0x03: 0x76807f, 0x11: 0x06316b}
		///////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Security Access Function Calls with seed keys for midpeed can modules
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		//////////////////////////////////////////////////////////////////
		//var obfuscator = new Obfuscator();
		//string obfuscatedID = obfuscator.Obfuscate(15);   // e.g. xVrAndNb
		// Reverse-process:
		//int deobfuscatedID = obfusactor.Deobfuscate(obfuscatedID);  // 15
		////////////////////////////////////////////////////////////////////
		// Seed Key Bruter Forcer
		
		// ##################################################################################################################
		///////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// STANDARD SECURITY ACCESS SERVICE 0x27 BOOLEAN - Mark I Algo "KeyGenMkI(int s, int sknum, int sknum2, 
		///                                                                 int sknum3, int sknum4, int sknum5)"
		/// </summary>
		/// <returns></returns>
		///////////////////////////////////////////////////////////////////////////////////////////////////
		bool requestSecurityAccess0x27()
		{
			if (j2534Flag == true)
			{
				try
				{
					var num = 0;
					addTxt1("Service: [0x27 reqSecurityAccess]\r\n");
					byte[] requestSecurityAccess = new byte[] { 0, 0, ecuRxIdentifier1, ecuRxIdentifier2, 0x27, 0x01 };
					string requestSecurityAccessMsg = sendPassThruMsg(requestSecurityAccess);
					// parse response and build seed key algo into flow...
					//00  00  07  AE  67  01  AF  BB  7F 
					string responseData = requestSecurityAccessMsg.Replace(" ", "");//remove whitespaces from the string
					string responseData1 = responseData.Substring(8, 2); //to grab the positive or negative response byte as a string
					string responseErr = responseData.Substring(12, 2); // to grab the error code if we get a 0x7F response
					int responseSec = int.Parse(responseData1, System.Globalization.NumberStyles.HexNumber);// convert the string to an int for the switch
					switch (responseSec)
					{
						case 0x67:
							addTxt1($"FDIM Recieved Security Seed.\r\n");
							string seed = responseData.Substring(12, 6);
							addTxt1($"PCM  Security Seed: " + seed + "\r\n");
							string rxbuf3 = responseData.Substring(12, 2);
							string rxbuf4 = responseData.Substring(14, 2);
							string rxbuf5 = responseData.Substring(16, 2);
							int buf3 = Convert.ToInt32(rxbuf3, 16);
							int buf4 = Convert.ToInt32(rxbuf4, 16);
							int buf5 = Convert.ToInt32(rxbuf5, 16);
							var num22 = 0;
							num22 += buf3 << 0x10;
							num22 += buf4 << 8;
							num22 += buf5;
							addTxt1($" Calculating Response.. \r\n");
							string responseKey = key_from_seed(num22).ToString("X6");
							string response1 = responseKey.Substring(0, 2);
							string response2 = responseKey.Substring(2, 2);
							string response3 = responseKey.Substring(4, 2);
							byte responseByte1 = Convert.ToByte(response1, 16);
							byte responseByte2 = Convert.ToByte(response2, 16);
							byte responseByte3 = Convert.ToByte(response3, 16);
							byte[] requestSecurityAccess02 = new byte[] { 0, 0, ecuRxIdentifier1, ecuRxIdentifier2, 0x27, 0x02, responseByte1, responseByte2, responseByte3 };
							string requestSecurityAccess02Msg = sendPassThruMsg(requestSecurityAccess02);
							string responseDataA = requestSecurityAccess02Msg.Replace(" ", "");
							string responseDataB = responseDataA.Substring(8, 2);
							//  00  00  07  2F  67  02  
							int response = int.Parse(responseDataB, System.Globalization.NumberStyles.HexNumber);
							switch (response)
							{
								case 0x67:
									addTxt1($"FDIM Security Access Granted.\r\n");
									return true;
								case 0x7F:
									string responseDataBErr = responseDataA.Substring(12, 2);
									addTxt1($"FDIM Security Access Denied. \r\n");
									int responseErr2 = Convert.ToInt32(responseDataBErr, 16);
									addTxt1(printerr(responseErr2)); // printing the error code definition from printerr(int)
									//Commented this out because it is breaking the security accesss bruteforcer 17/06/2024
									//System.Windows.Forms.MessageBox.Show(printerr(responseErr2), "FoA Orion Comms - 0x27 requestSecurityAccess", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);		
									return false;
							}
							break;
						case 0x7F:
							addTxt1($"FDIM Security Access Failed \r\n");
							int responseErr1 = Convert.ToInt32(responseErr, 16); // coverting response2 string to an int
							addTxt1(printerr(responseErr1)); // printing the error code definition from printerr(int)		
							//System.Windows.Forms.MessageBox.Show(printerr(responseErr1), "FoA Orion Comms - 0x27 requestSecurityAccess", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
							return false;
					}
				}
				catch (Exception ex)
				{
					// Catching any exception of type Exception
					// Handle the exception here, such as logging or displaying an error message
					addTxt1($"FDIM error occurred: " + ex.Message);
				}
				return false;
			}
			return false;
		}	
	
		// // // /// // // // // // // // // // 
		//
		int KeyGenMkI(int s, int sknum, int sknum2, int sknum3, int sknum4, int sknum5)
		{
			var sknum13 = (int)((byte)(s >> 0x10 & 0xFF));
			var b2 = (byte)(s >> 8 & 0xFF);
			var b3 = (byte)(s & 0xFF);
			var sknum6 = (sknum13 << 0x10) + ((int)b2 << 8) + (int)b3;
			var sknum7 = (sknum6 & 0xFF0000) >> 0x10 | (sknum6 & 0xFF00) | sknum << 0x18 | (sknum6 & 0xFF) << 0x10;
			var sknum8 = 0xC541A9;
			for (int i = 0; i < 0x20; i++)
			{
				int sknum10;
				int sknum9;
				sknum8 = (((sknum9 = (sknum10 = (((sknum7 >> i & 1) ^ (sknum8 & 1)) << 0x17 | sknum8 >> 1))) & 0xEF6FD7) | ((sknum9 & 0x100000) >> 0x14 ^ (sknum10 & 0x800000) >> 0x17) << 0x14 | ((sknum8 >> 1 & 0x8000) >> 0xF ^ (sknum10 & 0x800000) >> 0x17) << 0xF | ((sknum8 >> 1 & 0x1000) >> 0xC ^ (sknum10 & 0x800000) >> 0x17) << 0xC | 0x20 * ((sknum8 >> 1 & 0x20) >> 5 ^ (sknum10 & 0x800000) >> 0x17) | 8 * ((sknum8 >> 1 & 8) >> 3 ^ (sknum10 & 0x800000) >> 0x17));
			}
			for (int j = 0; j < 0x20; j++)
			{
				int sknum12;
				int sknum11;
				sknum8 = (((sknum11 = (sknum12 = ((((sknum5 << 0x18 | sknum4 << 0x10 | sknum2 | sknum3 << 8) >> j & 1) ^ (sknum8 & 1)) << 0x17 | sknum8 >> 1))) & 0xEF6FD7) | ((sknum11 & 0x100000) >> 0x14 ^ (sknum12 & 0x800000) >> 0x17) << 0x14 | ((sknum8 >> 1 & 0x8000) >> 0xF ^ (sknum12 & 0x800000) >> 0x17) << 0xF | ((sknum8 >> 1 & 0x1000) >> 0xC ^ (sknum12 & 0x800000) >> 0x17) << 0xC | 0x20 * ((sknum8 >> 1 & 0x20) >> 5 ^ (sknum12 & 0x800000) >> 0x17) | 8 * ((sknum8 >> 1 & 8) >> 3 ^ (sknum12 & 0x800000) >> 0x17));
			}
			return (sknum8 & 0xF0000) >> 0x10 | 0x10 * (sknum8 & 0xF) | ((sknum8 & 0xF00000) >> 0x14 | (sknum8 & 0xF000) >> 8) << 8 | (sknum8 & 0xFF0) >> 4 << 0x10;
		}
		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////		

		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary> vdo secret key from secret software origins unknown. 
		/// SECURITY ACCESS IPC SYSTEM SUPPLIER SPECIFIC 0X10FA
		/// </summary>
		/// <returns></returns>
		//////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////
		//////////////////////////////////////////////////////////////////
		// MARK II SECURITY ACCESS PROCESS
	
		public int calculateKeyfromSeed(int s)
		{
			var b = (byte)(s >> 0x10 & 0xFF);
			var b2 = (byte)(s >> 8 & 0xFF);
			var b3 = (byte)(s & 0xFF);
			int num;
			int num2;
			int num3;
			int num4;
			int num5;
			// ///////////////////////////////
			// pre-obfuscado-the-vdo-avocado
			// 0xbf, 0xa4, 0x90, 0x56, 0xcd 
			num = 0xBF;
			num2 = 0xA4;
			num3 = 0x90;
			num4 = 0x56;
			num5 = 0xCD;
			// ///////////////////////////////
			// ///////////////////////////////
			var num6 = ((int)b << 0x10) + ((int)b2 << 8) + (int)b3;
			var num7 = (num6 & 0xFF0000) >> 0x10 | (num6 & 0xFF00) | num << 0x18 | (num6 & 0xFF) << 0x10;
			var num8 = 0xC541A9;
			for (int i = 0; i < 0x20; i++)
			{
				var num9 = ((num7 >> i & 1) ^ (num8 & 1)) << 0x17;
				int num11;
				int num10;
				num8 = (((num10 = (num11 = (num9 | num8 >> 1))) & 0xEF6FD7) | ((num10 & 0x100000) >> 0x14 ^ (num11 & 0x800000) >> 0x17) << 0x14 | ((num8 >> 1 & 0x8000) >> 0xF ^ (num11 & 0x800000) >> 0x17) << 0xF | ((num8 >> 1 & 0x1000) >> 0xC ^ (num11 & 0x800000) >> 0x17) << 0xC | 0x20 * ((num8 >> 1 & 0x20) >> 5 ^ (num11 & 0x800000) >> 0x17) | 8 * ((num8 >> 1 & 8) >> 3 ^ (num11 & 0x800000) >> 0x17));
			}
			for (int i = 0; i < 0x20; i++)
			{
				var num9 = (((num5 << 0x18 | num4 << 0x10 | num2 | num3 << 8) >> i & 1) ^ (num8 & 1)) << 0x17;
				int num14;
				int num13;
				var num12 = num13 = (num14 = (num9 | num8 >> 1));
				num8 = ((num13 & 0xEF6FD7) | ((num12 & 0x100000) >> 0x14 ^ (num14 & 0x800000) >> 0x17) << 0x14 | ((num8 >> 1 & 0x8000) >> 0xF ^ (num14 & 0x800000) >> 0x17) << 0xF | ((num8 >> 1 & 0x1000) >> 0xC ^ (num14 & 0x800000) >> 0x17) << 0xC | 0x20 * ((num8 >> 1 & 0x20) >> 5 ^ (num14 & 0x800000) >> 0x17) | 8 * ((num8 >> 1 & 8) >> 3 ^ (num14 & 0x800000) >> 0x17));
			}
			return (num8 & 0xF0000) >> 0x10 | 0x10 * (num8 & 0xF) | ((num8 & 0xF00000) >> 0x14 | (num8 & 0xF000) >> 8) << 8 | (num8 & 0xFF0) >> 4 << 0x10;
		}




	


	//
	//////////////////////////////////////////////////////////////////

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//      .___       _____  __ __________               .__                     /\ ____   _________________________.__                .__                  
	//    __| _/____ _/ ____\/  |\______   \_____    ____ |__| ____    ____      / / \   \ /   /\______   \_   _____/|  | _____    _____|  |__   ___________ 
	//   / __ |\__  \\   __\\   __\       _/\__  \ _/ ___\|  |/    \  / ___\    / /   \   Y   /  |    |  _/|    __)  |  | \__  \  /  ___/  |  \_/ __ \_  __ \
	//  / /_/ | / __ \|  |   |  | |    |   \ / __ \\  \___|  |   |  \/ /_/  >  / /     \     /   |    |   \|     \   |  |__/ __ \_\___ \|   Y  \  ___/|  | \/
	//  \____ |(____  /__|   |__| |____|_  /(____  /\___  >__|___|  /\___  /  / /       \___/    |______  /\___  /   |____(____  /____  >___|  /\___  >__|   
	//       \/     \/                   \/      \/     \/        \//_____/   \/                        \/     \/              \/     \/     \/     \/       
	//#############################################################################################################
	//# function that generates the key
	//#############################################################################################################
	//def supplierKeyGen(seed):
	//    fixed  = 0xBFA49056CD
	//    try: 
	//        challengeCode = array('Q')
	//        challengeCode.append(fixed & 0xff)
	//        challengeCode.append((fixed >> 8) & 0xff)
	//        challengeCode.append((fixed >> 16) & 0xff)
	//        challengeCode.append((fixed >> 24) & 0xff)
	//        challengeCode.append((fixed >> 32) & 0xff)
	//        challengeCode.append(seed[2])
	//        challengeCode.append(seed[1])
	//        challengeCode.append(seed[0])
	//        temp1 = 0xC541A9
	//        for i in range(64):
	//            abit = temp1 & 0x01
	//            chbit = challengeCode[7] & 0x01
	//            bbit = abit ^ chbit
	//            temp2 = (temp1 >> 1) + bbit * 0x800000 & -1
	//            temp1 = (temp2 ^ 0x109028 * bbit) & -1
	//            challengeCode[7] = challengeCode[7] >> 1 & 0xff
	//            for a in range(7, 0, -1):
	//                 challengeCode[a] = challengeCode[a] + (challengeCode[a - 1] & 1) * 128 & 0xff
	//                 challengeCode[a - 1] = challengeCode[a - 1] >> 1
	//        key = [ temp1 >> 4 & 0xff, ((temp1 >> 12 & 0x0f) << 4) + (temp1 >> 20 & 0x0f), (temp1 >> 16 & 0x0f) + ((temp1 & 0x0f) << 4) ]
	//        print("Succesfully got key: {key}")
	//        return key
	//    except can.CanError():
	//        print("CAN Error")       
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

}
}
