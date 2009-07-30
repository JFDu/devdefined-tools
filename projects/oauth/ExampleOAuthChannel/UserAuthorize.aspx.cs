#region License

// The MIT License
//
// Copyright (c) 2006-2008 DevDefined Limited.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

#endregion

using System;
using System.Web;
using System.Web.UI;
using DevDefined.OAuth.Framework;
using DevDefined.OAuth.Storage.Basic;

namespace ExampleOAuthChannel
{
  public partial class UserAuthorize : Page
  {
    ITokenRepository<DevDefined.OAuth.Storage.Basic.AccessToken> AccessTokenRepository
    {
      get { return OAuthServicesLocator.AccessTokenRepository; }
    }

    ITokenRepository<DevDefined.OAuth.Storage.Basic.RequestToken> RequestTokenRepository
    {
      get { return OAuthServicesLocator.RequestTokenRepository; }
    }

    public string ConsumerKey
    {
      get { return RequestTokenRepository.GetToken(GetTokenString()).ConsumerKey; }
    }

    string GetTokenString()
    {
      string token = Request[Parameters.OAuth_Token];

      if (string.IsNullOrEmpty(token)) throw new Exception("The token string in the parameters collection was invalid/missing");

      return token;
    }

    protected void Approve_Click(object sender, EventArgs e)
    {
      string token = GetTokenString();

      ApproveRequestForAccess(token);

      RedirectToClient(token, true);
    }

    protected void Deny_Click(object sender, EventArgs e)
    {
      string token = GetTokenString();

      DenyRequestForAccess(token);

      RedirectToClient(token, false);
    }

    void ApproveRequestForAccess(string tokenString)
    {
      DevDefined.OAuth.Storage.Basic.RequestToken requestToken = RequestTokenRepository.GetToken(tokenString);

      var accessToken = new DevDefined.OAuth.Storage.Basic.AccessToken
        {
          ConsumerKey = requestToken.ConsumerKey,
          Realm = requestToken.Realm,
          Token = Guid.NewGuid().ToString(),
          TokenSecret = Guid.NewGuid().ToString(),
          UserName = HttpContext.Current.User.Identity.Name,
          ExpireyDate = DateTime.Now.AddMinutes(1),
          Roles = new string[] {}
        };

      AccessTokenRepository.SaveToken(accessToken);

      requestToken.AccessToken = accessToken;

      RequestTokenRepository.SaveToken(requestToken);
    }

    void DenyRequestForAccess(string tokenString)
    {
      DevDefined.OAuth.Storage.Basic.RequestToken requestToken = RequestTokenRepository.GetToken(tokenString);

      requestToken.AccessDenied = true;

      RequestTokenRepository.SaveToken(requestToken);
    }

    void RedirectToClient(string token, bool granted)
    {
      string callBackUrl = Request[Parameters.OAuth_Callback];

      if (!string.IsNullOrEmpty(callBackUrl))
      {
        if (!callBackUrl.Contains("?")) callBackUrl += "?";
        else callBackUrl += "&";

        callBackUrl += Parameters.OAuth_Token + "=" + UriUtility.UrlEncode(token);

        Response.Redirect(callBackUrl, true);
      }
      else
      {
        if (granted) Response.Write("Authorized");
        else Response.Write("Denied");

        Response.End();
      }
    }
  }
}