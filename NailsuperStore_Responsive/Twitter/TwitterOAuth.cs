using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using Hammock;
using Hammock.Authentication.Basic;
using Hammock.Authentication.OAuth;

namespace Twitter
{
	/// <summary>
	/// This class uses the Hammock REST API as a wrapper to perform OAuth REST requests to authenticate requests to the Twitter API
	/// </summary>
	/// <remarks>
	/// See http://dev.twitter.com/pages/auth
	/// And http://hammock.codeplex.com/
	/// And http://oauth.net/documentation/getting-started/
	/// </remarks>
	public class TwitterOAuth
	{
		#region Properties

		/// <summary>
		/// Gets the consumer key value for the application
		/// </summary>
		public string ConsumerKey { get; private set; }
		/// <summary>
		/// Gets the consumer secret value for the application
		/// </summary>
		public string ConsumerSecret { get; private set; }

		/// <summary>
		/// Gets or sets the call back URL that Twitter returns data to
		/// </summary>
		public string CallBackUrl { get; set; }

		#endregion Properties

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="HammockOAuth"/> class used to create OAuth requests against the Twitter API
		/// </summary>
		/// <remarks>
		/// The consumer key and secret is obtained by registering an application at http://dev.twitter.com/apps/new
		/// </remarks>
		/// <param name="consumerKey">The consumer key for the application</param>
		/// <param name="consumerSecret">The consumer secret for the application</param>
		public TwitterOAuth(string consumerKey, string consumerSecret)
		{
			this.ConsumerKey = consumerKey;
			this.ConsumerSecret = consumerSecret;
			System.Net.ServicePointManager.Expect100Continue = false; // See http://blogs.msdn.com/b/shitals/archive/2008/12/27/9254245.aspx
		}

		#endregion Constructor

		#region Authorisation Methods

		/// <summary>
		/// Gets the OAuth authentication response from Twitter, passing in the ConsumerKey and ConsumerSecret
		/// </summary>
		/// <returns>A Hammock RestResponse from the Twitter API</returns>
		public RestResponse GetAuthenticationResponse()
		{
			// Use Hammock to set up our authentication credentials
			OAuthCredentials credentials = new OAuthCredentials()
			{
				Type = OAuthType.RequestToken,
				SignatureMethod = OAuthSignatureMethod.HmacSha1,
				ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader,
				ConsumerKey = this.ConsumerKey,
				ConsumerSecret = this.ConsumerSecret
			};

			if (!String.IsNullOrEmpty(this.CallBackUrl))
				credentials.CallbackUrl = this.CallBackUrl;

			// Use Hammock to create a rest client, specifying the authority (the base URL for requests) and the credentials we just created
			var client = new RestClient
			{
				Authority = TwitterConstants.OAuthAuthority,
				Credentials = credentials
			};

			// Use Hammock to create a request, specifiying the rest of the path (which is appended to Authority)
			var request = new RestRequest
			{
				Path = "request_token"
			};

			// Return the response from the request
			return client.Request(request);
		}

		/// <summary>
		/// Calls GetAuthenticationResponse() and returns an OAuthToken with the parameters populated from the Twitter server's response.
		/// This token is then used in future requests.
		/// </summary>
		/// <param name="callbackUrl">The callback URL that the results are posted to after authorisation</param>
		/// <returns>
		/// An OauthToken if a valid, accepted response is returned by the Twitter authentication server
		/// </returns>
		public OAuthToken GetAuthenticationToken()
		{
			RestResponse response = GetAuthenticationResponse();

			if (response == null)
				throw new NullReferenceException("The response returned from GetAuthenticationResponse() was null.");

			if (response.StatusCode == System.Net.HttpStatusCode.OK)
			{
				NameValueCollection responseCol = HttpUtility.ParseQueryString(response.Content);

				OAuthToken token = new OAuthToken();
				token.Token = responseCol["oauth_token"];
				token.TokenSecret = responseCol["oauth_token_secret"];
				token.CallbackConfirmed = bool.Parse(responseCol["oauth_callback_confirmed"]);

				return token;
			}
			else
			{
				throw new ApplicationException("The server responded with a status code of " + response.StatusCode.ToString());
			}
		}

		/// <summary>
		/// Gets the authorisation URL to which the end user is redirected to enter their Twitter username and password
		/// </summary>
		/// <param name="token">The OauthToken return by GetAuthenticationToken()</param>
		/// <returns>A URI like http://api.twitter.com/oauth/authorize?oauth_token=ABCDEFGHI</returns>
		public Uri GetAuthorisationUrl(OAuthToken token)
		{
			if (token == null)
				throw new ArgumentNullException("token", "The OAuthToken that was passed was null.");

			string url = "http://api.twitter.com/oauth/authorize?oauth_token=" + token.Token;

			return new Uri(url);
		}

		#endregion Authorisation Methods

		#region Access Methods

		/// <summary>
		/// Gets the access response for the Twitter API after the user has logged-in
		/// </summary>
		/// <param name="request">The HTTP request returned from the authorise access page</param>
		/// <returns>A Hammock RestResponse from the Twitter API</returns>
		public RestResponse GetAccessResponse(System.Web.HttpRequest request)
		{
			if (request == null)
				throw new ArgumentNullException("request", "The request stream cannot be null");

			string token = request["oauth_token"];
			string verifier = request["oauth_verifier"];

			if (String.IsNullOrEmpty(token))
				throw new NullReferenceException("The oauth_token returned from the request was null or empty");

			if (String.IsNullOrEmpty(verifier))
				throw new NullReferenceException("The oauth_verifier returned from the request was null or empty");

			return GetAccessResponse(token, verifier);
		}

		/// <summary>
		/// Gets the access response for the Twitter API after the user has logged-in
		/// </summary>
		/// <param name="token">The token returned to the callback URL</param>
		/// <param name="verifier">The verifier returned to the callback URL</param>
		/// <returns>A Hammock RestResponse from the Twitter API</returns>
		public RestResponse GetAccessResponse(string token, string verifier)
		{
			OAuthCredentials credentials = new OAuthCredentials()
			{
				Type = OAuthType.AccessToken,
				SignatureMethod = OAuthSignatureMethod.HmacSha1,
				ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader,
				ConsumerKey = this.ConsumerKey,
				ConsumerSecret = this.ConsumerSecret,
				Token = token,
				Verifier = verifier,
				Version = TwitterConstants.Version
			};

			var client = new RestClient
			{
				Authority = TwitterConstants.OAuthAuthority,
				Credentials = credentials
			};

			var request = new RestRequest
			{
				Path = "access_token"
			};

			return client.Request(request);
		}

		/// <summary>
		/// Gets the access token for the passed in token string and verifier string
		/// </summary>
		/// <param name="token">The token</param>
		/// <param name="verifier">The verifier</param>
		/// <returns>An OAuthToken containing the requisite properties required to grant access to further REST requests</returns>
		public OAuthToken GetAccessToken(string token, string verifier)
		{
			RestResponse response = GetAccessResponse(token, verifier);

			if (response == null)
				throw new NullReferenceException("The response returned from GetAccessResponse() was null.");

			if (response.StatusCode == System.Net.HttpStatusCode.OK)
			{
				NameValueCollection responseCol = HttpUtility.ParseQueryString(response.Content);

				OAuthToken authToken = new OAuthToken();
				authToken.Token = responseCol["oauth_token"];
				authToken.TokenSecret = responseCol["oauth_token_secret"];
				authToken.ScreenName = responseCol["screen_name"];
				authToken.UserId = responseCol["user_id"];

				return authToken;
			}
			else
			{
				throw new ApplicationException("The server responded with a status code of " + response.StatusCode.ToString());
			}
		}

		/// <summary>
		/// Saves the validated OAuth data from a REST response returned after requesting access
		/// </summary>
		/// <param name="data">The OAuth data</param>
		/// <param name="response">The REST response returned </param>
		public void SaveValidatedDataFromResponse(OAuthData data, RestResponse response)
		{
			if (data == null)
				throw new ArgumentNullException("data", "The OAuthData cannot be null");

			if (response == null)
				throw new ArgumentNullException("response", "The REST response cannot be null");

			NameValueCollection responseCol = HttpUtility.ParseQueryString(response.Content);

			data.Token = responseCol["oauth_token"];
			data.TokenSecret = responseCol["oauth_token_secret"];
			data.UserID = int.Parse(responseCol["user_id"]);
			data.ScreenName = responseCol["screen_name"];

			//data.Save();

		}

		#endregion Access Methods
	}
}
