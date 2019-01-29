using System;
using System.Linq;
using System.Text;

namespace Pangu
{
    public class EncodingManager
    {
        /* 判断产生该字节流的文件是否为文本文件 */
        public static bool IsTextFile(byte[] fileBytes)
        {
            var fileByteList = fileBytes.ToList(); // 利用字节数组创建字节列表
            return !fileByteList.Contains(0); // 若存在字节 0，则不是文本文件
        }

        /* 解析获取字节流的编码类型 */
        public static Encoding GetEncodingType(byte[] fileBytes)
        {
            if (IsUTF8WithoutBom(fileBytes))
                return new UTF8Encoding(); // 不带 BOM 的 UTF-8
            else if (fileBytes[0] == 0xEF && fileBytes[1] == 0xBB && fileBytes[2] == 0xBF)
                return Encoding.UTF8; // 带 BOM 的 UTF-8
            else if (fileBytes[0] == 0xFE && fileBytes[1] == 0xFF && fileBytes[2] == 0x00)
                return Encoding.BigEndianUnicode; // Unicode (Big Endian)
            else if (fileBytes[0] == 0xFF && fileBytes[1] == 0xFE && fileBytes[2] == 0x41)
                return Encoding.Unicode; // Unicode
            else
                return Encoding.Default; // TODO: .NET Framework 和 .NET Core 对此会有不同的实现，应在确保正确性的基础上使用
        }

        /* 判断是否为不带 BOM 的 UTF-8 编码 */
        private static bool IsUTF8WithoutBom(byte[] fileBytes)
        {
            var charByteCounter = 1;  // 计算当前分析的字符应还有的字节数 
            byte curByte; // 当前分析的字节
            foreach (var fileByte in fileBytes)
            {
                curByte = fileByte;
                if (charByteCounter == 1)
                {
                    if (curByte >= 0x80)
                    {
                        while (((curByte <<= 1) & 0x80) != 0) // 判断当前
                            charByteCounter++;
                        if (charByteCounter == 1 || charByteCounter > 6) // 标记位首位若为非 0，则至少以 2 个 1 开始，如：110XXXXX……1111110X
                            return false;
                    }
                }
                else
                {
                    if ((curByte & 0xC0) != 0x80) // 若是 UTF-8，此时第一位必须为 1
                        return false;
                    charByteCounter--;
                }
            }
            if (charByteCounter > 1)
                throw new FormatException("非预期的 byte 格式！");
            return true;
        }
    }
}