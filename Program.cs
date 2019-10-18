using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRC16_Modbus
{
    class Program
    {        
        #region CRC-16 Modbus
        public static byte[] ToModbus(byte[] byteData)
        {
            byte[] CRC = new byte[2];   // CRC-16 returns 2 bytes
            UInt16 CRC_Chk = 0xFFFF;    // The Initial Value of CRC-16 modbus

            // Check every Bytes 
            for (int i = 0; i < byteData.Length; i++)
            {
                // CRC_Chk XOR with byte[i]
                CRC_Chk ^= Convert.ToUInt16(byteData[i]);   

                // Check every bits in byte
                for (int j = 0; j < 8; j++)
                {
                    // 若右移位元 = 1 , 則 XOR 0xA001
                    if ((CRC_Chk & 0x0001) == 1)
                    {
                        CRC_Chk >>= 1;
                        CRC_Chk ^= 0xA001; // 0xA001 is reverse from poly : 0x8005H
                    }
                    else
                    {
                        CRC_Chk >>= 1;
                    }
                }
            }
            // 計算結果須交換位置
            CRC[1] = (byte)((CRC_Chk&0xFF00)>>8);
            CRC[0] = (byte)(CRC_Chk &0x00FF);

            return CRC;
        }

        public static byte[] ToMsbLsb(byte[] byteData)
        {
            byte[] CRC = new byte[2];
            byte[] crcSwitch = new byte[2];

            CRC = ToModbus(byteData);
            crcSwitch[0] = CRC[1];
            crcSwitch[1] = CRC[0];

            return crcSwitch;
        }
        #endregion

        static void Main(string[] args)
        {   
            byte[] CRC_16 = new byte[] {0x01,0x03};
            byte[] CRC_result = ToMsbLsb(CRC_16);
            Console.WriteLine("CRC[1] = 0x{0} ,CRC[0] = 0x{1} ", Convert.ToString(CRC_result[1], 16).ToUpper(), Convert.ToString(CRC_result[0], 16).ToUpper());
            Console.ReadKey();
        }
    }

}
