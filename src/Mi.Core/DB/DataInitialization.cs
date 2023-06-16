using System.Text;

using Mi.Core.Extension;
using Mi.Core.Models;
using Mi.Core.Toolkit.Helper;

using Microsoft.Extensions.Configuration;

namespace Mi.Core.DB
{
    public class DataInitialization
    {
        private static long UiConfigKeyId = 0;
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
            //站点配置字典
            var key = "UiConfig";
            var config = configuration.Bind<SysConfigModel>(key);
            if (!DapperDB.Exist("select 1 from SysDict where ParentKey=@key", new { key }))
            {
                UiConfigKeyId = DapperDB.ExecuteScalar<long>("select id from SysDict where Key=@key limit 1;", new {key});
                var sql = new StringBuilder("INSERT INTO SysDict('Id', 'Name', 'Key', 'Value', 'ParentKey', 'Sort', 'ParentId', 'Remark', 'CreatedBy', 'CreatedOn', 'ModifiedBy', 'ModifiedOn', 'IsDeleted') VALUES");
                sql.Append(GenValueSql("站点标题", nameof(config.site_title), config.site_title));
                sql.Append(GenValueSql("站点ico", nameof(config.site_icon), config.site_icon));
                sql.Append(GenValueSql("进入后侧边栏Logo", nameof(config.logo), config.logo));
                sql.Append(GenValueSql("进入后侧边栏显示名称", nameof(config.header_name), config.header_name));
                sql.Append(GenValueSql("登录页中间显示名称", nameof(config.login_middle_name), config.login_middle_name));
                sql.Append(GenValueSql("登录页底部显示文本", nameof(config.login_footer_word), config.login_footer_word));
                sql.Append(GenValueSql("默认页行数", nameof(config.default_page_size), config.default_page_size.ToString()));
                sql.Append(GenValueSql("分页行数下拉选项", nameof(config.default_size_array), config.default_size_array));
                sql.Append(GenValueSql("首页名称", nameof(config.home_page_name), config.home_page_name));
                sql.Append(GenValueSql("首页地址", nameof(config.home_page_url), config.home_page_url));
                await DapperDB.ExecuteAsync(sql.ToString().Trim(','));
            }

            return true;
        }

        private static string GenValueSql(string name, string key, string? value)
        {
            return $"({IdHelper.SnowflakeId()},'{name}','{key}','{value}','UiConfig',0,'{UiConfigKeyId}','{name}',-1,'{DateTime.Now:D}',NULL,NULL,0),";
        }
    }
}
