using McsCoreLib.Configs;
using McsCoreLib.Models.UserAuth;

using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace McsCoreLib.Tools
{
    public class McsTokenTool
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 强制token 失效
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static bool InvalidateToken(string token)
        {
            try
            {
                var dbRef = McsDbTool.GetDBRepositoryRef<TokenBuffer>();
                if (!string.IsNullOrEmpty(token))
                {
                    var handler = new JwtSecurityTokenHandler();
                    var payload = handler.ReadJwtToken(token).Payload;
                    var id = (string)payload[ClaimTypes.NameIdentifier];
                    dbRef.AsDeleteable().Where(a => a.UserID == id).ExecuteCommand();
                    dbRef.InsertOrUpdate(new TokenBuffer() { UserToken = token, UserID = id });
                }
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return false;
            }
        }

        public static string GetUserToken(string userid, int mAuthLevelid, bool isLogin, string oldToken = null)
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Jwtsetting.SecretKey));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var claims = new[]
                {
                new Claim(ClaimTypes.NameIdentifier, userid),
                new Claim(JwtRegisteredClaimNames.Sub, mAuthLevelid.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

                var token = new JwtSecurityToken(
                    issuer: "McsAuth",
                    audience: "McsClient",
                    claims: claims,
                    notBefore: DateTime.Now,
                    expires: DateTime.Now.Add(Jwtsetting.ExpireTime),
                    signingCredentials: credentials);

                var encodejwt = new JwtSecurityTokenHandler().WriteToken(token);

                if (isLogin)
                {
                    var dbRef = McsDbTool.GetDBRepositoryRef < TokenBuffer>();
                    dbRef.AsDeleteable().Where(a => a.UserID == userid).ExecuteCommand();
                }

                return encodejwt;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// token 验证
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static bool ValidateAccessToken(string token, out string mNewToken)
        {
            string mTokenStr = token;
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(Jwtsetting.SecretKey);
            try
            {
                var dbRef = McsDbTool.GetDBRepositoryRef < TokenBuffer>();
                //在作废列表中,则直接返回false
                if (dbRef.AsQueryable().Where(a => a.UserToken == token).Any())
                {
                    mNewToken = "";
                    return false;
                }

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "McsAuth",
                    ValidAudience = "McsClient",
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                }, out var mTokenInfo);
            }
            catch (SecurityTokenExpiredException)
            {
                var handler = new JwtSecurityTokenHandler();
                var mplayload = handler.ReadJwtToken(token);
                var time = (DateTimeOffset.FromUnixTimeSeconds((long)mplayload.Payload["exp"]).LocalDateTime).Add(Jwtsetting.RefreshExpireTime);
                var s = time - DateTime.Now;

                if (s.TotalSeconds > 0)
                {
                    mNewToken = GetUserToken(
                        (string)mplayload.Payload[ClaimTypes.NameIdentifier],
                        int.Parse((string)mplayload.Payload[JwtRegisteredClaimNames.Sub]), false, token);
                    return true;
                }
                else
                {
                    mNewToken = "";
                    return false;
                }
            }
            catch (Exception)
            {
                mNewToken = "";
                return false;
            }
            mNewToken = mTokenStr;
            return true;
        }
    }
}