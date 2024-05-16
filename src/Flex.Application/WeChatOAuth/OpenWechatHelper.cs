using Flex.Application.ContentModel;
using SKIT.FlurlHttpClient.Wechat.Api;
using SKIT.FlurlHttpClient.Wechat.Api.Models;

namespace Flex.Application.WeChatOAuth;
/// <summary>
/// 微信开放平台帮助类
/// </summary>
public class OpenWechatHelper
{
    private WechatApiClient client;

    /// <summary>
    /// 构造函数注入
    /// </summary>
    public OpenWechatHelper()
    {
        var wechatConfig = "OpenWeixin".Config<OpenWeixinOptions>();
        var options = new WechatApiClientOptions()
        {
            AppId = wechatConfig.OpenWechatAppId,
            AppSecret = wechatConfig.OpenWechatAppSecret,
        };
        client = new WechatApiClient(options);
    }

    public static OpenWechatHelper instance =
            Singleton<OpenWechatHelper>.Instance
            ?? (Singleton<OpenWechatHelper>.Instance = new OpenWechatHelper());

    /// <summary>
    /// 根据code获取某个应用下的微信用户id
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    public async Task<string> GetOpenId(string code)
    {
        var request = new SnsOAuth2AccessTokenRequest();
        request.Code = code;
        var tokenResponse = await client.ExecuteSnsOAuth2AccessTokenAsync(request);
        if (!tokenResponse.IsSuccessful())
        {
            return string.Empty;
        }

        return tokenResponse.OpenId;
    }
}
