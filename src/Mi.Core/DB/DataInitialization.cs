using System.ComponentModel;
using System.Reflection;
using System.Text;

using Mi.Core.Extension;
using Mi.Core.GlobalVar;
using Mi.Core.Models;
using Mi.Core.Models.WxWork;
using Mi.Core.Service;
using Mi.Core.Toolkit.Helper;
using Mi.Repository.DB;

using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;

namespace Mi.Core.DB
{
    public class DataInitialization
    {
        public static async Task<bool> RunAsync()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("initdata.json").Build();
            //超级管理添加
            var userName = configuration["SuperAdmin:UserName"];
            var password = configuration["SuperAdmin:Password"];
            if (!string.IsNullOrWhiteSpace(userName) && !string.IsNullOrWhiteSpace(password) && !DapperDB.Exist("select 1 from SysUser where LOWER(UserName)=@name", new { name = userName.ToLower() }))
            {
                var salt = EncryptionHelper.GetPasswordSalt();
                var pwd = EncryptionHelper.GenEncodingPassword(password, salt);
                await DapperDB.ExecuteAsync(@"insert into SysUser(Id,UserName,Password,PasswordSalt,IsSuperAdmin,CreatedOn,CreatedBy,IsEnabled,IsDeleted)
							values(@id,@name,@pwd,@salt,1,@time,-1,1,0)", new { id = IdHelper.SnowflakeId(), name = userName, pwd, salt, time = TimeHelper.LocalTime() });
            }

            #region 站点配置字典

            if (!DapperDB.Exist("select 1 from SysDict where key=@key", new { key = DictKeyConst.UiConfig }))
            {
                var parentId = IdHelper.SnowflakeId();
                var config = configuration.Bind<SysConfigModel>(DictKeyConst.UiConfig);    
                var dicts = new List<SysDict>();
                foreach (var prop in typeof(SysConfigModel).GetProperties())
                {
                    var desc = prop.GetCustomAttribute<DescriptionAttribute>();
                    dicts.Add(new SysDict
                    {
                        CreatedOn = TimeHelper.LocalTime(),
                        CreatedBy = -1,
                        Id = IdHelper.SnowflakeId(),
                        Key = prop.Name,
                        Value = Convert.ToString(prop.GetValue(config)),
                        Name = desc?.Description,
                        Remark = desc?.Description,
                        ParentId = parentId,
                        ParentKey = DictKeyConst.UiConfig
                    });
                }
                using (var db = DotNetService.Get<MIDB>())
                {
                    await db.AddAsync(new SysDict
                    {
                        CreatedOn = TimeHelper.LocalTime(),
                        CreatedBy = -1,
                        Id = parentId,
                        Name = "UI配置",
                        Remark = "系统UI配置",
                        Key = DictKeyConst.UiConfig,
                        Sort = 99
                    });
                    await db.AddRangeAsync(dicts);
                    await db.SaveChangesAsync();
                }
            }

            #endregion

            #region 企业微信配置

            if (!DapperDB.Exist("select 1 from SysDict where key=@key", new { key = DictKeyConst.WxWork }))
            {
                var config = configuration.Bind<WxConfig>(DictKeyConst.WxWork);
                var parentId = IdHelper.SnowflakeId();
                var dicts = new List<SysDict>
                {
                    new SysDict
                    {
                        CreatedOn = TimeHelper.LocalTime(),
                        CreatedBy = -1,
                        Id = IdHelper.SnowflakeId(),
                        Name = "企业Id",
                        Remark = "企业Id",
                        Key = nameof(config.corpid),
                        ParentId = parentId,
                        ParentKey = DictKeyConst.WxWork,
                        Value = config.corpid,
                        Sort = 1
                    },
                    new SysDict
                    {
                        CreatedOn = TimeHelper.LocalTime(),
                        CreatedBy = -1,
                        Id = IdHelper.SnowflakeId(),
                        Name = "密钥-通讯录同步",
                        Remark = "密钥-通讯录同步",
                        Key = nameof(config.wx_work_contact_list_secret_sync),
                        ParentId = parentId,
                        ParentKey = DictKeyConst.WxWork,
                        Value = config.wx_work_contact_list_secret_sync,
                        Sort = 2
                    },
                    new SysDict
                    {
                        CreatedOn = TimeHelper.LocalTime(),
                        CreatedBy = -1,
                        Id = IdHelper.SnowflakeId(),
                        Name = "密钥-通讯录",
                        Remark = "密钥-通讯录",
                        Key = nameof(config.wx_work_contact_list_secret),
                        ParentId = parentId,
                        ParentKey = DictKeyConst.WxWork,
                        Value = config.wx_work_contact_list_secret,
                        Sort = 3
                    },
                };
                using (var db = DotNetService.Get<MIDB>())
                {
                    await db.AddAsync(new SysDict
                    {
                        CreatedOn = TimeHelper.LocalTime(),
                        CreatedBy = -1,
                        Id = parentId,
                        Name = "UI配置",
                        Remark = "系统UI配置",
                        Key = DictKeyConst.UiConfig,
                        Sort = 99
                    });
                    await db.AddRangeAsync(dicts);
                    await db.SaveChangesAsync();
                }
            }
            #endregion

            return true;
        }

        private static string GenValueSql(string name, string key, string? value, string parentKey, long parentId)
        {
            return $"({IdHelper.SnowflakeId()},'{name}','{key}','{value}','{parentKey}',0,'{parentId}','{name}',-1,'{DateTime.Now:D}',NULL,NULL,0),";
        }
    }
}
