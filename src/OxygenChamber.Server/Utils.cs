using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace OxygenChamber.Server
{
    public static class Utils
    {
        public static string To16String(this byte[] bytes) {
            return BitConverter.ToString(bytes, 0).Replace("-", string.Empty);//.ToLower()
        }

        public static byte CalculateAdd(this byte[] buffer)//取低八位
        {
            int cks = 0;
            foreach (byte item in buffer)
            {
                cks = (cks + item) % 0xffff;
            }
            //byte bt = (byte)((cks & 0xff00) >> 8);//取校验和高8位
            byte bt = (byte)(cks & 0xff);//取校验和低8位
            return bt;
        }
    }
}
