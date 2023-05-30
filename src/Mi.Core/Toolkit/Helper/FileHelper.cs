﻿namespace Mi.Core.Toolkit.Helper
{
    public class FileHelper
    {
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="path">文件路径</param>
        public static void Delete(string path)
        {
            if (!File.Exists(path))
                throw new Ouch("文件不存在");
            File.Delete(path);
        }

        /// <summary>
        /// 获取文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public static byte[] Get(string path)
        {
            using var fs = new FileStream(path, FileMode.Open);
            var byt = new byte[fs.Length];
            fs.Read(byt);
            return byt;
        }
    }
}