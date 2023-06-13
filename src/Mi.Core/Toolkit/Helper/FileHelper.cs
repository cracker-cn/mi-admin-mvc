using System.Drawing.Imaging;
using System.Drawing;

namespace Mi.Core.Toolkit.Helper
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
            if (!File.Exists(path))
                throw new Ouch("文件不存在");
            using var fs = new FileStream(path, FileMode.Open);
            var byt = new byte[fs.Length];
            fs.Read(byt);
            return byt;
        }

        /// <summary>
        /// Image 转成 base64
        /// </summary>
        /// <param name="fileFullName">路径</param>
        public static string GetBase64(string fileFullName)
        {
            try
            {
                Bitmap bmp = new Bitmap(fileFullName);
                MemoryStream ms = new MemoryStream();
                var suffix = fileFullName.Substring(fileFullName.LastIndexOf('.') + 1,
                                    fileFullName.Length - fileFullName.LastIndexOf('.') - 1).ToLower();
                var suffixName = suffix == "png"
                                    ? ImageFormat.Png
                                    : suffix == "jpg" || suffix == "jpeg"
                                        ? ImageFormat.Jpeg
                                        : suffix == "bmp"
                                            ? ImageFormat.Bmp
                                            : suffix == "gif"
                                                ? ImageFormat.Gif
                                                : ImageFormat.Jpeg;
                bmp.Save(ms, suffixName);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                string base64String = "data:image/" + suffix + ";base64," + Convert.ToBase64String(arr);
                return base64String;
            }

            catch (Exception ex)
            {
                throw new Ouch(ex.Message);
            }
        }
    }
}