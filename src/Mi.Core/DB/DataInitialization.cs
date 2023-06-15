using Mi.Core.Toolkit.Helper;

using Microsoft.Extensions.Configuration;

namespace Mi.Core.DB
{
	public class DataInitialization
	{
		public static async Task<bool> RunAsync()
		{
			var configuration = new ConfigurationBuilder()
				.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
				.AddJsonFile("systemdata.json").Build();
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

			return true;
		}
	}
}
